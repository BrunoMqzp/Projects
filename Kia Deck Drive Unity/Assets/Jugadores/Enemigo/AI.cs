using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using KDDC;

public class AI_easy : MonoBehaviour
{
    public Turnos turnos;
    public Enemigo enemigo;
    public Jugador jugador;
    public SaludEnemigo s_salud;
    public EscudoEnemigo s_escudo;
    public Salud salud;

    private Animator anim;
    AudioSource efectos;

    public AudioClip[] Sonidos;
    public GameObject objetoEnemigo;

    // Evento para indicar el cambio de turno
    public delegate void CambioDeTurno(bool esTurnoIA);
    public static event CambioDeTurno OnCambioDeTurno;

    // Variables para controlar la probabilidad de cada acción
    public float probabilidadCurar = 0.1f;
    public float probabilidadEscudo = 0.2f; // La probabilidad de atacar se calcula como 1 - probabilidadCurar - probabilidadEscudo
    public float probabilidadAtacar = 0.4f;
    public int CostoAtaque = 1;
    public int CostoCurar = 1;
    public int CostoEscudo1 = 1;

    void Start()
    {
        Resources.UnloadUnusedAssets();
        efectos = GetComponent<AudioSource>();
        anim = objetoEnemigo.GetComponent<Animator>();
    }

    // Función para atacar al enemigo
    void AtacarEnemigo()
    {
        if (turnos.TurnoJugadorVerdadero == false)
        {
            //CostoAtaque = Random.Range(CostoAtaque, 3);
            //if (turnos.TurnosActualesMaquina >= CostoAtaque) { 
            // Reduce la vida del enemigo (puedes ajustar el daño)
            salud.salud -= 1;
            //turnos.TurnosActualesMaquina -= 1;
            Debug.Log("IA ataca al enemigo. Vida del enemigo: " + salud.salud);

            // Termina el turno de la IA
            turnos.TurnoJugadorVerdadero = true;

            //// Notifica el cambio de turno
            //if (OnCambioDeTurno != null)
            //{
            //    OnCambioDeTurno(esTurnoIA);
            //}
            //}
        }
    }

    // Función para curar a la IA
    void CurarIA()
    {
        if (turnos.TurnoJugadorVerdadero == false)
        {
            // Aumenta la vida de la IA (puedes ajustar la cantidad de curación)
            s_salud.salud += 1;

            Debug.Log("IA se cura. Vida de la IA: " + s_salud.salud);
            s_salud.salud = Mathf.Clamp(s_salud.salud, 0, 20);
            // Termina el turno de la IA
            turnos.TurnoJugadorVerdadero = true;

            // Notifica el cambio de turno
            //if (OnCambioDeTurno != null)
            //{
            //    OnCambioDeTurno(esTurnoIA);
            //}
        }
    }

    // Función para aumentar el escudo de la IA
    void AumentarEscudo()
    {
        if (turnos.TurnoJugadorVerdadero == false)
        {
            // Aumenta el escudo de la IA (puedes ajustar la cantidad de escudo)
            s_escudo.escudo += 1;
            Debug.Log("IA aumenta su escudo. Escudo de la IA: " + s_escudo.escudo);
            s_escudo.escudo = Mathf.Clamp(s_escudo.escudo, 0, 20);
            // Termina el turno de la IA
            turnos.TurnoJugadorVerdadero = true;

            // Notifica el cambio de turno
            //if (OnCambioDeTurno != null)
            //{
            //    OnCambioDeTurno(esTurnoIA);
            //}
        }
    }

    // Función para que la IA realice una acción en su turno
    public void RealizarAccion()
    {
        jugador = FindObjectOfType<Jugador>();
        enemigo = FindObjectOfType<Enemigo>();
        turnos = FindObjectOfType<Turnos>();
        s_escudo = FindObjectOfType<EscudoEnemigo>();
        s_salud = FindObjectOfType<SaludEnemigo>();
        salud = FindObjectOfType<Salud>();
        if (turnos.TurnoJugadorVerdadero == false && turnos.TurnosActualesMaquina > 0)
        {
            // Genera un número aleatorio para decidir la acción
            float random = UnityEngine.Random.value;

            // Decide la acción según la probabilidad
            if (random < probabilidadCurar)
            {
                CurarIA();
                anim.Play("Vida_enemigo");
                efectos.clip = Sonidos[1];
                efectos.Play();
            }
            else if (random < probabilidadCurar + probabilidadEscudo)
            {
                AumentarEscudo();
                anim.Play("Escudo_enemigo");
                efectos.clip = Sonidos[2];
                efectos.Play();
            }
            else if (random < probabilidadAtacar)
            {
                AtacarEnemigo();
                anim.Play("Ataque_enemigo");
                efectos.clip = Sonidos[0];
                efectos.Play();
            }
        }
    }
}
