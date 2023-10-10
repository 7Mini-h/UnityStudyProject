using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    float mouseX, mouseY;
    float rX, rY;

    public float rotSpeed = 300f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        rX = mouseX * Time.deltaTime * rotSpeed;
        rY = mouseY * Time.deltaTime * rotSpeed;

        transform.eulerAngles += new Vector3(-rY, rX, 0);
    }
}
