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
    public GameObject FS_SCREEN;

    private LevelManager levelManager;

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
        // Subscribe to scene change events to manage FS_SCREEN visibility
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        // Set initial state based on current scene
        ManageFSScreenVisibility();

        levelManager = LevelManager.Instance;
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene change events to prevent memory leaks
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Call FS_SCREEN visibility management when any scene loads
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ManageFSScreenVisibility();
        }
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        // Also manage visibility when scenes are unloaded (for additive scenes)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ManageFSScreenVisibility();
        }
    }

    public void ManageFSScreenVisibility()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        // Show FS_SCREEN only in MainMenu scene (assuming that's the name of your main menu scene)
        if (FS_SCREEN != null)
        {
            Debug.Log("Managing FS_SCREEN visibility for scene: " + currentScene.name);
            FS_SCREEN.SetActive(currentScene.name == "MainMenu");
        }
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
        if (winscreen != null)
        {
            winscreen.SetActive(true);
        }

        // Set level completed in json
        if (LevelManager.Instance != null)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            LevelManager.Instance.CompleteLevel(currentScene.buildIndex - 1);
        }
        else
        {
            Debug.LogError("LevelManager.Instance is null - cannot complete level");
        }
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
        // FS_SCREEN will be automatically activated when MainMenu scene loads
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
