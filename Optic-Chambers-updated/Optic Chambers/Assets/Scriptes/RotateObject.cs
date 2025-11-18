using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private Camera myCamera;
    private Vector3 screenPos;
    private float angleOffset;
    private Collider2D selectedObjectCollider;
    private bool isRotating = false;
    private float initialAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
        selectedObjectCollider = GetComponent<Collider2D>();
    }

    void on_rotate_begin()
    {
        isRotating = true;
        // Calculate the initial offset between mouse and object rotation
        screenPos = myCamera.WorldToScreenPoint(transform.position);
        Vector3 mouseVector = Input.mousePosition - screenPos;
        float mouseAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        initialAngle = transform.eulerAngles.z;
        angleOffset = initialAngle - mouseAngle;
    }

    void on_rotate_end()
    {
        isRotating = false;
    }

    void on_rotating(Vector2 mousePos)
    {
        // Update screen position of the object
        screenPos = myCamera.WorldToScreenPoint(transform.position);
        
        // Calculate angle from object center to mouse
        Vector3 mouseVector = Input.mousePosition - screenPos;
        float mouseAngle = Mathf.Atan2(mouseVector.y, mouseVector.x) * Mathf.Rad2Deg;
        
        // Apply rotation with the initial offset
        float targetAngle = mouseAngle + angleOffset;
        transform.eulerAngles = new Vector3(0, 0, targetAngle);
    }



    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedObjectCollider == Physics2D.OverlapPoint(mousePos))
            {
                on_rotate_begin();
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            on_rotate_end();
        }
        

        if (isRotating)
        {
            on_rotating(mousePos);
        }
        
    }
}
