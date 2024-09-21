using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class Response
{
    public string status;
    public Message messege;
}

[Serializable]
public class LoginResponse
{
    public string refresh;
    public string access;
}

[Serializable]
public class Message
{
    public string[] username;
    public string[] email;
    public string[] password;
}


[System.Serializable]
public class RequestData
{
    public string username;
    public string email;
    public string password;
}

[System.Serializable]
public class LoginErrors
{
    public string[] username;
    public string[] password;
    public string detail;
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
        string user_field = UsernameField.text.ToString().Trim();
        string email_field = EmailField.text.ToString().Trim();
        string pass_field = PasswordField.text.ToString().Trim() ;
        Debug.Log($"Username: {user_field}, Email: {email_field}, Password: {pass_field}");
        if (user_field.Equals("") || email_field.Equals("") || pass_field.Equals(""))
        {
            ErrorMessage = "Enter A valid username or password or email";
            ErrorPanel.SetActive(true);
            return;
        }
        if (!isRequestProcessing)
        {
            var form = new WWWForm();
            form.AddField("username", "test3");
            form.AddField("email", "test3@gmail.com");
            form.AddField("password", "testpassword");
            Debug.Log(form.ToString());
            StartCoroutine(SendPostRequest("https://sih-5at5.onrender.com/users/create/"));
        }
    }

    public void OnLoginClicked()
    {
        Debug.Log($"Username: {UsernameField.text}, Password: {PasswordField.text}");
        if (UsernameField.text.Trim().Equals("") || PasswordField.text.Trim().Equals(""))
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
            StartCoroutine(SendPostRequest("https://sih-5at5.onrender.com/users/login/", true));
        }
    }

    IEnumerator SendPostRequest(string uri, bool login=false)
    {
        isRequestProcessing = true;

        string username = UsernameField.text.Replace("\u200b", ""), password = PasswordField.text.Replace("\u200b", "");
        
        // Convert the data to JSON format
        string jsonData ;
        if (!login)
        {
            string email = email = EmailField.text.Replace("\u200b", "");
            jsonData = $"{{\"username\": \"{username}\", \"email\": \"{email}\", \"password\": \"{password}\"}}";
        }else
        {
            //jsonData = $"{{\"username\": \"\", \"password\": \"\"}}";
            jsonData = $"{{\"username\": \"{username}\", \"password\": \"{password}\"}}";
        }
        Debug.Log(jsonData);

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
            Debug.Log("Error: " + request.error);
            Debug.Log(request.downloadHandler.text);

            if (!login)
            {
                // Parse the JSON string into the Response object
                Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);

                Message errs = (Message)response.messege;
                string[] null_str = new string[] { "" };
                string[] user_error = errs.username == null ? null_str : errs.username;
                string[] email_error = errs.email == null ? null_str : errs.email;
                string[] pass_error = errs.password == null ? null_str : errs.password;

                string error_msg = $"{request.error}";

                foreach (string s in user_error)
                {
                    error_msg += $"\n{s.ToString()}";
                }
                foreach (string s in email_error)
                {
                    error_msg += $"\n{s.ToString()}";
                }
                foreach (string s in pass_error)
                {
                    error_msg += $"\n{s.ToString()}";
                }
                ErrorMessage = error_msg;
            }
            else
            {
                LoginErrors loginErrors = JsonUtility.FromJson<LoginErrors>(request.downloadHandler.text);

                string[] null_str = new string[] { "" };
                string[] user_error = loginErrors.username == null ? null_str : loginErrors.username;
                string details = loginErrors.detail == null ? "" : loginErrors.detail;
                string[] pass_error = loginErrors.password == null ? null_str : loginErrors.password;

                string error_msg = $"{request.error}";

                foreach (string s in user_error)
                {
                    error_msg += $"\n{s.ToString()}";
                }
                error_msg += details;
                foreach (string s in pass_error)
                {
                    error_msg += $"\n{s.ToString()}";
                }
                ErrorMessage = error_msg;
            }
            ErrorPanel.SetActive(true);
        }
        else
        {
            if (!login)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                OnGoToLogin();
            }
            else
            {
                LoginResponse tokens = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                PlayerPrefs.SetString("Refresh", tokens.refresh);
                PlayerPrefs.SetString("Access", tokens.access);
                PlayerPrefs.Save();
                GoToMainGarden();
            }
        }
        isRequestProcessing =false;
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
