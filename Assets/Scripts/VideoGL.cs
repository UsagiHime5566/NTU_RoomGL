using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Networking;

public class VideoGL : MonoBehaviour
{
    public VideoPlayer Wall;
    public VideoPlayer Floor;
    public AudioSource Audio;
    public List<string> VideoNameWall;
    public List<string> VideoNameGround;
    public List<string> SoundName;

    int readyCount = 0;

    void Start()
    {
        Wall.prepareCompleted += OnVideoPrepared;
        Floor.prepareCompleted += OnVideoPrepared;

        StopPlay();
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
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            SetupPlay(4);
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)){
            StopPlay();
        }
        if(Input.GetKeyDown(KeyCode.Alpha7)){
            StopPlay();
        }
    }

    public void StopPlay(){
        Wall.Stop();
        Floor.Stop();
        Audio.Stop();
        
        ClearTextureToTransparent(Wall.targetTexture);
        ClearTextureToTransparent(Floor.targetTexture);
    }

    private void ClearTextureToTransparent(RenderTexture renderTexture)
    {
        RenderTexture currentRT = RenderTexture.active;
        
        RenderTexture.active = renderTexture;
        
        GL.Clear(true, true, new Color(0, 0, 0, 0));
        
        RenderTexture.active = currentRT;
    }

    public void SetupPlay(int index){
        Wall.Stop();
        Floor.Stop();
        Audio.Stop();

        readyCount = 0;

        Wall.url = Path.Combine(Application.streamingAssetsPath, VideoNameWall[index]);
        Wall.Prepare();

        Floor.url = Path.Combine(Application.streamingAssetsPath, VideoNameGround[index]);
        Floor.Prepare();

        StartCoroutine(LoadAudioFile(index));
    }

    private IEnumerator LoadAudioFile(int index)
    {
        if(string.IsNullOrEmpty(SoundName[index])){
            Audio.clip = null;
            readyCount++;
            ShouldPlay();
            yield break;
        }

        string audioPath = Path.Combine(Application.streamingAssetsPath, SoundName[index]);
        
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Audio.clip = DownloadHandlerAudioClip.GetContent(www);
                readyCount++;
                ShouldPlay();
            }
            else
            {
                Debug.LogError($"音頻加載失敗: {www.error}");
            }
        }
    }

    void OnVideoPrepared(VideoPlayer player){
        readyCount++;
        ShouldPlay();
    }

    void ShouldPlay(){
        if(readyCount == 3){
            Wall.Play();
            Floor.Play();
            Audio.Play();
        }
    }
}
