using UnityEngine;
using UnityEngine.SceneManagement;

public class NTSceneManager : HimeLib.SingletonMono<NTSceneManager>
{
    protected override void OnSingletonAwake(){
        MarkAsCrossSceneSingleton();
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
