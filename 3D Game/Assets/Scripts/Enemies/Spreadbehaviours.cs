using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spreadbehaviours : ScriptableObject
{
    public abstract bool TargetTile(Vector3Int enemyPosition, out Vector3Int target, Vector2Int playerPosition);

    public abstract bool TargetTiles(Vector3Int origin, out List<Vector3Int> targets, Vector2Int closestPlayerPosition);
}
