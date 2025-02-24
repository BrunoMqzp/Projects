using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KDDC;
public class Salud : MonoBehaviour
{
    public Text SaludTexto;
    public Image SaludImage;
    public int salud;
    public int saludmaxima = 10;
    float lerpSpeed;
    public Carta DatosCarta;

    private void Start()
    {
        salud = saludmaxima;
    }

    private void Update()
    {
        SaludTexto.text = "Salud: " + salud + "Pt";
        
        ConversionBarraVida();
        ColorChanger();
    }

    void ConversionBarraVida()
    {
        SaludImage.fillAmount = (float)salud / saludmaxima;

    }

    void ColorChanger()

    {
        Color healthColor = Color.Lerp(Color.red, Color.green, ((float)salud / saludmaxima));
        SaludImage.color = healthColor;

    }

}
