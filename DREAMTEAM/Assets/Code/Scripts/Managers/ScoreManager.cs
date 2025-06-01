using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using UnityEngine.UI;


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
    private Color p1Color;
    private Color p2Color;
    [SerializeField] private RawImage tank1Image;
    [SerializeField] private RawImage tank2Image;
    //Changing UI of the PowerUps of the players
    private GameObject imageUI1;
    private GameObject imageUI2;
    private Sprite originalSpritePlayer1;
    private Sprite originalSpritePlayer2;

    private int timeForNextStage = 3;
    private int leftStages = 0;

    private int player1Points = 0;
    private int player2Points = 0;

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }

    private void Start()
    {
        leftStages = GameManager.Instance.GetLeftStages();
        leftStagesTMP.text = "Left Stages: " + leftStages;
        gameOverTitlteTMP.text = "";

        GameObject uiCanvas = GameObject.Find("Canvas");

        if (uiCanvas != null)
        {
            imageUI1 = uiCanvas.transform.Find("PwPlayer1")?.gameObject;
            imageUI2 = uiCanvas.transform.Find("PwPlayer2")?.gameObject;
        }

        GameManager.Instance.OnEndGame += SetNextStage;


        // set player color
        p1Color = GameManager.Instance.LoadColor("TankP1");
        p2Color = GameManager.Instance.LoadColor("TankP2");
        tank1Image.color = p1Color;
        tank2Image.color = p2Color;

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
                if(player1Points == 0 && player2Points == 0)
                {
                    player1Points = GameManager.Instance.GetPlayer1Points();
                    player2Points = GameManager.Instance.GetPlayer2Points();
                }
                // who win
                if (player1Points > player2Points)
                { // player 1 win
                    gameOverTitlteTMP.text = "Player 1 WIN";
                    gameOverTitlteTMP.color = p1Color;
                    Debug.Log("PLAYER 1 WIN");
                }
                else if (player1Points < player2Points)
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

    public void ChangePowerUpUI(PowerUpEffect powerUp, int playerNumber)
    {
        GameObject imageUI = (playerNumber == 1) ? imageUI1 : imageUI2;

        // Activar el objeto UI en caso de que est� desactivado
        imageUI.SetActive(true);

        // Cambiar el sprite por el del power-up activo
        Image img = imageUI.GetComponent<Image>();

        if (playerNumber == 1 && originalSpritePlayer1 == null)
            originalSpritePlayer1 = img.sprite;
        else if (playerNumber == 2 && originalSpritePlayer2 == null)
            originalSpritePlayer2 = img.sprite;

        if (img != null && powerUp.powerUpSprite != null)
        {
            img.sprite = powerUp.powerUpSprite;
        }

        // Ocultar despu�s de X segundos
        StartCoroutine(HideAfterDelay(imageUI, 5f, playerNumber)); // o el tiempo que dure el power-up
    }


    private IEnumerator HideAfterDelay(GameObject imageUI, float delay, int playerNumber)
    {
        yield return new WaitForSeconds(delay);
        Image img = imageUI.GetComponent<Image>();
        if (img != null)
        {
            if (playerNumber == 1 && originalSpritePlayer1 != null)
                img.sprite = originalSpritePlayer1;
            else if (playerNumber == 2 && originalSpritePlayer2 != null)
                img.sprite = originalSpritePlayer2;
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
