using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool AwareofPlayer
    {
        get;
        private set;
    }
    public Vector2 DirectionToPlayer
    {
        get;
        private set;
    }
    public float _awareofPlayerDistance;
    public float _minimumDistance;

    public Transform _player;
    public bool _retreat = false;
    public void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionToPlayer = _player.position - transform.position;
        if (_minimumDistance > directionToPlayer.magnitude)
        {
            // AwareofPlayer = true;
            _retreat = true;
            AwareofPlayer = false;
            // Debug.Log("Player is in range");
            DirectionToPlayer = directionToPlayer;
        }
        else if (_awareofPlayerDistance < directionToPlayer.magnitude)
        {
            AwareofPlayer = true;
            _retreat = false;
            DirectionToPlayer = directionToPlayer;
        }
        else
        {
            AwareofPlayer = false;
            DirectionToPlayer = Vector2.zero;
        }
        //_  awareofPlayerDistance = directionToPlayer.magnitude;


    }
}