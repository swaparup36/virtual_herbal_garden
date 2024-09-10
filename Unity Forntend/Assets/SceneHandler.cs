using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ReadMore()
    {
        SceneManager.LoadScene("Interaction Page");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Main Garden");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
