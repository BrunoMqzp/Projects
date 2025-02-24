using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KDDC;
using TMPro.Examples;
using System.Linq;
using Unity.VisualScripting;

public class CartEscudo : MonoBehaviour
{
    public Carta DatosCarta;
    public Salud s_salud;
    public Jugador jugador;
    public Escudo s_escudo;
    public int id;
    public void EfectoEscudo()
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
            if (s_escudo == null)
            {
                s_escudo = FindObjectOfType<Escudo>();
                if (s_escudo == null)
                {
                    Debug.LogError("No se encontró un componente Salud en la escena.");
                    return;  // Sal de la función si no se encuentra Salud
                }
            }
            if (DatosCarta != null && DatosCarta.tipocarta.Contains(Carta.TipoCarta.Escudo) || (DatosCarta.escudo > 0))
            {
                if (s_escudo.escudo < s_escudo.escudomaximo)
                {
                    Debug.Log("INICIO ESCUDO");
                    s_escudo.escudo += DatosCarta.escudo;
                    if (s_escudo.escudo > s_escudo.escudomaximo)
                    {
                        s_escudo.escudo = s_escudo.escudomaximo;
                    }
                    Debug.Log("ESCUDO realizado");
                }
                else
                {
                    Debug.Log("Escudo MAXIMO");
                }
            }
            else
            {
                Debug.Log("ESCUDO MAXIMO");
            }
        }
    }

}
