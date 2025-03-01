using UnityEngine;
using UnityEngine.UI;

public class UIStageIntro : MonoBehaviour
{
    public bool isIntroVisivle = true;

    public System.Action<bool> onIntroVisibleChanged;

    public void TriggerIntroCurrent(){
        onIntroVisibleChanged?.Invoke(isIntroVisivle);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isIntroVisivle = !isIntroVisivle;
            onIntroVisibleChanged?.Invoke(isIntroVisivle);
        }
    }
}
