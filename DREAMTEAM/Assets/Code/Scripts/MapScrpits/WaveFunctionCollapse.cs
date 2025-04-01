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
    public MapTile LowTile;

    [SerializeField] private MarchingSquares marchingSquares;
    private int gridSizeX;
    private int gridSizeY;

    private List<Cell> gridComponents;//local Cell grid well use
    private int iteration;//counter to keep track of collapsed cells

    private List<GenerationBlock> blocks;
    private int blockSize = 10; 
    private int blockOverlap = 2;

    [Header("Debug")]
    public bool showEntropy = true;
    public Color entropyColor = Color.red;


    //predefined array well use to loop thru all propagating directions
    Vector2Int[] directions = 
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    //Create the grid using our map size
    private void Awake()
    {
        gridComponents = new List<Cell>();

        // Get grid size from Marching Squares
        gridSizeX = marchingSquares.gridSizeX;
        gridSizeY = marchingSquares.gridSizeY;

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
        CreateBlocks();
        StartCoroutine(ProcessBlocks());
    }
    void CreateBlocks()
    {
        blocks = new List<GenerationBlock>();

        for (int y = 0; y < gridSizeY; y += blockSize - blockOverlap)
        {
            for (int x = 0; x < gridSizeX; x += blockSize - blockOverlap)
            {
                blocks.Add(new GenerationBlock(
                    new Vector2Int(x, y),
                    blockSize,
                    blockOverlap,
                    gridComponents,
                    gridSizeX,
                    gridSizeY
                ));
            }
        }
    }
    IEnumerator ProcessBlocks()
    {
        foreach (var block in blocks)
        {
            int retries = 0;
            bool success = false;

            while (!success && retries < 3)
            {
                yield return ProcessSingleBlock(block);
                success = ValidateBlock(block);

                if (!success)
                {
                    ResetBlock(block);
                    retries++;
                }
            }

            if (!success) ApplyFallbackToBlock(block);
        }
    }
    IEnumerator ProcessSingleBlock(GenerationBlock block)
    {
        // Free internal cells
        foreach (var cell in block.cells)
        {
            if (IsBorderCell(cell.GridPosition, block))
                cell.RecreateCell(GetBorderConstraints(cell.GridPosition, block));
            else
                cell.RecreateCell(tileObjects);
        }

        // Run WFC for this block
        int localIteration = 0;
        while (localIteration < block.size * block.size)
        {
            yield return CheckEntropy(block);
            localIteration++;
        }
    }
    MapTile[] GetBorderConstraints(Vector2Int pos, GenerationBlock block)
    {
        List<MapTile> constraints = new List<MapTile>();
        int blockRightEdge = block.origin.x + block.size - 1;
        int blockBottomEdge = block.origin.y + block.size - 1;

        // Left neighbor check
        if (pos.x == block.origin.x && pos.x > 0)
        {
            var neighbor = blocks.FirstOrDefault(b =>
                b.origin.x + b.size - blockOverlap == pos.x &&
                pos.y >= b.origin.y &&
                pos.y < b.origin.y + b.size);

            if (neighbor != null)
            {
                int neighborX = neighbor.size - 1; // Rightmost column of neighbor
                int neighborY = pos.y - neighbor.origin.y;
                if (neighborY >= 0 && neighborY < neighbor.size)
                {
                    constraints.AddRange(neighbor.cells[neighborX, neighborY].Options);
                }
            }
        }

        // Right neighbor check
        if (pos.x == blockRightEdge && pos.x < gridSizeX - 1)
        {
            var neighbor = blocks.FirstOrDefault(b =>
                b.origin.x == pos.x + 1 - blockOverlap &&
                pos.y >= b.origin.y &&
                pos.y < b.origin.y + b.size);

            if (neighbor != null)
            {
                int neighborX = 0; // Leftmost column of neighbor
                int neighborY = pos.y - neighbor.origin.y;
                if (neighborY >= 0 && neighborY < neighbor.size)
                {
                    constraints.AddRange(neighbor.cells[neighborX, neighborY].Options);
                }
            }
        }

        // Bottom neighbor check
        if (pos.y == block.origin.y && pos.y > 0)
        {
            var neighbor = blocks.FirstOrDefault(b =>
                b.origin.y + b.size - blockOverlap == pos.y &&
                pos.x >= b.origin.x &&
                pos.x < b.origin.x + b.size);

            if (neighbor != null)
            {
                int neighborX = pos.x - neighbor.origin.x;
                int neighborY = neighbor.size - 1; // Bottom row of neighbor
                if (neighborX >= 0 && neighborX < neighbor.size)
                {
                    constraints.AddRange(neighbor.cells[neighborX, neighborY].Options);
                }
            }
        }

        // Top neighbor check
        if (pos.y == blockBottomEdge && pos.y < gridSizeY - 1)
        {
            var neighbor = blocks.FirstOrDefault(b =>
                b.origin.y == pos.y + 1 - blockOverlap &&
                pos.x >= b.origin.x &&
                pos.x < b.origin.x + b.size);

            if (neighbor != null)
            {
                int neighborX = pos.x - neighbor.origin.x;
                int neighborY = 0; // Top row of neighbor
                if (neighborX >= 0 && neighborX < neighbor.size)
                {
                    constraints.AddRange(neighbor.cells[neighborX, neighborY].Options);
                }
            }
        }

        return constraints.Distinct().Count() > 0 ?
            constraints.Distinct().ToArray() :
            new[] { backupTile };
    }
    bool ValidateBlock(GenerationBlock block)
    {
        return block.cells.Cast<Cell>()
            .All(c => c.Collapsed || IsBorderCell(c.GridPosition, block));
    }
    bool IsBorderCell(Vector2Int pos, GenerationBlock block)
    {
        return pos.x == block.origin.x ||
               pos.y == block.origin.y ||
               pos.x == block.origin.x + block.size - 1 ||
               pos.y == block.origin.y + block.size - 1;
    }

    void ResetBlock(GenerationBlock block)
    {
        foreach (var cell in block.cells)
        {
            if (!IsBorderCell(cell.GridPosition, block))
                cell.RecreateCell(tileObjects);
        }
    }
    void ApplyFallbackToBlock(GenerationBlock block)
    {
        foreach (var cell in block.cells)
        {
            if (!cell.Collapsed)
            {
                cell.Collapse(new[] { backupTile });
                tilemap.SetTile(new Vector3Int(cell.GridPosition.x, cell.GridPosition.y, 0), backupTile.tile);
            }
        }
    }

    IEnumerator CheckEntropy(GenerationBlock block)
    {
        // Get uncollapsed cells
        List<Cell> tempGrid = block.cells
        .Cast<Cell>()
        .Where(c => !c.Collapsed && !IsBorderCell(c.GridPosition, block))
        .ToList();

        if (tempGrid.Count == 0) yield break;

        // Sort by entropy
        tempGrid = tempGrid
            .OrderBy(c => c.Options.Length)
            .ThenBy(c => Random.value)
            .ToList();

        int minEntropy = tempGrid[0].Options.Length;

        // Handle contradictions
        if (minEntropy == 0)
        {
            Debug.LogError("Contradiction! No valid options left");
            yield return HandleContradiction();
            yield break;
        }

        // Filter to min entropy
        tempGrid = tempGrid
            .Where(c => c.Options.Length == minEntropy)
            .ToList();
        
        CollapseCell(tempGrid);
    }

    IEnumerator HandleContradiction()
    {
        foreach (Cell cell in gridComponents.Where(c => c.Options.Length == 0))
        {
            if (cell.Collapsed) break;
            cell.Collapse(new MapTile[] { backupTile });
            tilemap.SetTile(new Vector3Int(cell.GridPosition.x, cell.GridPosition.y, 0), backupTile.tile);
            yield return PropagateConstraints(cell);
        }
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

            cell.Collapse(new MapTile[] { LowTile });
            StartCoroutine(PropagateConstraints(cell)); 
            tilemap.SetTile(tilePosition, LowTile.tile);
        }

    }
    void CollapseCell(List<Cell> cells)
    {
        // Randomly select a cell
        int randomIndex = Random.Range(0, cells.Count);
        Cell cellToCollapse = cells[randomIndex];
        
        // Collapse to a random option
        MapTile selectedTile = (cellToCollapse.Options.Length > 0) ?  cellToCollapse.Options[Random.Range(0, cellToCollapse.Options.Length)] : backupTile;
        
        // Update the cell and tilemap
        cellToCollapse.Collapse(new MapTile[] { selectedTile });
        Vector3Int tilePosition = new Vector3Int(cellToCollapse.GridPosition.x, cellToCollapse.GridPosition.y, 0);
        tilemap.SetTile(tilePosition, selectedTile.tile);
        
        // Propagate constraints to neighbors
        StartCoroutine(PropagateConstraints(cellToCollapse));
    }

    //Iterate through all neighours and update their possible colapsing options
    IEnumerator PropagateConstraints(Cell cell)
    {
        List<Cell> neighbors = GetNeighbors(cell);
        
        foreach (Cell neighbor in neighbors)
        {
            bool changed = FilterNeighborOptions(neighbor, cell);
            
            if (changed)
            {
                // If we reduced options to 1, collapse this cell immediately
                if (neighbor.Options.Length == 1)
                {
                    MapTile selectedTile = neighbor.Options[0];
                    neighbor.Collapse(new MapTile[] { selectedTile });
                    Vector3Int tilePosition = new Vector3Int(neighbor.GridPosition.x, neighbor.GridPosition.y, 0);
                    tilemap.SetTile(tilePosition, selectedTile.tile);
                    
                    // Continue propagation from this newly collapsed cell
                    yield return PropagateConstraints(neighbor);
                }
                else
                {
                    // Just update options and continue propagation
                    yield return PropagateConstraints(neighbor);
                }
            }
        }
    }

    List<Cell> GetNeighbors(Cell cell)
    {
        List<Cell> neighbors = new List<Cell>();
        //loop thru all 4 directions
        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = cell.GridPosition + dir;
            Cell neighbor = gridComponents.Find(c => c.GridPosition == neighborPos);//find the neigbour in our Cell grid array

            if (neighbor != null && !neighbor.Collapsed)
            {
                neighbors.Add(neighbor);
            }
        }
        
        return neighbors;
    }   
    //Change the possible colapsing options given a tile and its negibour, returns true if the resulting option list changed from the original
    bool FilterNeighborOptions(Cell neighbor, Cell sourceCell)
    {
        MapTile[] originalOptions = neighbor.Options;
        List<MapTile> newOptions = new List<MapTile>();
        
        Vector2Int direction = sourceCell.GridPosition - neighbor.GridPosition;//get the direction
        
        foreach (MapTile option in neighbor.Options) //for every option we originaly had in the neigbour cell obj
        {
            bool isValid = false;
            
            foreach (MapTile sourceOption in sourceCell.Options) //and for every option we can have after collapse
            {
                if (direction == Vector2Int.up && option.upNeighbours.Contains(sourceOption)) isValid = true;
                else if (direction == Vector2Int.down && option.downNeighbours.Contains(sourceOption)) isValid = true;
                else if (direction == Vector2Int.left && option.leftNeighbours.Contains(sourceOption)) isValid = true;
                else if (direction == Vector2Int.right && option.rightNeighbours.Contains(sourceOption)) isValid = true;
                
                if (isValid) break;
            }
            
            if (isValid) newOptions.Add(option); //add all possible valid options to the new array
        }
        
        if (newOptions.Count != originalOptions.Length)
        {
            // update the cells options
            neighbor.RecreateCell(newOptions.ToArray());
            return true;
        }
        
        return false;
    }

    void OnDrawGizmos()
    {
        if (!showEntropy || gridComponents == null) return;
        
        foreach (Cell cell in gridComponents)
        {
            float alpha = cell.Collapsed ? 0 : cell.Options.Length / (float)tileObjects.Length;
            Gizmos.color = new Color(entropyColor.r, entropyColor.g, entropyColor.b, alpha);
            Gizmos.DrawCube(new Vector3(cell.GridPosition.x * marchingSquares.gridResolution, cell.GridPosition.y * marchingSquares.gridResolution, 0), Vector3.one * marchingSquares.gridResolution);
        }
    }
}

public class GenerationBlock
{
    public Vector2Int origin;
    public int size;
    public int overlap;
    public Cell[,] cells;
    public bool isGenerated;

    public GenerationBlock(Vector2Int origin, int size, int overlap, List<Cell> grid, int gridSizeX, int gridSizeY)
    {
        this.origin = origin;
        this.size = size;
        this.overlap = overlap;
        cells = new Cell[size, size];

        // Populate cells from main grid
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2Int pos = new Vector2Int(
                    Mathf.Clamp(origin.x + x, 0, gridSizeX - 1),
                    Mathf.Clamp(origin.y + y, 0, gridSizeY - 1)
                );
                cells[x, y] = grid.Find(c => c.GridPosition == pos);
            }
        }
    }
}