using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;


public class ScoreManager : MonoBehaviour
{
    [Header("Setup")]

    [SerializeField]private TextMeshProUGUI score1TMP;
    [SerializeField] private TextMeshProUGUI score2TMP;
    [SerializeField] private TextMeshProUGUI leftStagesTMP;
    [SerializeField] private TextMeshProUGUI gameOverTitlteTMP;
    [SerializeField] private GameObject gameOver;
    private bool endGame;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Color p1Color;
    [SerializeField] private Color p2Color;

    private int timeForNextStage = 3;
    private int leftStages = 0;

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }

    private void Start()
    {
        leftStages = GameManager.Instance.GetLeftStages();
        leftStagesTMP.text = "Left Stages: " + leftStages;
        gameOverTitlteTMP.text = "";


        GameManager.Instance.OnEndGame += SetNextStage;
    }

    void Update()
    {
        score1TMP.text = "Player 1: " + GameManager.Instance.GetPlayer1Points().ToString();
        score2TMP.text = "Player 2: " + GameManager.Instance.GetPlayer2Points().ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        if (endGame)
        {
            if (leftStages <= 0) // last stage?
            {
                // who win
                if (GameManager.Instance.GetPlayer1Points() > GameManager.Instance.GetPlayer2Points())
                { // player 1 win
                    gameOverTitlteTMP.text = "Player 1 WIN";
                    gameOverTitlteTMP.color = p1Color;
                    Debug.Log("PLAYER 1 WIN");
                }
                else if (GameManager.Instance.GetPlayer1Points() < GameManager.Instance.GetPlayer2Points())
                { // player 2 win
                    gameOverTitlteTMP.text = "Player 2 WIN";
                    gameOverTitlteTMP.color = p2Color;
                    Debug.Log("PLAYER 2 WIN");
                }
                else
                {// TIE
                    gameOverTitlteTMP.text = "TIE";
                    gameOverTitlteTMP.color = Color.grey;
                    Debug.Log("TIE");
                }
            }
            else if (timeForNextStage > 0)
            {
                timeForNextStage = (int)GameManager.Instance.GetTimeForNextStage();

                gameOverTitlteTMP.text = "Next stage in: " + timeForNextStage.ToString();
            }
        }
        
    }

    private void SetNextStage (bool timeEventWarnig)
    {
        endGame = true;
        gameOver.GetComponent<TranslatorUI>().ToTarget();
    }


    private void OnDestroy()
    {
        GameManager.Instance.OnEndGame -= SetNextStage;
    }
}
