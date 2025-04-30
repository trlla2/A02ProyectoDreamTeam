using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    // Manager stuff
    private static TransitionManager instance;
    static public TransitionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TransitionManager();

            }
            return instance;
        }
    }

    [Header("SETUP")]
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // calls when a new scene is loaded
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionAnimation(sceneName)); // Start transition animation
    }

    private IEnumerator TransitionAnimation(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single) // Trigger only when loading in
            transition.SetTrigger("End");

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
