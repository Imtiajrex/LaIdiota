using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTimeManipulation : MonoBehaviour
{
    [SerializeField] float _slowTimeScale = 0.5f;


    private void OnReverse(InputValue inputValue) { 

        if(inputValue.Get<float>() == 1) GameManager.Instance.isReversing = true;
        else if(inputValue.Get<float>() == 0) GameManager.Instance.isReversing = false;
    }

    private void OnSlow(InputValue inputValue) { 
        if(inputValue.Get<float>() == 1) Time.timeScale = _slowTimeScale;
        else if(inputValue.Get<float>() == 0) Time.timeScale = 1f;
    }
}
