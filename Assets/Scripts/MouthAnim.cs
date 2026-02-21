using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthAnim : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;

    public bool moving=true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {

        if (moving)
            anim.speed = rb.velocity.magnitude+0.2f;

    }
}
