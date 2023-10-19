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

    [Header ("Tile Presets")]
    [SerializeField] float innerSize;
    [SerializeField] float outerSize;
    [SerializeField] float height;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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
            foreach(GridCell cell in GetComponentsInChildren<GridCell>())
            {
                Destroy(cell.gameObject);
            }

            ;
            GenerateGrid();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                GridCell tile = Instantiate(GridCellPrefab, GetPositionForHexFromCoordinate(new Vector2Int(x,y)),Quaternion.identity, transform);
                tile.name = $"Hex{x},{y}";
                tile.innerSize = innerSize;
                tile.outerSize = outerSize;
                tile.height = height;
                tile.DrawMesh();
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

        offset = (column % 2 == 0)? height / 2 : 0;

        xpos = column * horizontalDistance;
        ypos = row * verticalDistance - offset;

        return new Vector3(xpos,0,ypos);
    }
}
