using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class VFXSwipe : MonoBehaviour
{
    //public VisualEffect VFXGraphicDragon;
    public SnakeMove snakemove;
    public SnakeMovement snakemovement;
    bool bursted = false;
    public float const_normal_value;
    public float const_burst_value;
    public float burst_value;
    public float burst_speed;
    public float turn_speed;

    public float blend_value;
    public float blend_burst_speed;
    public float blend_turn_speed;


    public AudioSource hang;

    private void Start()
    {
        burst_value = 1;
    }

    void Update()
    {
        //VFXGraphicDragon.SetFloat("Sphere_Radius", burst_value);
        //VFXGraphicDragon.SetFloat("Blend_value", blend_value);

        if (burst_value< 1.75f) 
        {
            BurstByHead();
        }
        
        if (bursted)
        {
            burst_value = Mathf.LerpUnclamped(burst_value, const_burst_value, Time.deltaTime * burst_speed);
            blend_value = 1;
        }
        else
        {
            burst_value = Mathf.Lerp(burst_value, const_normal_value, Time.deltaTime * turn_speed);
            blend_value = Mathf.Lerp(blend_value, 0f, Time.deltaTime * blend_turn_speed);
            
        }
        if (burst_value >= const_burst_value - .5f)
        {
            bursted = false;
        }
    }

    [Button]
    public void BurstActionSwipe()
    {
        //if(bursted) return;

        Debug.Log("Burst_Trigger");
        bursted = true;
        hang.Play();
    }
    void BurstByHead()
    {
        /*if (snakemove.movedistance < 0.05f && snakemove.controlled) 
        {
            bursted = true;
            hang.Play();
        }*/
    }
}

