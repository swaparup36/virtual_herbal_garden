using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[Serializable]
class RefreshResponse
{
    public string refresh;
    public string access;
}

public class WebRequestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static IEnumerator RefreshAccessToken(string uri)
    {
        string jsonData = $"{{\"refresh\": \"{PlayerPrefs.GetString("Refresh")}\"}}";

        // Create a UnityWebRequest for the POST request
        UnityWebRequest request = new UnityWebRequest(uri, "POST");

        // Convert the JSON string to a byte array
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        // Attach the byte array to the UploadHandlerRaw
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Set the DownloadHandler to receive the response
        request.downloadHandler = new DownloadHandlerBuffer();

        // Set the request header for JSON data
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check for network or server errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            if (request.responseCode == 401)
            {
                SceneManager.LoadScene("Login");
            }
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            RefreshResponse response = JsonUtility.FromJson<RefreshResponse>(request.downloadHandler.text);
            if (response != null)
            {
                PlayerPrefs.SetString("Refresh", response.refresh);
                PlayerPrefs.SetString("Access", response.access);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
