using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 4f;

    private Player _player_one;
    private Player _player_two;

    private Animator _animator;

    [SerializeField]
    private AudioClip _explosionSoundClip;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;

    private GameManager _gameManager;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private bool _isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null) {
            Debug.Log("Game Manager is null");
        }

        bool checkCoop = _gameManager.checkCoop();

        Respawn();
        
        _player_one = GameObject.Find("Player_one").GetComponent<Player>();
        Debug.Log("Coop" + checkCoop);
        if (checkCoop) {
            _player_two = GameObject.Find("Player_two").GetComponent<Player>();
        } else {
            _player_two = null;
        }
        if (_player_one == null) {
            Debug.Log("Player is null");
        }
        if (_player_two == null) {
            Debug.Log("Player 2 is null");
        }
        _animator = this.GetComponent<Animator>();
        if (_animator == null) {
            Debug.Log("Animator is null");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) {
            Debug.LogError("Audio Source is null");
        } else {
            _audioSource.clip = _explosionSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // goes down by 4 per second
        transform.position += Vector3.down * _speed * Time.deltaTime;
        // after goes to the end of screen, spawn with random x 
        if (transform.position.y < -14.0f) {
            Respawn();
        }

        if (Time.time > _canFire && _isDestroyed == false) {
            _fireRate = UnityEngine.Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++) {
                lasers[i].AssignEnemyLaser();
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag.Equals("Laser")) {

            Laser laser = other.GetComponent<Laser>();
            if (laser.isEnemyLaser()) {

            } else {
                
                this._speed = 0;
                
                Destroy(other.gameObject);

                _animator.SetTrigger("OnEnemyDeath");

                _audioSource.Play();

                // _isDestroyed = true;

                Destroy(GetComponent<Collider2D>());

                Destroy(this.gameObject, 2.2f);


                if (_player_one != null) {
                    Debug.Log("Updating score for Player 1");
                    _player_one.addScore();
                }
                if (_player_two != null) {
                    Debug.Log("Updating score for Player 2");
                    _player_two.addScore();
                }
            }
        } 
        if (other.transform.tag.Equals("Player") || other.transform.tag.Equals("Player_two")) {
            
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.2f);

            Player player = other.GetComponent<Player>();

            if (player != null) {
                player.Damage();
            } else {
                Debug.Log("Can't find player");
            }
        }
    }

    

     private void Respawn()
    {
        float randomX = UnityEngine.Random.Range(-21.0f, 7.0f);
        transform.position = new Vector3(randomX, 7.0f, 0);
    }

    
}
