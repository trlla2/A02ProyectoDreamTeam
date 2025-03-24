using UnityEngine;

//Custom cell class to make up our texture grid
public class Cell
{
    public Vector2Int GridPosition;
    public bool Collapsed;
    public MapTile[] Options; //Local list of all possible colapsable options
    public float HeightValue; // assigned height value from map gemerator

    //Constructor, we need a pos, possible tile list(I'll bee all tiles at start) and the correspondig heigth value)
    public Cell(Vector2Int position, MapTile[] tiles, float[,] heightMap)
    {
        GridPosition = position;
        Collapsed = false;
        Options = tiles;

        // Get height value from Marching Squares' height map
        HeightValue = heightMap[position.x, position.y];
    }

    //To colapse we just assign the Options to a single object 
    public void Collapse(MapTile[] finalOption)
    {
        Options = finalOption;
        Collapsed = true;
    }

    //Reassign The options list 
    public void RecreateCell(MapTile[] newOptions)
    {
        Options = newOptions;
        Collapsed = false;
    }
}