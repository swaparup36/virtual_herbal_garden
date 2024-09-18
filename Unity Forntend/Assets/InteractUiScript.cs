using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InteractUiScript : MonoBehaviour
{
    public Transform BushTransform;
    public Transform PlayerTransform;
    public bool isPlayerClose = false;
    public string CommonName = "";
    // Start is called before the first frame update
    void Start()
    {
        if (BushTransform != null)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            transform.position = new Vector3(BushTransform.position.x, (BushTransform.position.y + 1.75f), BushTransform.position.z);
            transform.SetParent(BushTransform);
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && isPlayerClose)
        {
            PlayerPrefs.SetString("Position", $"{PlayerTransform.position.x},{PlayerTransform.position.y},{PlayerTransform.position.z}");
            PlayerPrefs.SetString("ActiveCommonName", CommonName);
            PlayerPrefs.Save();
            Debug.Log("Interact");
            SceneManager.LoadScene("ShortDetails");
        }
    }
}
