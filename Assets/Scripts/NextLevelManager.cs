using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelManager : MonoBehaviour
{
    public string nextLevel = "";

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag=="Player")
        {
            PlayerPrefs.SetString("level", nextLevel);
        }
    }
}
