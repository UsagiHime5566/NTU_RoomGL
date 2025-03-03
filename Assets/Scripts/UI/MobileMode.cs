using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MobileMode : MonoBehaviour
{
    public VideoGL videoGL;
    public LineInteractive lineInteractive;
    public VRManager vrManager;
    public CanvasGroup uiElements;

    public Button BTN_1;
    public Button BTN_2;
    public Button BTN_3;
    public Button BTN_4;
    public Button BTN_5;
    public Button BTN_6;
    public Button BTN_7;

    public Button BTN_Space;
    public Button BTN_Enter;
    public Button BTN_B;
    public Button BTN_M;

    public Button BTN_9;
    public Button BTN_0;

    void Start(){

        BTN_1?.onClick.AddListener(() => {
            videoGL.SetupPlay(0);
            lineInteractive.LineRoom.SetActive(false);
        });

        BTN_2?.onClick.AddListener(() => {
            videoGL.SetupPlay(1);
            lineInteractive.LineRoom.SetActive(false);
        });

        BTN_3?.onClick.AddListener(() => {
            videoGL.SetupPlay(2);
            lineInteractive.LineRoom.SetActive(false);
        });

        BTN_4?.onClick.AddListener(() => {
            videoGL.SetupPlay(3);
            lineInteractive.LineRoom.SetActive(false);
        });

        BTN_5?.onClick.AddListener(() => {
            videoGL.SetupPlay(4);
            lineInteractive.LineRoom.SetActive(false);
        });

        BTN_6?.onClick.AddListener(() => {
            videoGL.StopPlay();
            lineInteractive.LineRoom.SetActive(true);
        });

        BTN_7?.onClick.AddListener(() => {
            videoGL.StopPlay();
            lineInteractive.LineRoom.SetActive(false);
        });


        BTN_Space?.onClick.AddListener(() => {
            vrManager.BurstActionSwipe();
        });

        BTN_Enter?.onClick.AddListener(() => {
            vrManager.SwitchManual();
        });

        BTN_B?.onClick.AddListener(() => {
            InteractMode.instance.SetGameMode(InteractMode.Mode.Dance);
        });

        BTN_M?.onClick.AddListener(() => {
            InteractMode.instance.SetGameMode(InteractMode.Mode.Artist);
        });

        BTN_9.onClick.AddListener(() => {
            SceneManager.LoadScene(1);
        });

        BTN_0.onClick.AddListener(() => {
            SceneManager.LoadScene(2);
        });

        StartCoroutine(CheckMobile());
    }

    IEnumerator CheckMobile(){
        yield return new WaitForSeconds(1);
        if(NTSceneManager.instance.isMobile){
            UseMobileUI();
        }
        else{
            Show(false);
        }
    }


    public void UseMobileUI(){
        Show(true);
    }

    public void Show(bool show){
        uiElements.alpha = show ? 1 : 0;
        uiElements.interactable = show;
        uiElements.blocksRaycasts = show;
    }
}
