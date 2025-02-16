using UnityEngine;

[RequireComponent(typeof(Camera))]
public class UnifiedCameraProjection : MonoBehaviour
{
    public enum ProjectionMode
    {
        Ground,         // 地面投影
        Front,         // 正面投影
        Back,          // 背面投影
        AngleFront,    // 角度正面投影
        Left,          // 左侧投影
        Right          // 右侧投影
    }

    [Header("基本设置")]
    public ProjectionMode mode = ProjectionMode.Front;
    [HideInInspector] public Camera Acamera;
    public GameObject AScreenOBJ;
    [HideInInspector] public GameObject UserOBJ;

    [Header("偏移设置")]
    public Vector3 OffsetAngle;              // 相机角度偏移量
    public float BottomOffset;               // 底部偏移量
    public float TopOffset;                  // 顶部偏移量
    public float DistanceOffset;             // 距离偏移量

    [Header("屏幕参数")]
    [Tooltip("Width of the display in meters.")]
    public float screenWidth = 1.6f;
    [Tooltip("Height of the display in meters.")]
    public float screenHeight = 0.9f;

    // 内部变量
    private float left = -0.2F;
    private float right = 0.2F;
    private float bottom = -0.2F;
    private float top = 0.2F;
    private float standardUserDistance = 2f;
    private Vector3 userheadLimit;

    private Vector3 initialCamPos;
    private Quaternion initialCamRot;
    private Matrix4x4 initialCamMat;
    private Matrix4x4 initialCamPrjMat;

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
        userheadLimit = UserOBJ == null ? Vector3.zero : UserOBJ.transform.position;

        // 根据不同模式计算标准用户距离
        CalculateStandardDistance();

        // 计算投影边界
        CalculateProjectionBounds();

        // 更新相机位置和旋转
        UpdateCameraTransform();

        // 应用投影矩阵
        Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, Acamera.nearClipPlane, Acamera.farClipPlane);
        Acamera.projectionMatrix = m;
    }

    private void CalculateStandardDistance()
    {
        switch (mode)
        {
            case ProjectionMode.Ground:
            case ProjectionMode.AngleFront:
                standardUserDistance = Mathf.Abs(AScreenOBJ.transform.position.y - userheadLimit.y) + DistanceOffset;
                break;
            case ProjectionMode.Front:
            case ProjectionMode.Back:
                standardUserDistance = Mathf.Abs(AScreenOBJ.transform.position.z - userheadLimit.z) + DistanceOffset;
                break;
            case ProjectionMode.Left:
            case ProjectionMode.Right:
                standardUserDistance = Mathf.Abs(AScreenOBJ.transform.position.x - userheadLimit.x);
                break;
        }
    }

    private void CalculateProjectionBounds()
    {
        switch (mode)
        {
            case ProjectionMode.Ground:
            case ProjectionMode.AngleFront:
                left = Acamera.nearClipPlane * (-screenWidth / 2 - userheadLimit.x - AScreenOBJ.transform.position.x) / standardUserDistance;
                right = Acamera.nearClipPlane * (screenWidth / 2 - userheadLimit.x - AScreenOBJ.transform.position.x) / standardUserDistance;
                bottom = Acamera.nearClipPlane * (-screenHeight / 2 - userheadLimit.z + AScreenOBJ.transform.position.z + BottomOffset) / standardUserDistance;
                top = Acamera.nearClipPlane * (screenHeight / 2 - userheadLimit.z + AScreenOBJ.transform.position.z + TopOffset) / standardUserDistance;
                break;

            case ProjectionMode.Front:
                left = Acamera.nearClipPlane * (-screenWidth / 2 - userheadLimit.x - AScreenOBJ.transform.position.x) / standardUserDistance;
                right = Acamera.nearClipPlane * (screenWidth / 2 - userheadLimit.x - AScreenOBJ.transform.position.x) / standardUserDistance;
                bottom = Acamera.nearClipPlane * (-screenHeight / 2 - userheadLimit.y + AScreenOBJ.transform.position.y + BottomOffset) / standardUserDistance;
                top = Acamera.nearClipPlane * (screenHeight / 2 - userheadLimit.y + AScreenOBJ.transform.position.y + TopOffset) / standardUserDistance;
                break;

            case ProjectionMode.Back:
                left = Acamera.nearClipPlane * (-screenWidth / 2 - userheadLimit.x + AScreenOBJ.transform.position.x) / standardUserDistance;
                right = Acamera.nearClipPlane * (screenWidth / 2 - userheadLimit.x + AScreenOBJ.transform.position.x) / standardUserDistance;
                bottom = Acamera.nearClipPlane * (-screenHeight / 2 - userheadLimit.y + AScreenOBJ.transform.position.y + BottomOffset) / standardUserDistance;
                top = Acamera.nearClipPlane * (screenHeight / 2 - userheadLimit.y + AScreenOBJ.transform.position.y + TopOffset) / standardUserDistance;
                break;

            case ProjectionMode.Left:
                left = Acamera.nearClipPlane * (-screenWidth / 2 - userheadLimit.z + AScreenOBJ.transform.position.z) / standardUserDistance;
                right = Acamera.nearClipPlane * (screenWidth / 2 - userheadLimit.z + AScreenOBJ.transform.position.z) / standardUserDistance;
                bottom = Acamera.nearClipPlane * (-screenHeight / 2 - userheadLimit.y + AScreenOBJ.transform.position.y) / standardUserDistance;
                top = Acamera.nearClipPlane * (screenHeight / 2 - userheadLimit.y + AScreenOBJ.transform.position.y) / standardUserDistance;
                break;

            case ProjectionMode.Right:
                left = Acamera.nearClipPlane * (-screenWidth / 2 + userheadLimit.z - AScreenOBJ.transform.position.z) / standardUserDistance;
                right = Acamera.nearClipPlane * (screenWidth / 2 + userheadLimit.z - AScreenOBJ.transform.position.z) / standardUserDistance;
                bottom = Acamera.nearClipPlane * (-screenHeight / 2 - userheadLimit.y + AScreenOBJ.transform.position.y) / standardUserDistance;
                top = Acamera.nearClipPlane * (screenHeight / 2 - userheadLimit.y + AScreenOBJ.transform.position.y) / standardUserDistance;
                break;
        }
    }

    private void UpdateCameraTransform()
    {
        Vector3 headCamPos = userheadLimit;
        Quaternion headCamRot;

        switch (mode)
        {
            case ProjectionMode.Ground:
            case ProjectionMode.AngleFront:
                headCamRot = Quaternion.LookRotation(Vector3.down, Vector3.up) * Quaternion.Euler(OffsetAngle);
                break;
            case ProjectionMode.Front:
                headCamRot = Quaternion.LookRotation(Vector3.forward, Vector3.up) * Quaternion.Euler(OffsetAngle);
                break;
            case ProjectionMode.Back:
                headCamRot = Quaternion.LookRotation(Vector3.forward, Vector3.up) * Quaternion.Euler(OffsetAngle);
                break;
            default:
                headCamRot = Quaternion.LookRotation(Vector3.forward, Vector3.up) * Quaternion.Euler(OffsetAngle);
                break;
        }
        
        Matrix4x4 camPoseMat = Matrix4x4.TRS(headCamPos, headCamRot, Vector3.one);
        camPoseMat = camPoseMat * initialCamMat;

        Acamera.transform.position = camPoseMat.GetColumn(3);
        Acamera.transform.localRotation = camPoseMat.rotation;
    }

    private Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;

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