using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public Transform[] BodyParts;
    public float speed = 5;
    public float mindistance = 5;
    public bool BUSRTING;

    void Update()
    {
        if(!BUSRTING) Movements();
    }

    public void Movements()
    {
        float currentSpeed = speed;
        
        for (int i = 1; i < BodyParts.Length; i++)
        {
            var currentTransfrom = BodyParts[i];
            var PreviousTransfrom = BodyParts[i - 1];

            var dis = Vector3.Distance(PreviousTransfrom.position, currentTransfrom.position);

            Vector3 newpos = PreviousTransfrom.position;

            float T = Time.deltaTime * dis / mindistance * currentSpeed;

            if (T > 0.5f) T = 0.5f;

            currentTransfrom.position = Vector3.Slerp(currentTransfrom.position, newpos, T);
            currentTransfrom.rotation = Quaternion.Slerp(currentTransfrom.rotation, PreviousTransfrom.rotation, T);
        }
    }
}
