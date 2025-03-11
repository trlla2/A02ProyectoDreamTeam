using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class WaveFunctionCollapse : MonoBehaviour
{
    [Header("WFC Settings")]
    public TileBase[] tileSet;
    public Vector2Int gridSize;
    public int initialCollapses = 5;

    [Header("Height Constraints")]
    public AnimationCurve heightProbabilityCurve;
    public float heightThreshold = 0.2f;

    private Tilemap tilemap;
    private MarchingSquares mapGenerator;
    private WFCCell[,] waveGrid;
    private List<Vector2Int> collapseQueue = new List<Vector2Int>();

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        mapGenerator = FindObjectOfType<MarchingSquares>();
        InitializeWaveGrid();
    }

    void InitializeWaveGrid()
    {
        waveGrid = new WFCCell[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                float height = GetMappedHeight(x, y);
                waveGrid[x, y] = new WFCCell(new Vector2Int(x, y), height, tileSet);
            }
        }
    }

    float GetMappedHeight(int x, int y)
    {
        // Convert tilemap coordinates to heightmap coordinates
        float normalizedX = (float)x / gridSize.x * mapGenerator.gridSizeX;
        float normalizedY = (float)y / gridSize.y * mapGenerator.gridSizeY;
        return mapGenerator.heightMap[
            Mathf.Clamp((int)normalizedX, 0, mapGenerator.gridSizeX - 1),
            Mathf.Clamp((int)normalizedY, 0, mapGenerator.gridSizeY - 1)
        ];
    }

    public void GenerateTilemap()
    {
        StartCoroutine(RunWFC());
    }

    IEnumerator RunWFC()
    {
        // Initial height-based collapses
        CollapseLowestHeightCells();

        while (collapseQueue.Count > 0)
        {
            Vector2Int currentPos = collapseQueue[0];
            collapseQueue.RemoveAt(0);

            PropagateConstraints(currentPos);
            yield return null;
        }
    }

    void CollapseLowestHeightCells()
    {
        List<WFCCell> cells = new List<WFCCell>();
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                cells.Add(waveGrid[x, y]);
            }
        }

        cells.Sort((a, b) => a.height.CompareTo(b.height));

        for (int i = 0; i < Mathf.Min(initialCollapses, cells.Count); i++)
        {
            CollapseCell(cells[i].position);
        }
    }

    void CollapseCell(Vector2Int pos)
    {
        WFCCell cell = waveGrid[pos.x, pos.y];
        if (cell.collapsed) return;

        TileBase selectedTile = SelectTileBasedOnHeight(cell);
        cell.Collapse(selectedTile);
        tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), selectedTile);

        collapseQueue.Add(pos);
    }

    TileBase SelectTileBasedOnHeight(WFCCell cell)
    {
        List<TileBase> validTiles = new List<TileBase>();
        List<float> weights = new List<float>();

        foreach (TileBase tile in cell.possibleTiles)
        {
            float weight = heightProbabilityCurve.Evaluate(cell.height);

            if (cell.height < heightThreshold) weight *= 2f;

            validTiles.Add(tile);
            weights.Add(weight);
        }

        return validTiles[WeightedRandom(weights)];
    }

    int WeightedRandom(List<float> weights)
    {
        float total = 0;
        foreach (float w in weights) total += w;

        float random = Random.Range(0, total);
        for (int i = 0; i < weights.Count; i++)
        {
            if (random < weights[i]) return i;
            random -= weights[i];
        }
        return 0;
    }

    void PropagateConstraints(Vector2Int pos)
    {
        foreach (Vector2Int dir in Directions.Cardinal)
        {
            Vector2Int neighborPos = pos + dir;
            if (IsValidPosition(neighborPos))
            {
                WFCCell neighbor = waveGrid[neighborPos.x, neighborPos.y];
                if (neighbor.collapsed) continue;

                TileBase[] validNeighbors = GetValidNeighbors(pos, dir);
                neighbor.UpdateOptions(validNeighbors);

                if (neighbor.possibleTiles.Count == 1)
                {
                    CollapseCell(neighborPos);
                }
            }
        }
    }

    TileBase[] GetValidNeighbors(Vector2Int pos, Vector2Int dir)
    {
        WFCCell cell = waveGrid[pos.x, pos.y];
        List<TileBase> valid = new List<TileBase>();

        foreach (Tile tile in cell.possibleTiles)
        {
            valid.AddRange(tile.GetNeighborsInDirection(dir));
        }

        return valid.ToArray();
    }

    bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
    }
}
