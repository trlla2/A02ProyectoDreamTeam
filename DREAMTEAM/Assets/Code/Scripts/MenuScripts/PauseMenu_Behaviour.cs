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

    private void OnDisable()
    {
        Time.timeScale = 1; // Set normal TimeScale
        Cursor.visible = false;// unshow cursor
        Cursor.lockState = CursorLockMode.Locked;// lock cursor
    }

    public void Resume()
    {
        OnExitPauseMenu();// Exit Pause Menu Behaviour

        this.gameObject.SetActive(false); // Disable Pause Menu
    }

    public void MainMenu()
    {
        OnExitPauseMenu();// Exit Pause Menu Behaviour

        TransitionManager.Instance.LoadScene("MainMenu");
    }

    private void OnExitPauseMenu()
    {
        Time.timeScale = 1; // Set normal TimeScale
        Cursor.visible = false;// unshow cursor
        Cursor.lockState = CursorLockMode.Locked;// lock cursor
        GameManager.Instance.ResetVaiables();
        GameManager.Instance.ResetPlayerPoints();
    }
}
