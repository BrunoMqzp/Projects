using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rondas : MonoBehaviour
{
    public int RondaActual;
    public bool Inicio = true;
    public Jugador jugador;
    public Enemigo enemigo;
    public void Ronda1()
    {
        jugador = FindObjectOfType<Jugador>();
        enemigo = FindObjectOfType<Enemigo>();
        RondaActual = 1;
        
        //while (Inicio)
        //{
        //    if ()
        //        RondaActual += 1;
        //    if (jugador.Vida <= 0 || enemigo.enemigo_Vida <= 0)
        //    {
        //        Inicio = false;
        //    }
        //}
    }

    void Start()
    {
        Ronda1();
    }

    void Update()
    {

    }
}
