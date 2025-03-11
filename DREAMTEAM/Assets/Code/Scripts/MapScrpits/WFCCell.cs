using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WFCCell
{
    public Vector2Int position;
    public float height;

    public List<TileBase> possibleTiles;
    public bool collapsed;

    public WFCCell(Vector2Int pos, float heightValue, TileBase[] tiles)
    {
        position = pos;
        height = heightValue;
        possibleTiles = new List<TileBase>(tiles);
        collapsed = false;
    }

    public void Collapse(TileBase tile)
    {
        possibleTiles = new List<TileBase> { tile };
        collapsed = true;
    }

    public void UpdateOptions(TileBase[] validOptions)
    {
        List<TileBase> newOptions = new List<TileBase>();
        foreach (TileBase option in possibleTiles)
        {
            if (System.Array.Exists(validOptions, t => t == option))
            {
                newOptions.Add(option);
            }
        }
        possibleTiles = newOptions;
    }
}

public static class Directions
{
    public static List<Vector2Int> Cardinal = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
}