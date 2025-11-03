using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public GameObject winscreen;
    public GameObject MainMenu;
    public GameObject MenuBackground;
    public GameObject Loading;
    public GameObject LevelsMenu;

    Scene scene;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start()
    {
        PlayMusic("Menu");
    }
    public static string ConvertIntToRoman(int num)
    {  
        
        string rep = "";
        while(num != 0)
        {
        if (num >= 40)    // 40 - xl
            {
            rep += "xl";           
            num -= 40;
            }

            else if (num >= 10)    // 10 - x
            {
            rep += "x";
            num -= 10;           
            }

            else if (num >= 9)     // 9 - ix
            {
            rep += "ix";
            num -= 9;                         
            }

            else if (num >= 5)     // 5 - v
            {
            rep += "v";
            num -= 5;                                     
            }

            else if (num >= 4)     // 4 - iv
            {
            rep += "iv";
            num -= 4;                                                            
            }

            else if (num >= 1)     // 1 - i
            {
            rep += "i";
            num -= 1;                                                                                   
            }
        }
            Debug.Log(rep);
            return rep;
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySfx(string name)
        {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    public void LevelVictory()
    {   
        winscreen.SetActive(true);

    }
    public void NextLevel()
    {
        Debug.Log("Test");
        scene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync((scene.buildIndex + 1));
        winscreen.SetActive(false);
    }
    
    public void BackMainScreen()
    {
        
        winscreen.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
        UIManager.Instance.PlaySfx("Menu_back");
        UIManager.Instance.musicSource.Stop();
        UIManager.Instance.PlayMusic("Menu");
    }
    
    public void BackLevelScreen()
    {

        winscreen.SetActive(false);
        SceneManager.LoadSceneAsync("MainMenu");
        UIManager.Instance.PlaySfx("Menu_back");
        MainMenu.SetActive(false);
        LevelsMenu.SetActive(true);
        UIManager.Instance.musicSource.Stop();
        UIManager.Instance.PlayMusic("Menu");
    }
}
