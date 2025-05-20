using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private RegionDetector regionDetector;

    [Header("Deitals")]
    [SerializeField] private Tile[] OutOfBounds;
    [SerializeField] private Tile Default;

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

        List<Vector2Int> BiggestRegion = new List<Vector2Int> { };

        foreach (List<Vector2Int> region in regionDetector.Regions)
        {
            if (region.Count > BiggestRegion.Count)
            {
                BiggestRegion = region;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                float heightValue = HeightMap[x, y];
                if (BiggestRegion.Contains((Vector2Int)tilePosition))
                {
                    AssignTile(heightValue, tilePosition);
                }
                else
                {
                    AssignRandomOutTile(heightValue, tilePosition);
                }
            }
        }
    }

    void AssignRandomOutTile(float height, Vector3Int position)
    {
        if (height <= marchingSquares.heightThreshold)
        {
            backgroundTilemap.SetTile(position, OutOfBounds[Random.Range(0, OutOfBounds.Length)]);
        }
        else 
        {
            backgroundTilemap.SetTile(position, Default);
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