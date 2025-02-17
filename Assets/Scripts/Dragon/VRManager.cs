using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VRManager : MonoBehaviour
{
    [Header("物件設定")]
    public ExhibitionModel exhibitionModel;
    public SnakeParticle snakeParticle;

    [Header("參數")]
    public float exhibitionFadeTime = 0.7f;
    public float mousePointDistance = 30;
    public float MinBallSpeed = 0.75f;
    public float MaxBallSpeed = 5;

    [Header("Runtimes")]
    [SerializeField] float currentBallMoveSpeed = 0;

    void Start()
    {
        currentBallMoveSpeed = MaxBallSpeed;

#if UNITY_EDITOR
        //Editor 模式下顯示偵錯用的Point
        SnakeMove.instance.TurnTargetPointRender(true);
#endif

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            snakeParticle.BurstActionSwipe();
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            exhibitionModel.SwitchManual(exhibitionFadeTime);
        }
        if(Input.GetKeyDown(KeyCode.B)){
            InteractMode.instance.SetGameMode(InteractMode.Mode.Dance);
        }
        if(Input.GetKeyDown(KeyCode.M)){
            InteractMode.instance.SetGameMode(InteractMode.Mode.Artist);
        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mousePointDistance; // 設定滑鼠在3D空間中的深度
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 finalPos = targetPos;

        SnakeMove.instance.TargetPointSetPos(finalPos);
        SnakeMove.instance.SetupNewDragonBallMoveSpeed(currentBallMoveSpeed);
    }

    float distanceXZ(Vector3 point1, Vector3 point2)
    {
        return Vector2.Distance(new Vector2(point1.x, point1.z), new Vector2(point2.x, point2.z));
    }
}
