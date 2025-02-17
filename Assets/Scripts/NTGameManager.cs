using UnityEngine;
using UnityEngine.UI;
//using TriLibCore.SFB;

public class NTGameManager : MonoBehaviour
{
    public Button BTN_Wall;
    public Text TXT_Wall;
    void Start()
    {
        // BTN_Wall.onClick.AddListener(() =>
        // {
        //     LoadVideo(0, TXT_Wall);
        //     //Debug.Log("click event");
        // });
    }

    void LoadVideo(int index, Text txt)
    {
        // var extensions = new[] {
        //     new ExtensionFilter("Support Video Files", "mp4"),
        //     new ExtensionFilter("All Files", "*" ),
        // };
        // var result = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        // if (result != null)
        // {
        //     var hasFiles = result.Count > 0 && result[0].HasData;

        //     if (hasFiles)
        //     {
        //         string filePath = result[0].Name;

        //         if (txt) txt.text = filePath;
        //         SystemConfig.Instance.SaveData("Video" + index, filePath);
        //         //SetupVideo(index, filePath);
        //         Debug.Log("Success");
        //     }
        // }
    }
}
