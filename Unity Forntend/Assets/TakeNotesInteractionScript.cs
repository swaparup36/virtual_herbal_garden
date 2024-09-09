using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakeNotesInteractionScript : MonoBehaviour
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
        SceneManager.LoadScene("Interaction Page");
    }

    public void OnSave()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
