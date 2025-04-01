using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NewBehaviourScript : MonoBehaviour
{
    [Header("Setup")]

    [SerializeField]private TextMeshProUGUI score1;
    [SerializeField] private TextMeshProUGUI score2;
    [SerializeField] private GameObject gameOverTitlte;
    [SerializeField] private GameObject pauseMenu;

    void Update()
    {
        score1.text = "player 1 score: " + GameManager.Instance.GetPlayer1Points().ToString();
        score2.text = "player 2 score: " + GameManager.Instance.GetPlayer2Points().ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}
