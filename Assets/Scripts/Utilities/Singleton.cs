using UnityEngine;

namespace Utilities {

    public class Singleton<T> : MonoBehaviour where T : Component {
        protected static T internalInstance;
        public static bool hasInstance => internalInstance != null;
        public static T TryGetInstance() => hasInstance ? internalInstance : null;
        public static T current => internalInstance;

        public static T instance {
            get {
                if (internalInstance == null) {
                    internalInstance = FindFirstObjectByType<T>();
                    if (internalInstance == null) {
                        GameObject obj = new GameObject();
                        obj.name = $"{typeof(T).Name} - AutoCreated";
                        internalInstance = obj.AddComponent<T>();
                    }
                }

                return internalInstance;
            }
        }

        public void StartSingleton() {
            if (internalInstance == null) {
                internalInstance = FindFirstObjectByType<T>();
                if (internalInstance == null) {
                    GameObject obj = new GameObject();
                    obj.name = $"{typeof(T).Name} - AutoCreated";
                    internalInstance = obj.AddComponent<T>();
                }
            }
        }

        protected virtual void Awake() => InitialiseSingleton();

        protected virtual void InitialiseSingleton() {
            if (!Application.isPlaying) {
                return;
            }
            internalInstance = this as T;
        }
    }
}