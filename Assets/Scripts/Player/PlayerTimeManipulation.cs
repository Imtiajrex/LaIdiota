using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTimeManipulation : MonoBehaviour
{
    bool _isReversing = false;
    [SerializeField] float _slowTimeScale = 0.5f;


    private void OnReverse(InputValue inputValue) { 

        if(inputValue.Get<float>() == 1) _isReversing = true;
        else if(inputValue.Get<float>() == 0) _isReversing = false;
    }

    private void OnSlow(InputValue inputValue) { 
        if(inputValue.Get<float>() == 1) Time.timeScale = _slowTimeScale;
        else if(inputValue.Get<float>() == 0) Time.timeScale = 1f;
    }
}
