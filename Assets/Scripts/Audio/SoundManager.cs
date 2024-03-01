using UnityEngine.Audio;

using Utilities;

namespace Audio {
    public class SoundManager : PersistentSingleton<SoundManager> {
        public BGM[] bgmList;
        public SFX[] sfxList;
        public AudioMixer mixer;
        public AudioMixerGroup sfxMixer;
        public AudioMixerGroup bgmMixer;
    }
}