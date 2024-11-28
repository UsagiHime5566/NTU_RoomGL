using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoGL : MonoBehaviour
{
    public VideoPlayer Wall;
    public string fileWall;
    public VideoPlayer Floor;
    public string fileFloor;
    void Start()
    {
        Wall.url = Path.Combine(Application.streamingAssetsPath, fileWall);
        Wall.Play();

        Floor.url = Path.Combine(Application.streamingAssetsPath, fileFloor);
        Floor.Play();
    }
}
