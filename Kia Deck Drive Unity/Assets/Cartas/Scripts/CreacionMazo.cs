using System.Collections;
using System.Collections.Generic;
using KDDC;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CreacionMazo : MonoBehaviour
{
    public List<Carta> CartasTotales = new List<Carta>();
    private int ID_C = 0;
    public int MazoInicial = 5;
    public int TamanoMaximoMazo =12;
    public int TamanoActualMazo;
    private Mazo mazogestion;
    private ContainerPile containerPile;
    private bool IniciarBatalla = true;

    private void Start()
    {
        Resources.UnloadUnusedAssets();
        Carta[] cartas = Resources.LoadAll<Carta>("Cartas");
        CartasTotales.AddRange(cartas);
    }

    void Awake()
    {
        if (containerPile == null)
        {
            containerPile = FindObjectOfType<ContainerPile>();

        }
        if (mazogestion == null)
        {
            mazogestion = FindObjectOfType<Mazo>();
        }
    }

    void Update()
    {
        if (IniciarBatalla)
        {
            BatallaSetup();
        }
    }

    public void BatallaSetup()
    {
        mazogestion.BatallaSetup(TamanoMaximoMazo);
        containerPile.CreacionDrawPile(CartasTotales);
        containerPile.BatallaSetup(MazoInicial, TamanoMaximoMazo);
        IniciarBatalla = false;
    }
}