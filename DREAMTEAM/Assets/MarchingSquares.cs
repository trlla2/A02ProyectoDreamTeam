using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This code was made possible thanks to various recources on the internet that give great explanations of the concept
*  The video Coding marching squares by The codig train (https://www.youtube.com/watch?v=0ZONMNUKTfU&t=1049s&ab_channel=TheCodingTrain) is a fantástic video to
*  understand the functioning of the algorithm. Sebastian Lague also has incredible videos about marching squares and the 3d version of the algorithm marching cubes
*  
*  The main struggle i had was implementing this algorithm to fit into Unitys' mesh creation sistem, as the algorithm uses lines
*  and unity uses triangles. Luckyly videos like Brakeys mesh basics helped understand the concept, Sebastian Lague's videdo and Freedom Coding's marching squares videdo offered exemples on how 
*  to implement the mesh genearation in unity
*/

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MarchingSquares : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField][Range(5, 500)] private int gridSizeX = 15;
    [SerializeField][Range(5, 500)] private int gridSizeY = 15;
    [SerializeField][Range(0.01f, 0.4f)] private float noiseScale = 0.1f;
    [SerializeField][Range(0.05f, 2f)] private float gridResolution = 1f;
    [SerializeField][Range(0f, 1f)] private float heightThreshold = 0.5f;

    [SerializeField] private int Seed = 0;

    [Header("Debug Settings")]
    [SerializeField] private bool drawGizmos = true;

    private MeshFilter meshFilter;
    private EdgeCollider2D edgeCollider;

    private float[,] heightMap; //float array where we'll store perlin noise values

    //Using lists allows us to have dynamic grid sizes
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private void Start()
    {
        UpdateGrid();
    }

    
    //public so we can access it thru the editor script
    public void UpdateGrid()
    {
        meshFilter = GetComponent<MeshFilter>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        GenerateHeightMap(Seed);
        MarchSquares();
        CreateMesh();
    }

    private void GenerateHeightMap(int seed)
    {
        //we add +1 to each array dimention so the borders are not disconected
        heightMap = new float[gridSizeX + 1, gridSizeY + 1];
        //These values are based on nothing and could be any other
        int seedx = seed % 10 * 13 + 367;
        int seedy = seed * 404 - 21;

        for (int x = 0; x <= gridSizeX; x++)
        {
            for (int y = 0; y <= gridSizeY; y++)
            {
                //These values represent the edges of the board and all should be 1 to delimit the playing space
                if (x == 0 || x == gridSizeX || y == 0 || y == gridSizeY)
                {
                    heightMap[x, y] = 1;
                }
                else
                {
                    heightMap[x, y] = Mathf.PerlinNoise(x * noiseScale + seedx, y * noiseScale + seedy); //Z offset allows us to move through the image
                }
            }
        }
    }

    private void MarchSquares()
    {
        //clear the lists
        vertices.Clear();
        triangles.Clear();

        //Loop through all the squares
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //this floats represent the square vertecies and store the values for each one, we count the vertecies of a square 
                //clock-wise (so top left is n1, top rigth is n2, bottom rigth is n3 and bottom left is n4) this is important because
                //when creating meshes in unity triangles need to be inputed clockwise to determine the facin firection
                float a = heightMap[x, y];
                float b = heightMap[x + 1, y];
                float c = heightMap[x + 1, y + 1];
                float d = heightMap[x, y + 1];

                //convert the float falues to 0 or 1 and storing them in these ints
                int aVal = GetBinaryHeight(a);
                int bVal = GetBinaryHeight(b);
                int cVal = GetBinaryHeight(c);
                int dVal = GetBinaryHeight(d);

                //in a square with 4 vertecies that can be active or deactive, there are 16 possible configurations that can be represented in 4 binary bits
                //here by converting the binary values to decimal we can know in what configuration we are
                int configuration = aVal * 8 + bVal * 4 + cVal * 2 + dVal * 1;
                ProcessSquare(configuration, x, y);
            }
        }
    }

    private void ProcessSquare(int configuration, float offsetX, float offsetY)
    {
        Vector3[] localVertices = null; // Store the vertices of the current square
        int[] localTriangles = null; // Store de the index of triangles

        int vertexCount = vertices.Count;

        //The configuration tables have to be hardcoded, luckyly FreedomCoding (https://www.youtube.com/watch?v=LNiTnX7tyVE&ab_channel=FreedomCoding) 
        //provides us with it
        switch (configuration)
        {
            case 0:
                return;
            case 1:
                localVertices = new Vector3[] { new Vector3(0, 1f), new Vector3(0, 0.5f), new Vector3(0.5f, 1) };
                localTriangles = new int[] { 2, 1, 0 };
                break;
            case 2:
                localVertices = new Vector3[] { new Vector3(1, 1), new Vector3(1, 0.5f), new Vector3(0.5f, 1) };
                localTriangles = new int[] { 0, 1, 2 };
                break;
            case 3:
                localVertices = new Vector3[] { new Vector3(0, 0.5f), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0.5f) };
                localTriangles = new int[] { 0, 1, 2, 0, 2, 3 };
                break;
            case 4:
                localVertices = new Vector3[] { new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(1, 0.5f) };
                localTriangles = new int[] { 0, 1, 2 };
                break;
            case 5:
                localVertices = new Vector3[] { new Vector3(0, 0.5f), new Vector3(0, 1), new Vector3(0.5f, 1), new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(1, 0.5f) };
                localTriangles = new int[] { 0, 1, 2, 3, 4, 5 };
                break;
            case 6:
                localVertices = new Vector3[] { new Vector3(0.5f, 0), new Vector3(0.5f, 1), new Vector3(1, 1), new Vector3(1, 0) };
                localTriangles = new int[] { 0, 1, 2, 0, 2, 3 };
                break;
            case 7:
                localVertices = new Vector3[] { new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(0, 0.5f) };
                localTriangles = new int[] { 2, 3, 1, 3, 4, 1, 4, 0, 1 };
                break;
            case 8:
                localVertices = new Vector3[] { new Vector3(0, 0.5f), new Vector3(0, 0), new Vector3(0.5f, 0) };
                localTriangles = new int[] { 2, 1, 0 };
                break;
            case 9:
                localVertices = new Vector3[] { new Vector3(0, 0), new Vector3(0.5f, 0), new Vector3(0.5f, 1), new Vector3(0, 1) };
                localTriangles = new int[] { 1, 0, 2, 0, 3, 2 };
                break;
            case 10:
                localVertices = new Vector3[] { new Vector3(0, 0), new Vector3(0, 0.5f), new Vector3(0.5f, 0), new Vector3(1, 1), new Vector3(0.5f, 1), new Vector3(1, 0.5f) };
                localTriangles = new int[] { 0, 1, 2, 5, 4, 3 };
                break;
            case 11:
                localVertices = new Vector3[] { new Vector3(0, 0), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0.5f), new Vector3(0.5f, 0) };
                localTriangles = new int[] { 0, 1, 2, 0, 2, 3, 4, 0, 3 };
                break;
            case 12:
                localVertices = new Vector3[] { new Vector3(0, 0), new Vector3(1, 0), new Vector3(1, 0.5f), new Vector3(0, 0.5f) };
                localTriangles = new int[] { 0, 3, 2, 0, 2, 1 };
                break;
            case 13:
                localVertices = new Vector3[] { new Vector3(0, 0), new Vector3(0, 1), new Vector3(0.5f, 1), new Vector3(1, 0.5f), new Vector3(1, 0) };
                localTriangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4 };
                break;
            case 14:
                localVertices = new Vector3[] { new Vector3(1, 1), new Vector3(1, 0), new Vector3(0, 0), new Vector3(0, 0.5f), new Vector3(0.5f, 1) };
                localTriangles = new int[] { 0, 1, 4, 1, 3, 4, 1, 2, 3 };
                break;
            case 15:
                localVertices = new Vector3[] { new Vector3(0, 0), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0) };
                localTriangles = new int[] { 0, 1, 2, 0, 2, 3 };
                break;
        }

        if (localVertices != null)
        {
            // Add the local vertices to the global vertices list, adjusting for grid position(offsetY and offsetX) and resolution
            foreach (Vector3 vert in localVertices)
            {
                Vector3 newVert = new Vector3((vert.x + offsetX) * gridResolution, (vert.y + offsetY) * gridResolution, 0);
                vertices.Add(newVert);
            }
            // Add the local triangles to the global triangles list, adjusting for vertex count
            foreach (int triangle in localTriangles)
            {
                triangles.Add(triangle + vertexCount);
            }
        }
    }

    private void CreateMesh()
    {
        //passin the vertices and triangles array
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals(); // Adjust ligthing

        meshFilter.mesh = mesh;

    }

    private int GetBinaryHeight(float value)
    {
        return value < heightThreshold ? 0 : 1;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos || heightMap == null) return;

        for (int x = 0; x <= gridSizeX; x++)
        {
            for (int y = 0; y <= gridSizeY; y++)
            {
                //draw spheres to represent the square grid vertices and ajust the color to visualize the heigthmap
                Vector3 pos = transform.TransformPoint(new Vector3(x * gridResolution, y * gridResolution, 0));
                Gizmos.color = new Color(heightMap[x, y], heightMap[x, y], heightMap[x, y], 1);
                Gizmos.DrawSphere(pos, gridResolution * 0.1f);
            }
        }
    }
}