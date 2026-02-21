using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator arms;
    private Rigidbody rb;   

    public float jumpForce;

    public float rayLength;

    public LayerMask groundLayer;

    public float grav=0;
    float x, y;

    public float speed = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Jump")
        {
            grav = -99;
            Jump();
        }
    }
    // Update is called once per frame
    void Update()
    {

        x=Input.GetAxisRaw("Horizontal");
        y=Input.GetAxisRaw("Vertical");

        CheckGround();
        grav+=1f;

        arms.SetBool("backwards", y==-1);
        arms.speed = Mathf.Abs(y*2);
        
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
        if (IsGrounded || Grapling.instance.grappling)
        {
            grav = 0;
        }
        rb.AddForce(0, -grav, 0);
        rb.AddForce(transform.forward*y*speed + transform.right*x*speed, ForceMode.Impulse);
    }
}
