using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class EnemySO : ScriptableObject
{
    [SerializeField] private string Name;
    public string myName
    {
        get { return name; }
        set { name = value; }
    }
    [SerializeField] private int currentHealth;
    public int myCurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    [SerializeField] private int maxHealth;
    public int mymaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    [SerializeField] private Ressource ressource;
    public Ressource myRessource
    {
        get { return ressource; }
        set { ressource = value; }
    }
    [SerializeField] private Mesh mesh;
    public Mesh myMesh
    {
        get { return mesh; }
        set { mesh = value; }
    }
    [SerializeField] private Material material;
    public Material myMaterial
    {
        get { return material; }
        set { material = value; }
    }
}
