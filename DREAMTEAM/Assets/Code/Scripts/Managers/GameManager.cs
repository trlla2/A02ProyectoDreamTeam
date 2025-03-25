using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour 
{

 // Manager stuff
    private static GameManager instance;
    static public GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();

            }
            return instance;
        }
    }

    [Header("Scene management")]
    [SerializeField] private const int totalStages = 10;
    private static int leftStages = totalStages;
    [SerializeField] private static float timeForNextStage = 3f;
    private static float timerNextStage = 0;
    [SerializeField] private List<string> biomesMaps;

    private bool endGame = false;
    private bool nextStage = false;

    [HideInInspector]
    public bool Spawned = false;

    public float rotationOffset = -90f;

    [Header("Game Stuff")]
    [SerializeField] private GameObject tank1;
    [SerializeField] private GameObject tank2;

    [SerializeField] private static int player1Points = 0;
    [SerializeField] private static int player2Points = 0;
    [SerializeField] private int pointsForDeath = 100;

    [Header("PowerUp Stuff")]
    [SerializeField] private GameObject powerUpBase;
    [SerializeField] private List<PowerUpEffect> powerUpEffects;
    [SerializeField] private float spawnPowerUpTime = 5;
    private float spawnPowerUpTimer = 0;


   

    

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
    }

    private void Update()
    {
        if (endGame)
        {// timer chungo para cambiar de escena
            timerNextStage += Time.deltaTime;
            if(timerNextStage >= timeForNextStage)
            {
                nextStage = true;
            }
        }

        if (nextStage) { 
            endGame = false;
            nextStage = false;
            Debug.Log("leftStages: " + leftStages);
            if (leftStages > 0)
            {
                // Random between all biomes maps
                int nextMap = Random.Range(0, biomesMaps.Count - 1);

                leftStages--; // left stages too end the game
                Debug.Log("NxtMap: " + nextMap);
                Debug.Log("LeftStages: " + leftStages);
                SceneManager.LoadScene(biomesMaps[nextMap]);
            }
            else
            {
                leftStages = totalStages;
                SceneManager.LoadScene("MainMenu"); // Go to menu
            }
        }

        if(spawnPowerUpTimer < spawnPowerUpTime)
        {
            spawnPowerUpTimer += Time.deltaTime;
        }
        else
        {
            SpawnPowerUp(new Vector2(0,0)); // Spawn powerUP
            spawnPowerUpTimer = 0; // Reset timer
        }
    }
  
    public void GetSpawnLocation(Vector3 tank1Pos, Vector3 tank2Pos)
    {
        if (Spawned) return;

        GameObject temp1 = Instantiate(tank1, tank1Pos, Quaternion.identity);
        GameObject temp2 = Instantiate(tank2, tank2Pos, Quaternion.identity);

        //Rotation temp2
        Vector2 direction = temp1.transform.position - temp2.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        temp2.transform.rotation = Quaternion.Euler(0f,0f, rotationOffset + (angle));

        //Rotation temp1
        Vector2 direction2 = temp2.transform.position - temp1.transform.position;
        float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
        Debug.Log(angle2);
        temp1.transform.rotation = Quaternion.Euler(0f, 0f, rotationOffset + (angle2));

        //spawn tanks

        // set tanks Inputs
        temp1.GetComponent<Tank_Behaviour>().SetPlayer1();
        temp2.GetComponent<Tank_Behaviour>().SetPlayer2();


        //------------------------------------START GAME
        Spawned = true;
    }

    public void GetTank1IsDead()
    {
        player2Points += pointsForDeath; // add points

        Debug.Log("p2: " + player2Points);

        EndGame();// end game function
    }
    public void GetTank2IsDead()
    {
        player1Points += pointsForDeath; // add poitns

        Debug.Log("p1: " + player1Points);

        EndGame(); // end game function
    }

    private void EndGame()
    {
        // show for UI game ended             
        endGame = true; // set gameend true
    }
    public bool GetEndGame()
    {
        return endGame;
    }
    public void GoNextStage()
    {
        nextStage = true; // Go to the next stage
    }

    public int GetPlayer1Points()
    {
        return player1Points;
    }

    public int GetPlayer2Points()
    {
        return player2Points;
    }

    private void SpawnPowerUp(Vector2 spawnPoint)
    {
        GameObject temp1 = Instantiate(powerUpBase, spawnPoint, Quaternion.identity); //instantiate powerup

        int randomPowerUp = Random.Range(0, powerUpEffects.Count - 1); // random betewn all pwUp effects

        temp1.GetComponent <GetPowerUp>().SetPowerUpEffect(powerUpEffects[randomPowerUp]); // Set powerUp effect
    }
}
