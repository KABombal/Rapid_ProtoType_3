using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpiderController))]
public class SpiderDriver : MonoBehaviour
{
    SpiderController controller;

    private void Awake()
    {
        controller = GetComponent<SpiderController>();
    }
    private void OnEnable()
    {
        InputInstance.Controls.Spider.MoveAndTurn.performed += SetMoveInput;
        InputInstance.Controls.Spider.MoveAndTurn.canceled += SetMoveInput;

        InputInstance.Controls.Spider.Enable();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        InputInstance.Controls.Spider.MoveAndTurn.performed -= SetMoveInput;
        InputInstance.Controls.Spider.MoveAndTurn.canceled -= SetMoveInput;

        InputInstance.Controls.Spider.Disable();

        Cursor.lockState = CursorLockMode.None;
    }

    private void SetMoveInput(InputAction.CallbackContext ctx)
    {
        controller.MoveInput = ctx.ReadValue<Vector2>();
    }

}
