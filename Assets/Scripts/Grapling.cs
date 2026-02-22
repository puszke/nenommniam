using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapling : MonoBehaviour
{
    [Header("Settings")]
    public string targetTag = "Alive";
    public float maxDistance = 50f;
    public LayerMask obstacleMask; // warstwa ścian

    public Transform CurrentTarget;

    public static Grapling instance;

    public float currentDistance = 0;

    [SerializeField] private Rigidbody rb;

    public float goalFov = 60;

    
    public bool grappling = false;

    public GameObject grabArm, grabArmEnd, mouth, crosshair;
    public AudioSource audioSource; // Przeciągnij tu komponent AudioSource w Inspektorze
    public AudioClip actionSound;   // Przeciągnij tu plik dźwiękowy w Inspektorze
    void Awake()
    {
        instance=this;  
    }

    void Update()
    {
        if (!grappling)
        {
            FindNearestVisibleTarget();
            rb.transform.gameObject.layer = 8;
            
        }
        else
        {
            rb.transform.gameObject.layer = 11;
        }
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, goalFov, Time.deltaTime * 9);
        
        crosshair.SetActive(CurrentTarget!=null && !grappling);

        if (CurrentTarget!=null)
        {
            crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, CurrentTarget.transform.position, Time.deltaTime * 9);
            currentDistance = Vector3.Distance(CurrentTarget.position, transform.position);
            transform.LookAt(CurrentTarget);
            if(currentDistance<maxDistance)
            {
                if(Input.GetMouseButtonDown(1))
                {
                    StartCoroutine(StartGrabbing());
                }
            }
            if (currentDistance < 1f && grappling)
            {
                //rb.gameObject.transform.position = CurrentTarget.position;
                CurrentTarget.GetComponent<Alive>().DiePublic(gameObject);
                audioSource.Stop();
                //transform.parent.root.transform.position = CurrentTarget.position;
                Debug.Log(transform.parent.root.position);
                Debug.Log(CurrentTarget.position);
                //rb.gameObject.transform.LookAt(CurrentTarget.position);
                rb.velocity = Vector3.zero;
                grappling = false;
                grabArm.SetActive(false);
            }
        }
        else
        {
            grappling = false;
        }
    }

    IEnumerator StartGrabbing()
    {
        //audioSource.pitch = Random.Range(0.7f, 1.2f);
        grabArm.SetActive(true);
        grabArmEnd.transform.localPosition = Vector3.zero;
        mouth.GetComponent<MouthAnim>().moving = false;
        yield return new WaitForSeconds(0.3f);
        grappling = true;
        audioSource.PlayOneShot(actionSound);
        mouth.GetComponent<Animator>().speed = 1;
        mouth.GetComponent<Animator>().SetBool("attack", true);
    }

    private void FixedUpdate()
    {
        if(CurrentTarget!=null)
            grabArmEnd.transform.position = Vector3.Lerp(grabArmEnd.transform.position, CurrentTarget.position, 12 * Time.deltaTime);
        if (grappling && CurrentTarget!=null)
        {
            goalFov =120;
            //rb.useGravity = false;
            rb.transform.LookAt(CurrentTarget.position);
            //rb.transform.position = Vector3.Lerp(transform.position, CurrentTarget.position, 12* Time.deltaTime);
            rb.AddForce(transform.forward, ForceMode.Impulse);
            //grappling=false;
        }
        else
        {
            goalFov = 60;
        }
        //transform.parent.root.GetComponent<SphereCollider>().enabled = !grappling;
    }

   

    void FindNearestVisibleTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        float closestDistance = Mathf.Infinity;
        Transform bestTarget = null;


        foreach (GameObject obj in targets)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            

            if (distance > maxDistance)
                continue;

            if (distance < closestDistance)
            {
                
                if (HasLineOfSight(obj.transform)&&obj.GetComponent<Alive>().isAlive && obj.GetComponent<MeshRenderer>().isVisible)
                {
                    
                    closestDistance = distance;
                    bestTarget = obj.transform;
                }
            }
        }
        if(CurrentTarget!=bestTarget)
            grappling=false;
        CurrentTarget = bestTarget;
    }

    bool HasLineOfSight(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, obstacleMask))
        {
            
            // Trafiliśmy w ścianę zanim dotarliśmy do celu
            return false;
        }

        return true;
    }
}
