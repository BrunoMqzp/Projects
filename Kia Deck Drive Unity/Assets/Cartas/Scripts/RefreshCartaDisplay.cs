using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KDDC;

public class RefreshCartaDisplay : MonoBehaviour
{
    public Carta DatosCarta;
    public Image ImagenCarta;
    public Image FotoCarta;
    public TMP_Text TextoNombre;
    public TMP_Text TextoSalud;
    public TMP_Text TextoAtaque;
    public TMP_Text TextoEscudo;
    public TMP_Text TextoDescripcion;
    public TMP_Text TextoCosto;
    public Image[] TipoImagenes;

    private Color[] SimboloColor =
    {
        new Color(0.08f, 0.7f, 0.48f), //Vida
        new Color(0.0f, 0.36f, 0.66f), //escudo
        new Color(0.6f, 0.09f, 0.27f), //Ataque
        new Color(0.67f, 0.54f, 0.15f) //Especial
    };

    private Color[] TipoColor =
    {
        new Color(0.180f, 0.435f, 0.251f), //Vida
        new Color(0.510f, 0.784f, 0.898f), //escudo
        new Color(0.753f, 0.275f, 0.341f), //Ataque
        new Color(0.937f, 0.749f, 0.016f) //Especial
    };

    void Start()
    {
        ActualizarCartaDisplay();

    }
    public void ActualizarCartaDisplay()
    {
        ImagenCarta.color = TipoColor[(int)DatosCarta.tipocarta[0]];

        TextoNombre.text = DatosCarta.CartaNombre;
        TextoSalud.text = DatosCarta.salud.ToString();
        //TextoAtaque.text = $"{DatosCarta.damageMin} - {DatosCarta.damageMax}";
        TextoAtaque.text = DatosCarta.damageMax.ToString();
        TextoEscudo.text = DatosCarta.escudo.ToString();
        TextoDescripcion.text = DatosCarta.descripcion;
        FotoCarta.sprite = DatosCarta.imagen;
        TextoCosto.text = DatosCarta.Costo.ToString();

        for (int i = 0; i < TipoImagenes.Length; i++)
        { 
            if(i < DatosCarta.tipocarta.Count)
            {
                TipoImagenes[i].gameObject.SetActive(true);
                TipoImagenes[i].color = SimboloColor[(int)DatosCarta.tipocarta[i]];
            }
            else
            {
                TipoImagenes[i].gameObject.SetActive(false);
            }
        }
    }

}
