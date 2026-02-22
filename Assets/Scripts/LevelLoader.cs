using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    IEnumerator loadnext()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(PlayerPrefs.GetString("level"));

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loadnext()); 
    }

}
