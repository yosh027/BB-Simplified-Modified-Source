using UnityEngine;

public class NearExitTriggerScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (gc.exitsReached < 3 & gc.finaleMode & other.CompareTag("Player"))
		{
			gc.ExitReached();
			es.Enable();
			if (gc.baldiScrpt.isActiveAndEnabled)
			{
				gc.baldiScrpt.Hear(transform.position, 8f);
			}
			gameObject.SetActive(false);
		}
	}

	[Header("References")]
	[SerializeField] public GameControllerScript gc;
    [SerializeField] public EntranceScript es;
	
}
