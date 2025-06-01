using UnityEngine;
using TMPro;
using static UnityEditorInternal.ReorderableList;


public class Tank_Behaviour : MonoBehaviour
{
    [Header("SETUP")]
    [SerializeField]
    private int idTank;
    [SerializeField]
    private int player;

    [SerializeField] private GameObject explosionParticles;

    [SerializeField] private GameObject playerNumberGameObj;

    [Header("Player sprites")]
    private SpriteRenderer tank_body;
    [SerializeField] private SpriteRenderer tank_barrel;

    private Color tankColor;

    private void Awake()
    {
        tank_body = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // UI that show the number of the player
        GameObject temp = Instantiate(playerNumberGameObj, this.transform.position, Quaternion.identity);
        temp.GetComponent<PlayerNumberUI>().SetPlayer(this.gameObject);
    }

    public void SetTankColor(Color color)
    {
        tank_body.material.color = color;
        tank_barrel.material.color = color;
    }

    public void Dead() // death function
    {
        // stuff before dying
        Debug.Log(this.gameObject.name + " is dead");
        GameManager.Instance.Freeze(); // hit stop
        GameObject temp = Instantiate(explosionParticles, this.transform.position, Quaternion.identity);// Explotion
        Destroy(temp, temp.GetComponent<AudioSource>().clip.length);


        Destroy(this.gameObject);
    }

    public void SetPlayer1() { player = 1; } // Set player 1
    public void SetPlayer2() { player = 2; } // Set player 2

    public int GetPlayer() { return player; } // return player 
}