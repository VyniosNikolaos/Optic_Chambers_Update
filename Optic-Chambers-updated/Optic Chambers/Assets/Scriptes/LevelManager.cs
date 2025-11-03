using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
public class LevelManager : MonoBehaviour
{
    public GameObject Level1Icon;
    public GameObject Level2Icon;
    public GameObject Level3Icon;
    public GameObject Level4Icon;
    public GameObject Level5Icon;
    public GameObject Level6Icon;
    public GameObject Level7Icon;
    public GameObject Level8Icon;
    public GameObject Level9Icon;
    public GameObject Level10Icon;
    public GameObject Level11Icon;
    public GameObject Level12Icon;
    public GameObject Level13Icon;
    public GameObject Level14Icon;
    public GameObject Level15Icon;
    public GameObject Level16Icon;
    public GameObject Level17Icon;
    public GameObject Level18Icon;

    public void SavePrefs()
    {
    PlayerPrefs.SetInt("leveli", 1);
    PlayerPrefs.SetInt("levelii", 1);
    PlayerPrefs.SetInt("leveliii", 1);
    PlayerPrefs.SetInt("leveliv", 0);
    PlayerPrefs.SetInt("levelv", 0);
    PlayerPrefs.SetInt("levelvi", 0);
    PlayerPrefs.SetInt("levelvii", 0);
    PlayerPrefs.SetInt("levelviii", 0);
    PlayerPrefs.SetInt("levelix", 0);
    PlayerPrefs.SetInt("levelx", 0);
    PlayerPrefs.SetInt("levelxi", 0);
    PlayerPrefs.SetInt("levelxii", 0);
    PlayerPrefs.SetInt("levelxiii", 1);
    PlayerPrefs.SetInt("levelxiv", 1);
    PlayerPrefs.SetInt("levelxv", 1);
    PlayerPrefs.SetInt("levelxvi", 1);
    PlayerPrefs.SetInt("levelxvii", 1);
    PlayerPrefs.SetInt("levelxviii", 1);
    PlayerPrefs.Save();
    }
    private void Start()
    {
        SavePrefs();
        if (PlayerPrefs.GetInt("leveli") == 1)
        {
            Level1Icon.SetActive(true);
        }else{
            Level1Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelii") == 1)
        {
            Level2Icon.SetActive(true);
        }else{
            Level2Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("leveliii") == 1)
        {
            Level3Icon.SetActive(true);
        }else{
            Level3Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("leveliv") == 1)
        {
            Level4Icon.SetActive(true);
        }else{
            Level4Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelv") == 1)
        {
            Level5Icon.SetActive(true);
        }else{
            Level5Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelvi") == 1)
        {
            Level6Icon.SetActive(true);
        }else{
            Level6Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelvii") == 1)
        {
            Level7Icon.SetActive(true);
        }else{
            Level7Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelviii") == 1)
        {
            Level8Icon.SetActive(true);
        }else{
            Level8Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelix") == 1)
        {
            Level9Icon.SetActive(true);
        }else{
            Level9Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelx") == 1)
        {
            Level10Icon.SetActive(true);
        }else{
            Level10Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelxi") == 1)
        {
            Level11Icon.SetActive(true);
        }else{
            Level11Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelxii") == 1)
        {
            Level12Icon.SetActive(true);
        }else{
            Level12Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelxiii") == 1)
        {
            Level13Icon.SetActive(true);
        }else{
            Level13Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelxiv") == 1)
        {
            Level14Icon.SetActive(true);
        }else{
            Level14Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelxv") == 1)
        {
            Level15Icon.SetActive(true);
        }else{
            Level15Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelxvi") == 1)
        {
            Level16Icon.SetActive(true);
        }else{
            Level16Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelxvii") == 1)
        {
            Level17Icon.SetActive(true);
        }else{
            Level17Icon.SetActive(false);
        }
        if (PlayerPrefs.GetInt("levelxviii") == 1)
        {
            Level18Icon.SetActive(true);
        }else{
            Level18Icon.SetActive(false);
        }
        
    }
}
