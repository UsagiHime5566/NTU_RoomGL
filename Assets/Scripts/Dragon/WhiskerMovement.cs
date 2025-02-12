using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiskerMovement : MonoBehaviour
{
    public Transform[] WhiskerParts;
    public float speed;

    public float mindistance = 0.25f;
    Transform currentTrans;
    Transform PreviousTrans;

    float dis;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movements();
        Debug.Log(WhiskerParts[0].transform.position);
    }

    public void Movements()
    {
        float currentSpeed = speed;

        //Debug.Log();
        //BodyParts[0].Translate(BodyParts[0].forward * currentSpeed * Time.smoothDeltaTime);

        for (int i = 1; i < WhiskerParts.Length; i++)
        {
            currentTrans = WhiskerParts[i];
            PreviousTrans = WhiskerParts[i - 1];

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
