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

    public void BurstActionSwipe(){
        snakeParticle.BurstActionSwipe();
    }

    public void SwitchManual(){
        exhibitionModel.SwitchManual(exhibitionFadeTime);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            BurstActionSwipe();
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            SwitchManual();
        }
        if(Input.GetKeyDown(KeyCode.B)){
            InteractMode.instance.SetGameMode(InteractMode.Mode.Dance);
        }
        if(Input.GetKeyDown(KeyCode.M)){
            InteractMode.instance.SetGameMode(InteractMode.Mode.Artist);
        }

        SnakeMove.instance.TargetPointSetPos(exhibitionModel.MousePointInWorld);
        SnakeMove.instance.SetupNewDragonBallMoveSpeed(currentBallMoveSpeed);
    }
}
