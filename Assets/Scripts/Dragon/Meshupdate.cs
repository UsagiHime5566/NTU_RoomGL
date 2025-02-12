using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Meshupdate : MonoBehaviour
{
    public SkinnedMeshRenderer skinmesh;
    public VisualEffect VFXGraphic;
    public float RefreshRate;
    void Start()
    {
        StartCoroutine(updateCollidermesh());
    }
    private void Update()
    {
        updateCollidermesh();
    }
    IEnumerator updateCollidermesh()
    {
        /*Mesh m = new Mesh();
        skinmesh.BakeMesh(m);
        Vector3[] vertices = m.vertices;
        Mesh m2 = new Mesh();
        m2.vertices = vertices;
        VFXGraphic.SetMesh("Dragon_Mesh", m);*/

        VFXGraphic.SetSkinnedMeshRenderer(0, skinmesh);

        yield return new WaitForSeconds(RefreshRate); 
    }
}
