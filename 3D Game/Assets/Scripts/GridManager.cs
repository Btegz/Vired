using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] GridCell GridCellPrefab;

    public List<List<GridCell>> Grid;

    [SerializeField] int xSize;
    [SerializeField] int zSize;

    [SerializeField] float CellMargin = 0.025f;

    float xDiff = Mathf.Sign(60);


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
        Debug.Log(xDiff);
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateGrid()
    {
        Grid = new List<List<GridCell>>();
        for(int x = 0; x < xSize; x++)
        {
            Grid.Add(new List<GridCell>());
            for(int z = 0; z < zSize; z++)
            {
                Vector3 targetPosition = new Vector3(x, 0, (x % 2 == 1 ? z - .5f : z));

                targetPosition += targetPosition * CellMargin;

                GridCell newCell = Instantiate(GridCellPrefab,targetPosition,Quaternion.identity, transform);
                Grid[x].Add(newCell);

                newCell.Setup(new Vector3(x, 0, z));
            }
        }
    }
}
