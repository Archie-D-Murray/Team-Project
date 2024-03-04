using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

namespace Audio {

    [Serializable] public enum SFXType { ENTITY_HIT, ENTITY_DEATH }

    [Serializable] public class SFX {
        public SFXType type;
        public AudioClip clip;
    }

    public class SFXEmitter : MonoBehaviour {
        [SerializeField] private SFX[] sfxList;
        private Dictionary<SFXType, AudioSource> sfxSources;

        private void Start() {
            AudioMixerGroup sfxGroup = SoundManager.instance.sfxMixer;
            sfxSources = new Dictionary<SFXType, AudioSource>(sfxList.Length);
            foreach (SFX sfx in sfxList) {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = sfxGroup;
                source.loop = false;
                source.playOnAwake = false;
                source.clip = sfx.clip;
                sfxSources.Add(sfx.type, source);
            }
        }

        public void Play(SFXType type, float delay = 0f) {
            if (sfxSources.TryGetValue(type, out AudioSource source)) {
                source.PlayDelayed(delay);
            }
        }
    }
}