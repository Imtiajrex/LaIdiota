using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PatrolController : MonoBehaviour
{
    public float _speed;
    [SerializeField]
    private List<Transform> _waypoints;
    private int _waypointIndex;

    [SerializeField]
    private bool _rotate = false;
    private bool _isReversing;
    void Update()
    {
        if (GameManager.Instance && GameManager.Instance.isReversing)
            return;
        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_waypointIndex].position, _speed * Time.deltaTime);
        if (_rotate)
        {
            Vector3 dir = _waypoints[_waypointIndex].position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        if (Vector2.Distance(transform.position, _waypoints[_waypointIndex].position) < 0.1f)
        {
            if (_waypointIndex < _waypoints.Count - 1 && !_isReversing)
            {
                _waypointIndex++;
            }
            else if (_waypointIndex > 0 && _isReversing)
            {
                _waypointIndex--;
            }
            _isReversing = _waypointIndex == _waypoints.Count - 1 ? true : _waypointIndex == 0 ? false : _isReversing;
        }
    }
}