using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public float timer;

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.5f)
        {
            _animator.SetBool("Contact", false);
        }

        timer += Time.deltaTime;
    }
}
