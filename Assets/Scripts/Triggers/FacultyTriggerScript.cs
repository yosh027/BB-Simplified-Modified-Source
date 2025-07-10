using UnityEngine;

public class FacultyTriggerScript : MonoBehaviour
{
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			ps.ResetGuilt("faculty", 1f);
		}
	}

	[Header("References")]
	[SerializeField] private PlayerScript ps;
}