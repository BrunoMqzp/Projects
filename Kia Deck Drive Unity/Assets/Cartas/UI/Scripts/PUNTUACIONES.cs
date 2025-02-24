using JetBrains.Annotations;
using System;
using System.Data;
using System.Data.SqlClient;
using TMPro;
using TMPro.Examples;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PUNTUACIONES : MonoBehaviour
{
    public static PUNTUACIONES instance;
    public TMP_Text scoreText;
    public TMP_Text PuntuacionMaxText;
    public int score = 0;
    public int PuntuacionMax = 0;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        PuntuacionMax = PlayerPrefs.GetInt("PuntuacionMax", 0);
        scoreText.text = "Puntaje: " + score.ToString();
        PuntuacionMaxText.text = "Puntaje Maximo: " + PuntuacionMax.ToString();
        
    }
    public void AddPoint(int pointValue)
    {
        score = score + pointValue;
        scoreText.text = score.ToString();
        scoreText.text = "Puntaje: " + score.ToString();
        if (PuntuacionMax < score)
        {
            PlayerPrefs.SetInt("PuntuacionMax", score);
        }
    }
}
