using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{   
    [SerializeField]
    private bool _isGameOver = false;

    private bool _isCoop = false;

    [SerializeField]
    private GameObject _pauseMenu;

    // private void Start() {
    //     _pauseMenu.SetActive(false);
    // }

    private void Update() {
        // if R key is pressed restart current scene
        // scene 1 = Single Player
        // scene 2 = Co op
        _isCoop = PlayerPrefs.GetInt("Coop", 0) == 0 ? false : true;

        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true && _isCoop == false) {
            SceneManager.LoadScene(1);

        } else if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true && _isCoop == true) {
            SceneManager.LoadScene(2);
        }

        // if escape key is pressed quit application
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        // if P key is pressed then pause the game
        if (Input.GetKeyDown(KeyCode.P)) {
            PauseGame();
        }
    }
    public void GameOver() {
        _isGameOver = true;
    }

    public void isCoop() {
        _isCoop = true;
    }

    public void notCoop() {
        _isCoop = false;
    }

    public bool checkCoop() {
        return _isCoop;
    }

    public void PauseGame() {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
        
        Time.timeScale = 1;
    }
}
