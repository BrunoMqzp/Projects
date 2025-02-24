using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarEscenaBatalla : MonoBehaviour
{
    public static int escenaBatalla = 0;
    public void CambiarEscena(string nombre)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nombre);
    }

    public void CambiarEscenaBatalla1(string nombre)
    {
        escenaBatalla = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nombre);
    }

    public void CambiarEscenaBatalla2(string nombre)
    {
        escenaBatalla = 2;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nombre);
    }
}
