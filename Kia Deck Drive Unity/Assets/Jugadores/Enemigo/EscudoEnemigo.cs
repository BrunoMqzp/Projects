using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KDDC;

public class EscudoEnemigo : MonoBehaviour
{
    public Text EscudoTexto;
    public Image EscudoImage;
    public int escudo;
    public int escudomaximo = 20;
    float lerpSpeed;
    public Carta DatosCarta;

    private void Start()
    {
        escudo = 0;
    }

    private void Update()
    {
        EscudoTexto.text = "Escudo: " + escudo;

        ConversionBarraEscudo();
        ColorChanger();
    }

    void ConversionBarraEscudo()
    {
        EscudoImage.fillAmount = (float)escudo / escudomaximo;

    }

    void ColorChanger()

    {
        Color ShieldColor = Color.Lerp(Color.red, Color.blue, ((float)escudo / escudomaximo));
        EscudoImage.color = ShieldColor;

    }
}