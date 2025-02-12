using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using com.rfilkov.kinect;
using UnityEngine.UI;

public class SnakeMove : HimeLib.SingletonMono<SnakeMove>
{
    [Header("常數設定")]
    public float targetspeed;       //龍珠移動速率
    public float constmovespeed;    //龍體移動速度
    public float arriveRange;       //多少距離內需要切換新位置
    public float minNextRange = 15; //新位置至少要離上一點多少距離
    public float minNextFromRoot = 22; //新位置至少要離原點多少距離
    public float constdamping;      //龍轉頭速率
    public float Controllspeed;     //擺頭控制移動速度
    public Vector2 X_RangeWalk = new Vector2(-25, 25);
    public Vector2 Y_RangeWalk = new Vector2(-20, 20);
    public Vector2 Z_RangeWalk = new Vector2(-25, 25);

    [Header("Runtime Params")]
    Vector3 targetPoint;            //當前龍珠目標位置
    public float currentdistance;   //當前龍珠與龍頭的距離
    public float currentmovespeed;  //當前龍體移動速度
    public float currentdamping;    //當前龍轉頭速率
    public bool CanControll;        //是否可以被操作
    public bool BUSRTING;
    public bool isDebug = true;

    [Header("特效設定")]
    public GameObject TargetOBJ;
    public Transform DragonHead;
    public SkinnedMeshRenderer targetRender;
    public SkinnedMeshRenderer targetRenderTwo;

    [Header("其餘設定")]

    public float ControllValueX;
    public float ControllValueY;


    void Start()
    {
        currentmovespeed = constmovespeed; //設定移動速度
        currentdamping = constdamping; //設定轉向速度
    }

    void Update()
    {
        DragomMove(); //米龍隨時更新位置
        BurstAction(); //指定動作揮散米龍的狀態銜接(未使用)

        if (!CanControll)
        {
            TurnBallMaterial(false);
            BallAutoTrunNext(); //龍到到點，自動切換下移動目標
        }
        else
        {
            TurnBallMaterial(true);
            //HandSetNewTargetpoint();
        }
    }

    public void TurnController(bool val)
    {
        CanControll = val;
    }
    void BallAutoTrunNext()
    {
        TargetOBJ.transform.position = Vector3.SlerpUnclamped(TargetOBJ.transform.position, targetPoint, Time.deltaTime * targetspeed);

        if (Vector3.Distance(TargetOBJ.transform.position, targetPoint) < arriveRange)
        {
            float x = Random.Range(X_RangeWalk.x, X_RangeWalk.y);
            float y = Random.Range(Y_RangeWalk.x, Y_RangeWalk.y);
            float z = Random.Range(Z_RangeWalk.x, Z_RangeWalk.y);

            Vector3 randomVec = new Vector3(x, y, z); //預設目標位置

            //距離過小，再更新一個位置
            while (Vector3.Distance(targetPoint, randomVec) < minNextRange && Vector3.Magnitude(randomVec) > minNextFromRoot)
            {
                x = Random.Range(X_RangeWalk.x, X_RangeWalk.y);
                y = Random.Range(Y_RangeWalk.x, Y_RangeWalk.y);
                z = Random.Range(Z_RangeWalk.x, Z_RangeWalk.y);
                randomVec = new Vector3(x, y, z); //預設目標位置
            }
            targetPoint = new Vector3(x, y, z);
            currentdistance = Vector3.Distance(TargetOBJ.transform.position, targetPoint);
        }
    }
    void HandSetNewTargetpoint()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 計算移動方向
        Vector3 moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);

        TargetOBJ.transform.Translate(moveDirection * Time.deltaTime * Controllspeed, Space.World);
        //TargetOBJ.transform.Translate(new Vector3((controllXR + controllXL) * .5f, (controllYR + controllYL) * .5f, 0) * Time.deltaTime * Controllspeed, Space.World);

        //移動邊界設定
        // if (TargetOBJ.transform.localPosition.x >= 40) 
        //     TargetOBJ.transform.localPosition = new Vector3(40, TargetOBJ.transform.localPosition.y, TargetOBJ.transform.localPosition.z);
        // if (TargetOBJ.transform.localPosition.x <= -40)
        //     TargetOBJ.transform.localPosition = new Vector3(-40, TargetOBJ.transform.localPosition.y, TargetOBJ.transform.localPosition.z);
        // if (TargetOBJ.transform.localPosition.y >= 20)
        //     TargetOBJ.transform.localPosition = new Vector3(TargetOBJ.transform.localPosition.x, 20, TargetOBJ.transform.localPosition.z);
        // if (TargetOBJ.transform.localPosition.y <= -20)
        //     TargetOBJ.transform.localPosition = new Vector3(TargetOBJ.transform.localPosition.x, -20, TargetOBJ.transform.localPosition.z);
    }

    public void DragonBallTranslate(Vector3 moveDirection){
        if(!CanControll) return;
        TargetOBJ.transform.Translate(moveDirection * Controllspeed * Time.deltaTime);
    }

    public void DragonBallSetPos(Vector3 pos){
        if(!CanControll) return;
        TargetOBJ.transform.position = pos;
    }

    void DragomMove()
    {
        DragonHead.localPosition = Vector3.MoveTowards(DragonHead.localPosition, TargetOBJ.transform.localPosition, currentmovespeed * Time.deltaTime); //For Position

        var rotation = Quaternion.LookRotation(TargetOBJ.transform.position - DragonHead.position);
        DragonHead.rotation = Quaternion.Slerp(DragonHead.rotation, rotation, Time.deltaTime * currentdamping);
    }

    void BurstAction()
    {
        if (!BUSRTING)
        {
            currentmovespeed = Mathf.Lerp(currentmovespeed, constmovespeed, Time.deltaTime * 1);
            currentdamping = Mathf.Lerp(currentdamping, constdamping, Time.deltaTime * 1);
        }
        else
        {
            currentmovespeed = Mathf.Lerp(currentmovespeed, 0, Time.deltaTime * 5);
            currentdamping = Mathf.Lerp(currentdamping, 0, Time.deltaTime * 5);
        }
    }

    void TurnBallMaterial(bool val){
        targetRender.enabled = val;
        targetRenderTwo.enabled = val;

        if(isDebug){
            targetRender.enabled = true;
            targetRenderTwo.enabled = true;
        }
    }
}
