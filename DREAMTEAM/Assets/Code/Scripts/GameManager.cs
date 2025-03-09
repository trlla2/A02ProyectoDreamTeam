using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    public float rotationOffset = -90f;

    private void Awake()
    {
        GetSpawnLocation(new Vector3(0,0,0), new Vector3(0,0,0)); //DEBUG
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
        temp1.GetComponent<TankMovement>().SetPlayer1();
        temp2.GetComponent<TankMovement>().SetPlayer2();


        //------------------------------------START GAME
    }

    public void GetTank1IsDead()
    {
        player2Points += pointsForDeath;

        Debug.Log("p2: " + player2Points);
    }
    public void GetTank2IsDead()
    {
        player1Points += pointsForDeath;

        Debug.Log("p1: " + player1Points);

    }
}
