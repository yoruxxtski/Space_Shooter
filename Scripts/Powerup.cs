using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _speed = 3.0f;
    // Start is called before the first frame update

    // ID = 0 -> tripleshot, ID = 1 -> Speed , ID = 2 -> Shield
    [SerializeField]
    private int powerID;

    [SerializeField]
    private AudioClip _powerSound;


    void Start()
    {
        // start position
        float randomX = UnityEngine.Random.Range(-21.0f, 7.0f);
        transform.position = new Vector3(randomX, 7.0f , 0);
    }


    // Update is called once per frame
    // FPS = 60 ; update 60 lan trong 1 s -> duration = 5s thi update se 300 lan 
    void Update()
    {
        // move down at a speed of 3 (adjust in inspector)
        Movement();
        // when leave screen destroy this object
        if (transform.position.y < -15.0f) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // collide with player
        if (other.transform.tag.Equals("Player") || other.transform.tag.Equals("Player_two")) {
            Player player = other.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerSound, transform.position);
            if (powerID == 0) {
                player.setTripleShot();
            } 
            else if (powerID == 1) {
                player.setSpeedBoost();
            } else if (powerID == 2) {
                player.getShieldBoost();
            }
            Destroy(this.gameObject);
        }
    }

    private void Movement() {
        transform.position += Vector3.down * _speed * Time.deltaTime;
    }
}
