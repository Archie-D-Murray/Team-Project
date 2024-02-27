using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
        
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {
        [SerializeField] private List<TKey> keys;
        [SerializeField] private List<TValue> values;

        public void OnBeforeSerialize() {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this) {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize() {
            this.Clear();
            if (keys.Count != values.Count) {
                Debug.LogError("SerializeDictionary got inconsistent numbers of keys to values, file data is most likely bad!");
                return;
            }
            for (int i = 0; i < keys.Count; i++) {
                this.Add(keys[i], values[i]);
            }
        }
    }
}