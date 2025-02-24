using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KDDC;
using TMPro.Examples;
using System.Linq;
using Unity.VisualScripting;

public class CartVida : MonoBehaviour
{
    public Carta DatosCarta;
    public Salud s_salud;
    public Jugador jugador;
    public int id;

    public void EfectoCuracion()
    {
        DatosCarta = FindAnyObjectByType<Carta>();
        jugador = FindObjectOfType<Jugador>();

        if (jugador.CartasJugadas.Count > 0)
        {
            int lastItem = jugador.CartasJugadas.Count - 1;
            Debug.Log("Inicio Loop");
            //id = 0;
            DatosCarta = jugador.CartasJugadas[lastItem];
            if (s_salud == null)
            {
                s_salud = FindObjectOfType<Salud>();
                if (s_salud == null)
                {
                    Debug.LogError("No se encontró un componente Salud en la escena.");
                    return;  // Sal de la función si no se encuentra Salud
                }
            }
            if (DatosCarta != null && DatosCarta.tipocarta.Contains(Carta.TipoCarta.Vida) || (DatosCarta.salud > 0))
            {
                if (s_salud.salud < s_salud.saludmaxima)
                {
                    Debug.Log("INICIO CURACION");
                    s_salud.salud += DatosCarta.salud;
                    if (s_salud.salud > s_salud.saludmaxima)
                    {
                        s_salud.salud = s_salud.saludmaxima;
                    }
                    Debug.Log("CURACION realizado");
                }
                else
                {
                    Debug.Log("No se puede CURAR");
                }
            }
            else
            {
                Debug.Log("No se puede CURAR");
            }
        }
    }

}

