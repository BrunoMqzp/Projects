using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volumen : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image imagenMute;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume = slider.value;
        RevisarSiEstoyMuteado();
    }

    public void ChangeSlider(float valor)  // Cambié el nombre a "ChangeSlider" en lugar de "ChanceSlider"
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("volumenAudio", sliderValue);
        AudioListener.volume = slider.value;
        RevisarSiEstoyMuteado();
    }

    public void RevisarSiEstoyMuteado()
    {
        if (sliderValue == 0)
        {
            imagenMute.enabled = true;  // Se cambió a "enabled" en lugar de "enable"
        }
        else
        {
            imagenMute.enabled = false;
        }
    }
}

