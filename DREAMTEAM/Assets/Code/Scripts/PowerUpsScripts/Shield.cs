using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shield : MonoBehaviour
{
    private Weapon weapon;


    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }
    

}
