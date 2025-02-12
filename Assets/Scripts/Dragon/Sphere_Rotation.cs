using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_Rotation : MonoBehaviour
{
    public GameObject[] Spheres; //球體外框
    public float R_Time; //運作時間
    public float[] rotationSpeed; //旋轉速度
    public Vector3[] R_Vec; //旋轉軸向
    float RandomT() { return Random.Range(10, 15); }
    Vector3 RandomV() { return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)); }

    float RandomS() { return Random.Range(10, 20f); }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < Spheres.Length; i++)
        {
            rotationSpeed[i] = Mathf.Lerp(rotationSpeed[i], 0, Time.deltaTime * .5f);
            if (rotationSpeed[i] <= 0.5f)
            {
                R_Vec[i] = RandomV();
                rotationSpeed[i] = RandomS();


            }
            Spheres[i].transform.Rotate(R_Vec[i], rotationSpeed[i] * Time.deltaTime * .25f);
        }
        
        



    }
}
