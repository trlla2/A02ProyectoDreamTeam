using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shield : MonoBehaviour
{
    private Weapon weapon;

    [SerializeField] private GameObject explosionShield;

    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    //GameObject temp = Instantiate(explosionShield, this.transform.position, Quaternion.identity);// Explotion

}
