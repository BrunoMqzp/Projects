using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KDDC;
using TMPro.Examples;
using System.Linq;
using Unity.VisualScripting;

public class CartEspecialCorregido : MonoBehaviour
{
    public Carta DatosCarta;
    public Carta DatosCarta2;
    public SaludEnemigo s_salud;
    public CartAtaqueCorregido s_ataque;
    public Jugador jugador;
    public bool debuffActivo;
    public Turnos turnos;
    public Mazo mazo;
    public ContainerPile container;

    public void EfectoDebuff()
    {
        DatosCarta = FindAnyObjectByType<Carta>();
        //DatosCarta2 = FindAnyObjectByType<Carta>();
        jugador = FindObjectOfType<Jugador>();
        s_ataque = FindObjectOfType<CartAtaqueCorregido>();
        if (jugador.CartasJugadas.Count > 0)
        {
            int lastItem = jugador.CartasJugadas.Count - 1;
            Debug.Log("Inicio Loop");
            DatosCarta = jugador.CartasJugadas[lastItem];
            if (jugador.CartasJugadas.Count > 1)
            {
                DatosCarta2 = FindAnyObjectByType<Carta>();
                DatosCarta2 = jugador.CartasJugadas[lastItem - 1];
            }
            if (s_salud == null)
            {
                s_salud = FindObjectOfType<SaludEnemigo>();
            }

            if (DatosCarta2 != null && DatosCarta2.tipocarta.Contains(Carta.TipoCarta.Especial) && DatosCarta2.damageType.Contains(Carta.DamageType.Debuff))
            {
                debuffActivo = true;
                if (debuffActivo)
                {
                    s_ataque = FindObjectOfType<CartAtaqueCorregido>();
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

    public void EfectoMasTurnos()
    {
        DatosCarta = FindAnyObjectByType<Carta>();
        jugador = FindObjectOfType<Jugador>();
        turnos = FindObjectOfType<Turnos>();
        if (jugador.CartasJugadas.Count > 0)
        {
            int lastItem = jugador.CartasJugadas.Count - 1;
            Debug.Log("Inicio Loop");
            DatosCarta = jugador.CartasJugadas[lastItem];
            if (DatosCarta != null && DatosCarta.tipocarta.Contains(Carta.TipoCarta.Especial) && DatosCarta.damageType.Contains(Carta.DamageType.Seguir))
            {
                Debug.Log("Aumento de Turnos");
                if (turnos.TurnosActuales < turnos.MaxTurnos)
                {
                    turnos = FindObjectOfType<Turnos>();
                    turnos.TurnosActuales += 2;
                }
                else if (turnos.TurnosActuales> turnos.MaxTurnos)
                {
                    turnos = FindObjectOfType<Turnos>();
                    turnos.TurnosActuales = turnos.MaxTurnos;
                }
            }
        }

    }

    public void EfectoAgregar()
    {
        DatosCarta = FindAnyObjectByType<Carta>();
        jugador = FindObjectOfType<Jugador>();
        mazo = FindObjectOfType<Mazo>();
        container = FindObjectOfType<ContainerPile>();
        if (jugador.CartasJugadas.Count > 0)
        {
            int lastItem = jugador.CartasJugadas.Count - 1;
            Debug.Log("Inicio Loop");
            DatosCarta = jugador.CartasJugadas[lastItem];
            if (DatosCarta != null && DatosCarta.tipocarta.Contains(Carta.TipoCarta.Especial) && DatosCarta.damageType.Contains(Carta.DamageType.Retraso))
            {
                Debug.Log("Aumento Carta");
                if (mazo.CartasActivas.Count <= 7)
                {
                    jugador = FindObjectOfType<Jugador>();
                    container = FindObjectOfType<ContainerPile>();
                    container.SorteoCarta(mazo);
                    container.SorteoCarta(mazo);
                }
            }
        }
    }
}