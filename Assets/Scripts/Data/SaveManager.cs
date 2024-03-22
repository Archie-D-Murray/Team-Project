using Utilities;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using Entity.Player;
using System;

namespace Data {
    public class SaveManager : PersistentSingleton<SaveManager> {
        [SerializeField] private GameData data;

        [SerializeField] private List<ISerialize> serializeObjects;

        public Entity.Player.PlayerClass playerSpawnClass { get; private set; }

        [SerializeField] string path;

        private void Start() {
            serializeObjects = FindSerializeObjects();
            path = Path.Combine(Application.dataPath, "Game.json");
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => Save();
        }

        public void Save() {
            StartCoroutine(SaveIEnumerator());
        }

        IEnumerator SaveIEnumerator() {
            data = new GameData();
            data.sceneID = SceneManager.GetActiveScene().buildIndex;
            foreach (ISerialize serializeObject in serializeObjects) {
                serializeObject?.OnSerialize(ref data);
            }
            yield return Yielders.waitForEndOfFrame;
            string buffer = JsonUtility.ToJson(data, true);
            yield return Yielders.waitForEndOfFrame;
            File.WriteAllText(path, buffer);
        }

        IEnumerator LoadIEnumerator(bool switchScene = true) {
            string buffer = File.ReadAllText(path);
            yield return Yielders.waitForEndOfFrame;
            data = JsonUtility.FromJson<GameData>(buffer);
            if (switchScene) {
                SceneManager.LoadSceneAsync(data.sceneID);
            }
            foreach (ISerialize serializeObject in serializeObjects) {
                serializeObject?.OnDeserialize(data);
            }
        }

        public void Load() {
            StartCoroutine(LoadIEnumerator(true));
        }

        public void New() {
            data = new GameData();
            data.sceneID = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(SaveIEnumerator());
        }

        public void ContinueFromSave() {
            StartCoroutine(LoadIEnumerator(true));
        }
        

        ///<summary>
        ///Always loads Inventory, PlayerController, EnemyControllers then all Stat, Health and Mana components
        ///</summary>
        private List<ISerialize> FindSerializeObjects() {
            List<ISerialize> serializeObjects = FindObjectsOfType<MonoBehaviour>().Where((MonoBehaviour mono) => !mono.gameObject.HasComponent<EnemyScript>()).OfType<ISerialize>().ToList();

            ISerialize playerController = serializeObjects.OfType<Entity.Player.PlayerController>().FirstOrDefault();
            ISerialize inventory = serializeObjects.OfType<Items.Inventory>().FirstOrDefault();
            List<ISerialize> serializeObjectList = new List<ISerialize>();
            if (playerController != null && inventory != null) { // Need to ensure inventory is loaded before PlayerController as it relies on items to initialise!
                serializeObjectList.Add(inventory);
                serializeObjectList.Add(playerController);
                foreach (ISerialize obj in serializeObjects.Where((ISerialize obj) => obj != playerController && obj != inventory)) {
                    serializeObjectList.Add(obj);
                }
                return serializeObjectList;
            } else {
                return new List<ISerialize>(serializeObjects);
            }
        }

        public void SetPlayerClass(PlayerClass playerClass) {
            playerSpawnClass = playerClass;
        }

        public void DeleteSave() {
            if (!File.Exists(path)) {
                return;
            }
            File.Delete(path);
        }
    }
}