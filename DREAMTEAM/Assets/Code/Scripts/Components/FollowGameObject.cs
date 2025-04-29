using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerNumberUI : MonoBehaviour
{
    [Header("SETUP")]
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private Color colorPlayer1;
    [SerializeField] private Color colorPlayer2;
    [SerializeField] private float numberDuration = 3;
    [SerializeField] private TextMeshProUGUI playerTextNumber;
    private SpriteRenderer sr;
    private GameObject followGameObject;
    private int idPlayer;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (followGameObject != null)
        {
            transform.position = followGameObject.transform.position + offset;
            transform.rotation = Quaternion.Euler(0f, 0f, 180);
        }
    }

    public void SetPlayer(GameObject playerReference)
    {

        
        followGameObject = playerReference;

        idPlayer = playerReference.GetComponent<Tank_Behaviour>().GetPlayer();  // Setplayer

        playerTextNumber.text = idPlayer.ToString(); // set text

        switch (idPlayer) // set color depending off the player
        {
            case 1:
                sr.color = colorPlayer1;
                break;
            case 2:
                sr.color = colorPlayer2;
                break;
        }

        Destroy(this.gameObject, numberDuration);
    }
}
