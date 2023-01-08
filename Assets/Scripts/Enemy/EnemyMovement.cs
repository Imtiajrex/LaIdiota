using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private GameObject projectile;

    public Transform moveSpot;
    private float waitTime;
    private float startWaitTime;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private Rigidbody2D _rigidbody;
    private Vector2 _targetDirection;
    private PlayerAwarenessController _playerAwarenessController;

    public float timebetweenshots;
    private float nextShotTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAwarenessController = GetComponent<PlayerAwarenessController>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Patrol();
        ShootingPlayer();
        UpdateTargetDirection();
        targetRotation();
        SetVelocity();

    }
    public void Patrol()
    {
        waitTime = startWaitTime;
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, _speed * Time.deltaTime);
        if (Vector2.Distance(moveSpot.position, transform.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    private void ShootingPlayer()
    {
        if (Time.time > nextShotTime)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            nextShotTime = Time.time + timebetweenshots;

        }
    }
    private void UpdateTargetDirection()
    {
        if (_playerAwarenessController.AwareofPlayer)
        {
            _targetDirection = _playerAwarenessController.DirectionToPlayer;
        }
        else
        {
            _targetDirection = Vector2.zero;
        }
    }

    private void targetRotation()
    {

        if (_targetDirection == Vector2.zero)
        {
            _rigidbody.MoveRotation(transform.rotation);
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        _rigidbody.MoveRotation(rotation);
    }

    private void SetVelocity()
    {
        if (_playerAwarenessController.AwareofPlayer == true)
        {
            _rigidbody.position = Vector2.MoveTowards(_rigidbody.position, _playerAwarenessController.DirectionToPlayer, _speed * Time.deltaTime);
        }
        else if (_playerAwarenessController.AwareofPlayer == false && _playerAwarenessController._retreat == false)
        {
            _rigidbody.position = this._rigidbody.position;
        }

        else if (_playerAwarenessController._retreat == true)
        {
            _rigidbody.position = Vector2.MoveTowards(_rigidbody.position, _playerAwarenessController.DirectionToPlayer, -_speed * Time.deltaTime);
        }

        //_smoothMovement = Vector2.SmoothDamp(_smoothMovement, _inputMovement, ref _smoothMovementVelocity, .1f); // for smooth movement
        //   if(_targetDirection == Vector2.zero){
        //       _rigidbody.velocity = Vector2.zero;
        //       return;
        //   }
        // //   else if(_playerAwarenessController._retreat == true){
        // //       _rigidbody.velocity = -transform.up*_speed;
        // //       return;
        // //   }
        // //   else if(_playerAwarenessController._retreat == false){
        // //       _rigidbody.velocity = transform.up*_speed;
        // //       return;
        // //   }




        // // 
        // if(_playerAwarenessController._retreat == true){
        //     _rigidbody.velocity = transform.up*-_speed;
        //    // return;
        // }
        // else if(_playerAwarenessController._retreat == false){
        //     _rigidbody.velocity = transform.up*_speed;
        //   //  return;
        // }
    }

}