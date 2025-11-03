using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    public LayerMask draggableMask;
    private GameObject selectedObject;
    private bool isDragging;
    private Camera mainCamera;

    private void Start()
    {
        isDragging = false;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, draggableMask);
            if (hit.collider != null)
            {
                 //Debug.Log(hit.collider.gameObject.name);
                 selectedObject = hit.collider.gameObject;
                 isDragging = true;
            }
        }

        if (isDragging)
        {
            Vector2 pos = mousePosition();
            selectedObject.transform.position = new Vector3(pos.x, pos.y, selectedObject.transform.position.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    Vector2 mousePosition()
    {
        return (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
