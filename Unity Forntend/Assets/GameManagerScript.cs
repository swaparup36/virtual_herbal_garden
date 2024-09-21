using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[Serializable]
class Tree
{
    public string common_name;
    public string botanical_name;
    public string habitant;
    public string region;
    public string type;
    public string image_link;
    public string audio_link;
    public string information;
    public string medical_use;
}

[Serializable]
class TreeResponse
{
    public Tree[] plant_array;
}

public class GameManagerScript : MonoBehaviour
{
    public bool isRequestProcessing = false;
    public GameObject PrefabType;
    public Transform PlayerTransform;
    public float Radius;
    public Canvas Canvas;
    private GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        obj = PrefabType;
        if (obj.GetComponent<PlantInteractionScript>() == null)
        {
            PlantInteractionScript pis = obj.AddComponent<PlantInteractionScript>();
        }
        if (!isRequestProcessing)
        {
            StartCoroutine(SendPostRequest("https://sih-5at5.onrender.com/trees/"));
        }
    }

    IEnumerator SendPostRequest(string uri)
    {
        isRequestProcessing = true;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Add the Bearer token to the Authorization header
            webRequest.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("Access"));

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            // Check for errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                if (webRequest.responseCode == 401)
                {
                    StartCoroutine(WebRequestScript.RefreshAccessToken("https://sih-5at5.onrender.com/users/token/refresh/"));
                    StartCoroutine(SendPostRequest("https://sih-5at5.onrender.com/trees/"));
                }
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                //Instantiate(obj);
                // Handle the response
                string json = $"{{\"plant_array\":{webRequest.downloadHandler.text}}}";
                Debug.Log("Response: " + json);
                TreeResponse trees = JsonUtility.FromJson<TreeResponse>(json);
                PlantInteractionScript pis = obj.GetComponent<PlantInteractionScript>();
                pis.Radius = Radius;
                pis.Mask = -1;
                pis.PlayerTransform = PlayerTransform;

                if (trees != null)
                {
                    int x = -10, z = 0;
                    foreach (Tree tree in trees.plant_array)
                    {
                        Debug.Log(tree.common_name);
                        obj.name = $"prefab {tree.common_name}";
                        obj.GetComponent<Transform>().position = new Vector3(x, 0, z);
                        pis.PlantCommonName = tree.common_name;
                        Instantiate(obj);
                        x += 10;
                        z += 10;
                    }
                    x = 20; z = -20;
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            //Debug.Log(tree.common_name);
                            obj.name = $"prefab {x} {z}";
                            obj.GetComponent<Transform>().position = new Vector3(x, 0, z);
                            pis.PlantCommonName = "Garlic";
                            Instantiate(obj);
                            x += 10;
                        }
                        x = 0;
                        z += 5;
                    }
                }
            }
        }
        isRequestProcessing = false;
    }

    public void OnSearchClick()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
