using System.Collections;
using System.Collections.Generic;
using KDDC;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int enemigo_Vida;
    public int enemigo_Energia;
    public int enemigo_Escudo;
    public SaludEnemigo enemigo_salud;
    public EscudoEnemigo enemigo_escudo;
    private Mazo enemigo_mazogestion;
    private CartAtaqueCorregido cartAtaque;
    private CartVida cartVida;
    private CartEscudo cartEscudo;
    private CartEspecial cartEspecial;
    //public Carta DatosCarta;
    public List<Carta> CartasJugadas = new List<Carta>();
    //public bool cartaDescartada = false;
    public Jugador jugador;
    public Turnos turnos;

    void Start()
    {
        Resources.UnloadUnusedAssets();
        enemigo_salud = FindObjectOfType<SaludEnemigo>();
        if (enemigo_salud != null)
        {
            enemigo_Vida = enemigo_salud.saludmaxima;
        }
        else
        {
            Debug.LogError("Salud component not found on the GameObject.");
        }
        enemigo_escudo = FindObjectOfType<EscudoEnemigo>();
        cartAtaque = FindObjectOfType<CartAtaqueCorregido>();
        cartVida = FindObjectOfType<CartVida>();
        cartEscudo = FindObjectOfType<CartEscudo>();
        cartEspecial = FindObjectOfType<CartEspecial>();
        jugador = FindObjectOfType<Jugador>();
        turnos = FindObjectOfType<Turnos>();

    }

    void Update()
    {
        enemigo_Vida = enemigo_salud.salud;
        enemigo_Escudo = enemigo_escudo.escudo;
        enemigo_Energia = turnos.TurnosActualesMaquina;
        //if (cartaDescartada)
        //{
        //    Debug.Log("Efecto ATAQUE");
        //    cartaDescartada = false;
        //    cartAtaque.EfectoAtaque();

        //    //cartEspecial.EfectoDebuff();
        //}
    }

    public void AgregarCartaJugada(Carta carta)
    {
        CartasJugadas.Add(carta);

    }

    public void CreacionCartasJugadas(List<Carta> cartasJugadas)
    {
        CartasJugadas.AddRange(cartasJugadas);

    }

    public void ReiniciarCartasJugadas()
    {
        CartasJugadas.Clear();


    }
}