using System.IO;

using Data;

using Entity.Player;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    [SerializeField] private Button playButton;
    [SerializeField] private TMP_Text playButtonText;
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup classMenu;

    private void Start() {
        if (CheckForExistingSave()) {
            playButtonText.text = "Continue";
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(ContinueFromSave);
        } else {
            playButtonText.text = "Play";
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(GoClassPage);
        }
        mainMenu.FadeCanvas(0.1f, false, this);
    }

    public void ContinueFromSave() {
        SaveManager.instance.Load();
    }

    public void GoClassPage() {
        mainMenu.FadeCanvas(0.1f, true, this);
        classMenu.FadeCanvas(0.1f, false, this);
    }

    private bool CheckForExistingSave() {
        return File.Exists(SaveManager.instance.GetPath());
    }

    public void GoMainPage() {
        mainMenu.FadeCanvas(0.1f, false, this);
        classMenu.FadeCanvas(0.1f, true, this);
    }

    public void StartGame(int playerClass) {
        SaveManager.instance.SetPlayerClass((PlayerClass)playerClass);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit() {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}