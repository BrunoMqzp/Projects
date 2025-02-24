using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaPuntos : MonoBehaviour
{
    public Jugador jugador;
    public PUNTUACIONES PUNTUACIONES;
    public Enemigo enemigo;
    public SaludEnemigo saludEnemigo;
    public GameControlador gm;
    public GameObject encontrar;


    public void PuntosPorVida()
    {
        // Calcular la diferencia en la vida del enemigo
        int diferenciaVida = saludEnemigo.saludmaxima - saludEnemigo.salud;

        // Si la vida del enemigo se ha reducido
        if (diferenciaVida > 0)
        {
            int puntos = 100;
            // Añadir la diferencia a la puntuación del jugador
            puntos = puntos * diferenciaVida;

            // Actualizar la puntuación en la UI (si es necesario)
            PUNTUACIONES.AddPoint(puntos);
        }

    }

    public void PuntosAlGanar()
    {
            Debug.Log("PUNTOS PARTIDA FINALIZADA");
            int puntosFinalizar = 1000;
            PUNTUACIONES.AddPoint(puntosFinalizar);
    }


    public void PuntosAlPerder()
    {
            Debug.Log("PUNTOS PARTIDA FINALIZADA");
            int puntosderrota = 500;
            PUNTUACIONES.AddPoint(puntosderrota);
    }

    void Start()
    {
        jugador = FindObjectOfType<Jugador>();
        PUNTUACIONES = FindObjectOfType<PUNTUACIONES>();
        enemigo = FindObjectOfType<Enemigo>();
        saludEnemigo = FindObjectOfType<SaludEnemigo>();
    }

    //void Update()
    //{
    //    if (jugador.cartaDescartada)
    //    {
    //        Debug.Log("pUNTOS AGREGADOS");
    //        PuntosPorVida();
    //    }
    //}
}
