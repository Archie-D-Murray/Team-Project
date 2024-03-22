using System.Collections;
using System.Collections.Generic;

using Data;

using Entity.Player;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] mainPage;
    [SerializeField] private GameObject[] classPage;

    private void Start() {
        GoMainPage();    
    }

    public void GoClassPage() {
        foreach(GameObject button in mainPage) {
            button.SetActive(false);
        }
        foreach(GameObject button in classPage) {
            button.SetActive(true);
        }
    }

    public void GoMainPage() {
        foreach (GameObject button in mainPage) {
            button.SetActive(true);
        }
        foreach (GameObject button in classPage) {
            button.SetActive(false);
        }
    }

    public void StartGame(int playerClass) {
        //singleton stuff idk lol
        SaveManager.instance.SetPlayerClass((PlayerClass) playerClass);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit() {
        Application.Quit();
    }
}