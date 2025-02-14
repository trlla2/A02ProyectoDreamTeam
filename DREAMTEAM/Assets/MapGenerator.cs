using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject Hexagon;

    [SerializeField]private int gridWidth = 10;
    [SerializeField]private int gridHeight = 10;

    //I found this specific values testing the Hexagon object i created, prbably dependiong on the hex sprite it will change
    [SerializeField]private float HorizontalOffset = 0.76f;
    [SerializeField]private float VerticalOffset = 0.875f;
    [SerializeField] private float DiagonalOffset = 0.437f;

    private float tileWidth = 2.0f;
    private float tileHeight = 2.0f;

    private GameObject[,] hexGrid;

    void Start()
    {
        GenerateHexGrid();
    }

    //Creating a hexagonal tiled map is not that dificult, if we try to buid it by hand we can see that every 2 rows the 
    //pattern repeats itself, narrowing down to basics, i (and a bit of help from the internet) found out that you only need to know 
    //where to put the left and upepr tile relative to the initial one. The rest tiles itself
    void GenerateHexGrid()
    {
        hexGrid = new GameObject[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Calculate the position for each hex tile
                float xPos = x * tileWidth * HorizontalOffset;
                float yPos = y * tileHeight * VerticalOffset;

                //as said previously, the pattern repeats itself every 2 rows, where the heigth will change
                if (x % 2 == 1)
                {
                    yPos += tileHeight * DiagonalOffset;
                }

                Vector2 tilePosition = new Vector2(xPos, yPos);

                GameObject hexTile = Instantiate(Hexagon, tilePosition, Quaternion.identity);

                hexTile.transform.parent = transform; 
                hexTile.name = "Hexagon" + "_" + x + "_" + y;


                hexGrid[x, y] = hexTile;
            }
        }
    }
}