using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class Request
{
    public string message;
}

public class PopulateData : MonoBehaviour
{
    public TMP_Text Title;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Making Request");
        StartCoroutine(GetRequest("http://localhost:3000/"));
    }

    IEnumerator GetRequest(string uri)
    {
        // Create a new UnityWebRequest for the specified URL
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();


            // Check for network errors or HTTP errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Making Request");
                // Handle the response data
                //string req = "{ \"message\": \"Hello, Express!\" }";
                string req = webRequest.downloadHandler.text;
                Request r = JsonUtility.FromJson<Request>(req);
                Debug.Log("Response: " + r.message);
                Title.text = r.message;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
