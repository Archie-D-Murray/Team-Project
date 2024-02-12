using System;

using UnityEngine;

namespace Utilities {
    public abstract class Timer {
        [SerializeField] protected float _initialTime;
        [SerializeField] protected float _time;

        public bool isRunning { get; protected set; }
        public virtual float Progress() => _time / _initialTime;
        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected Timer(float initialTime) {
            _initialTime = initialTime;
            isRunning = false;
        }

        public void Start() {
            _time = _initialTime;
            if (!isRunning) {
                isRunning = true;
                OnTimerStart.Invoke();
            }
        }

        public void Stop() {
            if (isRunning) {
                isRunning = false;
                OnTimerStop.Invoke();
            }
        }

        public void Finish() {
            isRunning = false;
            _time = _initialTime;
            OnTimerStop?.Invoke();
        }

        public void Resume() => isRunning = true;
        public void Pause() => isRunning = false;
        public abstract void Update(float deltaTime);
    }

    [Serializable]
    public class CountDownTimer : Timer {
        public CountDownTimer(float initialTime, bool startRunning = false) : base(initialTime, startRunning) { }

        public override void Update(float deltaTime) {
            if (isRunning && _time > 0f) {
                _time = Mathf.Max(_time - deltaTime, 0f);
            }

            if (isRunning && _time == 0f) {
                Stop();
            }
        }
        
        /// <summary>
        /// Overriden as returning time / initial causes progress to go from 100% => 0%
        /// </summary>
        /// <returns></returns>
        public override float Progress() {
            return 1f - base.Progress();
        }

        public bool isFinished => _time == 0f;
        public void Reset() => _time = _initialTime;
        public void Reset(float newTime, bool fireEvents = false) {
            _initialTime = newTime;
            Reset();
            if (fireEvents) {
                OnTimerStart.Invoke();
            }
        }

        public void Restart(float newTime) {
            _initialTime = newTime;
            Reset();
            Resume();
        }

        public void Restart(float newTime) {
            _initialTime = newTime;
            _time = 0f;
            isRunning = true;
        }
    }

    [Serializable]
    public class StopwatchTimer : Timer {
        public StopwatchTimer(bool startRunning = false) : base(0f, startRunning) { }

        public override void Update(float deltaTime) {
            if (isRunning) {
                _time += deltaTime;
            }
        }

        public void Reset() {
            _time = 0f;
        }

        public float GetTime() => _time;
    }
}