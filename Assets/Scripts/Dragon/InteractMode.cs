using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMode : HimeLib.SingletonMono<InteractMode>
{
    public SnakeMove m_snakemove;
    public VFXSwipe vfxSwipe;
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
            CurrentMode = (Mode)(((int)CurrentMode + 1) % 2);
            
            SetGameMode(CurrentMode);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            vfxSwipe.BurstActionSwipe();
        }
    }

    public void SetGameMode(Mode mode){
        if(mode == Mode.Artist){
            m_snakemove.TurnController(false);
        } else {
            m_snakemove.TurnController(true);
        }
        CurrentMode = mode;
    }
}
