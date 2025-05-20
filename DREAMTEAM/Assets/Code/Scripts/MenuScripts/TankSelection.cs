using System.Collections;
using System.Collections.Generic;
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
    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    private WaitForEndOfFrame currentWaitForEndOfFrame = new WaitForEndOfFrame();
    private int lastP1Tank = 0;
    private int lastP2Tank = 0;
    private bool isP1Changing = false;
    private bool isP2Changing = false;

    private void Start()
    {
        StartCoroutine(UpdateSlider(true));
        StartCoroutine(UpdateSlider(false));
    }

    private IEnumerator UpdateSlider(bool isP1)
    {
        while (currentTime < animationDuration)
        {
            currentTime += Time.deltaTime;

            if (isP1)
            {
                isP1Changing = true;
                SetCurrentSliderP1Value();
            }
            else
            {
                isP2Changing = true;
                SetCurrentSliderP2Value();
            }


            yield return currentWaitForEndOfFrame;
        }

        currentTime = animationDuration;

        SetCurrentSliderP1Value();

        currentTime = 0;// reset
        if (isP1)
            isP1Changing = false;
        else
            isP2Changing = false;
    }

    private void SetCurrentSliderP1Value()
    {
        float interpolatedValue = currentTime / animationDuration;
        interpolatedValue = curve.Evaluate(interpolatedValue);

        float velocityBarDisplacement = tankTypes[currentP1Tank].GetComponent<TankMovement>().GetSpeed() - tankTypes[lastP1Tank].GetComponent<TankMovement>().GetSpeed();
        speedBarP1.value = tankTypes[lastP1Tank].GetComponent<TankMovement>().GetSpeed() + (velocityBarDisplacement * interpolatedValue);

        float fireRateBarDisplacement = tankTypes[currentP1Tank].GetComponent<Weapon>().GetFireRate() - tankTypes[lastP1Tank].GetComponent<Weapon>().GetFireRate();
        firerateBarP1.value = 2 - (tankTypes[lastP1Tank].GetComponent<Weapon>().GetFireRate() + (fireRateBarDisplacement * interpolatedValue));
    }
    private void SetCurrentSliderP2Value()
    {
        float interpolatedValue = currentTime / animationDuration;
        interpolatedValue = curve.Evaluate(interpolatedValue);

        float velocityBarDisplacement = tankTypes[currentP2Tank].GetComponent<TankMovement>().GetSpeed() - tankTypes[lastP2Tank].GetComponent<TankMovement>().GetSpeed();
        speedBarP2.value = tankTypes[lastP2Tank].GetComponent<TankMovement>().GetSpeed() + (velocityBarDisplacement * interpolatedValue);

        float fireRateBarDisplacement = tankTypes[currentP2Tank].GetComponent<Weapon>().GetFireRate() - tankTypes[lastP2Tank].GetComponent<Weapon>().GetFireRate();
        firerateBarP2.value = 2 - (tankTypes[lastP2Tank].GetComponent<Weapon>().GetFireRate() + (fireRateBarDisplacement * interpolatedValue));
    }

    public void SwitchLeftTankPlayer1()
    {
        if(currentP1Tank > 0 && !isP1Changing)
        {
            isP1Changing = true;
            lastP1Tank = currentP1Tank;
            currentP1Tank--;
            Debug.Log("Current P1 tank " + tankTypes[currentP1Tank].name);
            StartCoroutine(UpdateSlider(true));
        }
    }

    public void SwitctRightTankPlayer1()
    {
        if (currentP1Tank < tankTypes.Count - 1 && !isP1Changing)
        {
            isP1Changing = true;
            lastP1Tank = currentP1Tank;
            currentP1Tank++;
            Debug.Log("Current P1 tank " + tankTypes[currentP1Tank].name);
            StartCoroutine(UpdateSlider(true));
        }
    }

    public void SwitchLeftTankPlayer2()
    {
        if (currentP2Tank > 0 && !isP2Changing)
        {
            isP2Changing = true;
            lastP2Tank = currentP2Tank;
            currentP2Tank--;
            Debug.Log("Current P2 tank " + tankTypes[currentP2Tank].name);
            StartCoroutine(UpdateSlider(false));
        }
    }

    public void SwitctRightTankPlayer2()
    {
        if (currentP2Tank < tankTypes.Count - 1 && !isP2Changing)
        {
            isP2Changing = true;
            lastP2Tank = currentP2Tank;
            currentP2Tank++;
            Debug.Log("Current P2 tank " + tankTypes[currentP2Tank].name);
            StartCoroutine(UpdateSlider(false));
        }
    }

    public void Play()
    {

        PlayerPrefs.SetString("TankP1", tankTypes[currentP1Tank].name); // set tank types
        PlayerPrefs.SetString("TankP2", tankTypes[currentP2Tank].name);
        PlayerPrefs.Save();

        Cursor.visible = false; // unshow cursor
        Cursor.lockState = CursorLockMode.Locked; // lock cursor
        TransitionManager.Instance.LoadScene("Map1"); // load map1
        Debug.Log("Loading scene");
    }
}
