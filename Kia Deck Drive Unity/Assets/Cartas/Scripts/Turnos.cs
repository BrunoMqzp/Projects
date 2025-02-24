using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KDDC;

public class Turnos : MonoBehaviour
{

    public bool TurnoJugadorVerdadero;
    public int TurnoMaquina;
    public int TurnoJugador;
    public TMP_Text TurnoQuienTexto;

    public int MaxTurnos;
    public int TurnosActuales;
    public int MaxTurnosMaquina;
    public int TurnosActualesMaquina;
    public TMP_Text TurnosTexto;
    public TMP_Text TurnosMaquinaTexto;
    public int Rondas;

    public Carta DatosCarta;
    public Jugador jugador;
    public Enemigo enemigo;
    public AI_easy AI;
    public AIMEDIUM AIMEDIUM;
    public AIHARD AIHARD;
    public AIMOREINTELLIGENT AIMOREINTELLIGENT;
    public AIPRUEBA AIPRUEBA;
    //public bool cartaDescartada = false;

    void Start()
    {
        TurnoJugadorVerdadero = true;
        TurnoMaquina = 0;
        TurnoJugador = 1;

        MaxTurnos = 10;
        TurnosActuales = 3;
        Rondas = 0;

        MaxTurnosMaquina = 10;
        TurnosActualesMaquina = 5;
        jugador = FindObjectOfType<Jugador>();
        enemigo = FindObjectOfType<Enemigo>();
    }

    void Update()
    {
        if (TurnoJugadorVerdadero == true)
        {
            TurnoQuienTexto.text = "Tu turno";
        }
        else
        {
            jugador = FindObjectOfType<Jugador>();
            enemigo = FindObjectOfType<Enemigo>();
            AI = FindObjectOfType<AI_easy>();
            AIMEDIUM = FindObjectOfType<AIMEDIUM>();
            AIHARD = FindObjectOfType<AIHARD>();
            AIMOREINTELLIGENT = FindObjectOfType<AIMOREINTELLIGENT>();
            AIPRUEBA = FindObjectOfType<AIPRUEBA>();
            TurnoQuienTexto.text = "Turno del oponente";
            if (AI != null)
            {
                AI.RealizarAccion();
            }
            if (AIMEDIUM != null)
            {
                AIMEDIUM.RealizarAccion();
            }
            if (AIHARD != null)
            {
                AIHARD.RealizarAccion();
            }
            if (AIPRUEBA != null)
            {
                AIPRUEBA.PerformAction();
            }
            if (AIMOREINTELLIGENT != null)
            {
                AIMOREINTELLIGENT.PerformAction();
            }

        }
        TurnosTexto.text = TurnosActuales + "/" + MaxTurnos;
        TurnosMaquinaTexto.text = TurnosActualesMaquina + "/" + MaxTurnosMaquina;
    }

    public void FinalizarTurnoJugador()
    {
        TurnoJugadorVerdadero = false;
        TurnoJugador += 1;
        Rondas += 1;
        //if (TurnosActualesMaquina < MaxTurnosMaquina)
        //{
        //    TurnosActualesMaquina += 1;
        //}

        if (TurnosActuales < MaxTurnos)
        {
            TurnosActuales += 3;
            if (TurnosActuales > MaxTurnos)
            {
                TurnosActuales = MaxTurnos;
            }
        }
    }

    public void FinalizarTurnoMaquina()
    {
        TurnoJugadorVerdadero = true;
        TurnoMaquina += 1;
        //if (TurnosActuales < MaxTurnos)
        //{
        //    TurnosActuales += 1;
        //}

        if (TurnosActualesMaquina < MaxTurnosMaquina)
        {
            TurnosActualesMaquina += 1;
        }
        //MaxTurnos += 1;
        //TurnosActuales = MaxTurnos;
    }

    public void InteraccionPuntos()
    {
        DatosCarta = FindAnyObjectByType<Carta>();
        jugador = FindObjectOfType<Jugador>();


        if (jugador.CartasJugadas.Count > 0)
        {
            int lastItem = jugador.CartasJugadas.Count - 1;
            Debug.Log("Costo de Turno Iniciado");
            DatosCarta = jugador.CartasJugadas[lastItem];
            TurnosActuales -= DatosCarta.Costo;
            Debug.Log("Costo:" + TurnosActuales + "Costo Carta" + DatosCarta.Costo);
        }

    }

    public void VerificarTurno()
    {
        if (TurnoJugadorVerdadero == true && TurnosActuales > 0)
        {
            InteraccionPuntos();
            if (TurnosActuales < 0)
            {
                TurnosActuales = 0;
            }
        }
    }

    public void InteraccionPuntosMaquina()
    {
        //DatosCarta = FindAnyObjectByType<Carta>();
        //jugador = FindObjectOfType<Jugador>();
        //int lastItem = jugador.CartasJugadas.Count - 1;
        //Debug.Log("Costo de Turno Iniciado");
        //DatosCarta = jugador.CartasJugadas[lastItem];
        //if (jugador.CartasJugadas.Count > 0)
        //{
        //    TurnosActualesMaquina -= DatosCarta.Costo;
        //}

    }
}
