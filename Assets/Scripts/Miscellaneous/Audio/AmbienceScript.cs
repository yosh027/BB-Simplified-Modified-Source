using UnityEngine;

public class AmbienceScript : MonoBehaviour
{
    #region AmbiencePlaybackLogic
    public void PlayAudio()
    {
        int num = Mathf.RoundToInt(Random.Range(0f, 49f)); 
        if (!audioDevice.isPlaying & num == 0)
        {
            transform.position = aiLocation.position;
            int num2 = Mathf.RoundToInt(Random.Range(0f, sounds.Length - 1));
            audioDevice.PlayOneShot(sounds[num2]);
        }
    }
    #endregion

    #region SerializedConfiguration
    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private AudioSource audioDevice;

    [Header("Location Settings")]
	[SerializeField] private Transform aiLocation;
    #endregion
}