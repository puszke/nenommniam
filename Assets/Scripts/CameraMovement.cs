using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;

    public float sensitivity = 5;
    float x, y;

    float c_x, c_y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        c_x += x;
        c_y += y;
        c_y = math.clamp(c_y, -90, 90);

        if (!Grapling.instance.grappling)
        {

            player.transform.rotation = Quaternion.Euler(0, c_x * sensitivity, 0);

            transform.localRotation = Quaternion.Euler(-c_y * sensitivity, 0, 0);
        }
    }
}
