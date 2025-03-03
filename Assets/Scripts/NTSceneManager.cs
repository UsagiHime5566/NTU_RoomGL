using UnityEngine;
using UnityEngine.SceneManagement;

public class NTSceneManager : MonoBehaviour
{
    public static NTSceneManager instance;
    public UIStageIntro uiStageIntro;
    public bool isMobile = false;

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
            SceneManager.LoadScene(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            SceneManager.LoadScene(2);
        }
    }

    public void UseMobile(){
        isMobile = true;
    }

    public void LoadScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }
}
