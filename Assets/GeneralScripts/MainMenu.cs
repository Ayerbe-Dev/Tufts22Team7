using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool goToMain;
    // Start is called before the first frame update
    void Start()
    {
        goToMain = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (goToMain)
        {
            click();
        }
    }
    void click()
    {
        SceneManager.LoadScene("SceneMain");
        goToMain = false;
    }
}
