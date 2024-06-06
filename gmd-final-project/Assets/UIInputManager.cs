using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;

public class UIInputManager : MonoBehaviour
{
    public InputActionAsset inputActions;

    private void OnEnable()
    {
        var uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
        if (uiInputModule != null)
        {
            uiInputModule.point = InputActionReference.Create(inputActions.FindAction("UI/Point"));
            uiInputModule.move = InputActionReference.Create(inputActions.FindAction("UI/Navigate"));
            uiInputModule.submit = InputActionReference.Create(inputActions.FindAction("UI/Submit"));
            uiInputModule.cancel = InputActionReference.Create(inputActions.FindAction("UI/Cancel"));
            uiInputModule.leftClick = InputActionReference.Create(inputActions.FindAction("UI/Click"));
        }
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
