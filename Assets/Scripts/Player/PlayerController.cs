using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private bool _canDash = true;
    private bool _isDashing;

    [SerializeField]
    private float _dashingPower = 24f;

    [SerializeField]
    private float _dashingTime = 0.2f;

    [SerializeField]
    private float _dashingCooldown = 1f;
    public float _dashCooldownTime = 1f;

    private Rigidbody2D _rigidbody;
    private TrailRenderer _tr;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    [SerializeField] float _slowTimeScale = 0.5f;

    public float health = 100f;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _tr = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (isNotActive())
            return;
        if (!_canDash)
        {
            if (_dashCooldownTime >= _dashingCooldown) _dashCooldownTime = _dashingCooldown;
            else
                _dashCooldownTime += Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        if (_isDashing || isNotActive())
        {
            return;
        }
        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.1f
        );

        _rigidbody.velocity = _smoothedMovementInput * (GameManager.Instance.isReversing ? _speed / _slowTimeScale : _speed);
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void OnDash(InputValue inputValue)
    {
        if (isNotActive()) return;

        if ((inputValue.isPressed && _canDash && !_isDashing))
        {
            StartCoroutine(Dash());
        }
    }
    private void OnReverse(InputValue inputValue)
    {
        if (inputValue.Get<float>() == 1) GameManager.Instance.isReversing = true;
        else if (inputValue.Get<float>() == 0) GameManager.Instance.isReversing = false;
    }

    private bool isNotActive()
    {
        return (GameManager.Instance && GameManager.Instance.isReversing) || health <= 0;
    }
    private void OnSlow(InputValue inputValue)
    {
        if (isNotActive()) return;

        if (inputValue.Get<float>() == 1)
        {
            Time.timeScale = _slowTimeScale;
            Time.fixedDeltaTime = _slowTimeScale * Time.timeScale;
            GameManager.Instance.isTimeSlowed = true;
        }
        else if (inputValue.Get<float>() == 0)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale;
            GameManager.Instance.isTimeSlowed = false;
        }
    }
    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        _dashCooldownTime = 0;
        float xValue = Mathf.Ceil(Mathf.Abs(_movementInput.x));
        float yValue = Mathf.Ceil(Mathf.Abs(_movementInput.y));
        xValue = _movementInput.x < 0 ? -xValue : xValue;
        yValue = _movementInput.y < 0 ? -yValue : yValue;
        bool goingCorner = Mathf.Abs(xValue) == 1 && Mathf.Abs(yValue) == 1;


        xValue = goingCorner ? xValue / 2 : xValue;
        yValue = goingCorner ? yValue / 2 : yValue;
        Vector2 direction = new Vector2(xValue, yValue);
        _rigidbody.velocity = direction * _dashingPower;
        _tr.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        _tr.emitting = false;
        _isDashing = false;
        yield return new WaitForSeconds(_dashingCooldown);
        _canDash = true;
    }

    public IEnumerator Fall(Vector3 holePos)
    {
        if (_isDashing) yield break;
        _rigidbody.velocity = Vector2.zero;
        health = 0;
        //decrease size of player
        Vector3 originalScale = transform.localScale;

        float currentTime = 0.0f;
        float time = 0.35f; // the time to reach the destination & scale
        do
        {
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, currentTime / time);
            transform.position = Vector3.Lerp(transform.position, holePos, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }
}
