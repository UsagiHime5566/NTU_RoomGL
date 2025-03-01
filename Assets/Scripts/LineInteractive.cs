using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class LineInteractive : MonoBehaviour
{
    public GameObject LineRoom;
    public List<MeshRenderer> meshRenderers;
    public float planeSize = 5;
    public float planceWidth = 1920;
    public float planceHeight = 1080;

    public float colorSaturationTo = 0f;
    public float colorBrightnessTo = 0.5f;

    public Transform TargetDebugPointModel;

    [Header("滑鼠座標 Runtimes")]
    public Vector3 MousePointInModel;

    void UpdateMeshRenderer(float posX, float posY){
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetVector("iMouse", new Vector4(posX, posY, 0, 0));
        }
    }

    void Start(){
        DOTween.To(() => 1.1f, x => {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material.SetFloat("_ColorSaturation", x);
            }
        }, colorSaturationTo, 7).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuart);

        DOTween.To(() => 1.0f, x => {
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material.SetFloat("_ColorBrightness", x);
            }
        }, colorBrightnessTo, 5).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            LineRoom.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            LineRoom.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            LineRoom.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            LineRoom.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            LineRoom.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)){
            LineRoom.SetActive(true);
        }

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
                
                // 计算碰撞点在碰撞器上的归一化坐标(0~1)
                Vector2 normalizedPoint = CalculateNormalizedPointOnCollider(hit);
                
                // 更新材质的鼠标位置
                UpdateMeshRenderer(normalizedPoint.x, normalizedPoint.y);
            }
        }
    }
    
    // 计算碰撞点在碰撞器上的归一化坐标
    private Vector2 CalculateNormalizedPointOnCollider(RaycastHit hit)
    {
        // 获取碰撞器的变换
        Transform colliderTransform = hit.collider.transform;
        
        // 将世界坐标的碰撞点转换为碰撞器的局部坐标
        Vector3 localHitPoint = colliderTransform.InverseTransformPoint(hit.point);
        
        Vector2 outputPoint = Vector2.zero;
        
        outputPoint.x = (localHitPoint.x + planeSize) / (planeSize * 2) * planceWidth;
        outputPoint.y = (localHitPoint.z + planeSize) / (planeSize * 2) * planceHeight;

        if(colliderTransform.name == "0"){
            // Nothing
        }
        if(colliderTransform.name == "1"){
            outputPoint.x += planceWidth;
        }
        if(colliderTransform.name == "2"){
            outputPoint.x += planceWidth * 2;
        }
        if(colliderTransform.name == "4"){
            outputPoint.x += planceWidth;
            outputPoint.y -= planceHeight;
        }

        Debug.Log(outputPoint);
        return outputPoint;
    }
}
