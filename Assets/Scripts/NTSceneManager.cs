using UnityEngine;
using UnityEngine.SceneManagement;

public class NTSceneManager : MonoBehaviour
{
    public static NTSceneManager instance;
    public UIStageIntro uiStageIntro;

    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9)){
            SceneManager.LoadScene(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            SceneManager.LoadScene(1);
        }
    }
}
