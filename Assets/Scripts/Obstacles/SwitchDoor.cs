using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoor : MonoBehaviour
{
    [SerializeField]
    private GameObject _switch;
    [SerializeField]
    private Vector3 _openPosition;
    [SerializeField]
    private Vector3 _closePosition;

    [SerializeField]
    private float _speed = 10f;

    private void Update()
    {
        if (_switch.GetComponent<Switch>().on)
        {
            transform.position = Vector2.MoveTowards(transform.position, _openPosition, _speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _closePosition, _speed * Time.deltaTime);
        }
    }
}
