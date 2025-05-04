using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class WaterController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MapTextureGenerator mapGenerator;
    [SerializeField] private MarchingSquares marchingSquares;
    [SerializeField] private RegionDetector regionDetector;

    [Header("Settings")]
    [SerializeField] private float blinkInterval = 0.2f;
    [SerializeField] private float conversionDelay = 3f;
    [SerializeField] private int expansionPhases = 3;
    [SerializeField] private Tile waterTile;

    private DepthTileBand lowestHeightBand;
    private List<Vector3Int> allConvertedTiles = new List<Vector3Int>();
    private List<Vector3Int> currentFrontier = new List<Vector3Int>();

    public void StartWaterExpansionEvent()
    {
        StartCoroutine(WaterExpansionRoutine());
    }

    private IEnumerator WaterExpansionRoutine()
    {
        FindLowestHeightBand();
        InitializeStartingTiles();

        for (int phase = 0; phase < expansionPhases; phase++)
        {
            List<Vector3Int> tilesToConvert = GetExpandableTiles();

            if (tilesToConvert.Count == 0)
            {
                Debug.Log("No more tiles to expand to");
                yield break;
            }

            //sotring the blink effect coroutines in a list so we can call all tiles at once
            List<Coroutine> blinkRoutines = new List<Coroutine>();
            foreach (var tilePos in tilesToConvert)
            {
                blinkRoutines.Add(StartCoroutine(BlinkTileEffect(tilePos)));
            }

            yield return new WaitForSeconds(conversionDelay);

            foreach (var routine in blinkRoutines) // make sure we stop routines to save memory
            {
                StopCoroutine(routine);
            }
            ConvertTiles(tilesToConvert);

            yield return new WaitForSeconds(1f);
        }
    }

    //function to know what heigthband does water ocuppy
    private void FindLowestHeightBand()
    {
        lowestHeightBand = mapGenerator.heightBands[0];
        foreach (var band in mapGenerator.heightBands)
        {
            if (band.minHeight < lowestHeightBand.minHeight)
            {
                lowestHeightBand = band;
            }
        }
    }

    private void InitializeStartingTiles()
    {
        allConvertedTiles.Clear();
        currentFrontier.Clear();

        for (int x = 0; x < marchingSquares.gridSizeX; x++)
        {
            for (int y = 0; y < marchingSquares.gridSizeY; y++)
            {
                //foreach tile in the map tilemap,  
                Vector3Int pos = new Vector3Int(x, y, 0);
                Tile currentTile = mapGenerator.backgroundTilemap.GetTile<Tile>(pos);

                //compare if the tile is water
                bool valid = false;
                foreach(Tile tile in  lowestHeightBand.tiles)
                {
                    if (tile == currentTile) valid = true;
                }

                if (valid)
                {
                    // Mark initial lowest tiles as water
                    marchingSquares.heightMap[x, y] = lowestHeightBand.minHeight - 0.1f;
                    mapGenerator.backgroundTilemap.SetTile(pos, waterTile);
                    allConvertedTiles.Add(pos);
                    currentFrontier.Add(pos);
                }
            }
        }
    }

    //get non-water neigbours 
    private List<Vector3Int> GetExpandableTiles()
    {
        List<Vector3Int> expandableTiles = new List<Vector3Int>();

        foreach (var tile in currentFrontier)
        {
            Vector3Int[] neighbors = {
                tile + Vector3Int.up,
                tile + Vector3Int.down,
                tile + Vector3Int.left,
                tile + Vector3Int.right
            };

            foreach (var neighbor in neighbors)
            {
                //if we hav not visited the tile yet
                if (IsValidPosition(neighbor) && !allConvertedTiles.Contains(neighbor) && !expandableTiles.Contains(neighbor))
                {
                    expandableTiles.Add(neighbor);
                }
            }
        }

        return expandableTiles;
    }

    private IEnumerator BlinkTileEffect(Vector3Int tilePosition)
    {
        Tile originalTile = mapGenerator.backgroundTilemap.GetTile<Tile>(tilePosition);
        bool visible = true;
        float endTime = Time.time + conversionDelay;

        while (Time.time < endTime)
        {
            mapGenerator.backgroundTilemap.SetTile(tilePosition, visible ? null : originalTile);
            visible = !visible;
            yield return new WaitForSeconds(blinkInterval);
        }

        mapGenerator.backgroundTilemap.SetTile(tilePosition, originalTile);
    }

    private void ConvertTiles(List<Vector3Int> tiles)
    {
        foreach (var tilePos in tiles)
        {
            // Update heightmap to water level
            marchingSquares.heightMap[tilePos.x, tilePos.y] = lowestHeightBand.minHeight - 0.1f;
            mapGenerator.backgroundTilemap.SetTile(tilePos, waterTile);
            allConvertedTiles.Add(tilePos);
        }

        // Update frontier for next phase
        currentFrontier.Clear();
        currentFrontier.AddRange(tiles);
    }

    //function to make sure we are inside the map
    private bool IsValidPosition(Vector3Int position)
    {
        return position.x >= 0 && position.x < marchingSquares.gridSizeX &&
               position.y >= 0 && position.y < marchingSquares.gridSizeY;
    }
}