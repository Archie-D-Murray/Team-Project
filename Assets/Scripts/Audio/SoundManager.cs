using UnityEngine.Audio;

using Utilities;

namespace Audio {
    public class SoundManager : Singleton<SoundManager> {
        public AudioMixer mixer;
        public AudioMixerGroup sfxMixer;
        public AudioMixerGroup bgmMixer;

        public void SetBGMVol(float volume) {
            mixer.SetFloat("BGM", volume);
        }
    }
}