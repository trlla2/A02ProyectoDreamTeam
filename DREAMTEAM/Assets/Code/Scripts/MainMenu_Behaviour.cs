using UnityEngine;
using UnityEngine.Audio;
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
    [SerializeField]
    private AudioMixer audioMixer; 
    [SerializeField]
    private Slider mainSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider musicSlider;

    private void Start()
    {
        SetVolume();
        SetVolumeMusic();
        SetVolumeSFX();

        Cursor.visible = true;// show cursor
        Cursor.lockState = CursorLockMode.None;// unlock cursor
    }

    public void OnBack()
    {
        mainMenu.SetActive(true); // Show Main Menu, Hide all other panels
        settings.SetActive(false);
        credits.SetActive(false);
    }
    public void OnSettings()
    {
        settings.SetActive(true); // Show Settings, Hide all other panels
        mainMenu.SetActive(false);
        credits.SetActive(false);
    }
    public void OnCredits()
    {
        credits.SetActive(true); // Show Credits, Hide all other panels
        mainMenu.SetActive(false);
        settings.SetActive(false);
    }
    public void OnPlay()
    {
        SceneManager.LoadScene(playSceneName);
    }
    public void OnExit()
    {
        Application.Quit();
    }

    public void SetVolume()
    { 
        audioMixer.SetFloat("VolumeMaster", Mathf.Log10(mainSlider.value)* 20);
    }
    public void SetVolumeSFX()
    {
        audioMixer.SetFloat("VolumeSFX", Mathf.Log10(sfxSlider.value) * 20);
    }
    public void SetVolumeMusic()
    {
        audioMixer.SetFloat("VolumeMusic", Mathf.Log10(musicSlider.value) * 20);
    }
}
