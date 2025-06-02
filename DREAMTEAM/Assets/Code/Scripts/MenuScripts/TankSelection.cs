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
    [SerializeField] private RawImage p1Image;
    [Header("Tank2")]
    [SerializeField] private Slider firerateBarP2;
    [SerializeField] private Slider speedBarP2;
    [SerializeField] private RawImage p2Image;
    [Header("Tanks")]
    [SerializeField] private List<GameObject> tankTypes;
    [SerializeField] private List<Color> tank1Colors;
    [SerializeField] private List<Color> tank2Colors;
    private int currentP1Tank = 0;
    private int currentP2Tank = 0;
    private int currentP1Color = 0;
    private int currentP2Color = 0;

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

        p1Image.color = tank1Colors[currentP1Color];
        p2Image.color = tank2Colors[currentP2Color];

    }
    private void SaveColor(string key, Color color)
    {
        PlayerPrefs.SetFloat(key + "_r", color.r);
        PlayerPrefs.SetFloat(key + "_g", color.g);
        PlayerPrefs.SetFloat(key + "_b", color.b);
        PlayerPrefs.SetFloat(key + "_a", color.a);
        PlayerPrefs.Save();
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

    public void SwitchLeftColorTankPlayer1()
    {
        if(currentP1Color > 0)
        {
            currentP1Color--;
            p1Image.color = tank1Colors[currentP1Color];
        }
    }

    public void SwitchRightColorTankPlayer1()
    {
        if (currentP1Color < tank1Colors.Count - 1)
        {
            currentP1Color++;
            p1Image.color = tank1Colors[currentP1Color];
        }
    }
    public void SwitchLeftColorTankPlayer2()
    {
        if(currentP2Color > 0)
        {
            currentP2Color--;
            p2Image.color = tank2Colors[currentP2Color];
        }
    }

    public void SwitchRightColorTankPlayer2()
    {
        if (currentP2Color < tank1Colors.Count - 1)
        {
            currentP2Color++;
            p2Image.color = tank2Colors[currentP2Color];
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
        SaveColor("TankP1", tank1Colors[currentP1Color]);
        SaveColor("TankP2", tank2Colors[currentP2Color]);
        PlayerPrefs.Save();

        Cursor.visible = false; // unshow cursor
        Cursor.lockState = CursorLockMode.Locked; // lock cursor
        TransitionManager.Instance.LoadScene("Map1"); // load map1
        Debug.Log("Loading scene");
    }
}
