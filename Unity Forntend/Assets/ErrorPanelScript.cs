using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorPanelScript : MonoBehaviour
{
    public TMP_Text ErrorText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        ErrorText.text = EventHandlerScript.ErrorMessage;    
    }

    public void OnClose()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
