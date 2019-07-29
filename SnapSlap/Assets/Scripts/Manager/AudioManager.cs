using UnityEngine;

using MyBox;

public class AudioManager : Singleton<AudioManager> {

    [System.Serializable]
    public struct SFXAudioClip {
        [SerializeField, Tooltip("The type of audio"), SearchableEnum]
        private AudioType audioType;

        [SerializeField, Tooltip("The audio clip for the audio type"), MustBeAssigned]
        private AudioClip audioClip;

        public AudioType AudioType {
            get => audioType;
        }

        public AudioClip AudioClip {
            get => audioClip;
        }
    }

    [SerializeField, Tooltip("The respective audio clips for the sound effects"), MustBeAssigned]
    private SFXAudioClip[] sfxAudioClips;

    [SerializeField, Tooltip("The respective audio sources"), MustBeAssigned]
    private AudioSource bgmAudioSource, sfxAudioSource;

    public void PlayBGMAudioSource() {
        if (!bgmAudioSource.isPlaying) {
            bgmAudioSource.Play();
        }
    }

    public void StopBGMAudioSource() {
        if (bgmAudioSource.isPlaying) {
            bgmAudioSource.Stop();
        }
    }

    public void PlaySFXByAudioType(AudioType audioType) {
        AudioClip audioClipToPlay = null;

        FindAudioClipToPlayByAudioType();

        if (audioClipToPlay != null) {
            sfxAudioSource.PlayOneShot(audioClipToPlay);
        } else {
            Debug.LogWarning("Tried to play SFX audio of type " + audioType.ToString() + " but the audio clip did not exists!");
        }

        #region Local_Function

        void FindAudioClipToPlayByAudioType() {
            foreach (var sfxAudioClip in sfxAudioClips) {
                if (audioType == sfxAudioClip.AudioType) {
                    audioClipToPlay = sfxAudioClip.AudioClip;
                    break;
                }
            }
        }

        #endregion
    }
}
