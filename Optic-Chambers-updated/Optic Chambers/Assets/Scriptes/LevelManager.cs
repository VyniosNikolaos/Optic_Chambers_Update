using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class LevelProgressData
{
    public LevelState[] levelStates;
    public int lastUnlockedLevel;
    public float totalPlayTime;
    public string lastSaveDate;
    
    public LevelProgressData(int totalLevels)
    {
        levelStates = new LevelState[totalLevels];
        for (int i = 0; i < totalLevels; i++)
        {
            levelStates[i] = LevelState.Locked;
        }
        lastUnlockedLevel = 0;
        totalPlayTime = 0f;
        lastSaveDate = System.DateTime.Now.ToString();
    }
}

public enum LevelState
{
    Locked,    // Can't click, shows lock icon
    Unlocked,  // Can click, normal appearance
    Completed  // Can click, shows check icon
}

public class LevelManager : MonoBehaviour
{
    // Singleton instance
    public static LevelManager Instance { get; private set; }
    
    [Header("Level Configuration")]
    public GameObject[] levelButtons = new GameObject[18];
    
    [Header("Level Settings")]
    public int totalLevels = 18;
    public int initialUnlockedLevels = 1;
    
    [Header("UI Sprites")]
    public Sprite lockSprite;
    public Sprite checkSprite;
    
    [Header("File Settings")]
    public string saveFileName = "level_progress.json";
    
    private LevelProgressData progressData;
    private string saveFilePath;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ValidateConfiguration();
        InitializeLevelProgress();
        RefreshAllLevelButtons();
        
        // Subscribe to scene loaded event to refresh buttons when returning to menu
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from scene events to prevent memory leaks
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Refresh level buttons when returning to MainMenu scene
        if (scene.name == "MainMenu")
        {
            Debug.Log("MainMenu loaded - refreshing level buttons");
            // Use a small delay to ensure UI elements are ready
            StartCoroutine(DelayedRefresh());
        }
    }
    
    private IEnumerator DelayedRefresh()
    {
        yield return new WaitForEndOfFrame();
        RefreshAllLevelButtons();
    }
    
    /// <summary>
    /// Validates the level manager configuration and logs warnings for issues
    /// </summary>
    private void ValidateConfiguration()
    {
        // Ensure array size matches expected level count
        if (levelButtons.Length != totalLevels)
        {
            Debug.LogWarning($"LevelManager: levelButtons array size ({levelButtons.Length}) doesn't match totalLevels ({totalLevels}). Please resize the array in inspector.");
        }
        
        // Check for missing level button references
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] == null)
            {
                Debug.LogWarning($"LevelManager: Level button at index {i} (Level {i + 1}) is not assigned in the inspector.");
            }
        }
        
        // Validate sprites
        if (lockSprite == null)
        {
            Debug.LogWarning("LevelManager: Lock sprite is not assigned. Please assign Assets/Sprites/level_lock.png");
        }
        if (checkSprite == null)
        {
            Debug.LogWarning("LevelManager: Check sprite is not assigned. Please assign Assets/Sprites/level_check.png");
        }
        
        // Validate initial unlocked levels
        if (initialUnlockedLevels > totalLevels)
        {
            Debug.LogWarning($"LevelManager: initialUnlockedLevels ({initialUnlockedLevels}) is greater than totalLevels ({totalLevels}). Clamping to totalLevels.");
            initialUnlockedLevels = totalLevels;
        }
    }

    /// <summary>
    /// Initializes level progress data from JSON file or creates new data
    /// </summary>
    public void InitializeLevelProgress()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
        
        if (File.Exists(saveFilePath))
        {
            LoadProgressFromFile();
        }
        else
        {
            CreateNewProgressData();
        }
    }
    
    /// <summary>
    /// Creates new progress data with initial unlocked levels
    /// </summary>
    private void CreateNewProgressData()
    {
        progressData = new LevelProgressData(totalLevels);
        
        // Unlock initial levels
        for (int i = 0; i < initialUnlockedLevels && i < totalLevels; i++)
        {
            progressData.levelStates[i] = LevelState.Unlocked;
        }
        progressData.lastUnlockedLevel = initialUnlockedLevels - 1;
        
        SaveProgressToFile();
    }
    
    /// <summary>
    /// Loads progress data from JSON file
    /// </summary>
    private void LoadProgressFromFile()
    {
        try
        {
            string jsonData = File.ReadAllText(saveFilePath);
            progressData = JsonUtility.FromJson<LevelProgressData>(jsonData);
            
            // Validate loaded data
            if (progressData.levelStates.Length != totalLevels)
            {
                Debug.LogWarning("Level count mismatch in save file. Creating new progress data.");
                CreateNewProgressData();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load progress file: {e.Message}. Creating new progress data.");
            CreateNewProgressData();
        }
    }
    
    /// <summary>
    /// Saves progress data to JSON file
    /// </summary>
    private void SaveProgressToFile()
    {
        try
        {
            progressData.lastSaveDate = System.DateTime.Now.ToString();
            string jsonData = JsonUtility.ToJson(progressData, true);
            File.WriteAllText(saveFilePath, jsonData);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save progress file: {e.Message}");
        }
    }

    /// <summary>
    /// Unlocks a specific level and saves progress
    /// </summary>
    public void UnlockLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < totalLevels && 
            progressData.levelStates[levelIndex] == LevelState.Locked)
        {
            progressData.levelStates[levelIndex] = LevelState.Unlocked;
            progressData.lastUnlockedLevel = Mathf.Max(progressData.lastUnlockedLevel, levelIndex);
            SaveProgressToFile();
            UpdateLevelButtonState(levelIndex);
        }
    }
    
    /// <summary>
    /// Marks a level as completed and saves progress
    /// </summary>
    public void CompleteLevel(int levelIndex)
    {
        Debug.Log($"Attempting to complete level {levelIndex + 1}");
        
        if (levelIndex >= 0 && levelIndex < totalLevels)
        {
            LevelState currentState = progressData.levelStates[levelIndex];
            Debug.Log($"Level {levelIndex + 1} current state: {currentState}");
            
            // Allow completion for both Unlocked and already Completed levels
            if (currentState == LevelState.Unlocked || currentState == LevelState.Completed)
            {
                progressData.levelStates[levelIndex] = LevelState.Completed;
                Debug.Log($"Level {levelIndex + 1} marked as completed");
                
                // Unlock next level if it exists and is locked
                if (levelIndex + 1 < totalLevels && progressData.levelStates[levelIndex + 1] == LevelState.Locked)
                {
                    UnlockLevel(levelIndex + 1);
                    Debug.Log($"Level {levelIndex + 2} unlocked");
                }
                
                SaveProgressToFile();
                UpdateLevelButtonState(levelIndex);
                
                // Also update the next level button if it was unlocked
                if (levelIndex + 1 < totalLevels)
                {
                    UpdateLevelButtonState(levelIndex + 1);
                }
            }
            else
            {
                Debug.LogWarning($"Cannot complete level {levelIndex + 1} - it is {currentState}");
            }
        }
        else
        {
            Debug.LogError($"Invalid level index: {levelIndex}");
        }
    }
    
    /// <summary>
    /// Gets the current state of a level
    /// </summary>
    public LevelState GetLevelState(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < totalLevels && progressData != null)
        {
            return progressData.levelStates[levelIndex];
        }
        return LevelState.Locked;
    }
    
    /// <summary>
    /// Checks if a level can be played (unlocked or completed)
    /// </summary>
    public bool CanPlayLevel(int levelIndex)
    {
        LevelState state = GetLevelState(levelIndex);
        return state == LevelState.Unlocked || state == LevelState.Completed;
    }

    /// <summary>
    /// Updates the state of a specific level button (visuals and interactivity)
    /// </summary>
    private void UpdateLevelButtonState(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelButtons.Length && levelButtons[levelIndex] != null)
        {
            GameObject button = levelButtons[levelIndex];
            LevelState state = GetLevelState(levelIndex);
            
            // Get button component for interactivity
            Button buttonComponent = button.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.interactable = CanPlayLevel(levelIndex);
            }
            
            // Update second child image based on state
            if (button.transform.childCount > 1)
            {
                GameObject secondChild = button.transform.GetChild(1).gameObject;
                Image childImage = secondChild.GetComponent<Image>();
                
                if (childImage != null)
                {
                    switch (state)
                    {
                        case LevelState.Locked:
                            childImage.sprite = lockSprite;
                            secondChild.SetActive(true);
                            break;
                        case LevelState.Unlocked:
                            secondChild.SetActive(false); // Hide overlay for normal state
                            break;
                        case LevelState.Completed:
                            childImage.sprite = checkSprite;
                            secondChild.SetActive(true);
                            break;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Updates all level button states based on current progress
    /// </summary>
    public void RefreshAllLevelButtons()
    {
        Debug.Log($"Refreshing {levelButtons.Length} level buttons");
        
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] != null)
            {
                UpdateLevelButtonState(i);
            }
            else
            {
                Debug.LogWarning($"Level button {i} is null - cannot update");
            }
        }
        
        Debug.Log($"Level progress summary: {GetCompletedLevelCount()}/{totalLevels} completed");
    }

    /// <summary>
    /// Gets the total number of completed levels
    /// </summary>
    public int GetCompletedLevelCount()
    {
        int count = 0;
        for (int i = 0; i < totalLevels; i++)
        {
            if (GetLevelState(i) == LevelState.Completed)
            {
                count++;
            }
        }
        return count;
    }
    
    /// <summary>
    /// Gets the total number of unlocked (but not completed) levels
    /// </summary>
    public int GetUnlockedLevelCount()
    {
        int count = 0;
        for (int i = 0; i < totalLevels; i++)
        {
            if (GetLevelState(i) == LevelState.Unlocked)
            {
                count++;
            }
        }
        return count;
    }
    
    /// <summary>
    /// Gets progress percentage (completed levels / total levels)
    /// </summary>
    public float GetProgressPercentage()
    {
        return (float)GetCompletedLevelCount() / totalLevels * 100f;
    }
    
    /// <summary>
    /// Gets the highest unlocked level index
    /// </summary>
    public int GetLastUnlockedLevel()
    {
        return progressData?.lastUnlockedLevel ?? 0;
    }
    
    /// <summary>
    /// Clears all progress and deletes the save file (preparation for new game)
    /// </summary>
    public void ClearAllProgress()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                Debug.Log("Progress file deleted successfully.");
            }
            
            // Create fresh progress data
            CreateNewProgressData();
            RefreshAllLevelButtons();
            
            Debug.Log("All progress cleared and reset to initial state.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to clear progress: {e.Message}");
        }
    }
    
    /// <summary>
    /// For debugging: prints current progress to console
    /// </summary>
    [ContextMenu("Debug: Print Progress")]
    public void DebugPrintProgress()
    {
        if (progressData == null)
        {
            Debug.Log("No progress data loaded.");
            return;
        }
        
        Debug.Log($"=== Level Progress Debug ===");
        Debug.Log($"Last Unlocked Level: {progressData.lastUnlockedLevel}");
        Debug.Log($"Completed: {GetCompletedLevelCount()}/{totalLevels} ({GetProgressPercentage():F1}%)");
        Debug.Log($"Save File: {saveFilePath}");
        Debug.Log($"Last Save: {progressData.lastSaveDate}");
        
        for (int i = 0; i < totalLevels; i++)
        {
            Debug.Log($"Level {i + 1}: {GetLevelState(i)}");
        }
    }
}