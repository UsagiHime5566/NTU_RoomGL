using UnityEngine;
using UnityEngine.Video;

public class Player360 : MonoBehaviour
{
    public Material matToPlay;
    private VideoPlayer videoPlayer;
    public string videoUrl;

    void Start()
    {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
            videoPlayer = gameObject.AddComponent<VideoPlayer>();

        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.MaterialOverride;

        if (!string.IsNullOrEmpty(videoUrl))
        {
            videoPlayer.url = videoUrl;
            videoPlayer.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
