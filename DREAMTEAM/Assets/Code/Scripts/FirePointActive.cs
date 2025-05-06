using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to determie if the firepoint is inside a wall
public class FirePointActive : MonoBehaviour
{
    public bool CanShoot = true;
    [SerializeField] private int WallLayer = 3;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == WallLayer || other.CompareTag("Player"))
        {
            CanShoot = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == WallLayer || other.CompareTag("Player"))
        {
            CanShoot = true;
        }
    }
}
