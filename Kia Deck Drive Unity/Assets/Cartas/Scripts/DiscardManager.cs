using System.Collections;
using System.Collections.Generic;
using KDDC;
using UnityEngine;
using TMPro;

public class DiscardManager : MonoBehaviour
{
    [SerializeField] public List<Carta> DiscardPile = new List<Carta>();
    public TextMeshProUGUI DiscardCount;
    public int DiscardCardsCount;

    void Awake()
    {
        ActualizarToDiscard();
    }

    public void ActualizarToDiscard()
    {
        DiscardCount.text = DiscardPile.Count.ToString();
        DiscardCardsCount = DiscardPile.Count;
        Debug.Log("DiscardPile: " + DiscardPile.Count);
    }

    public void AddToDiscard(Carta carta)
    {
        DiscardPile.Add(carta);
        ActualizarToDiscard();
    }

    public Carta PullFromDiscard()
    {
        if (DiscardPile.Count > 0)
        {
            Carta carta = DiscardPile[DiscardPile.Count - 1];
            DiscardPile.RemoveAt(DiscardPile.Count - 1);
            ActualizarToDiscard();
            return carta;
        }
        else
        {
            return null;
        }
    }

    public bool PullSelectFromDiscard(Carta carta)
    {
        if (DiscardPile.Count > 0 && DiscardPile.Contains(carta))
        {
            DiscardPile.Clear();
            ActualizarToDiscard();
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Carta> PullAllFromDiscard()
    {
        if (DiscardPile.Count > 0)
        {
            List<Carta> cartasARegresar = new List<Carta>(DiscardPile);
            DiscardPile.Clear();
            ActualizarToDiscard();
            return cartasARegresar;
        }
        else 
        {
            return new List<Carta>();
        }
    }
}
