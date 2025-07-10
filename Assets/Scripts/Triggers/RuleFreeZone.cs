using UnityEngine;

public class RuleFreeZone : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outside = true;
        }
    }

    public void Update()
    {
        if ((transform.position - player.transform.position).magnitude >= 180f)
        {
            outside = false;
        }
        if (outside)
        {
            if (player.stamina <= 100f)
            { 
                player.stamina += player.staminaRise * Time.fixedDeltaTime;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outside = false;
        }
    }

    [Header("References")]
    [SerializeField] private PlayerScript player;

    [Header("Zone State")]
    [SerializeField] private bool outside;
}