using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerNumberUI : MonoBehaviour
{
    [Header("SETUP")]
    [SerializeField] private Vector3 offset = Vector3.zero;
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
            transform.rotation = Quaternion.Euler(0f, 0f, 180); // rotation offset
        }
    }

    public void SetPlayer(GameObject playerReference)
    {

        
        followGameObject = playerReference;

        idPlayer = playerReference.GetComponent<Tank_Behaviour>().GetPlayer();  // Setplayer

        playerTextNumber.text = idPlayer.ToString(); // set text
        if(idPlayer == 1)
            sr.color = GameManager.Instance.LoadColor("TankP1");// set color
        else
            sr.color = GameManager.Instance.LoadColor("TankP2");// set color

        Destroy(this.gameObject, numberDuration);
    }
}
