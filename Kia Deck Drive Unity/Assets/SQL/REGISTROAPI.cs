using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class RegistrationManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public InputField persNoInput;
    public TMP_Text messageText;

    private string apiUrl = "http://localhost:5000/api/usersgame/registergame"; // Your API endpoint

    public void OnRegisterButtonClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        string persNo = persNoInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(persNo))
        {
            messageText.text = "All fields must be filled out.";
            return;
        }

        StartCoroutine(Register(username, password, persNo));
    }

    private IEnumerator Register(string username, string password, string persNo)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("persNo", persNo);

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                messageText.text = "Error: " + www.error;
            }
            else
            {
                messageText.text = www.downloadHandler.text; // Display the response message
                Debug.Log("Response: " + www.downloadHandler.text);
            }
        }
    }
}
