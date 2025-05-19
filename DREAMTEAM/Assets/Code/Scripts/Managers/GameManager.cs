using System.Collections;
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

    [Header("Scene management")]
    [SerializeField] private int totalStages = 10;
    private int leftStages;
    [SerializeField] private float timeForNextStage = 3f;
    private float timerNextStage;
    [SerializeField] private List<string> biomesMaps;

    private bool endGame = false;
    public delegate void GetEndGame(bool endGame);
    public event GetEndGame OnEndGame;

    [HideInInspector]
    public bool Spawned = false;

    public float rotationOffset = -90f;

    [Header("Game Stuff")]
    [SerializeField] private GameObject tank1;
    [SerializeField] private GameObject tank2;

    [SerializeField] private static int player1Points = 0;
    [SerializeField] private static int player2Points = 0;
    [SerializeField] private int pointsForDeath = 100;
    private float gridRes = 0.0f;
    [Header("Tanks")]
    [SerializeField] private List<GameObject> tankTypes;

    public static class SelectionMenuData
    {
        public static string TankP1 { get; set; }
        public static string TankP2 { get; set; }
    }


    [Header("PowerUp Stuff")]
    [SerializeField] private GameObject powerUpBase;
    [SerializeField] private float spawnPowerUpTime = 2.5f;
    [SerializeField, Min(0)] private int numMaxPowerUps = 5;
    [SerializeField] private List<PowerUpEffect> powerUpEffects;
    private int numPowerUps = 0;
    private float spawnPowerUpTimer = 0;
    private List<Vector2Int> validPositions = new List<Vector2Int>();

    [Header("Time Event Stuff")]
    [SerializeField] private GameObject timeEvent;
    [SerializeField] private float timeToStartTimeEvent = 30f;
    private float timerToStartTimeEvent = 0f;
    [SerializeField] private float timeEventDuration = 10f;
    [SerializeField] private float timeForTimeEventWarning = 3f;
    private bool timeEventWarnig = false;
    public delegate void TimeEventWarning(bool timeEventWarnig);
    public event TimeEventWarning OnTimeEventWarning;

    [Header("HitPause Stuff")]
    [SerializeField, Range(0,1)] private float freezeDuration = 0.2f;
    private float currentFreezeTime = 0;
    [SerializeField] private AnimationCurve curveFreeze = AnimationCurve.Linear(0, 0, 1, 1);
    private bool isForzen = false;

    [Header("Water Event Stuff (Only Map2)")]
    private bool isMap2 = false;
    [SerializeField] private float timeToStartWaterEvent = 30f;
    private float timerToStartWaterEvent = 0f;
    private bool waterEvent = false;
    public delegate void WaterEvent();
    public event WaterEvent OnWaterEvent;

    [Header("DEBUG")]
    [SerializeField] private bool spawnTanksOnStart = false; // FOR DEBUG ONLY (spawn tanks without marching sqares)
    [SerializeField] private Vector3 tank1SpawnPos = Vector3.zero;
    [SerializeField] private Vector3 tank2SpawnPos = Vector3.zero;


    //Adabtative Music Stuff
    private int musicLevel = 0;
    public delegate void musicLevelEvent(int musicLevel);
    public event musicLevelEvent OnMusicLevelChanging;
    private void Awake()
    {
        if (instance != null && instance != this) // makes GameManager 
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        timerNextStage = timeForNextStage;
        leftStages = totalStages;

        foreach(GameObject tank in tankTypes)// set tank types for the players
        {
            if(tank.name == SelectionMenuData.TankP1)
            {
                tank1 = tank;
            }

            if (tank.name == SelectionMenuData.TankP2)
            {
                tank2 = tank;
            }
        }

    }

    private void Start()
    {
        if (spawnTanksOnStart) // Debug spawn
        {
            GetSpawnLocation(tank1SpawnPos, tank2SpawnPos);
        }
    }

    private void Update()
    {
        if (endGame) // if a player was killed waits to change scene
        {
            timerNextStage -= Time.deltaTime;
            if (timerNextStage <= 0) // change Scene
            {
                if (leftStages > 0)
                {
                    ResetVaiables();
                    // Random between all biomes maps
                    int nextMap = Random.Range(0, biomesMaps.Count);

                    leftStages--; // left stages too end the game
                    Debug.Log("NxtMap: " + nextMap);
                    Debug.Log("LeftStages: " + leftStages);

                    TransitionManager.Instance.LoadScene(biomesMaps[nextMap]);
                    if (biomesMaps[nextMap] == "Map2") // cheks if next scene is map2
                    {
                        isMap2 = true;
                    }
                    else
                    {
                        isMap2 = false;
                    }
                }
                else
                {
                    ResetVaiables();
                    ResetPlayerPoints();
                    Cursor.visible = true;// show cursor
                    Cursor.lockState = CursorLockMode.None;// unlock cursor
                    TransitionManager.Instance.LoadScene("MainMenu");// Go to menu

                }
            }
        }

        

        if (spawnPowerUpTimer < spawnPowerUpTime) // Spawn PowerUps
        {
            spawnPowerUpTimer += Time.deltaTime;
        }
        else
        {
            Debug.Log(numPowerUps <= numMaxPowerUps ? "Spawn powerup" : "Cant spawn a pwUp");
            if (numPowerUps <= numMaxPowerUps)
            {
                SpawnPowerUp(validPositions[Random.Range(0, validPositions.Count - 1)]); // Spawn powerUP 
                spawnPowerUpTimer = 0; // Reset timer
                numPowerUps++;
                AddMusicLevel(); // Add one more level to the adabtative music
            }

        }


        if (timerToStartTimeEvent >= timeToStartTimeEvent) // Time events
        {
            timeEvent.GetComponent<TestTimeEvent>().TriggerRandomEffect(); // Start TimeEvent
            timerToStartTimeEvent = 0; // Reset timer
            timeEventWarnig = false;
            OnTimeEventWarning.Invoke(timeEventWarnig); // call event
        }
        else if (timerToStartTimeEvent >= timeToStartTimeEvent - timeForTimeEventWarning && !timeEventWarnig) // Start Warning (One Time Execute)
        {
            timeEventWarnig = true;
            OnTimeEventWarning.Invoke(timeEventWarnig); // call event
            timerToStartTimeEvent += Time.deltaTime;
        }
        else
        {
            timerToStartTimeEvent += Time.deltaTime;
        }

        if (isMap2)// Water event
        {
            if (timerToStartWaterEvent >= timeToStartWaterEvent && !waterEvent) 
            {
                Debug.Log("Water Event");
                OnWaterEvent.Invoke(); // Invoke Water Event
                waterEvent = true;
            }
            else
            {
                timerToStartWaterEvent += Time.deltaTime;
            }
        }

    }
    private void SpawnPowerUp(Vector2 spawnPoint) // Spawn powerup to the recived position
    {
        spawnPoint *= gridRes;
        GameObject temp1 = Instantiate(powerUpBase, spawnPoint, Quaternion.identity); //instantiate powerup


        int randomPowerUp = Random.Range(0, powerUpEffects.Count); // random betewn all pwUp effects

        temp1.GetComponent<GetPowerUp>().SetPowerUp(powerUpEffects[randomPowerUp]); // Set powerUp effect
    }

    private void EndGame() // start EndGame Stuff
    {
        // show for UI game ended             
        endGame = true; // set gameend true
        OnEndGame.Invoke(endGame);

        
    }

    public void ResetVaiables() // Resets variables for the next Scene
    {
        Debug.Log("Reseting Variables");
        //Reset variables
        endGame = false;
        timerNextStage = timeForNextStage;
        Spawned = false;
        timeEventWarnig = false;
        spawnPowerUpTimer = 0;
        numPowerUps = 0;
        musicLevel = 0;
        waterEvent = false;
        timerToStartWaterEvent = 0;
        currentFreezeTime = 0;
    }

    public void ResetPlayerPoints()
    {
        player1Points = 0;
        player2Points = 0;
    }

    private IEnumerator HitPause() // Slowdown the game for a short time
    {   
        isForzen = true;
        float originalTimeScale = Time.timeScale;
        SetTimeScaleForCurrentTime(originalTimeScale);
        while (currentFreezeTime < freezeDuration)
        {
            currentFreezeTime += Time.deltaTime;

            SetTimeScaleForCurrentTime(originalTimeScale);

            yield return new WaitForEndOfFrame();
        }

        currentFreezeTime = 0;
        Time.timeScale = originalTimeScale;
        isForzen = false;
    }

    private void SetTimeScaleForCurrentTime(float originalTimeScale) // set the timeScale value to the current value of the curve
    {
        float interpolateValue = currentFreezeTime / freezeDuration;
        interpolateValue = curveFreeze.Evaluate(interpolateValue);
        Time.timeScale = originalTimeScale * interpolateValue;
    }

    public void Freeze() // Starts the hitpause
    {
        if (!isForzen) {
            Debug.Log("Freeze");
            StartCoroutine(HitPause());
        }
    }
    public void GetSpawnLocation(Vector3 tank1Pos, Vector3 tank2Pos) // Spawn Player Tanks
    {
        if (Spawned) return;

        GameObject temp1 = Instantiate(tank1, tank1Pos, Quaternion.identity);
        GameObject temp2 = Instantiate(tank2, tank2Pos, Quaternion.identity);

        //Rotation temp2
        Vector2 direction = temp1.transform.position - temp2.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        temp2.transform.rotation = Quaternion.Euler(0f,0f, rotationOffset + (angle));

        //Rotation temp1
        Vector2 direction2 = temp2.transform.position - temp1.transform.position;
        float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
        temp1.transform.rotation = Quaternion.Euler(0f, 0f, rotationOffset + (angle2));

        //spawn tanks

        // set tanks Inputs
        temp1.GetComponent<Tank_Behaviour>().SetPlayer1();
        temp2.GetComponent<Tank_Behaviour>().SetPlayer2();

        if(temp1.GetComponent<PlayerWaterDetector>().IsInWater || temp2.GetComponent<PlayerWaterDetector>().IsInWater)
        {
            endGame = true;
        }
        //------------------------------------START GAME
        Spawned = true;
    }

    public void GetTank1IsDead() // add points too tank2 and Starts the endGame
    {
        player2Points += pointsForDeath; // add points

        Debug.Log("p2: " + player2Points);

        EndGame();// end game function
    }
    public void GetTank2IsDead()// add points too tank1 and Starts the endGame
    {
        player1Points += pointsForDeath; // add poitns

        Debug.Log("p1: " + player1Points);

        EndGame(); // end game function
    }

    

    public int GetPlayer1Points() { return player1Points; }

    public int GetPlayer2Points() { return player2Points; }

    public int GetLeftStages() { return leftStages; }
    
    public float GetTimeForNextStage() { return timerNextStage; }

    public bool IsForzen() { return isForzen; }

    public void DecreaseNumPowerUps() 
    {
        numPowerUps--;
    }
    public void SetValidPositions(List<Vector2Int> validPositions, float gridRes) // set valid positions to spawn the powerUps
    {
        this.validPositions = validPositions;
        this.gridRes = gridRes;
    }

    private void AddMusicLevel() // add a level to the adabtative music
    {
        if (musicLevel <= 6) 
        { // MaxMusicLevelCases
            musicLevel++;
            OnMusicLevelChanging.Invoke(musicLevel);
        }
    }

    public void ReduceMusicLevel() // reduce a level to the adabtative music
    {
        if(musicLevel != 0)
        {
            musicLevel--;
            OnMusicLevelChanging.Invoke(musicLevel);
        }
    }

}
