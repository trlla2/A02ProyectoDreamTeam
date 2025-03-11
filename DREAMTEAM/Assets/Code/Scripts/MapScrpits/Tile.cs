using UnityEngine.Tilemaps;
using UnityEngine;

public class Tile : TileBase
{
    public TileBase[] upNeighbors;
    public TileBase[] downNeighbors;
    public TileBase[] leftNeighbors;
    public TileBase[] rightNeighbors;

    public TileBase[] GetNeighborsInDirection(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return upNeighbors;
        if (dir == Vector2Int.down) return downNeighbors;
        if (dir == Vector2Int.left) return leftNeighbors;
        if (dir == Vector2Int.right) return rightNeighbors;
        return new TileBase[0];
    }
}