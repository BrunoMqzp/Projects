using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KDDC;
using TMPro.Examples;
using System.Linq;
using Unity.VisualScripting;

public class CartEspecial : MonoBehaviour
{
    public Carta DatosCarta;
    public Carta DatosCarta2;
    public Salud s_salud;
    public CartAtaque s_ataque;
    public Jugador jugador;
    public bool debuffActivo;

    public void EfectoDebuff()
    {
        DatosCarta = FindAnyObjectByType<Carta>();
        //DatosCarta2 = FindAnyObjectByType<Carta>();
        jugador = FindObjectOfType<Jugador>();
        s_ataque = FindObjectOfType<CartAtaque>();
        if (jugador.CartasJugadas.Count > 0)
        {
            int lastItem = jugador.CartasJugadas.Count - 1;
            Debug.Log("Inicio Loop");
            //id = 0;
            DatosCarta = jugador.CartasJugadas[lastItem];
            if (jugador.CartasJugadas.Count > 1)
            {
                DatosCarta2 = FindAnyObjectByType<Carta>();
                DatosCarta2 = jugador.CartasJugadas[lastItem - 1];
            }
            if (s_salud == null)
            {
                s_salud = FindObjectOfType<Salud>();
            }

            if (DatosCarta2 != null && DatosCarta2.tipocarta.Contains(Carta.TipoCarta.Especial) && DatosCarta2.damageType.Contains(Carta.DamageType.Debuff))
            {
                debuffActivo = true;
                if (debuffActivo)
                {
                    s_ataque = FindObjectOfType<CartAtaque>();
                    Debug.Log("Debuff activado: El siguiente daño será duplicado.");
                    int DanosDuplicado = DatosCarta.damageMax;
                    Debug.Log("INICIO ATAQUE DUPLICADO");
                    s_salud.salud -= DanosDuplicado;
                    if (s_salud.salud > s_salud.saludmaxima)
                    {
                        s_salud.salud = s_salud.saludmaxima;
                    }
                    Debug.Log("Ataque DUPLICADO REALIZADO");
                }
            }
        }
    }

    public void EfectoBuff()
    {

    }
}

