using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerManagment : MonoBehaviour
{
    [SerializeField]private Animator _animator;
    private float lastHitTime;
    const float collisionInterval = 5f;

    public void Update()
    {
        lastHitTime += Time.deltaTime;
        // Debug.Log(lastHitTime);
        if (lastHitTime >= collisionInterval) //Time elapsed since last collision
        {
            shutDown();
        }
    }

    public void Hit()
    {
        // Debug.Log("Hit");
        lastHitTime = 0;
    }

    private void shutDown()
    {
        _animator.SetBool("Activated", false);
    }   
}
