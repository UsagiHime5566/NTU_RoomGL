using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntro : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    void OnEnable() {
        StartCoroutine(WaitForNTSceneManager());
    }

    void OnDisable() {
        NTSceneManager.instance.uiStageIntro.onIntroVisibleChanged -= OnIntroVisibleChanged;
    }

    IEnumerator WaitForNTSceneManager() {
        yield return new WaitUntil(() => NTSceneManager.instance != null);
        NTSceneManager.instance.uiStageIntro.onIntroVisibleChanged += OnIntroVisibleChanged;
        NTSceneManager.instance.uiStageIntro.TriggerIntroCurrent();
    }

    private void OnIntroVisibleChanged(bool isVisible)
    {
        canvasGroup.alpha = isVisible ? 1 : 0;
    }
}
