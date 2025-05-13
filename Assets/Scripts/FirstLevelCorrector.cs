using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevelCorrector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");

    }
}
