using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private bool _isInside = false;
    private GameObject _player;
    void Update()
    {
        if (_isInside && _player && _player.GetComponent<Rigidbody2D>().velocity.magnitude < 0.05f)
        {
            _player.GetComponent<PlayerController>().Fall();
            _player = null;
            _isInside = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isInside = true;
            _player = other.gameObject;
            other.gameObject.GetComponent<PlayerController>().Fall();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isInside = false;
            _player = null;
        }
    }
}
