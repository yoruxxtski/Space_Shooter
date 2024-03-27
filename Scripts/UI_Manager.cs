using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    // Handle to text
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private TextMeshProUGUI _bestScoreText;

    [SerializeField]
    private TextMeshProUGUI _scoreText_two;

    [SerializeField]
    private TextMeshProUGUI _gameOverText;

    [SerializeField]
    private TextMeshProUGUI _restartText;

    [SerializeField]
    private TextMeshProUGUI _gameStartText;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Sprite[] _liveSprites_2;
    
    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private Image _LivesImg_2;

    private GameManager _gameManager;

    private int life_1;
    private int life_2;

    private Player player;

    private SpawnManager _spawnManager;

    

    // Start is called before the first frame update
    void Start()
    {
        life_1 = 3;
        life_2 = 3;
        _scoreText.text = "Score : " + 0;
        _bestScoreText.text = "Best : " + PlayerPrefs.GetInt("HighestScore", 0) ;
        _gameOverText.text = "Game Over";
        _restartText.text = "Press the R key to restart the level";
        _gameStartText.text = "Fire the asteroid for the game to start";

        _gameStartText.gameObject.SetActive(true);

        _gameOverText.gameObject.SetActive(false);

        _restartText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null) {
            Debug.Log("GameManager is null");
        }
        player = GameObject.Find("Player_one").GetComponent<Player>();
        if (player == null) {
            Debug.Log("player is null");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) {
            Debug.Log("The Spawn Manager is null");
        }
    }

    // Update is called once per frame
    public void UpdateScore(int score) {
        _scoreText.text = "Score : " + score.ToString();
    }

    public void UpdateScore_2(int score) {
        _scoreText_two.text = "Score : " + score.ToString();
    }

    public void updateBestScore(int score) {
        _bestScoreText.text = "Best : " + score; 
    }

    public void updateLives(int currentLives) {
        _LivesImg.sprite = _liveSprites[currentLives];
        life_1 = currentLives;
        checkGameover();
    }

    public void updateLives_2(int currentLives) {
        _LivesImg_2.sprite = _liveSprites_2[currentLives];
        life_2 = currentLives;
        checkGameover();
    }

    public void checkGameover() {
        if (_gameManager.checkCoop()) {
            if (life_1 == 0 && life_2 == 0) {
                _spawnManager.onPlayerDeath();
                GameOverSequence();
            }
        } else {
            if (life_1 == 0) {
                GameOverSequence();
            }
        }
    }

    public void GameOverSequence() {
        player.bestScore();
        StartCoroutine(flicker());
         _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    public void gameStartOff() {
        _gameStartText.gameObject.SetActive(false);
    }
    IEnumerator flicker() {
        while(true) {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
