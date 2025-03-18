using UnityEngine;

public class Tank_Behaviour : MonoBehaviour
{
    [Header("SETUP")]
    [SerializeField]
    private int player;



    public void Dead() // death function
    {
        // stuff before dying

        Destroy(this.gameObject);
    }

    public void SetPlayer1() { player = 1; } // Set player 1
    public void SetPlayer2() { player = 2; } // Set player 2

    public int GetPlayer() { return player; } // return player
}
