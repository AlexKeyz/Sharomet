using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicControl : MonoBehaviour
{
    public Sprite onMusic;
    public Sprite offMusic;

    public Image MusicButton;
    public static bool isOn;
    public AudioSource hitAudioSource; // AudioSource для звука попадания
    public AudioSource missAudioSource; // AudioSource для звука промаха

    void Start()
    {
        isOn = true;
        LoadMusicSettings();
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("music") == 0)
        {
            MusicButton.GetComponent<Image>().sprite = onMusic;
            hitAudioSource.enabled = true;
            missAudioSource.enabled = true;
            isOn = true;
        }
        else if (PlayerPrefs.GetInt("music") == 1)
        {
            MusicButton.GetComponent<Image>().sprite = offMusic;
            hitAudioSource.enabled = false;
            missAudioSource.enabled = false;
            isOn = false;
        }
    }

    public void offSaund()
    {
        if (!isOn)
        {
            PlayerPrefs.SetInt("music", 0);
            isOn = true;
        }
        else if (isOn)
        {
            PlayerPrefs.SetInt("music", 1);
            isOn = false;
        }
    }

    void LoadMusicSettings()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            if (PlayerPrefs.GetInt("music") == 0)
            {
                isOn = true;
                hitAudioSource.enabled = true;
                missAudioSource.enabled = true;
            }
            else
            {
                isOn = false;
                hitAudioSource.enabled = false;
                missAudioSource.enabled = false;
            }
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
            isOn = true;
            hitAudioSource.enabled = true;
            missAudioSource.enabled = true;
        }
    }
}