using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject m_Player;
    private Rigidbody rb;   

    public float jumpForce;

    public float rayLength;

    public LayerMask groundLayer;

    public float grav=0;
    float x, y;
    
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        x=Input.GetAxisRaw("Horizontal");
        y=Input.GetAxisRaw("Vertical");

        CheckGround();
        grav+=0.1f;

        if(IsGrounded)
        {
            grav=0;
        }
        rb.AddForce(0,-grav,0);
        if(Input.GetKeyDown(KeyCode.Space)&&IsGrounded)
        {
            Jump();   
        }

    }
    public bool IsGrounded { get; private set; }

    void CheckGround()
    {
        IsGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            rayLength,
            groundLayer
        );
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward*y + transform.right*x, ForceMode.Impulse);
    }
}
