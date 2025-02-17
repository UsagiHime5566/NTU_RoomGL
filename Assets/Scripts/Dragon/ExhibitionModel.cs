using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class ExhibitionModel : MonoBehaviour
{
    public TextMeshPro [] manualTexts;
    public MeshRenderer [] manualIcons;
    public MeshRenderer [] manualModels;
    public Transform TargetDebugPointModel;
    public Transform TargetDebugPointWorld;

    [Header("滑鼠座標長度設置")]
    public float worldDistance = 10;
    public float worldDistanceMin = 15;
    public float worldDistanceMax = 25;

    [Header("滑鼠座標 Runtimes")]
    public Vector3 MousePointInModel;
    public Vector3 MousePointInWorld;
    
    bool isManual = true;

    void Update(){
        // 從攝影機發射一條射線通過滑鼠位置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        // 進行射線檢測
        if (Physics.Raycast(ray, out hit))
        {
            // 檢查碰撞物體的標籤
            if (hit.collider.CompareTag("ExhibitionModel"))
            {
                MousePointInModel = hit.point;
                if(TargetDebugPointModel) TargetDebugPointModel.position = MousePointInModel;

                // 模型座標的長度乘上worldDistance，並且限制在worldDistanceMin和worldDistanceMax之間, 並且將結果轉換為世界座標
                float newPosLengh = Mathf.Clamp(MousePointInModel.magnitude * worldDistance, worldDistanceMin, worldDistanceMax);
                MousePointInWorld = MousePointInModel.normalized * newPosLengh;
                if(TargetDebugPointWorld) TargetDebugPointWorld.position = MousePointInWorld;
            }
        }
    }

    public void ShowManual(bool visible, float duration = 5)
    {
        isManual = visible;
        foreach (var item in manualTexts)
        {
            item.DOFade(visible ? 1 : 0, duration);
        }
        foreach (var item in manualIcons)
        {
            item.material.DOFade(visible ? 1 : 0, duration);
        }
        foreach (var item in manualModels)
        {
            item.material.DOFade(visible ? 1 : 0, duration);
        }
    }

    public void SwitchManual(float duration)
    {
        isManual = !isManual;
        ShowManual(isManual, duration);
    }
}
