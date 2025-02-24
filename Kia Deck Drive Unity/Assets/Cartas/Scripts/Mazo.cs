using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KDDC;
using System;

public class Mazo : MonoBehaviour
{
    public CreacionMazo creacionMazo;
    public GameObject CartaPrefab;
    public Transform modificarMazo;
    public float GirarCarta = -7f;
    public float EspacioCarta = 200f;
    public float VerticalCarta = 100f;
    public int MaximoMazo;
    public List<GameObject> CartasActivas = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        //ActualizarVisualesMazo();
    }

    public void MoverCartaMano(Carta DatosCarta)
    {
        if (CartasActivas.Count < MaximoMazo)
        {
            GameObject nuevaCarta = Instantiate(CartaPrefab, modificarMazo.position, Quaternion.identity, modificarMazo);
            CartasActivas.Add(nuevaCarta);

            nuevaCarta.GetComponent<RefreshCartaDisplay>().DatosCarta = DatosCarta;
        }
        ActualizarVisualesMazo();
    }

    public void BatallaSetup(int setMaximoMazo)
    {
        MaximoMazo = setMaximoMazo;
    }

    public void ActualizarVisualesMazo()
    {
        int NumeroCartas = CartasActivas.Count;
        if (NumeroCartas == 1)
        {
            CartasActivas[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            CartasActivas[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        };
        for (int i = 0; i < NumeroCartas; i++) { 
            float RotacionCarta = (GirarCarta * (i-(NumeroCartas - i)/2f));
            CartasActivas[i].transform.localRotation = Quaternion.Euler(0f,0f,RotacionCarta);

            float Horizontal = (EspacioCarta * (i - (NumeroCartas - 1) / 2f));
            float NormalizarPosicion = (2f * i /  (NumeroCartas - 1) - 1f);
            float Vertical = VerticalCarta * (1 - NormalizarPosicion * NormalizarPosicion);
            CartasActivas[i].transform.localPosition=new Vector3(Horizontal,Vertical,0f);
        }
    }
}