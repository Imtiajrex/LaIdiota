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
        if (GameManager.Instance && GameManager.Instance.isReversing)
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
        if (GameManager.Instance && GameManager.Instance.isReversing) return;
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

    private void OnSlow(InputValue inputValue)
    {
        if (GameManager.Instance && GameManager.Instance.isReversing) return;

        if (inputValue.Get<float>() == 1) Time.timeScale = _slowTimeScale;
        else if (inputValue.Get<float>() == 0) Time.timeScale = 1f;
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

    public IEnumerator Fall()
    {
        if (_isDashing) yield break;
        _rigidbody.velocity = Vector2.zero;
        //decrease size of player
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = new Vector3(0.1f, 0.1f, 0.1f);
        float currentTime = 0.0f;
        do
        {
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / 1f);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= 0.5f);
    }
}
