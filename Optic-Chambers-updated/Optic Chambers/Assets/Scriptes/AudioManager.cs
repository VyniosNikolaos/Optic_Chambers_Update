using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;



 
    public void ToggleMusic()
    {
        UIManager.Instance.ToggleMusic();
    }

    public void ToggleSfx()
    {
        UIManager.Instance.ToggleSfx();
    }

    public void MusicVolume()
    {
        UIManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SfxVolume()
    {
        UIManager.Instance.SfxVolume(_sfxSlider.value);
    }
}
