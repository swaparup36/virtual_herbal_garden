using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[Serializable]
class ServerResponse
{
    public int status;
    public string jwt;
    public string message;
}

public class EventHandlerScript : MonoBehaviour
{
    public TMP_InputField UsernameField;
    public TMP_InputField EmailField;
    public TMP_InputField PasswordField;
    public GameObject ErrorPanel;
    static public string ErrorMessage = "";

    bool isRequestProcessing = false;
    // Start is called before the first frame update
    void Start()
    {
        ErrorPanel.SetActive(false);
    }

    public void OnSignUpClicked()
    {
        Debug.Log($"Username: {UsernameField.text}, Email: {EmailField.text}, Password: {PasswordField.text}");
        if (UsernameField.text.Trim() == "" || EmailField.text.Trim() == "" || PasswordField.text.Trim() == "")
        {
            ErrorMessage = "Enter A valid username or password or email";
            ErrorPanel.SetActive(true);
            return;
        }
        if (!isRequestProcessing)
        {
            var form = new WWWForm();
            form.AddField("username", UsernameField.text);
            form.AddField("email", EmailField.text);
            form.AddField("password", PasswordField.text);
            StartCoroutine(SendPostRequest("http://localhost:3000/signup", form));
        }
    }

    public void OnLoginClicked()
    {
        Debug.Log($"Username: {UsernameField.text}, Password: {PasswordField.text}");
        if (UsernameField.text.Trim() == "" || PasswordField.text.Trim() == "")
        {
            ErrorMessage = "Enter A valid username or password";
            ErrorPanel.SetActive(true);
            return;
        }
        if (!isRequestProcessing)
        {
            var form = new WWWForm();
            form.AddField("username", UsernameField.text);
            form.AddField("password", PasswordField.text);
            StartCoroutine(SendPostRequest("http://localhost:3000/login", form));
        }
    }

    IEnumerator SendPostRequest(string uri, WWWForm form)
    {
        isRequestProcessing = true;
        // Create a new UnityWebRequest for the specified URL
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            // Check for network errors or HTTP errors
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + webRequest.error);
                ErrorMessage = webRequest.error;
                ErrorPanel.SetActive(true);
            }
            else
            {
                Debug.Log("Making Request");
                string req = webRequest.downloadHandler.text;
                ServerResponse r = JsonUtility.FromJson<ServerResponse>(req);
                Debug.Log("Response: " + req);
                PlayerPrefs.SetString("jwt", r.jwt);
                PlayerPrefs.Save();
                GoToMainGarden();
            }
        }
        isRequestProcessing=false;
    }

    public void OnResetClick()
    {
        UsernameField.text = string.Empty;
        if(EmailField != null) { EmailField.text = string.Empty; }
        PasswordField.text = string.Empty;
    }

    public void GoToMainGarden()
    {
        SceneManager.LoadScene("Main Garden");
    }

    public void OnGoToLogin()
    {

        SceneManager.LoadScene("Login");
    }

    public void OnGoToSignUp()
    {
        SceneManager.LoadScene("SignUp");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
