using UnityEngine;

namespace Utilities {

    public class PersistentSingleton<T> : MonoBehaviour where T : Component {
        [Tooltip("If this is true, this singleton will auto detach if it finds itself parented on awake")]
        public bool unparentOnAwake = true;

        public static bool hasInstance => internalInstance != null;
        public static T current => internalInstance;

        protected static T internalInstance;

        public static T instance {
            get {
                if (internalInstance == null) {
                    internalInstance = FindFirstObjectByType<T>();
                    if (internalInstance == null) {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name + "AutoCreated";
                        internalInstance = obj.AddComponent<T>();
                    }
                }

                return internalInstance;
            }
        }

        public static void StartSingleton() {
            if (internalInstance == null) {
                internalInstance = FindFirstObjectByType<T>();
                if (internalInstance == null) {
                    GameObject obj = new GameObject();
                    obj.name = $"{typeof(T).Name} - AutoCreated";
                    internalInstance = obj.AddComponent<T>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
        }

        protected virtual void Awake() => InitializeSingleton();

        protected virtual void InitializeSingleton() {
            if (!Application.isPlaying) {
                return;
            }

            if (unparentOnAwake) {
                transform.SetParent(null);
            }

            if (internalInstance == null) {
                internalInstance = this as T;
                DontDestroyOnLoad(transform.gameObject);
                enabled = true;
            } else {
                if (this != internalInstance) {
                    Destroy(this.gameObject);
                }
            }
        }
    } 
}