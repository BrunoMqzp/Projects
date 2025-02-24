using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KDDC;
using TMPro.Examples;
using System.Linq;
using Unity.VisualScripting;

public class CartAtaqueCorregido : MonoBehaviour
{
    public Carta DatosCarta;
    public Carta DatosCarta2;
    public SaludEnemigo s_salud;
    public EscudoEnemigo s_escudo;
    public Jugador jugador;
    public CartEspecialCorregido s_especial;
    public int id;



    public void EfectoAtaque()
    {
        DatosCarta = FindAnyObjectByType<Carta>();
        jugador = FindObjectOfType<Jugador>();
        s_escudo = FindObjectOfType<EscudoEnemigo>();
        s_especial = FindObjectOfType<CartEspecialCorregido>();
        if (jugador.CartasJugadas.Count > 0)
        {
            int lastItem = jugador.CartasJugadas.Count - 1;
            Debug.Log("Inicio Loop");
            //id = 0;
            DatosCarta = jugador.CartasJugadas[lastItem];
            if (jugador.CartasJugadas.Count > 1)
            {
                DatosCarta2 = jugador.CartasJugadas[lastItem - 1];
            }
            //DatosCarta2 = jugador.CartasJugadas[lastItem-2];

            if (s_salud == null)
            {
                s_salud = FindObjectOfType<SaludEnemigo>();
                if (s_salud == null)
                {
                    Debug.LogError("No se encontró un componente Salud en la escena.");
                    return;  // Sal de la función si no se encuentra Salud
                }
            }
            if (s_escudo.escudo > 0)
            {
                if (DatosCarta2 != null && DatosCarta2.tipocarta.Contains(Carta.TipoCarta.Especial) && DatosCarta2.damageType.Contains(Carta.DamageType.Debuff))
                {
                    AplicarDanoDuplicadoEscudo();
                }
                else { AplicarDanoMaximo(); }
            }
            else if (DatosCarta != null && DatosCarta.tipocarta.Contains(Carta.TipoCarta.Damage) || (DatosCarta.damageMax > 0))
            {
                s_especial.EfectoDebuff();
                Debug.Log("INICIO ATAQUE");
                s_salud.salud -= DatosCarta.damageMax;
                if (s_salud.salud > s_salud.saludmaxima)
                {
                    s_salud.salud = s_salud.saludmaxima;
                }
                Debug.Log("Ataque realizado");
            }
            else
            {
                Debug.Log("No se puede atacar");
            }
        }
    }
    public void LimpiarJugador()
    {
        jugador = null;
    }

    public void AplicarDanoMaximo()
    {
        if (s_escudo != null)
        {
            s_escudo.escudo -= DatosCarta.damageMax;
            if (s_escudo.escudo < 0)
            {
                if (s_salud.salud > 0)
                {
                    s_salud.salud += s_escudo.escudo;
                }
                s_escudo.escudo = 0;
            }
            Debug.Log("Dano Maximo aplicado al escudo");
        }
        else
        {
            Debug.LogError("No se encontró un componente Escudo en la escena.");
        }
    }

    public void AplicarDanoDuplicadoEscudo()
    {
        if (s_escudo != null)
        {
            s_escudo.escudo -= DatosCarta.damageMax * 2;
            if (s_escudo.escudo < 0)
            {
                if (s_salud.salud > 0)
                {
                    s_salud.salud += s_escudo.escudo;
                }
                s_escudo.escudo = 0;
            }
            Debug.Log("Dano Maximo aplicado al escudo");
        }
        else
        {
            Debug.LogError("No se encontró un componente Escudo en la escena.");
        }
    }
}
