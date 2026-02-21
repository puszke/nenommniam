using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : MonoBehaviour
{

    public bool isAlive = true;

    public float dieSpeed = 5;

    private Rigidbody rb;

    [SerializeField] private GameObject blood;

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Claws" && isAlive)
        {
            StartCoroutine(Die(other.gameObject));
        }
    }

    public void DiePublic(GameObject b)
    {
        StartCoroutine(Die(b));
    }

    IEnumerator Die(GameObject other)
    {
        isAlive=false;
        transform.Rotate(Random.Range(-90,90),Random.Range(-90,90), Random.Range(-90,90));
        rb.isKinematic = true;
        Time.timeScale = 0.2f;
        blood.SetActive(true);
        CamShake.Instance.Shake(0.2f, 0.1f);
        Time.fixedDeltaTime = 0.002f * Time.timeScale;
        yield return new WaitForSecondsRealtime(0.3f);
        rb.isKinematic = false;
        Grapling.instance.grappling=false;
        rb.drag = 1f;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.002f;
        rb.AddForce((other.transform.forward*5+other.transform.right*Random.Range(-5,5))*dieSpeed, ForceMode.Impulse);
        rb.AddTorque(90,90,90);
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
//        Debug.Log(GetComponent<MeshRenderer>().isVisible);
    }
}
