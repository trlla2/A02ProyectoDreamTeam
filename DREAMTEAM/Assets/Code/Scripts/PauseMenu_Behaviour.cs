using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_Behaviour : MonoBehaviour
{

    //[Header("sfx")] ---------------------------------------------- Depenendcy SFX
    //[SerializeField]
    //private AudioSource clickSFX;

    private void OnEnable()
    {
        Time.timeScale = 0; // Pause Time Scale
        UnityEngine.Cursor.visible = true;// show cursor
        UnityEngine.Cursor.lockState = CursorLockMode.None;// unlock cursor
    }

    private void OnDisable()
    {
        Time.timeScale = 1; // Set normal TimeScale
        UnityEngine.Cursor.visible = false;// unshow cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;// lock cursor
    }

    public void Resume()
    {
        OnClickButton(); // Click Behaviour

        OnExitPauseMenu();// Exit Pause Menu Behaviour

        this.gameObject.SetActive(false); // Disable Pause Menu
    }

    public void MainMenu()
    {
        OnClickButton(); // Click Behaviour

        OnExitPauseMenu();// Exit Pause Menu Behaviour

        SceneManager.LoadScene("MainMenu"); 
    }

    private void OnClickButton()
    {
        //clickSFX.Play(); ---------------------------------------------- Depenendcy SFX
    }

    private void OnExitPauseMenu()
    {
        Time.timeScale = 1; // Set normal TimeScale
        UnityEngine.Cursor.visible = false;// unshow cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;// lock cursor
    }
}
