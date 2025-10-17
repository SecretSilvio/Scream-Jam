using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteRandomizer : MonoBehaviour
{
    public List<Material> npcMaterials = new List<Material>(); // List of materials to choose from
    public MeshRenderer meshRenderer; // Reference to the MeshRenderer component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer.material = npcMaterials[Random.Range(0, npcMaterials.Count)];
    }
}
