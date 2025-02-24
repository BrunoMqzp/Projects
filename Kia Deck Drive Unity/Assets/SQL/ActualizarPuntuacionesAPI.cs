using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
public class PartidaManager : MonoBehaviour
{
    public static string nombre; // Player's name
    public int duracion; // Duration of the game
    public int puntaje; // Player's score (Puntaje)
    private LoginManager loginManager;
    public PUNTUACIONES puntuaciones;
    public TMP_Text messageText;
    public Turnos turnos;

    private string apiUrl = "http://localhost:5000/api/usersgame/partidasgame"; // Your API endpoint

    public void OnSubmitPartida()
    {
        turnos = FindObjectOfType<Turnos>();
        loginManager = FindObjectOfType<LoginManager>();
        puntuaciones = FindObjectOfType<PUNTUACIONES>();

        nombre = LoginManager.username;
        duracion = turnos.Rondas;
        puntaje = puntuaciones.score;

        if (duracion == null  || puntaje == null || nombre == null)
        {
            messageText.text = "All fields must be filled out.";
            return;
        }

        StartCoroutine(SubmitPartida(duracion, puntaje, nombre));
    }

    private IEnumerator SubmitPartida(int duracion, int puntaje, string nombre)
    {
        WWWForm form = new WWWForm();
        form.AddField("duracion", duracion);
        form.AddField("puntaje", puntaje);
        form.AddField("nombre", nombre);  // Send only 'nombre', 'duracion', and 'puntaje'

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
