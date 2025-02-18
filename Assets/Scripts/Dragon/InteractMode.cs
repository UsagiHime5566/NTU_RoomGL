using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMode : HimeLib.SingletonMono<InteractMode>
{
    [HimeLib.HelpBox] public string tip = "按下Tab鍵切換模式";
    public enum Mode
    {
        Dance = 1,  //可操作 視角不可移動 (舉手後，決定操作者)
        Artist = 0  //不可操作 視角不可移動 (無)
    }
    public Mode CurrentMode = Mode.Artist; 

    void Start()
    {
        CurrentMode = Mode.Artist;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab)){
            var nextMode = (Mode)(((int)CurrentMode + 1) % 2);
            SetGameMode(nextMode);
        }
    }

#if UNITY_EDITOR    
    void OnValidate() {
        if(!Application.isPlaying) return;
        if(SnakeMove.instance == null) return;
        CheckMode();
    }
#endif

    public void SetGameMode(Mode mode){
        if(mode != CurrentMode)
        {
            CurrentMode = mode;
            CheckMode();
        }
    }

    void CheckMode(){
        if (CurrentMode == Mode.Artist)
        {
            SnakeMove.instance.TurnController(false);
            SnakeMove.instance.BornDragonBall();
        }
        else
        {
            SnakeMove.instance.TurnController(true);
            SnakeMove.instance.ResetControlBallPos();
        }
    }
}
