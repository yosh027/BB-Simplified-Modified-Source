using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioQueueScript : MonoBehaviour
{
    #region Initialization
    private void Start() => audioDevice = GetComponent<AudioSource>();
    #endregion

    #region PlaybackControl
    private void Update()
    {
        if (queuedAudios.Count > 0 && !audioDevice.isPlaying && Time.timeScale != 0f)
        {
            PlayNext();
        }
    }

    private void PlayNext()
    {
        audioDevice.PlayOneShot(queuedAudios[0]);
        queuedAudios.RemoveAt(0);
    }
    #endregion

    #region QueueManagement
    public void QueueAudio(AudioClip sound)
    {
        if (sound != null)
        {
            queuedAudios.Add(sound);
        }
    }

    public void ClearQueue() => queuedAudios.Clear();
    #endregion

    #region AudioEffects
    public IEnumerator FadeOut(AudioSource audioDevice, float duration)
    {
        float startVolume = audioDevice.volume;
        while (audioDevice.volume > 0)
        {
            audioDevice.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }
        audioDevice.Stop();
        audioDevice.volume = startVolume;
        yield break;
    }
    #endregion

    #region References
    private AudioSource audioDevice;
    [HideInInspector] public List<AudioClip> queuedAudios = new List<AudioClip>();
    #endregion
}