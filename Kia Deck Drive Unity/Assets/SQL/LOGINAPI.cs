using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public TMP_Text messageText;
    public string NombreEscena;
    public static string username;
    private string apiUrl = "http://localhost:5000/api/usersgame/logingame"; // Your API endpoint

    public void OnLoginButtonClick()
    {
        username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            messageText.text = "Username and password cannot be empty.";
            return;
        }

        StartCoroutine(Login(username, password));
    }

    private IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

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
                // Optionally, parse the response if you need to handle it further
                Debug.Log("Response: " + www.downloadHandler.text);
                ChangeScene();
            }
        }
    }

    public void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NombreEscena);
    }
}
