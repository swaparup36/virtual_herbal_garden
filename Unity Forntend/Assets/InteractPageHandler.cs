using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractPageHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void OnViewModel()
    {
        SceneManager.LoadScene("View Model");
    }

    public void OnGoBack()
    {
        SceneManager.LoadScene("Main Garden");
    }
    public void OnTakeNotes()
    {
        SceneManager.LoadScene("Take Notes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
