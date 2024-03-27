using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20.0f;

    private float _speed = 1.0f;

    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;

    private UI_Manager _uiManager;


    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) {
            Debug.Log("Spawn Manager is not found");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        if (_uiManager == null) {
            Debug.Log("UI manager is not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag.Equals("Laser")) {
            
            this._speed = 0;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            // start spawning
            _uiManager.gameStartOff();
            _spawnManager.startSpawning();

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        if (other.transform.tag.Equals("Player")) {
            Player player = other.GetComponent<Player>();
            player.Damage();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy (this.gameObject);
        }
    }
}
