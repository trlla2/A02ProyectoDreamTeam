using UnityEngine;
using System.Collections.Generic;

public class RegionDetector : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private MarchingSquares marchingSquares;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask wallLayerMask;

    private int borderSize;
    private float heightThreshold;

    private float[,] heightMap;
    private int gridSizeX;
    private int gridSizeY;

    public List<List<Vector2Int>> Regions { get; private set; }

    /*To detect regions on our map and be able to spawn the tanks in viiable positions we acctually dont need to do any pathfinding or a fancy algorithm,
     * we just need to correctly assert all the regions on our map and choose to spawn the tanks inside it.
     * To achieve this well choose a random epmplty point on our grid and check its neighbours, if thy are also empty add them to 
     * the region list, repeat untill no more valid empty neighous are left, if there are no empty tiles left on our
     * map gid we only have one region, if not we have more than one and well repeat the process, untill the map grid array has no more empty tiles
    */
    public void Initialize()
    {
        // Get references from MarchingSquares component
        heightMap = marchingSquares.heightMap;
        gridSizeX = marchingSquares.gridSizeX;
        gridSizeY = marchingSquares.gridSizeY;
        borderSize = marchingSquares.BorderSize;
        heightThreshold = marchingSquares.heightThreshold;
    }

    public void FindAllRegions()
    {
        Regions = new List<List<Vector2Int>>(); //list of v2 list separated by regions
        bool[,] visited = new bool[heightMap.GetLength(0), heightMap.GetLength(1)]; //bool array well use to store visited cells

        for (int x = borderSize; x < heightMap.GetLength(0) - borderSize; x++)
        {
            for (int y = borderSize; y < heightMap.GetLength(1) - borderSize; y++)
            {
                //for all possible map positions, chek if we have visited and if not and its empty; process it
                if (!visited[x, y] && heightMap[x, y] < heightThreshold)
                {
                    List<Vector2Int> region = FloodFill(new Vector2Int(x, y), visited);
                    if (region.Count > 0) Regions.Add(region);
                }
            }
        }
    }

    //run through all empty tiles
    private List<Vector2Int> FloodFill(Vector2Int start, bool[,] visited)
    {
        List<Vector2Int> region = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();//using a queue ensures order on processing empty tiles, we can also add more than one item at a time

        //Start the cue on the passed position and mark it as visited
        queue.Enqueue(start);
        visited[start.x, start.y] = true;

        while (queue.Count > 0)
        {
            Vector2Int cell = queue.Dequeue(); //dequeue the last processed cell
            region.Add(cell);//add the position to the region list

            CheckNeighbor(cell.x + 1, cell.y, visited, queue);
            CheckNeighbor(cell.x - 1, cell.y, visited, queue);
            CheckNeighbor(cell.x, cell.y + 1, visited, queue);
            CheckNeighbor(cell.x, cell.y - 1, visited, queue);
        }
        return region;
    }

    private void CheckNeighbor(int x, int y, bool[,] visited, Queue<Vector2Int> queue)
    {
        //if the neighbour is on bounds, has not yet been visited and is empty
        if (x >= borderSize && x < gridSizeX - borderSize && y >= borderSize && y < gridSizeY - borderSize && !visited[x, y] && heightMap[x, y] < heightThreshold)
        {
            visited[x, y] = true;
            queue.Enqueue(new Vector2Int(x, y)); //enqueue the position to be processed
        }
    }

    public void SpawnObjects(GameObject GO, int times)
    {
        List<Vector2Int> BiggestRegion = new List<Vector2Int>();
        foreach (List<Vector2Int> region in Regions)
        {
            if (region.Count > BiggestRegion.Count)
            {
                BiggestRegion = region;
            }
        }
        if (BiggestRegion.Count == 0)
        {
            Debug.LogWarning("Cannot spawn objects: No regions detected.");
            return;
        }

        int SpawnedN = 0;
        int i = Random.Range(0, BiggestRegion.Count);
        int y = Random.Range(0, BiggestRegion.Count);

        while (SpawnedN < times)
        {
            Vector3 position = new Vector3(BiggestRegion[i].x * marchingSquares.gridResolution, BiggestRegion[y].y * marchingSquares.gridResolution, 0);

            var overlap = Physics.OverlapSphere(position, 0.2f);

            if (overlap.Length == 0)
            {
                if (Random.value < (1f / times) * 0.25f)
                {
                    Instantiate(GO, position, Quaternion.identity);
                    SpawnedN++;
                    i += BiggestRegion.Count / 5;
                    y += BiggestRegion.Count / 6;
                }
            }
            else
            {
                i = Random.Range(0, BiggestRegion.Count);
                y = Random.Range(0, BiggestRegion.Count);
            }

            
            if (i >= BiggestRegion.Count) i = 0;
            if (y >= BiggestRegion.Count) y = 0;
        }
    }
}