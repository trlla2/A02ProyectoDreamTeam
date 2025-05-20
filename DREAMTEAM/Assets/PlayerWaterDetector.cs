using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PlayerWaterDetector : MonoBehaviour
{
    private MapTextureGenerator mapGenerator;

    [Header("References")]
    [SerializeField] private Tile[] waterTile;
    public bool IsInWater { get; private set; }

    private void Start()
    {
        mapGenerator = FindFirstObjectByType<MapTextureGenerator>();
    }
    void Update()
    {
        IsInWater = CheckWaterStatus(transform.position);
        if(IsInWater)
        {
            Tank_Behaviour behaviour = GetComponent<Tank_Behaviour>();
            int idPlayer = behaviour.GetPlayer();
            if (idPlayer == 1)
            {
                GameManager.Instance.GetTank1IsDead();

            }
            else if (idPlayer == 2)
            {
                GameManager.Instance.GetTank2IsDead();
            }

            behaviour.Dead();
        }
    }

    public bool CheckWaterStatus(Vector3 worldPosition)
    {
        Vector3Int cellPosition = mapGenerator.backgroundTilemap.WorldToCell(worldPosition);
        Tile currentTile = mapGenerator.backgroundTilemap.GetTile<Tile>(cellPosition);

        for(int i = 0; i < waterTile.Length; i++)
        {
            if(currentTile == waterTile[i]) return true;
        }
        return false;
    }
}