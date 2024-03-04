using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

using Utilities;

namespace Audio {

    [Serializable] public enum BGMType { NONE, MAIN_MENU, PASSIVE, COMBAT }

    [Serializable] public class BGM {
        public BGMType type;
        public AudioClip clip;
    }

    public class BGMEmitter : MonoBehaviour {
        [SerializeField] private BGMType current;
        [SerializeField] private BGMType target;
        [SerializeField] private AudioMixerGroup bgmGroup;
        [SerializeField] private BGM[] bgmList;
        [SerializeField] private BGMType startBGM = BGMType.NONE;

        private Dictionary<BGMType, AudioSource> bgmSources;
        private Coroutine mix;

        public void Start() {
            current = BGMType.NONE;
            bgmSources = new Dictionary<BGMType, AudioSource>(bgmList.Length);
            foreach (BGM bgm in bgmList) {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = true;
                source.clip = bgm.clip;
                source.outputAudioMixerGroup = SoundManager.instance.bgmMixer;
                bgmSources.Add(bgm.type, source);
            }
            if (startBGM != BGMType.NONE) {
                PlayBGM(startBGM);
            }
        }

        public void PlayBGM(BGMType type, float mixDuration = 0.5f) {
            if (type == current || type == BGMType.NONE) {
                return;
            }
            if (mix != null) {
                StopCoroutine(mix);
            }
            mix = StartCoroutine(MixBGM(type, mixDuration));
        }

        IEnumerator MixBGM(BGMType type, float duration) {
            target = type;
            CountDownTimer fadeTimer = new CountDownTimer(duration);
            bgmSources.TryGetValue(current, out AudioSource currentSource);
            bgmSources.TryGetValue(target, out AudioSource targetSource);
            targetSource.Play();
            if (currentSource) {
                while (fadeTimer.isRunning) {
                    fadeTimer.Update(Time.fixedDeltaTime);
                    currentSource.volume = 1f - fadeTimer.Progress();
                    targetSource.volume = fadeTimer.Progress();
                    yield return Yielders.waitForFixedUpdate;
                } 
            } else {
                while (fadeTimer.isRunning) {
                    targetSource.volume = fadeTimer.Progress();
                    yield return Yielders.waitForFixedUpdate;
                    fadeTimer.Update(Time.fixedDeltaTime);
                }
                currentSource.Stop();
                currentSource.volume = 1f;
            }
            current = target;
        }
    }
}