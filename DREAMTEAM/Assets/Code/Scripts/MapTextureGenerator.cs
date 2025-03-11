using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class DepthTileBand
{
    public float minHeight;
    public float maxHeight;
    public Tile[] tiles;
}

public class MapTextureGenerator : MonoBehaviour
{
    [Header("Tile Settings")]
    public Tilemap backgroundTilemap;
    public DepthTileBand[] heightBands;

    [Header("References")]
    [SerializeField] private MarchingSquares marchingSquares;

    private int gridSizeX;
    private int gridSizeY;
    public float gridResolution;

    public void Initial()
    {
        InitializeReferences();
        GenerateBackgroundTextures();
    }

    void InitializeReferences()
    {
        gridSizeX = marchingSquares.gridSizeX + 1;//+1 to make sure we have no holes
        gridSizeY = marchingSquares.gridSizeY + 1;
        backgroundTilemap.ClearAllTiles();
    }

    public void GenerateBackgroundTextures()
    {
        float[,] HeightMap = marchingSquares.heightMap;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(
                    Mathf.RoundToInt(x * gridResolution),
                    Mathf.RoundToInt(y * gridResolution),
                    0
                );

                float heightValue = HeightMap[x, y];
                AssignTile(heightValue, tilePosition);
            }
        }
    }

    void AssignTile(float height, Vector3Int position)
    {
        foreach (DepthTileBand band in heightBands)
        {
            if (height >= band.minHeight && height <= band.maxHeight)
            {
                Tile selectedTile = band.tiles[Random.Range(0, band.tiles.Length)];
                backgroundTilemap.SetTile(position, selectedTile);
                break;
            }
        }
    }
}