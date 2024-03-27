using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 8;

    private bool _isEnemyLaser = false;


    void Update()
    {
        if (_isEnemyLaser == false) {
            MoveUp();
        } else {
            MoveDown();
        }
    }

    void MoveUp() {
        transform.Translate( Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f) {
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -14.0f) {
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser() {
        _isEnemyLaser = true;
    }

    public bool isEnemyLaser() {
        return _isEnemyLaser;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((other.transform.tag == "Player" || 
        other.transform.tag == "Player_two") && _isEnemyLaser == true) {
            Destroy(this.gameObject);
            Player player = other.GetComponent<Player>();
            player.Damage();
        }
    }
}
