using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoGL : MonoBehaviour
{
    public VideoPlayer Wall;
    public VideoPlayer Floor;
    public List<string> VideoNameWall;
    public List<string> VideoNameGround;
    void Start()
    {
        SetupPlay(0);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            SetupPlay(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            SetupPlay(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            SetupPlay(2);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            SetupPlay(3);
        }
    }

    void SetupPlay(int index){
        Wall.Stop();
        Floor.Stop();

        Wall.url = Path.Combine(Application.streamingAssetsPath, VideoNameWall[index]);
        Wall.Play();

        Floor.url = Path.Combine(Application.streamingAssetsPath, VideoNameGround[index]);
        Floor.Play();
    }
}
