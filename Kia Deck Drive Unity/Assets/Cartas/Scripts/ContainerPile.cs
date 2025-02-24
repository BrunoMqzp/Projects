using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KDDC;
using TMPro;

public class ContainerPile : MonoBehaviour
{
    public List<Carta> drawPile = new List<Carta>();
    private int ID_C = 0;
    public int MazoInicial = 5;
    public int TamanoMaximoMazo;
    public int TamanoActualMazo;
    private Mazo mazogestion;
    private DiscardManager discardManager;
    public TextMeshProUGUI drawPileContador;

    private void Start()
    {
        Resources.UnloadUnusedAssets();
        mazogestion = FindObjectOfType<Mazo>();
    }
    void Update()
    {
        if (mazogestion != null)
        {
            TamanoActualMazo = mazogestion.CartasActivas.Count;
        }
    }

    public void CreacionDrawPile(List<Carta> cartasPendientes)
    {
        drawPile.AddRange(cartasPendientes);
        Utilidades.Shuffle(drawPile);
        ActualizarDrawPile();
    }

    public void BatallaSetup(int numertoCartasJugar, int setTamanoMaximoMazo)
    {
        TamanoMaximoMazo = setTamanoMaximoMazo;
        for (int i = 0; i < numertoCartasJugar; i++)
        {
            SorteoCarta(mazogestion);
        }
    }
    public void SorteoCarta(Mazo mazogestion)
    {
        if (drawPile.Count == 0)
        {
            RellenarCartaDescartadas();
        }
        if (TamanoActualMazo < TamanoMaximoMazo && TamanoActualMazo < 6)
        {
            Carta CartaSiguiente = drawPile[ID_C];
            mazogestion.MoverCartaMano(CartaSiguiente);
            drawPile.RemoveAt(ID_C);
            ActualizarDrawPile();
            if (drawPile.Count > 0) ID_C %= drawPile.Count;

        }
    }

    private void ActualizarDrawPile()
    {
        drawPileContador.text = drawPile.Count.ToString();
    }

    private void RellenarCartaDescartadas()
    {
        if (discardManager == null)
        {
            discardManager = FindObjectOfType<DiscardManager>();
        }
        if (discardManager != null && discardManager.DiscardCardsCount > 0)
        {
            drawPile = discardManager.PullAllFromDiscard();
            Utilidades.Shuffle(drawPile);
            ID_C = 0;
        }
    }
}
