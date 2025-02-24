using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIMANAGER : MonoBehaviour
{
    public RectTransform objectToPosition;
    public int divisionancho = 2;
    public int divisionalto = 2;
    public float multiplicacionancho = 1f;
    public float multiplicacionalto = 1f;
    public bool actualizarposicion = false;

    void Start()
    {
        FijarUIPosicion();
    }

    void Update()
    {
        if (actualizarposicion)
        {
            FijarUIPosicion();

        }
    }

    public void FijarUIPosicion()
    {
        if (objectToPosition != null && divisionancho != 0 && divisionalto != 0)
        {
            float ejex = multiplicacionancho / divisionancho;
            float ejey = multiplicacionalto / divisionalto;

            objectToPosition.anchorMin = new Vector2(ejex, ejey);
            objectToPosition.anchorMax = new Vector2(ejex, ejey);
            objectToPosition.pivot = new Vector2(0.5f, 0.5f);
            objectToPosition.anchoredPosition = Vector2.zero;

        }
    }
}
