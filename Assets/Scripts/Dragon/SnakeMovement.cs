using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public Transform[] BodyParts;
    public float speed;

    public float mindistance = 0.25f;
    Transform currentTrans;
    Transform PreviousTrans;

    float dis;

    public bool BUSRTING;

    void Update()
    {
        if(!BUSRTING)   Movements();
    }

    public void Movements()
    {
        float currentSpeed = speed;

        //BodyParts[0].Translate(BodyParts[0].forward * currentSpeed * Time.smoothDeltaTime);
        
        for (int i = 1; i < BodyParts.Length; i++)
        {
            currentTrans = BodyParts[i];
            PreviousTrans = BodyParts[i - 1];

            dis = Vector3.Distance(PreviousTrans.position, currentTrans.position);

            Vector3 newpos = PreviousTrans.position;

            float T = Time.deltaTime * dis / mindistance * currentSpeed;

            if (T > .5f)
                T = .5f;


            currentTrans.position = Vector3.Slerp(currentTrans.position, newpos, T);
            currentTrans.rotation = Quaternion.Slerp(currentTrans.rotation, PreviousTrans.rotation, T);
        }

    }
}
