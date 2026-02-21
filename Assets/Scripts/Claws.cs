using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claws : MonoBehaviour
{

    public Rigidbody rb;

    public bool canAttack = true, attacked = false;

    public Animator mouth;

    public float leapForce=1;

    [SerializeField] private SphereCollider killbox;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    IEnumerator DelayAttack()
    {
        canAttack=false;
        yield return new WaitForSecondsRealtime(0.5f);
        canAttack=true;
        attacked=false;
    }
    private void FixedUpdate()
    {
        if(!canAttack&&!attacked)
        {
            attacked=true;
            rb.AddForce(transform.forward* leapForce + transform.up, ForceMode.Impulse);
        }
    }
    // Update is called once per frame
    void Update()
    {
        killbox.enabled  = (!canAttack);
        mouth.SetBool("attack",!canAttack);
        mouth.gameObject.GetComponent<MouthAnim>().anim.speed = 1;
        mouth.gameObject.GetComponent<MouthAnim>().enabled = canAttack;
        if(Input.GetMouseButtonDown(0)&&canAttack)
        {
            StartCoroutine(DelayAttack());
            
        }
    }
}
