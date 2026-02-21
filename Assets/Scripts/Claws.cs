using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claws : MonoBehaviour
{

    public Rigidbody rb;

    public bool canAttack = true;

    public float leapForce=1;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    IEnumerator DelayAttack()
    {
        canAttack=false;
        yield return new WaitForSecondsRealtime(0.5f);
        canAttack=true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)&&canAttack)
        {
            StartCoroutine(DelayAttack());
            rb.AddForce(transform.forward*Grapling.instance.currentDistance*leapForce+transform.up*Grapling.instance.currentDistance*leapForce/2,ForceMode.Impulse);
        }
    }
}
