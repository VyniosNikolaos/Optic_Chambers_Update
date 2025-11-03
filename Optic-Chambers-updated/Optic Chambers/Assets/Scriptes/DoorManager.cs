using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    private bool curentState; // False Cloosed / True Open

    private Vector2 openPosition;
    private Vector2 closedPosition;
    private Vector2 targetPos;
    private GameObject doorBlock;

    public void changeDoorState()
    {
        curentState = !curentState;
        if (curentState)
        {// Open
            Open();
        }
        else
        { // Close
            Close();
        }
        
    }

    private void Open()
    {
        Debug.Log("Opening Door");
        targetPos = openPosition;
    }
    
    private void Close()
    {
        Debug.Log("Closing Door");
        targetPos = closedPosition;
    }

    private void Start()
    {
        doorBlock = transform.GetChild(0).gameObject;
        closedPosition = (Vector2)doorBlock.transform.position;
        openPosition = closedPosition - Vector2.up*2f;
        targetPos = closedPosition;
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = doorBlock.transform.position;

        if(currentPos == targetPos) {
            targetPos = currentPos;
        }

        if (targetPos != currentPos)
        {
            Transform doorGear = transform.GetChild(1).transform;
            float rotationsPerMinute = 100.0f;
            // Rotate Gear
            if (targetPos == openPosition)
            {
                doorGear.Rotate(0f,0f,-6.0f*rotationsPerMinute*Time.deltaTime);
            }
            else
            {
                doorGear.Rotate(0f,0f,6.0f*rotationsPerMinute*Time.deltaTime);
            }
            
        }
        
        // Debug.Log("tar "+targetPos);
        // Debug.Log("cur "+currentPos);
        Vector2 targetDirection = (targetPos-currentPos).normalized;
        float speed = 2f;
        doorBlock.GetComponent<Rigidbody2D>().MovePosition(currentPos + targetDirection * (Time.deltaTime * speed));
    }
}
