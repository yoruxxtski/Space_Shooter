using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject _asteroidPrefab;


    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void startSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Enemy spawn routine
    IEnumerator SpawnEnemyRoutine() 
    {
        yield return new WaitForSeconds(2.0f);
        // infinite loop
        // Instantiate enemey prefab
        // yield for 5 seconds
        while(_stopSpawning == false) {
            Vector3 SpawnPlace = new Vector3(UnityEngine.Random.Range(-21f, 10f), 0, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, SpawnPlace, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(4.0f);
        }
    }

    IEnumerator SpawnPowerUpRoutine() {
        yield return new WaitForSeconds(4.0f);
        while(_stopSpawning == false) {
        Vector3 SpawnPlace = new Vector3(UnityEngine.Random.Range(-21f, 10f), 0, 0);
        int randomPowerUp = UnityEngine.Random.Range(0, 2);

        switch(randomPowerUp) {
            case 0:
                GameObject speedPowerup = Instantiate(powerups[0], SpawnPlace, Quaternion.identity);
                break;
            case 1:
                GameObject tripleShotPowerup = Instantiate(powerups[1], SpawnPlace, Quaternion.identity);
                break;
            case 2:
                GameObject shieldPowerup = Instantiate(powerups[2], SpawnPlace, Quaternion.identity);
                break;
        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(3.0f, 7.0f));
        }
    }


    // Powerup spawn routine

    public void onPlayerDeath() {
        _stopSpawning = true;
    }
}
