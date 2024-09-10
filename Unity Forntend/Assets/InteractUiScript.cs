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
    static public bool isPlayerClose = false;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        transform.position = new Vector3(BushTransform.position.x, (BushTransform.position.y + 1.5f), BushTransform.position.z);
        transform.SetParent(BushTransform);

        string coords = PlayerPrefs.GetString("Position");


        if (coords != null || coords.Trim() != "")
        {
            try
            {
                var coodinate_list = coords.Split(',');
                float x = float.Parse(coodinate_list[0]), y = float.Parse(coodinate_list[1]), z = float.Parse(coodinate_list[2]);
                PlayerTransform.position = new Vector3(x, y, z);

                PlayerPrefs.SetString("Position", coords);
                PlayerPrefs.Save();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && isPlayerClose)
        {
            PlayerPrefs.SetString("Position", $"{PlayerTransform.position.x},{PlayerTransform.position.y},{PlayerTransform.position.z}");
            PlayerPrefs.Save();
            Debug.Log("Interact");
            SceneManager.LoadScene("ShortDetails");
        }
    }
}
