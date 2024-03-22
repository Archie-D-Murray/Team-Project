using System;
using System.Collections;

using Data;

using Entity;
using Entity.Enemy;
using Entity.Player;

using UnityEngine;
using UnityEngine.SceneManagement;

using Utilities;

public class GameManager : Singleton<GameManager> {
    public EnemyManager enemyManager;
    public GameObject levelDoor;
    public Fading screenFader;
    public Action onLevelClear = delegate { };
    public BossState bossState;

    private Coroutine levelLoad = null;
    [SerializeField] private GameObject playerPrefab;

    private void Start() {
        levelDoor = FindFirstObjectByType<LevelDoor>(FindObjectsInactive.Include).OrNull()?.gameObject;
        if (levelDoor) {
            onLevelClear += () => levelDoor.SetActive(true);
        } else {
            Debug.LogError("Could not find level door! Either GameManager should not be present, or there should be a LevelDoor in the scene!");
        }
        screenFader = FindFirstObjectByType<Fading>();
        screenFader.Fade(Color.clear);
        InitPlayer();
    }

    private void InitPlayer() {
        Debug.Log(SaveManager.instance.playerSpawnClass);
        FindFirstObjectByType<PlayerController>().DebugInitialise(SaveManager.instance.playerSpawnClass);
        Debug.Log("Inited player");
    }

    public void LoadNextLevel() {
        levelLoad ??= StartCoroutine(LoadNext());
    }

    public void RegisterBossSpawn() {
        bossState = BossState.ALIVE;
    }

    public void RegisterBossDeath() {
        bossState = BossState.DEAD;
        enemyManager.CheckLevelClear();
    }

    private IEnumerator LoadNext() {
        screenFader.Fade(Color.black);
        while (!screenFader.isFinished) {
            yield return Yielders.waitForEndOfFrame;
        }
        // NOTE: We don't need to worry about this overflowing into non existent level as final level in
        // build should be a credits scene otherwise put a check in and just load mainmenu scene (0)!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return Yielders.waitForEndOfFrame;
    }
}