using Unity.VisualScripting;
using UnityEngine;

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

    [SerializeField] private int player1Points = 0;
    [SerializeField] private int player2Points = 0;
    [SerializeField] private int pointsForDeath = 100;

    [Header("Tank stuff")]
    [SerializeField] private bool isAliveTank1 = false;
    [SerializeField] private bool isAliveTank2 = false;


    private void Start()
    {
        GetSpawnLocation(new Vector3(0,0,0), new Vector3(0,0,0)); //DEBUG
    }


    public void GetSpawnLocation(Vector3 tank1Pos, Vector3 tank2Pos)
    {
        //fore position to debug
        tank1Pos = new Vector3(Random.Range(-9, 9), Random.Range(-5, 5), 0);
        tank2Pos = new Vector3(Random.Range(-9, 9), Random.Range(-5, 5), 0);


        //Set position
        tank1.transform.position = tank1Pos;
        tank2.transform.position = tank2Pos;

        //look at center 
        if (tank1Pos.x < 0)
            tank1.transform.rotation = Quaternion.Euler(0, 90, 0);
        else
            tank1.transform.rotation = Quaternion.Euler(0, 270, 0);

        if (tank2Pos.x < 0)
            tank2.transform.rotation = Quaternion.Euler(0, 90, 0);
        else
            tank2.transform.rotation = Quaternion.Euler(0, 270, 0);


        //spawn tanks
        Instantiate(tank1);
        Instantiate(tank2);

        //------------------------------------START GAME
    }

    public void GetTank1IsDead()
    {

    }
    public void GetTank2IsDead()
    {

    }
}
