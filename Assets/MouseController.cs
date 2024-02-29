using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FPS_Controler : MonoBehaviour
{
    [Header("Mouse Variables")]
    [SerializeField]  private float mouseSensitivity = 1000;
    [SerializeField]  private Transform PlayerBody;
    [SerializeField]  private float RotationX = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        RotationX -=mouseY;
        RotationX = Mathf.Clamp(RotationX, -90f,90f);

        transform.localRotation = Quaternion.Euler(RotationX,0f,0f);
        PlayerBody.Rotate(Vector3.up * mouseX);
    }
}