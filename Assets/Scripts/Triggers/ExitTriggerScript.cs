using UnityEngine;

public class ExitTriggerScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (gc.notebooks >= gc.maxNotebooks & other.gameObject.CompareTag("Player"))
		{
			if (gc.failedNotebooks >= gc.maxNotebooks)
			{
				em.LoadSecretEnding();
			}
			else
			{
				em.LoadNormalResults();
			}
		}
	}

	[Header("Game Controller")]
	[SerializeField] private GameControllerScript gc;
	[SerializeField] private EndingManager em;
}
