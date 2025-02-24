using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MostrarTips : MonoBehaviour
{
    public Text tipsText;
    public string[] tips;
    public int tipCount;

    void Start()
    {
        GenerateTips();
    }

    public void GenerateTips()
    {
        tipCount = Random.Range(0, tips.Length);
        tipsText.text = tips[tipCount];
    }
}
