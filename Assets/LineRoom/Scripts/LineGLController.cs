using UnityEngine;

public class LineGLController : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        
        // 使用Shader中定義的屬性名稱 "iMouse"
        meshRenderer.material.SetVector("iMouse", mousePos);
    }
} 