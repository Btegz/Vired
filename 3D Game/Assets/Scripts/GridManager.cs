using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] GridCell GridCellPrefab;

    public List<List<GridCell>> Grid;

    [SerializeField] Vector2 gridSize;

    [Header("Tile Presets")]
    [SerializeField] float innerSize;
    [SerializeField] float outerSize;
    [SerializeField] float height;

    [Header("Resource Materials")]
    [SerializeField] public Material resourceAMaterial;
    [SerializeField] public Material resourceBMaterial;
    [SerializeField] public Material resourceCMaterial;
    [SerializeField] public Material resourceDMaterial;
    [SerializeField] public Material neutralMaterial;
    [SerializeField] public Material negativeMaterial;

    List<GridState> gridStates;

    //[Header("Resource Materials")]
    [SerializeField] public Enemy enemyPrefab;
    [SerializeField] public List<EnemySO> enemySOs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gridStates = new List<GridState>();
            gridStates.Add(new GS_positive());
            gridStates.Add(new GS_neutral());
            gridStates.Add(new GS_negative());
            gridStates.Add(new GS_Enemy());
            gridStates.Add(new GS_Boss());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            foreach (GridCell cell in GetComponentsInChildren<GridCell>())
            {
                Destroy(cell.gameObject);
            }
            try
            {
                GenerateGrid();
            }
            catch
            {
                Debug.Log("I FAILED TO GENERATE GRID IN ONVALIDATE");
            }

        }
    }

    public void GenerateGrid()
    {
        Grid = new List<List<GridCell>>((int)gridSize.x);
        for (int x = 0; x < gridSize.x; x++)
        {
            Grid.Insert(x, new List<GridCell>((int)gridSize.y));
            for (int y = 0; y < gridSize.y; y++)
            {
                GridCell tile = Instantiate(GridCellPrefab, GetPositionForHexFromCoordinate(new Vector2Int(x, y)), Quaternion.identity, transform);
                tile.Setup(new Vector2Int(x, y), (Ressource)Random.Range(0, 4), gridStates[Random.Range(0, gridStates.Count)]);
                tile.name = $"Hex{x},{y}";
                tile.innerSize = innerSize;
                tile.outerSize = outerSize;
                tile.height = height;
                tile.DrawMesh();
                Grid[x].Insert(y, tile);
            }
        }
    }

    public Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float width;
        float height;
        float xpos;
        float ypos;
        float offset;
        float horizontalDistance;
        float verticalDistance;
        float size = outerSize;

        width = 2f * size;
        height = Mathf.Sqrt(3f) * size;
        horizontalDistance = width * (3f / 4f);
        verticalDistance = height;

        offset = (column % 2 == 0) ? height / 2 : 0;

        xpos = column * horizontalDistance;
        ypos = row * verticalDistance - offset;

        return new Vector3(xpos, 0, ypos);
    }

    public GridCell PickRandomTile()
    {
        int x;
        int y;

        x = Random.Range(0, Grid.Count);
        y = Random.Range(0, Random.Range(0, Grid[x].Count));

        return Grid[x][y];
    }
}
