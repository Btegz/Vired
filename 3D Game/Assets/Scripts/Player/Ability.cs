using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ability : HexShape
{
    public List<Effect> Effects;

    public string name;

    public Mesh previewShape; 


    public Dictionary<Vector2Int, Effect> AbilityEffectDict;

    public Dictionary<Vector2Int, Effect> GetAbilityEffectDict()
    {
        Dictionary<Vector2Int, Effect> neww = new Dictionary<Vector2Int, Effect>();

        for (int i = 0; i < Effects.Count; i++)
        {
            neww.Add(Coordinates[i], Effects[i]);
        }

        return neww;
    }
}
