using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NewBehaviourScript : MonoBehaviour
{
    private TextMeshProUGUI Score1;
    private TextMeshProUGUI Score2;

    void Start()
    {
        
        Score1 = GameObject.Find("player 1 score").GetComponent<TextMeshProUGUI>();
        Score2 = GameObject.Find("player 2 score").GetComponent<TextMeshProUGUI>();

    }

    void Update()
    {
        Score1.text = "player 1 score: " + GetScore1().ToString();
        Score2.text = "player 2 score: " + GetScore2().ToString();
    }

    private int GetScore1()
    {
        return 0;
    }

    private int GetScore2()
    {
        return 0;
    }
}
