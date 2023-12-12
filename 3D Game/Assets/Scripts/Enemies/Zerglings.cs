using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zerglings : Enemy
{
    public bool isFirstZergling = true;
    public Zerglings zerglingPrefab;

     private List<Vector2Int> Zerklings;
     private List<Vector2Int> ZerklingNeighbor;
     private List<Vector2Int> possibleTiles;
     private List<Vector2Int> possibleNeighbors;


    public override void Setup(GridTile tile)
    {
        base.Setup(tile);

        if (isFirstZergling)
        {

            Zerklings = HexGridUtil.AxialNeighbors(axialLocation);
            possibleTiles = new List<Vector2Int>();
            foreach (Vector2Int zerkling in Zerklings)
            {
                if (GridManager.Instance.Grid.ContainsKey(zerkling))
                {
                    if (GridManager.Instance.Grid[zerkling].currentGridState == GridManager.Instance.gS_Positive && GridManager.Instance.Grid[zerkling].AxialCoordinate != PlayerManager.Instance.playerPosition)
                    {
                        possibleTiles.Add(zerkling);
                    }
                }
            }
            Vector2Int neighbor = possibleTiles[Random.RandomRange(0, possibleTiles.Count)];
            ZerklingNeighbor = new List<Vector2Int>();
            ZerklingNeighbor = HexGridUtil.AxialNeighbors(neighbor);

            possibleNeighbors = new List<Vector2Int>();
            foreach (Vector2Int neighbour in ZerklingNeighbor)
            {
                if (GridManager.Instance.Grid.ContainsKey(neighbour))
                {
                    if (GridManager.Instance.Grid[neighbour].currentGridState == GridManager.Instance.gS_Positive && GridManager.Instance.Grid[neighbour].AxialCoordinate != PlayerManager.Instance.playerPosition)
                    {
                        if (neighbour != axialLocation)

                            possibleNeighbors.Add(neighbour);

                    }
                }
            }
            Vector2Int neighboringNeighbor = possibleNeighbors[Random.Range(0, possibleNeighbors.Count)];



                Zerglings enemy2 = Instantiate(zerglingPrefab);
                GridTile targetlocato = GridManager.Instance.Grid[neighboringNeighbor];
                enemy2.isFirstZergling = false;

                enemy2.Setup(/*enemySOs[Random.Range(0, enemySOs.Count)], */targetlocato);
                enemy2.currentHealth = 1;
                targetlocato.ChangeCurrentState(GridManager.Instance.gS_Enemy);
                enemy2.transform.parent = targetlocato.transform;
                enemy2.transform.position = targetlocato.transform.position;

                Zerglings enemy3 = Instantiate(zerglingPrefab);
                GridTile targetlocatio = GridManager.Instance.Grid[neighbor];
                enemy3.isFirstZergling = false;
                enemy3.Setup(/*enemySOs[Random.Range(0, enemySOs.Count)], */targetlocatio);
                enemy3.currentHealth = 1;
                targetlocatio.ChangeCurrentState(GridManager.Instance.gS_Enemy);
                enemy3.transform.parent = targetlocatio.transform;
                enemy3.transform.position = targetlocatio.transform.position;




            }


        }
    }


