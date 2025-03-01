using System.Collections.Generic;
using UnityEngine;

public class LineGLController : MonoBehaviour
{
    public float mouseScale = 3;
    public float offset = 1920;
    public List<MeshRenderer> meshRenderers;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        
        // 使用Shader中定義的屬性名稱 "iMouse"
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material.SetVector("iMouse", new Vector4(mousePos.x * mouseScale + offset, mousePos.y * mouseScale, 0, 0));
        }
    }
} 