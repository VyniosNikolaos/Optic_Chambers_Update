using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private Camera myCamera;
    private Vector3 screePos;
    private float angleOffSet;
    private Collider2D selectedObjectCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
        selectedObjectCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedObjectCollider == Physics2D.OverlapPoint(mousePos))
            {
                screePos = myCamera.WorldToScreenPoint(transform.position);
                Vector3 vector3 = Input.mousePosition - screePos;
                angleOffSet = (Mathf.Atan2(transform.right.y, transform.right.x)- Mathf.Atan2(vector3.y, vector3.x))*Mathf.Rad2Deg;
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (selectedObjectCollider == Physics2D.OverlapPoint(mousePos))
            {
                Vector3 vector3 = Input.mousePosition - screePos;
                float angle = Mathf.Atan2(vector3.y, vector3.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle + angleOffSet);
            }
        }
        
        
    }
}
