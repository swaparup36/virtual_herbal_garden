using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Note
{
    public string content;
    public string created_at;
    public string updated_at;
}

[System.Serializable]
public class NoteResponse
{
    public string status;
    public Note note;
}
public class TakeNotesInteractionScript : MonoBehaviour
{
    bool isRequestProcessing = false;
    public TMP_Text BotanicName;
    public TMP_Text CommonName;
    public TMP_Text Habitat;
    public TMP_Text Region;
    public TMP_Text MedicinalUse;
    public TMP_InputField NoteText;
    public Image PlantImage;

    string note_text = null;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (!isRequestProcessing)
        {
            StartCoroutine(SendPostRequest($"https://sih-wxqc.onrender.com/trees/{PlayerPrefs.GetString("ActiveCommonName")}/"));
            StartCoroutine(GetNotesRequest($"https://sih-wxqc.onrender.com/notes/myNotes/{PlayerPrefs.GetString("ActiveCommonName")}/"));
        }
    }

    IEnumerator GetNotesRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Add the Bearer token to the Authorization header
            webRequest.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("Access"));

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            // Check for errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + webRequest.error);
                if (webRequest.responseCode == 401)
                {
                    StartCoroutine(WebRequestScript.RefreshAccessToken("https://sih-wxqc.onrender.com/users/token/refresh/"));
                    StartCoroutine(GetNotesRequest($"https://sih-wxqc.onrender.com/notes/myNotes/{PlayerPrefs.GetString("ActiveCommonName")}/"));
                }
                else if (webRequest.responseCode == 404)
                {
                    StartCoroutine(CreateNoteRequest($"https://sih-wxqc.onrender.com/notes/create/{PlayerPrefs.GetString("ActiveCommonName")}/"));
                }
            }
            else
            {
                NoteResponse response = JsonUtility.FromJson<NoteResponse>(webRequest.downloadHandler.text);
                if (!response.note.content.Equals(""))
                {
                    note_text = response.note.content;
                    NoteText.text = response.note.content;
                    Debug.Log(note_text);
                }

            }
        }
    }

    public void OnSave()
    {
        if (!isRequestProcessing)
            StartCoroutine(SaveRequest($"https://sih-wxqc.onrender.com/notes/update/{PlayerPrefs.GetString("ActiveCommonName")}/"));
    }

    IEnumerator SaveRequest(string url)
    {
        isRequestProcessing = true;
        string jsonData = $"{{\"content\": \"{NoteText.text}\"}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest webRequest = new UnityWebRequest(url, "PATCH");
        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        // Set the request content type to JSON
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Add the Bearer token to the Authorization header
        webRequest.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("Access"));

        // Send the request and wait for a response
        yield return webRequest.SendWebRequest();

        // Check for errors
        if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
            webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            //Debug.LogError("Error: " + webRequest.error);
            if (webRequest.responseCode == 401)
            {
                StartCoroutine(WebRequestScript.RefreshAccessToken("https://sih-wxqc.onrender.com/users/token/refresh/"));
                StartCoroutine(SaveRequest($"https://sih-wxqc.onrender.com/notes/update/{PlayerPrefs.GetString("ActiveCommonName")}/"));
            }
        }
        else
        {
            //Debug.Log(webRequest.downloadHandler.text);
        }

        isRequestProcessing = false;
    }

    IEnumerator CreateNoteRequest(string uri)
    {
        string jsonData = $"{{\"content\": \"\"}}";

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
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("Access"));

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check for network or server errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            if (request.responseCode == 401)
            {
                StartCoroutine(WebRequestScript.RefreshAccessToken("https://sih-wxqc.onrender.com/users/token/refresh/"));
                StartCoroutine(CreateNoteRequest($"https://sih-wxqc.onrender.com/notes/create/{PlayerPrefs.GetString("ActiveCommonName")}/"));
            }
        }
        else
        {

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
                    StartCoroutine(WebRequestScript.RefreshAccessToken("https://sih-wxqc.onrender.com/users/token/refresh/"));
                    StartCoroutine(SendPostRequest($"https://sih-wxqc.onrender.com/trees/{PlayerPrefs.GetString("ActiveCommonName")}/"));
                }
                //Debug.LogError("Error: " + webRequest.error);
            }
            else
            {

                //Debug.Log(webRequest.downloadHandler.text);
                Root root = JsonUtility.FromJson<Root>(webRequest.downloadHandler.text);
                if (root != null)
                {
                    BotanicName.text = root.message.botanical_name;
                    CommonName.text = root.message.common_name;
                    Habitat.text = root.message.habitant;
                    Region.text = root.message.region;
                    MedicinalUse.text = root.message.medical_use;
                    if (PlantImage != null)
                    {
                        string imgUrl = root.message.image_link;
                        Debug.Log($"Plnat Image place holder {root.message.image_link}");
                        StartCoroutine(LoadImageCoroutine(imgUrl));
                    }
                    else
                    {
                        VideoHandler.url = root.message.audio_link;
                    }
                }
            }
        }
        isRequestProcessing = false;
    }

    IEnumerator LoadImageCoroutine(string url)
    {
        // Create a UnityWebRequest to get the texture from the URL
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        // Handle errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // Extract the texture from the request
            Texture2D texture = DownloadHandlerTexture.GetContent(request);

            // Create a sprite from the texture and assign it to the UI Image
            if (texture != null)
            {
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                PlantImage.sprite = sprite;
            }
        }
    }

    public void OnViewModel()
    {
        SceneManager.LoadScene("View Model");
    }
    public void OnGoBack()
    {
        SceneManager.LoadScene("Interaction Page");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
