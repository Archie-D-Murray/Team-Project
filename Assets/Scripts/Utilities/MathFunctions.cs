using UnityEngine;

namespace Utilities {

    public static class MathFunctions {
        public static float Random(float min, float max, float step) {
            if (step == 0.0f) {
                return UnityEngine.Random.Range(min, max);
            } else {
                return Mathf.Ceil(UnityEngine.Random.Range(min, max) / step) * step;
            }
        }   
    }
}