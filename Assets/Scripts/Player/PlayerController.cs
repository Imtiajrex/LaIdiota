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

    private Rigidbody2D _rigidbody;
    private TrailRenderer _tr;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    [SerializeField] float _slowTimeScale = 0.5f;




    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _tr = GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        if (_isDashing || (GameManager.Instance && GameManager.Instance.isReversing))
        {
            return;
        }
        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.1f
        );

        _rigidbody.velocity = _smoothedMovementInput * _speed;
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void OnDash(InputValue inputValue)
    {
        if ((inputValue.isPressed && _canDash && !_isDashing) || _movementInput != Vector2.zero || (GameManager.Instance && !GameManager.Instance.isReversing))
        {
            StartCoroutine(Dash());
        }
    }
    private void OnReverse(InputValue inputValue)
    {

        if (inputValue.Get<float>() == 1) GameManager.Instance.isReversing = true;
        else if (inputValue.Get<float>() == 0) GameManager.Instance.isReversing = false;
    }

    private void OnSlow(InputValue inputValue)
    {
        if (inputValue.Get<float>() == 1) Time.timeScale = _slowTimeScale;
        else if (inputValue.Get<float>() == 0) Time.timeScale = 1f;
    }
    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        _rigidbody.velocity = _movementInput * _dashingPower;
        _tr.emitting = true;
        yield return new WaitForSeconds(_dashingTime);
        _tr.emitting = false;
        _isDashing = false;
        yield return new WaitForSeconds(_dashingCooldown);
        _canDash = true;
    }

    public void Fall()
    {
        if (_isDashing) return;
        Debug.Log("Die");
    }
}
