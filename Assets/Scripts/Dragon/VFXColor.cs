using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXColor : MonoBehaviour
{
    public VisualEffect VFXGraphicDragon;
    private void Awake()
    {
        VFXGraphicDragon = GetComponent<VisualEffect>();
    }
    void Update()
    {

    }
    void UpdateColor()
    {
        //VFXGraphicDragon.SetFloat("ParticleColor", );
    }
}
