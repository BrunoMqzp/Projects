using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using KDDC;

public class AIMEDIUM : MonoBehaviour
{
    public Turnos turnos;
    public Enemigo enemigo;
    public Jugador jugador;
    public SaludEnemigo s_salud;
    public EscudoEnemigo s_escudo;
    public Salud salud;
    public Escudo escudo;

    // Variables para controlar la probabilidad de cada acción
    public float probabilidadCurar = 0.1f;
    public float probabilidadEscudo = 0.2f;
    public float probabilidadAtacar = 0.4f;

    // Nuevas variables para la lógica más compleja
    private float umbralVidaBaja = 0.3f; // Umbral para considerar que la IA tiene poca vida
    private float umbralEscudoBajo = 0.2f; // Umbral para considerar que la IA tiene poco escudo

    private Animator anim;
    AudioSource efectos;

    public AudioClip[] Sonidos;
    public GameObject objetoEnemigo;

    void Start()
    {
        efectos = GetComponent<AudioSource>();
        anim = objetoEnemigo.GetComponent<Animator>();
    }

    // Función para atacar al enemigo
    void AtacarEnemigo()
    {
        if (turnos.TurnoJugadorVerdadero == false)
        {
            if(escudo.escudo > 0)
            {
                escudo.escudo -= 3;
                if (escudo.escudo < 0)
                {
                    if (salud.salud > 0)
                    {
                        salud.salud += escudo.escudo;
                    }
                    escudo.escudo = 0;
                }
                Debug.Log("IA ataca al enemigo. Escudo del jugador: " + jugador.Escudo);
            }
            else 
            {
                salud.salud -= 3;
                Debug.Log("IA ataca al enemigo. Vida del jugador: " + salud.salud);
            }
            turnos.TurnoJugadorVerdadero = true;
        }
    }

    // Función para curar a la IA
    void CurarIA()
    {
        if (turnos.TurnoJugadorVerdadero == false)
        {
            s_salud.salud += 3;
            Debug.Log("IA se cura. Vida de la IA: " + s_salud.salud);
            s_salud.salud = Mathf.Clamp(s_salud.salud, 0, 20);
            turnos.TurnoJugadorVerdadero = true;
        }
    }

    // Función para aumentar el escudo de la IA
    void AumentarEscudo()
    {
        if (turnos.TurnoJugadorVerdadero == false)
        {
            s_escudo.escudo += 3;
            Debug.Log("IA aumenta su escudo. Escudo de la IA: " + s_escudo.escudo);
            s_escudo.escudo = Mathf.Clamp(s_escudo.escudo, 0, 20);
            turnos.TurnoJugadorVerdadero = true;
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
        escudo = FindObjectOfType<Escudo>();

        if (turnos.TurnoJugadorVerdadero == false)
        {
            // Lógica más compleja para la toma de decisiones
            float vidaRelativaIA = s_salud.salud / 20f; // Vida relativa de la IA (entre 0 y 1)
            float escudoRelativoIA = s_escudo.escudo / 20f; // Escudo relativo de la IA (entre 0 y 1)

            if (vidaRelativaIA < umbralVidaBaja)
            {
                // Priorizar la curación si la vida es baja
                CurarIA();
                anim.Play("Vida_enemigo");
                efectos.clip = Sonidos[1];
                efectos.Play();
            }
            else if (escudoRelativoIA < umbralEscudoBajo)
            {
                // Aumentar el escudo si es bajo
                AumentarEscudo();
                anim.Play("Escudo_enemigo");
                efectos.clip = Sonidos[2];
                efectos.Play();
            }
            else
            {
                // Si la vida y el escudo están bien, decidir entre atacar, curar o aumentar escudo
                float random = UnityEngine.Random.value;
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
                else
                {
                    AtacarEnemigo();
                    anim.Play("Ataque_enemigo");
                    efectos.clip = Sonidos[0];
                    efectos.Play();
                }
            }
        }
    }
}
