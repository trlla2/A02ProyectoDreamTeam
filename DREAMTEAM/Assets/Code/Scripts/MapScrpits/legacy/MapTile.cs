using UnityEngine;
using UnityEngine.Tilemaps;

//Custom Tile object for texture generation, it consits of a tile texture and 4 lists of all possible neghbouring tiles

[CreateAssetMenu(fileName = "NewMapTile", menuName = "WFC/Map Tile")]
public class MapTile : ScriptableObject
{
    public TileBase tile;
    public MapTile[] upNeighbours;
    public MapTile[] downNeighbours;
    public MapTile[] leftNeighbours;
    public MapTile[] rightNeighbours;
}
