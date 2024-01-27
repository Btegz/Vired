using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFeature : MonoBehaviour
{
    [SerializeField] GameObject InfestationMesh;
    [SerializeField] List<ParticleSystem> InfestationParticles;

    public void Infest()
    {
        if (InfestationMesh != null)
        {
            InfestationMesh.SetActive(true);
        }
        if (InfestationParticles != null)
        {
            foreach (ParticleSystem particleSystem in InfestationParticles)
            {
                particleSystem.Play();
            }
        }

    }

    public void CleanUp()
    {
        if (InfestationMesh != null)
        {
            InfestationMesh.SetActive(false);
        }
        if (InfestationParticles != null)
        {
            foreach (ParticleSystem particleSystem in InfestationParticles)
            {
                particleSystem.Stop();
            }
        }
    }
}
