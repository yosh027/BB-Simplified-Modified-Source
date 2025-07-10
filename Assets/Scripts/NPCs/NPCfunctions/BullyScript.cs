using UnityEngine;
using System.Collections;

public class BullyScript : MonoBehaviour
{
    #region Unity Lifecycle
    private void Start()
    {
        audioDevice = GetComponent<AudioSource>();
        StartCoroutine(WaitAndActivate());
    }

    private void Update()
    {
        if (active)
        {
            activeTime += Time.deltaTime;
            if (activeTime >= 180f)
            {
                Reset();
                if (!audioDevice.isPlaying)
                {
                    audioDevice.PlayOneShot(aud_Bored);
                }
            }
        }

        guilt = Mathf.Max(guilt - Time.deltaTime, 0f);

        float distance = Vector3.Distance(Principal.position, Obstacle.transform.position);
        Obstacle.enabled = distance >= ignoreDistance;
    }

    private void FixedUpdate()
    {
        if (!active) return;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        if (distanceToPlayer >= 30f) return;

        if ((transform.position + Vector3.up * 4).RaycastFromPosition(directionToPlayer, out RaycastHit rch))
        {
            if (rch.transform.CompareTag("Player"))
            {
                if (!spoken)
                {
                    int num = Random.Range(0, aud_Taunts.Length);
                    audioDevice.PlayOneShot(aud_Taunts[num]);
                    spoken = true;
                }
            }
            guilt = 10f;
        }
    }
    #endregion

    #region Activation
    private IEnumerator WaitAndActivate()
    {
        while (true)
        {
            waitTime = Random.Range(60f, 120f);
            yield return new WaitForSeconds(waitTime);

            if (!active)
            {
                Activate();
            }
        }
    }

    private void Activate()
    {
        transform.position = wanderer.SetNewTargetForAgent(null, "hall") + Vector3.up * 5f;

        while ((transform.position - player.position).magnitude < 20f)
        {
            transform.position = wanderer.SetNewTargetForAgent(null, "hall") + Vector3.up * 5f;
        }

        active = true;
    }

    private void Reset()
    {
        transform.position += Vector3.down * 20f;
        active = false;
        activeTime = 0f;
        spoken = false;
    }
    #endregion

    #region Player Interaction
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePlayerPush(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "Principal of the Thing" && guilt > 0f)
        {
            Reset();
        }
    }

    private void HandlePlayerPush(Collider playerCollider)
    {
        PlayerScript playerScript = playerCollider.GetComponent<PlayerScript>();
        if (playerScript.bootsActive && ItemManager.Instance.HasNoItems()) return;

        bool hasItems = false;

        for (int i = 0; i < ItemManager.Instance.Inventory.Length; i++)
        {
            if (ItemManager.Instance.Inventory[i].ItemID != 0)
            {
                hasItems = true;
                break;
            }
        }

        if (!hasItems)
        {
            if (!audioDevice.isPlaying)
            {
                audioDevice.PlayOneShot(aud_Denied);
            }

            Vector3 pushDirection = (playerCollider.transform.position - transform.position).normalized;
            pushCoroutine = StartCoroutine(SmoothPushBack(playerCollider.transform, pushDirection, 16f, 16f / 32f));
        }
        else
        {
            int num = Mathf.RoundToInt(Random.Range(0, ItemManager.Instance.Inventory.Length));

            while (ItemManager.Instance.Inventory[num].ItemID == 0)
            {
                num = Mathf.RoundToInt(Random.Range(0, ItemManager.Instance.Inventory.Length));
            }

            if (ItemManager.Instance.Inventory[num].ItemInstance != null)
            {
                Destroy(ItemManager.Instance.Inventory[num].ItemInstance.gameObject);
            }

            ItemManager.Instance.ClearItem(num);
            ItemManager.Instance.UpdateItemUI();

            int num2 = Random.Range(0, aud_Thanks.Length);
            audioDevice.PlayOneShot(aud_Thanks[num2]);

            Reset();
        }
    }

    public void StopPushingPlayer()
    {
        if (pushCoroutine != null)
        {
            StopCoroutine(pushCoroutine);
            pushCoroutine = null;
        }
    }

    private IEnumerator SmoothPushBack(Transform playerTransform, Vector3 pushDirection, float pushDistance, float duration)
    {
        pushDirection.y = 0f;
        pushDirection.Normalize();

        Vector3 startPosition = playerTransform.position;
        Vector3 targetPosition = startPosition + pushDirection * pushDistance;
        float elapsedTime = 0f;

        if (Physics.Raycast(startPosition, pushDirection, out RaycastHit hit, pushDistance))
        {
            targetPosition = hit.point - pushDirection * 0.1f;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            playerTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        playerTransform.position = targetPosition;
    }
    #endregion

    #region Serialized Field States
    [Header("Player and Movement")]
    [SerializeField] private Transform player;
    [SerializeField] private AILocationSelectorScript wanderer;

    [Header("Timings")]
    [SerializeField] private float waitTime;
    [SerializeField] private float activeTime;

    [Header("Behavior Flags")]
    [SerializeField] private bool active;
    [SerializeField] private bool spoken;

    [Header("Activation and Guilt")]
    [SerializeField] private UnityEngine.AI.NavMeshObstacle Obstacle;
    [SerializeField] private Transform Principal;
    [SerializeField] private float ignoreDistance;
    public float guilt;

    [Header("Audio")]
    [SerializeField] private AudioClip[] aud_Taunts = new AudioClip[2];
    [SerializeField] private AudioClip[] aud_Thanks = new AudioClip[2];
    [SerializeField] private AudioClip aud_Denied, aud_Bored;

    private AudioSource audioDevice;
    private Coroutine pushCoroutine;
    #endregion
}