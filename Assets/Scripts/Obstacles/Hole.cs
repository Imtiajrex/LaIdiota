using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private GameObject _player;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_player && _player.GetComponent<Rigidbody2D>().velocity.magnitude < 0.05f)
        {
            _player.GetComponent<PlayerController>().Fall();
            _player = null;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player = other.gameObject;
            other.gameObject.GetComponent<PlayerController>().Fall();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player = null;
        }
    }
}
