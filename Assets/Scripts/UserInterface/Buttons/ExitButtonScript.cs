using UnityEngine;
using System.Collections;

public class ExitButtonScript : MonoBehaviour
{
	public void ExitGame()
	{
		BaldiSource.Stop();
		BaldiSource.PlayOneShot(aud_Thanks);
		Cursor.LockCursor();
		StartCoroutine(WaitForAudio());
	}

	private IEnumerator WaitForAudio()
	{
		while (BaldiSource.isPlaying)
		{
			yield return null;
		}
		Application.Quit();
		yield break;
	}

	[Header("References")]
	[SerializeField] private CursorControllerScript Cursor;
    [SerializeField] private AudioSource BaldiSource;
	[SerializeField] private AudioClip aud_Thanks;
}