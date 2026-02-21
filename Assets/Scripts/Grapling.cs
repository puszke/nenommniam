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

    void Awake()
    {
      instance=this;  
    }

    void Update()
    {
        FindNearestVisibleTarget();

        if(CurrentTarget!=null)
        {
            currentDistance = Vector3.Distance(CurrentTarget.position, transform.position);
            transform.LookAt(CurrentTarget);
            if(currentDistance<maxDistance)
            {
                if(Input.GetMouseButtonDown(1))
                {
                    rb.AddForce(transform.forward*90);
                }
            }

        }
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
                
                if (HasLineOfSight(obj.transform))
                {
                    closestDistance = distance;
                    bestTarget = obj.transform;
                }
            }
        }

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
