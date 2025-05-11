using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_Behaviour : MonoBehaviour
{


    private void OnEnable()
    {
        Time.timeScale = 0; // Pause Time Scale
        Cursor.visible = true;// show cursor
        Cursor.lockState = CursorLockMode.None;// unlock cursor
    }

    private void Start()
    {
        //translatorUI.ToTarget();// start animation
    }

    private void OnDisable()
    {
        Time.timeScale = 1; // Set normal TimeScale
        Cursor.visible = false;// unshow cursor
        Cursor.lockState = CursorLockMode.Locked;// lock cursor
        

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
        Cursor.visible = false;// unshow cursor
        Cursor.lockState = CursorLockMode.Locked;// lock cursor
    }
}
