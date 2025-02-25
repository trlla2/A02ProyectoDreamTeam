using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu_Behaviour : MonoBehaviour
{
    [Header("SETUP")]
    [SerializeField]
    public string playSceneName;

    [Header("Panels")]
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject credits;

    [Header("AudioMixer")]
    //[SerializeField]
    //private AudioMixer audioMixer; ------------------------------- Dependency AudioMixer
    [SerializeField]
    private Slider mainSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider musicSlider;

    //[Header("sfx")] ---------------------------------------------- Depenendcy SFX
    //[SerializeField]
    //private AudioSource clickSFX;

    private void Start()
    {
        //SetVolume(); ------------------------------- Dependency AudioMixer
        //SetVolumeMusic();
        //SetVolumeSFX();

        UnityEngine.Cursor.visible = true;// show cursor
        UnityEngine.Cursor.lockState = CursorLockMode.None;// unlock cursor
    }

    public void OnBack()
    {
        OnClickButton(); // Click Button Behaviour

        mainMenu.SetActive(true); // Show Main Menu, Hide all other panels
        settings.SetActive(false);
        credits.SetActive(false);
    }
    public void OnSettings()
    {
        OnClickButton();// Click Button Behaviour


        settings.SetActive(true); // Show Settings, Hide all other panels
        mainMenu.SetActive(false);
        credits.SetActive(false);
    }
    public void OnCredits()
    {
        OnClickButton();// Click Button Behaviour

        credits.SetActive(true); // Show Credits, Hide all other panels
        mainMenu.SetActive(false);
        settings.SetActive(false);
    }
    public void OnPlay()
    {
        OnClickButton();// Click Button Behaviour

        SceneManager.LoadScene(playSceneName);
    }
    public void OnExit()
    {
        OnClickButton();// Click Button Behaviour

        Application.Quit();
    }

    //public void SetVolume() ------------------------------- Dependency AudioMixer
    //{
    //    audioMixer.SetFloat("Volume", Mathf.Log10(soundSlider.value) * 20);
    //}
    //public void SetVolumeSFX()
    //{
    //    audioMixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value) * 20);
    //}
    //public void SetVolumeMusic()
    //{
    //    audioMixer.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
    //}

    private void OnClickButton()
    {
        //clickSFX.Play(); ---------------------------------------------- Depenendcy SFX
    }
}
