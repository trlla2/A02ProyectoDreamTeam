using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TankSelection : MonoBehaviour
{
    [Header("SETUP")]
    [Header("Tank1")]
    [SerializeField] private Slider firerateBarP1;
    [SerializeField] private Slider speedBarP1;
    //[Header("Tank2")]
    //[SerializeField] private Slider firerateBarP2;
    //[SerializeField] private Slider speedBarP2;
    [Header("Tanks")]
    [SerializeField] private List<GameObject> tankTypes;
    private int currentP1Tank = 1;
    private GameObject currentP2Tank;

    private void Start()
    {
        Debug.Log("Tank types: " + tankTypes.Count);
    }



    public void SwitchLeftTankPlayer1()
    {
        if(currentP1Tank > 0)
        {
            currentP1Tank--;
            UpdateP1Slider();
        }
    }



    public void SwitctRightTankPlayer1()
    {
        if (currentP1Tank < tankTypes.Count)
        {
            currentP1Tank++;
            UpdateP1Slider();
        }
    }

    private void UpdateP1Slider()
    {
        firerateBarP1.value = tankTypes[currentP1Tank].GetComponent<Weapon>().GetFireRate();
        speedBarP1.value = tankTypes[currentP1Tank].GetComponent<TankMovement>().GetSpeed();
    }
}
