using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    private GameObject _player;

    [SerializeField] private Slider _healthBar;
    [SerializeField] private Image _dashBar;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        _healthBar.value = Mathf.Lerp(_healthBar.value, _player.GetComponent<PlayerController>().health, Time.deltaTime * 5f);
        _dashBar.fillAmount = Mathf.Lerp(_dashBar.fillAmount, _player.GetComponent<PlayerController>()._dashCooldownTime, Time.deltaTime * 5f);
    }
}
