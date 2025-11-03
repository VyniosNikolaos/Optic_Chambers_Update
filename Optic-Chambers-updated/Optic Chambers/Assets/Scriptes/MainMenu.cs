using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
      // UIManager.Instance.Loading.SetActive(true);
      // UIManager.Instance.MainMenu.SetActive(false);
      // UIManager.Instance.MenuBackground.SetActive(false);


      UIManager.Instance.MainMenu.SetActive(true);
      UIManager.Instance.MenuBackground.SetActive(true);
      UIManager.Instance.Loading.SetActive(false);
    }



    public void PlayGame(int level)
    {
        SceneManager.LoadSceneAsync(level);
        UIManager.Instance.musicSource.Stop();
        UIManager.Instance.PlayMusic("Level");
    }
    public void GoTemplate()
    {
        SceneManager.LoadSceneAsync("Templatelvl");
        UIManager.Instance.musicSource.Stop();
        UIManager.Instance.PlayMusic("Level");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
