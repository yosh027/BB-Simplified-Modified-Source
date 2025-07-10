using UnityEngine;

public class NeedMoreScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (gc.notebooks < gc.UnlockAmount & other.CompareTag("Player"))
		{
			if (!audioDevice.isPlaying)
			{
                audioDevice.PlayOneShot(baldiDoor, 1f);
			}
		}
	}

	[Header("Game Controller")]
	[SerializeField] private GameControllerScript gc;

	[Header("Audio")]
	[SerializeField] private AudioSource audioDevice;
    [SerializeField] private AudioClip baldiDoor;
}