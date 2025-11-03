using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    public LayerMask draggableMask;
    private Rigidbody2D selectedObject;
    private bool isDragging;
    private Camera mainCamera;
    public float followSpeed = 0.1f; // higher = snappier following
    private Vector2 targetPosition;
    private float savedGravityScale = 0f;
    private bool gravitySaved = false;

    private void Start()
    {
        isDragging = false;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) TryBeginDrag();

        // update target position each frame while dragging
        if (isDragging && selectedObject != null) targetPosition = mousePosition();

        if (Input.GetMouseButtonUp(0)) TryEndDrag();
    }

    private void TryBeginDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, draggableMask);
        Rigidbody2D rb = hit.collider != null ? hit.collider.gameObject.GetComponent<Rigidbody2D>() : null;
        if (rb == null) return;

        selectedObject = rb;
        isDragging = true;
        
        // initialize target and optionally disable gravity while dragging (only for dynamic bodies)
        targetPosition = mousePosition();
        if (selectedObject.bodyType == RigidbodyType2D.Dynamic)
        {
            savedGravityScale = selectedObject.gravityScale;
            gravitySaved = true;
            selectedObject.gravityScale = 0f;
        }
    }

    private void TryEndDrag()
    {
        isDragging = false;
        if (selectedObject == null) return;

        // stop motion and restore gravity if we changed it
        selectedObject.linearVelocity = Vector2.zero;
        if (gravitySaved && selectedObject.bodyType == RigidbodyType2D.Dynamic)
        {
            selectedObject.gravityScale = savedGravityScale;
            gravitySaved = false;
        }
        selectedObject = null;
    }

    // Physics updates should happen in FixedUpdate so velocity/MovePosition are applied cleanly
    private void FixedUpdate()
    {
        if (!isDragging || selectedObject == null) return;

        Vector2 current = selectedObject.position;
        // move toward the target using MovePosition to preserve collisions
        float maxStep = followSpeed * Time.fixedDeltaTime;
        Vector2 next = Vector2.MoveTowards(current, targetPosition, maxStep);
        selectedObject.MovePosition(next);
        // optionally zero angular motion while dragging
        selectedObject.angularVelocity = 0f;
    }

    Vector2 mousePosition()
    {
        return (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
