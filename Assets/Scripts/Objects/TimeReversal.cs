using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReversal : MonoBehaviour
{
    private List<PointInTime> _pointsInTime = new List<PointInTime>();
    private Rigidbody2D _rigidBody;
    private float _reverseTime = 5f;
    private RigidbodyType2D _rigidBodyType;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody) _rigidBodyType = _rigidBody.bodyType;
    }


    void FixedUpdate()
    {
        if (GameManager.Instance && GameManager.Instance.isReversing)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
    void Rewind()
    {
        if (_rigidBody)
        {
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
        }
        if (_pointsInTime.Count > Mathf.Round(_reverseTime / Time.fixedDeltaTime))
        {
            _pointsInTime.RemoveAt(_pointsInTime.Count - 1);
        }

        if (_pointsInTime.Count > 0)
        {
            Debug.Log(_pointsInTime.Count);
            transform.position = _pointsInTime[0].position;
            transform.rotation = _pointsInTime[0].rotation;
            transform.localScale = _pointsInTime[0].localScale;
            _pointsInTime.RemoveAt(0);
        }
    }
    void Record()
    {
        if (_rigidBody) _rigidBody.bodyType = _rigidBodyType;
        _pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, transform.localScale));
    }
}
