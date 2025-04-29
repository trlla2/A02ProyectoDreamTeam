using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;


public class ScoreManager : MonoBehaviour
{
    [Header("Setup")]

    [SerializeField]private TextMeshProUGUI score1;
    [SerializeField] private TextMeshProUGUI score2;
    [SerializeField] private TextMeshProUGUI leftStages;
    [SerializeField] private TextMeshProUGUI gameOverTitlte;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }

    private void Start()
    {
        leftStages.text = "Left Stages: " + GameManager.Instance.GetLeftStages();
        gameOverTitlte.text = "";

    }

    void Update()
    {
        score1.text = "Player 1: " + GameManager.Instance.GetPlayer1Points().ToString();
        score2.text = "Player 2: " + GameManager.Instance.GetPlayer2Points().ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        if (GameManager.Instance.GetEndGame())
        {
            int tmp = (int)GameManager.Instance.GetTimeForNextStage();

            gameOverTitlte.text = "Next stage in: " + tmp.ToString();
        }
    }
}
