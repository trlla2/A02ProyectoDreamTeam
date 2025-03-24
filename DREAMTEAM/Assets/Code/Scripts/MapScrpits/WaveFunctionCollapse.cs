using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

//This code is an implementation of the popular wave function colapse algorithm, there are a hughe amoun of tutorials and explanations of this algorithim but i found 
//The coding trains video the easyiest in-depth explanation to understand(https://www.youtube.com/watch?v=rI_y2GAlQFM). For the specific implementation to unity i found Game dev Garnet
//tutorial really sraigth forward (https://www.youtube.com/watch?v=iJ_GnGD5BZA)
public class WaveFunctionCollapse : MonoBehaviour
{
    [Header("Tilemap Settings")]
    public Tilemap tilemap;
    public MapTile[] tileObjects; //All maptile objects well use in the generation
    public MapTile backupTile; //We'll use this if no other option is availabe

    private MarchingSquares marchingSquares;
    private int gridSizeX;
    private int gridSizeY;

    private List<Cell> gridComponents;//local Cell grid well use
    private int iteration;//counter to keep track of collapsed cells
    WaitForEndOfFrame wait;


    //Create the grid using our map size
    private void Awake()
    {
        marchingSquares = GetComponent<MarchingSquares>();
        gridComponents = new List<Cell>();

        // Get grid size from Marching Squares
        gridSizeX = marchingSquares.gridSizeX;
        gridSizeY = marchingSquares.gridSizeY;

        wait = new WaitForEndOfFrame();
    }

    public void Initialize()
    {
        tilemap.ClearAllTiles();

        //for all position is the grid, create a new empty cell and add it to gridComponents list
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Cell newCell = new Cell(new Vector2Int(x, y), tileObjects, marchingSquares.heightMap);
                gridComponents.Add(newCell);
            }
        }

        StartCoroutine(RunWFC());
    }

    IEnumerator RunWFC() 
    {
        // Initial height-based collapses
        CollapseLowestHeightCells(5);

        while (iteration < gridSizeX * gridSizeY)
        {
            yield return CheckEntropy();
            iteration++;
        }
    }

    IEnumerator CheckEntropy()
    {
        //1. Filter uncollapsed cells
        List<Cell> tempGrid = gridComponents;
        for (int y = 0; y < gridComponents.Count; y++)
        {
            if(tempGrid[y].Collapsed)
            {
                tempGrid.RemoveAt(y);
            }
        }
        if (tempGrid.Count == 0) yield break;

        //2. Sort by options count (entropy)
        Cell temp;
        //Bubble sort
        for (int write = 0; write < tempGrid.Count; write++)
        {
            for (int sort = 0; sort < tempGrid.Count - 1; sort++)
            {
                if (tempGrid[sort].Options.Length > tempGrid[sort + 1].Options.Length)//Sort the cells based on their possible options to colapse
                {
                    temp = tempGrid[sort + 1];
                    tempGrid[sort + 1] = tempGrid[sort];
                    tempGrid[sort] = temp;
                }
            }
        }

        //3. Find minimum entropy (available options) value
        int minEntropy = tempGrid[0].Options.Length;

        //4. Filter to only cells with minimum entropy
        for(int i = 0; i < tempGrid.Count; i++)
        {
            if (tempGrid[i].Options.Length != minEntropy)
            {
                tempGrid.RemoveAt(i);
            }
        }

        yield return wait;
        
        // 5. Collapse random cell from filtered list
        //CollapseCell(tempGrid);
    }

    //This function makes the initial collapses of the cells with the lowest heigth value
    void CollapseLowestHeightCells(int count)
    {
        //To collapse the lowest heght value cells firts we need to move them to a sorted array
        Cell[] sortedCells = gridComponents.ToArray();//Copy our grid array
        Cell temp;

        //Bubble sort, again :)
        for (int write = 0; write < sortedCells.Length; write++)
        {
            for (int sort = 0; sort < sortedCells.Length - 1; sort++)
            {
                if (sortedCells[sort].HeightValue > sortedCells[sort + 1].HeightValue)//Sort the cells based on their assigned heghit value
                {
                    temp = sortedCells[sort + 1];
                    sortedCells[sort + 1] = sortedCells[sort];
                    sortedCells[sort] = temp;
                }
            }
        }
        //Had to search the web for this, Using the Linq library we can use the Take method and reconvert the output to an array
        sortedCells = sortedCells.Take(count).ToArray();//Shorten the array to have a length = count


        foreach (Cell cell in sortedCells)
        {
            Vector3Int tilePosition = new Vector3Int(cell.GridPosition.x,cell.GridPosition.y,0);

            // Check for valid options 
            MapTile selectedTile = (cell.Options.Length > 0)? cell.Options[Random.Range(0, cell.Options.Length)] : backupTile;

            cell.Collapse(new MapTile[] { selectedTile });
            tilemap.SetTile(tilePosition, selectedTile.tile);
        }

       // UpdateGeneration();
    }
}