using JetBrains.Annotations;
using System.Collections.Generic;
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
    [Header("Game Stuff")]
    [SerializeField] private GameObject tank1;
    [SerializeField] private GameObject tank2;

    [SerializeField] private static int player1Points = 0;
    [SerializeField] private static int player2Points = 0;
    [SerializeField] private int pointsForDeath = 100;

    [SerializeField] private const int totalStages = 10;
    private static int leftStages = totalStages;

    private bool nextStage = false;

    [Header("Load Scene Stuff")]
    [SerializeField] private List<string> biomesMaps;

    private static bool endGame = false;

    public float rotationOffset = -90f;

    private void Awake()
    {
        GetSpawnLocation(new Vector3(0,0,0), new Vector3(0,0,0)); //DEBUG
    }

    private void Update()
    {
        if (endGame && nextStage) { 
            endGame = false;

            // Random between all biomes maps
            int nextMap = Random.Range(0, biomesMaps.Count - 1);

            leftStages--; // left stages too end the game
            Debug.Log("NxtMap: " + nextMap);
            Debug.Log("LeftStages: " + leftStages);
            SceneManager.LoadScene(biomesMaps[nextMap]);
        }
    }

    public void GetSpawnLocation(Vector3 tank1Pos, Vector3 tank2Pos)
    {

        GameObject temp1 = Instantiate(tank1);
        GameObject temp2 = Instantiate(tank2);
       
        
        //fore position to debug
        tank1Pos = new Vector3(Random.Range(-9, 9), Random.Range(-5, 5), 0);
        tank2Pos = new Vector3(Random.Range(-9, 9), Random.Range(-5, 5), 0);

        //Set position
        temp1.transform.position = tank1Pos;
        temp2.transform.position = tank2Pos;

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

        Debug.Log("Pres x to continue");

        endGame = true; // set gameend true
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
}
