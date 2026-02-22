using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayTimeLeft : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().fillAmount = PlayerDeath.instance.aktualnyCzas / PlayerDeath.instance.czasNaPrzezycie;
    }
}
