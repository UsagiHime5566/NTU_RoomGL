#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWithMouse : MonoBehaviour
{
    const float k_MouseSensitivityMultiplier = 0.01f;

    public float mouseSensitivity = 100f;
    public float mouseRotateMaxAngle = 90f;

    public float mouseRotateOverScreenX = 0.33f;

    public Transform playerBody;

    float xRotation = 0f;

    bool working = true;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Insert)){
            working = !working;
        }

        if(!working) return;

#if ENABLE_INPUT_SYSTEM
        float mouseX = 0, mouseY = 0;

        if (Mouse.current != null)
        {
            var delta = Mouse.current.delta.ReadValue() / 15.0f;
            mouseX += delta.x;
            mouseY += delta.y;
        }
        if (Gamepad.current != null)
        {
            var value = Gamepad.current.rightStick.ReadValue() * 2;
            mouseX += value.x;
            mouseY += value.y;
        }

        mouseX *= mouseSensitivity * k_MouseSensitivityMultiplier;
        mouseY *= mouseSensitivity * k_MouseSensitivityMultiplier;
#else
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * k_MouseSensitivityMultiplier;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * k_MouseSensitivityMultiplier;
        float mouseY = (Input.mousePosition.y - Screen.height / 2) / (Screen.height / 2) * mouseRotateMaxAngle;

        if(Input.mousePosition.x < 0){
            mouseX = -mouseRotateOverScreenX;
        }
        if(Input.mousePosition.x > Screen.width){
            mouseX = mouseRotateOverScreenX;
        }
#endif

        xRotation = -mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
