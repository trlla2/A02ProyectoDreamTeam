using UnityEngine;
using TMPro;


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
    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer topRenderer;
    [SerializeField] private SpriteRenderer barrelRenderer;

    [SerializeField] private Sprite tank1_body;
    [SerializeField] private Sprite tank1_barrel;

    [SerializeField] private Material tank1_material;
    [SerializeField] private Material tank1_top_material;
    [SerializeField] private Material tank1_barrel_material;


    [SerializeField] private Sprite tank2_body;
    [SerializeField] private Sprite tank2_barrel;

    [SerializeField] private Material tank2_barrel_material;
    [SerializeField] private Material tank2_top_material;
    [SerializeField] private Material tank2_material;


    
    
    public void SetTankColor(Color color)
    {
        if (GetPlayer() == 1)
        {
            tank1_material.color = color;
            tank1_barrel_material.color = color;
            tank1_top_material.color = color;
        }
        else
        {
            tank2_material.color = color;
            tank2_barrel_material.color = color;
            tank2_top_material.color = color;
        }

    }


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();



        if (GetPlayer() == 1)
        {
            spriteRenderer.sprite = tank1_body;
            spriteRenderer.material = tank1_material;

            //topRenderer.sprite = tank1_top;
            topRenderer.material = tank1_top_material;
            barrelRenderer.sprite = tank1_barrel;
            barrelRenderer.material = tank1_barrel_material;
        }
        else if (GetPlayer() == 2)
        {
            spriteRenderer.sprite = tank2_body;
            spriteRenderer.material = tank2_material;

            //topRenderer.sprite = tank1_top;
            topRenderer.material = tank2_top_material;
            barrelRenderer.sprite = tank2_barrel;
            barrelRenderer.material = tank2_barrel_material;
        }

        // UI that show the number of the player
        GameObject temp = Instantiate(playerNumberGameObj, this.transform.position, Quaternion.identity);
        temp.GetComponent<PlayerNumberUI>().SetPlayer(this.gameObject);
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
