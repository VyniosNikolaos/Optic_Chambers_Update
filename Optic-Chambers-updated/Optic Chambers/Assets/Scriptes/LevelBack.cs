using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelBack : MonoBehaviour
{
    private void Start()
    {
        //
    }
    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        UIManager.Instance.PlaySfx("Menu_back");
        UIManager.Instance.musicSource.Stop();
        UIManager.Instance.PlayMusic("Menu");
    }
}
