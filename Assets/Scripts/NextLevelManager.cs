using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevelManager : MonoBehaviour
{
    public string nextLevel = "";
    int enemies = 0;
    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Alive").Length;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag=="Player")
        {
            bool canGo = true;
            foreach(GameObject balls in GameObject.FindGameObjectsWithTag("Alive"))
            {
                if(balls.GetComponent<Alive>().isAlive)
                    canGo = false;
            }
            if (canGo)
            {
                PlayerPrefs.SetString("level", nextLevel);
                SceneManager.LoadScene("EpicTransitionScene");
            }
        }
    }
}
