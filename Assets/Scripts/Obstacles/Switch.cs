using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool on = false;
    [SerializeField]
    private Color _onColor;
    [SerializeField]
    private Color _offColor;

    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private bool _switchesOnEnter = true;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = _offColor;
    }
    void Update()
    {
        _spriteRenderer.color = on ? _onColor : _offColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            on = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_switchesOnEnter && other.CompareTag("Player"))
        {
            on = false;
        }
    }



}
