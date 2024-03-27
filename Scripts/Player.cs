using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laserPrefab;

    private float _speed = 3.5f;

    private float _speedMul = 2;
    
    
    [SerializeField]
    private float _fireRate = 0.5f;

    private float _nextFire = 0.0f;

    [SerializeField]
    private int _health = 3;

    private SpawnManager _spawnManager;

    // variable for isTripleShotActive
    [SerializeField]
    private GameObject _tripleLaserPrefab;

    [SerializeField]
    private bool _isTripleShotActive = false;

    private bool _isSpeedActive = false;

    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private int _score_one;

    [SerializeField]
    private int _score_two;

    private UI_Manager _uiManager;

    [SerializeField]
    private GameObject _speedVisualizer;

    private Coroutine _isTripleShotCoroutine;
    private Coroutine _isSpeedCoroutine;

    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private AudioClip _laserSound;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private int _playerID;
    private int _bestScore;

    // Start is called before the first frame update
    void Start()
    {
        // take current position as the start position
        transform.position = new Vector3(-3.17f, -7.64f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) {
            Debug.Log("The Spawn Manager is null");
        }
        _shieldVisualizer.SetActive(false);
        _score_one = 0;
        _score_two = 0;
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _speedVisualizer.SetActive(false);
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) {
            Debug.LogError("Audio Source is null");
        } else {
            _audioSource.clip = _laserSound;
        }
        _bestScore = 0;
    }

    // Update is called once per frame 
    void Update()
    {
        calculateMovement();
        if (_playerID == 1) {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire) {
                FireLaser();
            }
        } else if (_playerID == 2) {
            if (Input.GetKeyDown(KeyCode.R) && Time.time > _nextFire) {
                FireLaser();
            }
        }
    }

    // for co-op 
    void calculateMovement() {
        if(_playerID == 1) {
            one_player_movement();
        } else if (_playerID == 2) {
            co_op_movement();
        }
    }

    void one_player_movement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // a or d will turn float to -1 or 1 
        // transform.position += new Vector3(1, 0, 0) * _speed * horizontalInput * Time.deltaTime;
        // transform.position += new Vector3(0, 1, 0) * _speed * verticalInput * Time.deltaTime;
        Vector3 direction;
        if (!_isSpeedActive) {
            direction = new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime;
            _speedVisualizer.SetActive(false);
        } else {
            direction = new Vector3(horizontalInput, verticalInput, 0) * _speed * _speedMul * Time.deltaTime;
            _speedVisualizer.SetActive(true);
        }
        transform.position += direction;

        // Player bounds
        if (transform.position.x >= 12) {
            transform.position = new Vector3(-22.3f, transform.position.y, transform.position.z);
        }

        if (transform.position.x < -22.7) {
            transform.position = new Vector3(12, transform.position.y, transform.position.z );
        }
    }

    void co_op_movement() {
        Vector3 direction = Vector3.zero;
         if (!_isSpeedActive) {

            if (Input.GetKey(KeyCode.J)) {
                direction += new Vector3(-1 ,0 , 0) * _speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.K)) {
                direction += new Vector3(0 , -1 , 0) * _speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.L)) {
                direction += new Vector3(1 , 0 , 0) * _speed * Time.deltaTime;
            } 
            if (Input.GetKey(KeyCode.I)) {
                direction += new Vector3(0  ,1  , 0) * _speed * Time.deltaTime;
            }

            _speedVisualizer.SetActive(false);
        } else {
            if (Input.GetKey(KeyCode.J)) {
                direction += new Vector3(-1 ,0 , 0) * _speed * _speedMul * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.K)) {
                direction += new Vector3(0 ,-1 , 0) * _speed * _speedMul * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.L)) {
                direction += new Vector3(1 ,0 , 0) * _speed * _speedMul * Time.deltaTime;
            } 
            if (Input.GetKey(KeyCode.I)) {
                direction += new Vector3(0 ,1 , 0) * _speed * _speedMul * Time.deltaTime;
            }
            _speedVisualizer.SetActive(true);
        }
        transform.position += direction;
        // Player bounds
        if (transform.position.x >= 12) {
            transform.position = new Vector3(-22.3f, transform.position.y, transform.position.z);
        }

        if (transform.position.x < -22.7) {
            transform.position = new Vector3(12, transform.position.y, transform.position.z );
        }
    }

    
    // for co_op
    void FireLaser() {
        // Quaternion.identity = default rotation
        _nextFire = Time.time + _fireRate;
        if (!_isTripleShotActive) {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.51f, 0) , Quaternion.identity);
        } else {
            Instantiate(_tripleLaserPrefab, transform.position , Quaternion.identity);
        }
        // play audio clip
        _audioSource.Play();
    }

    public void Damage() {
        if (_isShieldActive) {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _health --;

        if (_health == 2) {
            _rightEngine.SetActive(true);
        }

        if (_health == 1) {
            _leftEngine.SetActive(true);
        }
        if (_playerID == 1) {
             _uiManager.updateLives(_health);
        } else if (_playerID == 2) {
             _uiManager.updateLives_2(_health);
        }
        if (_health < 1) {
            Destroy(this.gameObject);
        }
    }

    public void setTripleShot() {
    _isTripleShotActive = true;
    
    // Stop the existing coroutine if it's already running
    if (_isTripleShotCoroutine != null) {
        StopCoroutine(_isTripleShotCoroutine);
    }
    
    // Start a new coroutine
    _isTripleShotCoroutine = StartCoroutine(TripleShotPowerDownRoutine());
}

    IEnumerator TripleShotPowerDownRoutine() {
    yield return new WaitForSeconds(5.0f);
    _isTripleShotActive = false;
    
    // Reset the coroutine reference
    _isTripleShotCoroutine = null;
}
    

    public void setSpeedBoost() {
        _isSpeedActive = true;
        if (_isSpeedCoroutine != null) {
            StopCoroutine(_isSpeedCoroutine);
        }
        _isSpeedCoroutine = StartCoroutine(SpeedBoostRoutine());
    }
    IEnumerator SpeedBoostRoutine() {
        yield return new WaitForSeconds(5.0f);
        _isSpeedActive = false;
        _isSpeedCoroutine = null;
    }

    public void getShieldBoost() {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
    public void addScore() {
        Debug.Log("Player : " + _playerID);
        if (_playerID == 1) {
            _score_one += 10;
            _uiManager.UpdateScore(_score_one);
        } else if (_playerID == 2) {
            _score_two += 10;
            _uiManager.UpdateScore_2(_score_two);
        }
    }

    public void bestScore() {
        if (_bestScore >= _score_one) {
            _bestScore = _score_one;
            _uiManager.updateBestScore(_bestScore);
            PlayerPrefs.SetInt("HighestScore", _bestScore);
        }
    }
}
