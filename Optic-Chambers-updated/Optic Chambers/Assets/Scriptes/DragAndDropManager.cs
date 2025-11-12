using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    [SerializeField] private LayerMask draggableMask;
    private Rigidbody2D selectedObject;
    private bool isDragging;
    private Camera mainCamera;
    [SerializeField] private float followSpeed = 1f; // force applied toward mouse (higher = stronger pull)
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
        Debug.Log("trying to drag");
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, draggableMask);
        Rigidbody2D rb = hit.collider != null ? hit.collider.gameObject.GetComponent<Rigidbody2D>() : null;
        if (rb == null) return;

        Debug.Log("dragging object: " + rb.gameObject.name);

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
        selectedObject.linearDamping = 500f;
        selectedObject = null;
    }

    // Physics updates should happen in FixedUpdate so forces are applied cleanly
    private void FixedUpdate()
    {
        if (!isDragging || selectedObject == null) return;

        Vector2 current = selectedObject.position;
        Vector2 direction = (targetPosition - current);
        
        if (selectedObject.bodyType == RigidbodyType2D.Kinematic)
        {
            // For kinematic bodies, use smooth MovePosition
            Vector2 next = Vector2.Lerp(current, targetPosition, followSpeed * Time.fixedDeltaTime);
            selectedObject.MovePosition(next);
        }
        else
        {
            Debug.Log("applying force");
            // For dynamic bodies, apply force toward target (respects collisions naturally)
            Vector2 force = direction * followSpeed;
            selectedObject.AddForce(force, ForceMode2D.Force);
            
            // Optional: add damping to prevent oscillation
            selectedObject.linearVelocity *= 0.1f;

            selectedObject.linearDamping = 5f;
        }
        
        // Zero angular motion while dragging
        selectedObject.angularVelocity = 0f;
    }

    Vector2 mousePosition()
    {
        return (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
