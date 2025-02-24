using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ENDGAME : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }
    public void RestartBoton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");
    }

    public void SalirBoton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PantallaInicialJuego");
    }

    public void ContinueBoton()
    {
        gameObject.SetActive(false);
    }

    public void ENDSCREEN()
    {
        gameObject.SetActive(false);
    }
}
