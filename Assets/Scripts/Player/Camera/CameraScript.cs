using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private void Start() => offset = transform.position - player.transform.position;

    private void Update()
    {
        if (ps.jumpRope)
        {
            velocity -= gravity * Time.deltaTime;
            jumpHeight += 2.7f * velocity * Time.deltaTime;
            if (jumpHeight <= 0f)
            {
                jumpHeight = 0f;
                if (Singleton<InputManager>.Instance.GetActionKey(InputAction.Jump))
                {
                    velocity = initVelocity;
                }
            }
            jumpHeightV3 = new Vector3(0f, jumpHeight, 0f);
        }
        if (!ps.gc.KF.gamePaused)
        {
            lookBehind = Singleton<InputManager>.Instance.GetActionKey(InputAction.LookBehind) ? 180 : 0;
        }
    }

    private void LateUpdate()
    {
        if (!ps.gameOver)
        {
            transform.SetPositionAndRotation(player.transform.position + offset + (ps.jumpRope ? jumpHeightV3 : Vector3.zero), player.transform.rotation * Quaternion.Euler(0f, lookBehind, 0f));
        }
        else
        {
            transform.position = baldi.position + baldi.forward * 2f + Vector3.up * GameOverOffset;
            transform.LookAt(baldi.position + Vector3.up * 5f);
        }
    }

    [Header("Player References")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerScript ps;

    [Header("Camera Settings")]
    [SerializeField] private float GameOverOffset;
    public Vector3 offset;

    [Header("Jump Rope Physics")]
    [SerializeField] private float initVelocity, velocity, gravity;
    public float jumpHeight;

    [Header("Baldi References")]
    [SerializeField] private Transform baldi;
    
    private int lookBehind;
    private Vector3 jumpHeightV3;
}