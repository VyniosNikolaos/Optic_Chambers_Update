using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Test 2");
        SceneManager.LoadSceneAsync($"MainMenu");
    }
}
