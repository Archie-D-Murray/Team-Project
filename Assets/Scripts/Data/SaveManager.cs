using Utilities;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;

namespace Data {
    public class SaveManager : Singleton<SaveManager> {
        [SerializeField] private GameData data;

        [SerializeField] private List<ISerialize> serializeObjects;

        [SerializeField] string path;

        private void Start() {
            serializeObjects = FindSerializeObjects();
            path = Path.Combine(Application.dataPath, "Game.json");
        }

        public void Save() {
            StartCoroutine(SaveIEnumerator());
        }

        IEnumerator SaveIEnumerator() {
            data = new GameData();
            foreach (ISerialize serializeObject in serializeObjects) {
                serializeObject.OnSerialize(ref data);
            }
            yield return Yielders.waitForEndOfFrame;
            string buffer = JsonUtility.ToJson(data, true);
            yield return Yielders.waitForEndOfFrame;
            File.WriteAllText(path, buffer);
        }

        public void Load() {
            data = JsonUtility.FromJson<GameData>(File.ReadAllText(path));
            foreach (ISerialize serializeObject in serializeObjects) {
                serializeObject.OnDeserialize(data);
            }
        }
        

        ///<summary>
        ///Always loads Inventory, PlayerController, EnemyControllers then all Stat, Health and Mana components
        ///</summary>
        private List<ISerialize> FindSerializeObjects() {
            IEnumerable<ISerialize> serializeObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISerialize>();

            ISerialize playerController = serializeObjects.OfType<Entity.Player.PlayerController>().FirstOrDefault();
            ISerialize inventory = serializeObjects.OfType<Items.Inventory>().FirstOrDefault();
            IEnumerable<ISerialize> enemies = serializeObjects.OfType<EnemyScript>();
            List<ISerialize> serializeObjectList = new List<ISerialize>();
            if (playerController != null && inventory != null && enemies != null) { // Need to ensure inventory is loaded before PlayerController as it relies on items to initialise!
                serializeObjectList.Add(inventory);
                serializeObjectList.Add(playerController);
                foreach (ISerialize enemy in enemies) {
                    serializeObjectList.Add(enemy);
                }
                foreach (ISerialize obj in serializeObjects.Where((ISerialize obj) => obj != playerController && obj != inventory && !enemies.Contains(obj))) {
                    serializeObjectList.Add(obj);
                }
                return serializeObjectList;
            } else {
                return new List<ISerialize>(serializeObjects);
            }
        }
    }
}