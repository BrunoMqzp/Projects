using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KDDC;

public class AIHARD : MonoBehaviour
{
    public Turnos turnos;
    public Enemigo enemigo;
    public Jugador jugador;
    public SaludEnemigo s_salud;
    public EscudoEnemigo s_escudo;
    public Salud salud;
    public Escudo escudo;

    // Variables para controlar la probabilidad de cada acci�n
    public float probabilidadCurarBase = 0.1f;
    public float probabilidadEscudoBase = 0.2f;
    public float probabilidadAtacarBase = 0.7f;

    // Nuevas variables para la l�gica m�s compleja
    private float umbralVidaBaja = 0.3f; // Umbral para considerar que la IA tiene poca vida
    private float umbralEscudoBajo = 0.2f; // Umbral para considerar que la IA tiene poco escudo
    private float agresividad = 1f; // Factor que influye en la probabilidad de ataque

    // Historial de acciones del jugador
    private List<string> historialAccionesJugador = new List<string>();

    // Funci�n para atacar al enemigo
    void AtacarEnemigo()
    {
        if (turnos.TurnoJugadorVerdadero == false)
        {
            if (escudo.escudo > 0)
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
            historialAccionesJugador.Add("Atacar");
        }
    }

    // Funci�n para curar a la IA
    void CurarIA()
    {
        if (turnos.TurnoJugadorVerdadero == false)
        {
            s_salud.salud += 3;
            Debug.Log("IA se cura. Vida de la IA: " + s_salud.salud);
            s_salud.salud = Mathf.Clamp(s_salud.salud, 0, 20);
            turnos.TurnoJugadorVerdadero = true;
            historialAccionesJugador.Add("Curar");
        }
    }

    // Funci�n para aumentar el escudo de la IA
    void AumentarEscudo()
    {
        if (turnos.TurnoJugadorVerdadero == false)
        {
            s_escudo.escudo += 3;
            Debug.Log("IA aumenta su escudo. Escudo de la IA: " + s_escudo.escudo);
            s_escudo.escudo = Mathf.Clamp(s_escudo.escudo, 0, 20);
            turnos.TurnoJugadorVerdadero = true;
            historialAccionesJugador.Add("Escudo");
        }
    }

    // Funci�n para que la IA realice una acci�n en su turno
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
            // Ajustar probabilidades seg�n el estado del juego
            AjustarProbabilidades();

            // L�gica m�s compleja para la toma de decisiones
            float vidaRelativaIA = s_salud.salud / 20f;
            float escudoRelativoIA = s_escudo.escudo / 20f;

            if (vidaRelativaIA < umbralVidaBaja)
            {
                // Priorizar la curaci�n si la vida es baja
                CurarIA();
            }
            else if (escudoRelativoIA < umbralEscudoBajo)
            {
                // Aumentar el escudo si es bajo
                AumentarEscudo();
            }
            else
            {
                // Si la vida y el escudo est�n bien, decidir entre atacar, curar o aumentar escudo
                float random = UnityEngine.Random.value;
                if (random < probabilidadCurarBase)
                {
                    CurarIA();
                }
                else if (random < probabilidadCurarBase + probabilidadEscudoBase)
                {
                    AumentarEscudo();
                }
                else
                {
                    AtacarEnemigo();
                }
            }
        }
    }

    // Funci�n para ajustar las probabilidades de acci�n
    void AjustarProbabilidades()
    {
        // Aumentar la agresividad si el jugador ataca repetidamente
        if (historialAccionesJugador.Count >= 2 &&
            historialAccionesJugador[historialAccionesJugador.Count - 1] == "Atacar" &&
            historialAccionesJugador[historialAccionesJugador.Count - 2] == "Atacar")
        {
            agresividad += 0.1f;
        }
        else
        {
            agresividad = Mathf.Max(1f, agresividad - 0.05f); // Disminuir la agresividad gradualmente
        }

        // Calcular las nuevas probabilidades
        float total = probabilidadCurarBase + probabilidadEscudoBase + probabilidadAtacarBase * agresividad;
        probabilidadCurarBase /= total;
        probabilidadEscudoBase /= total;
        probabilidadAtacarBase = probabilidadAtacarBase * agresividad / total;
    }
}