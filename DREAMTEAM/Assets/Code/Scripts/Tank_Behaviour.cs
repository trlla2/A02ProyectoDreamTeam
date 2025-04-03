using UnityEngine;

public class Tank_Behaviour : MonoBehaviour
{
    [Header("SETUP")]
    [SerializeField]
    private int player;

    [SerializeField] private GameObject explosionParticles;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer topRenderer;
    [SerializeField] private SpriteRenderer barrelRenderer;

    [SerializeField] private Sprite tank1_body;
    [SerializeField] private Sprite tank2_body;
    [SerializeField] private Sprite tank1_barrel;
    [SerializeField] private Sprite tank2_barrel;
    [SerializeField] private Material tank1_material;
    [SerializeField] private Material tank2_material;
    [SerializeField] private Material tank1_top_material;
    [SerializeField] private Material tank2_top_material;
    [SerializeField] private Material tank1_barrel_material;
    [SerializeField] private Material tank2_barrel_material;

    void Start()
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
    }


    public void Dead() // death function
    {
        // stuff before dying
        Camera.main.GetComponent<ScreenShake>().Shake();// CameraShake

        GameObject temp = Instantiate(explosionParticles, this.transform.position, Quaternion.identity);// Explotion
        Destroy(temp, temp.GetComponent<ParticleSystem>().main.duration);

        
        Destroy(this.gameObject);
    }

    public void SetPlayer1() { player = 1; } // Set player 1
    public void SetPlayer2() { player = 2; } // Set player 2

    public int GetPlayer() { return player; } // return player 
}
