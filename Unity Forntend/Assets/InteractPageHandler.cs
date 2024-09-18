using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[Serializable]
class TreeDetails
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

[System.Serializable]
class Root
{
    public string status;
    public TreeDetails message;
    public bool bookmarked;
}


public class InteractPageHandler : MonoBehaviour
{
    bool isRequestProcessing = false;
    public TMP_Text BotanicName;
    public TMP_Text CommonName;
    public TMP_Text Habitat;
    public TMP_Text Region;
    public TMP_Text MedicinalUse;
    public TMP_Text BookmarkText;
    public TMP_Text DeatailInfo;
    public Image PlantImage;
    bool isBookMarked = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(SendPostRequest($"https://sih-wxqc.onrender.com/trees/{PlayerPrefs.GetString("ActiveCommonName")}"));
    }
    public void OnViewModel()
    {
        SceneManager.LoadScene("View Model");
    }

    public void OnGoBack()
    {
        SceneManager.LoadScene("ShortDetails");
    }
    public void OnTakeNotes()
    {
        SceneManager.LoadScene("Take Notes");
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
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {

                Debug.Log(webRequest.downloadHandler.text);
                Root root = JsonUtility.FromJson<Root>(webRequest.downloadHandler.text);
                if (root != null)
                {
                    BotanicName.text = root.message.botanical_name;
                    CommonName.text = root.message.common_name;
                    Habitat.text = root.message.habitant;
                    Region.text = root.message.region;
                    MedicinalUse.text = root.message.medical_use;
                    isBookMarked = root.bookmarked;
                    BookmarkText.text = isBookMarked ? "Remove Bookmark" : "Add Bookmark";
                    if (DeatailInfo != null)
                    {
                        DeatailInfo.text = root.message.information;
                    }
                    if(PlantImage != null)
                    {
                        string imgUrl = "https://t4.ftcdn.net/jpg/01/79/88/65/360_F_179886510_6xf0RHhDnLN5ovd2qmGF4WaZMJjqrt6o.jpg";
                        Debug.Log($"Plnat Image place holder {root.message.image_link}");
                        StartCoroutine(LoadImageCoroutine(imgUrl));
                    }
                }
            }
        }
        isRequestProcessing = false;
    }
    bool hasUpdatedBookMark = false;
    IEnumerator BookMarkReqeust()
    {
        isRequestProcessing = true;
        string url = "";
        if (isBookMarked)
        {
            url = $"https://sih-wxqc.onrender.com/users/bookmarks/removeBookmark/{PlayerPrefs.GetString("ActiveCommonName")}/";
        }
        else
        {
            url = $"https://sih-wxqc.onrender.com/users/bookmarks/createBookmark/{PlayerPrefs.GetString("ActiveCommonName")}/";
        }

        
        using (UnityWebRequest webRequest = isBookMarked ? UnityWebRequest.Delete(url) : UnityWebRequest.Post(url, new WWWForm()))
        {
            // Add the Bearer token to the Authorization header
            webRequest.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("Access"));

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            // Check for errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                if (isBookMarked)
                {
                    Debug.Log("book mark removed");
                    isBookMarked = false;
                }else
                {
                    Debug.Log("book mark added");
                    isBookMarked=true;
                }
                hasUpdatedBookMark = false;
                //Debug.Log(webRequest.downloadHandler.text);
            }
        }
        isRequestProcessing=false;
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

    public void OnBookMark()
    {
        if (!isRequestProcessing)
        {
            StartCoroutine(BookMarkReqeust());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasUpdatedBookMark)
        {
            BookmarkText.text = isBookMarked ? "Remove Bookmark" : "Add Bookmark";
            hasUpdatedBookMark = true;
        }
    }
}
