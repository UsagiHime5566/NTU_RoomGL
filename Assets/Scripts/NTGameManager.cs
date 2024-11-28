using UnityEngine;
using UnityEngine.SceneManagement;

public class NTGameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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
