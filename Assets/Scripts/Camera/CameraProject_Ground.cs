using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraProject_Ground : MonoBehaviour
{
    // 主要組件引用
    public Camera Acamera;                    // 相機組件
    public GameObject AScreenOBJ;             // 螢幕物件
    public GameObject UserOBJ;                // 使用者物件

    // 相機位置和角度調整參數
    public Vector3 OffsetAngle;              // 相機角度偏移量
    public float BottomOffset;               // 底部偏移量
    public float TopOffset;                  // 頂部偏移量
    public float DistanceOffset;             // 距離偏移量
    public Vector3 userheadLimit;            // 使用者頭部位置

    // 螢幕參數設置
    [Tooltip("Width of the display in meters.")]
    public float screenWidth = 1.6f;         // 螢幕寬度（米）

    [Tooltip("Height of the display in meters.")]
    public float screenHeight = 0.9f;        // 螢幕高度（米）

    // 視錐體參數
    private float left = -0.2F;              // 視錐體左邊界
    private float right = 0.2F;              // 視錐體右邊界
    private float bottom = -0.2F;            // 視錐體底部邊界
    private float top = 0.2F;                // 視錐體頂部邊界

    public float standardUserDistance = 2f;   // 標準使用者距離

    // 初始相機參數
    private Vector3 initialCamPos;           // 初始相機位置
    private Quaternion initialCamRot;        // 初始相機旋轉
    private Matrix4x4 initialCamMat;         // 初始相機變換矩陣
    private Matrix4x4 initialCamPrjMat;      // 初始相機投影矩陣

    private Quaternion initialHeadRot = Quaternion.Euler(0f, 180f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        Acamera = GetComponent<Camera>();
        initialCamPos = Acamera.transform.position;
        initialCamRot = Acamera.transform.rotation;
        initialCamMat = Matrix4x4.TRS(initialCamPos, initialCamRot, Vector3.one);
        initialCamPrjMat = Acamera.projectionMatrix;
    }

    void LateUpdate()
    {
        // 更新使用者頭部位置
        userheadLimit = UserOBJ.transform.position;

        // 計算使用者與螢幕的實際距離
        standardUserDistance = Mathf.Abs(AScreenOBJ.transform.position.y - userheadLimit.y) + DistanceOffset;

        // 根據使用者位置計算視錐體的邊界
        // 計算左右邊界，考慮使用者在x軸的偏移
        left = Acamera.nearClipPlane * (-screenWidth / 2 - userheadLimit.x + AScreenOBJ.transform.position.x) / standardUserDistance;
        right = Acamera.nearClipPlane * (screenWidth / 2 - userheadLimit.x + AScreenOBJ.transform.position.x) / standardUserDistance;

        // 計算上下邊界，考慮使用者在z軸的偏移
        bottom = Acamera.nearClipPlane * (-screenHeight / 2 - userheadLimit.z + AScreenOBJ.transform.position.z + BottomOffset) / standardUserDistance;
        top = Acamera.nearClipPlane * (screenHeight / 2 - userheadLimit.z + AScreenOBJ.transform.position.z + TopOffset) / standardUserDistance;

        // 設置相機位置和旋轉
        Vector3 headCamPos = new Vector3(userheadLimit.x, userheadLimit.y, userheadLimit.z);
        Quaternion headCamRot = Quaternion.LookRotation(Vector3.down, Vector3.up);    // 相機朝下看
        Quaternion OffsetRot = Quaternion.Euler(OffsetAngle);                         // 應用額外旋轉偏移

        // 計算最終相機變換矩陣
        Matrix4x4 camPoseMat = Matrix4x4.TRS(headCamPos, headCamRot * OffsetRot, Vector3.one);
        camPoseMat = camPoseMat * initialCamMat;

        // 更新相機位置和旋轉
        Acamera.transform.position = camPoseMat.GetColumn(3);
        Acamera.transform.localRotation = camPoseMat.rotation;

        // 使用自定義視錐體參數創建投影矩陣
        Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, Acamera.nearClipPlane, Acamera.farClipPlane);
        Acamera.projectionMatrix = m;
    }

    // 創建非對稱視錐體的投影矩陣
    private Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        // 計算投影矩陣的各個參數
        float x = 2.0F * near / (right - left);      // x軸縮放因子
        float y = 2.0F * near / (top - bottom);      // y軸縮放因子
        float a = (right + left) / (right - left);   // x軸偏移
        float b = (top + bottom) / (top - bottom);   // y軸偏移
        float c = -(far + near) / (far - near);      // z軸縮放
        float d = -(2.0F * far * near) / (far - near);// z軸偏移
        float e = -1.0F;

        // 構建投影矩陣
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;

        return m;
    }
}
