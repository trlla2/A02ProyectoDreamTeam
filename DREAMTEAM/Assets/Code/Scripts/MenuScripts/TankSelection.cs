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
    [Header("Tank2")]
    [SerializeField] private Slider firerateBarP2;
    [SerializeField] private Slider speedBarP2;
    [Header("Tanks")]
    [SerializeField] private List<GameObject> tankTypes;
    private int currentP1Tank = 0;
    private int currentP2Tank = 0;


    private void Start()
    {
        Debug.Log("Tank types: " + tankTypes.Count);
        UpdateP1Slider();
        UpdateP2Slider();
    }

    private void UpdateP1Slider()
    {
        firerateBarP1.value = 1.5f - tankTypes[currentP1Tank].GetComponent<Weapon>().GetFireRate();
        speedBarP1.value = tankTypes[currentP1Tank].GetComponent<TankMovement>().GetSpeed();
    }
    private void UpdateP2Slider()
    {
        firerateBarP2.value = 1.5f - tankTypes[currentP2Tank].GetComponent<Weapon>().GetFireRate();
        speedBarP2.value = tankTypes[currentP2Tank].GetComponent<TankMovement>().GetSpeed();
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
        if (currentP1Tank < tankTypes.Count - 1)
        {
            currentP1Tank++;
            UpdateP1Slider();
        }
    }

    public void SwitchLeftTankPlayer2()
    {
        if (currentP2Tank > 0)
        {
            currentP2Tank--;
            UpdateP2Slider();
        }
    }

    public void SwitctRightTankPlayer2()
    {
        if (currentP2Tank < tankTypes.Count - 1)
        {
            currentP2Tank++;
            UpdateP2Slider();
        }
    }

    public void Play()
    {
        Debug.LogWarning("FALTA IMPLEMENTAR EL LOAD SCENE");
    } 
}
