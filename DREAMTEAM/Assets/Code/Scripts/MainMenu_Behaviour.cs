using UnityEngine;
using UnityEngine.Audio;
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
        if(GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject); // Destroy Game Manager
        }

        SetVolume();
        SetVolumeMusic();
        SetVolumeSFX();

        Cursor.visible = true;// show cursor
        Cursor.lockState = CursorLockMode.None;// unlock cursor

        mainMenu.GetComponent<TranslatorUI>().ToTarget(); // start animation
    }

    public void OnBack()// Show Main Menu, Hide all other panels
    {
        mainMenu.GetComponent<TranslatorUI>().ToTarget();
        settings.GetComponent<TranslatorUI>().ToOrigin();
        credits.GetComponent<TranslatorUI>().ToOrigin();
    }
    public void OnSettings() // Show Settings, Hide all other panels
    {
        mainMenu.GetComponent<TranslatorUI>().ToOrigin();
        settings.GetComponent<TranslatorUI>().ToTarget();
        credits.GetComponent<TranslatorUI>().ToOrigin();
    }
    public void OnCredits() // Show Credits, Hide all other panels
    {
        mainMenu.GetComponent<TranslatorUI>().ToOrigin();
        settings.GetComponent<TranslatorUI>().ToOrigin();
        credits.GetComponent<TranslatorUI>().ToTarget();
    }
    public void OnPlay()
    {
        Cursor.visible = false;// unshow cursor
        Cursor.lockState = CursorLockMode.Locked;// lock cursor
        TransitionManager.Instance.LoadScene(playSceneName);

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
