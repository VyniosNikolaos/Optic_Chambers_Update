using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{
    private Vector3 _origin;
    private Vector3 _difference;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera subCamera;

    private bool _isDragging;

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _origin = GetMousePosition;
        }
        
        _isDragging = ctx.started || ctx.performed;
    }

    private void LateUpdate()
    {
        if (!_isDragging) return;

        _difference = GetMousePosition - transform.position;
        transform.position = _origin - _difference;
    }
    
    private Vector3 GetMousePosition => mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}
