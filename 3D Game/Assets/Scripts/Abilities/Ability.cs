using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum RotationMode 
{
    PlayerCenter,
    SelectedPointCenter,
} 

[CreateAssetMenu]

public class Ability : HexShape
{


    public List<Effect> Effects;
    [SerializeField] public List<Ability> NextLevel;
    [SerializeField] public int UpgradeCost;
    
    public string Name;
    public Mesh previewShape;
    public RotationMode rotato;

    [FormerlySerializedAs("Kosten")] public List<Ressource> costs;


    [SerializeField] public Sprite AbilityUISprite;

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

    [SerializeField] private ParticleSystem MyCastParticleSystem;

    public ParticleSystem CastParticleSystem
    {
        get { return CastParticleSystem; }
        set { CastParticleSystem = value; }
    }

    [SerializeField] private int myTierLevel;

    public int MyTierLevel
    {
        get { return myTierLevel; }
        set { myTierLevel = value; }
    }




}
