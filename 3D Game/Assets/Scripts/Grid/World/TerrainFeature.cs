using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFeature : MonoBehaviour
{
    [SerializeField] GameObject InfestationMesh;
    [SerializeField] List<ParticleSystem> InfestationParticles;

    public void Infest()
    {
        InfestationMesh.SetActive(true);
        foreach(ParticleSystem particleSystem in InfestationParticles)
        {
            particleSystem.Play();
        }
    }

    public void CleanUp()
    {
        InfestationMesh.SetActive(false);
        foreach (ParticleSystem particleSystem in InfestationParticles)
        {
            particleSystem.Stop();
        }
    }
}
