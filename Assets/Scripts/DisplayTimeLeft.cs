using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class DisplayTimeLeft : MonoBehaviour
{
    public GameObject b;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if()
        float balls = (PlayerDeath.instance.czasNaPrzezycie / PlayerDeath.instance.aktualnyCzas);
        GameObject g = GameObject.FindGameObjectWithTag("global");
        balls = Mathf.Clamp(balls, 0, 2);
        Vignette vignette;
        if(g.GetComponent<Volume>().profile.TryGet(out vignette))
        {
            vignette.intensity.value = balls/7;
            Debug.Log(vignette.intensity);
        }
        b.transform.localScale = new Vector3(balls,balls,balls);
        GetComponent<Image>().fillAmount = PlayerDeath.instance.aktualnyCzas / PlayerDeath.instance.czasNaPrzezycie;
    }
}
