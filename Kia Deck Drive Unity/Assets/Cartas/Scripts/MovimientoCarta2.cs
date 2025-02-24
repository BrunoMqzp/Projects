using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using KDDC;
using TMPro.Examples;

public class MovimientoCarta2 : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 originalScale;
    private int EstadoActual = 0;
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    public Turnos turnos;
    public Carta DatosCarta;
    public Jugador jugador;

    [SerializeField] private float ElegirEscala = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;
    [SerializeField] private float lerpFactor = 0.1f;

    private Camera uiCamera; // For world space conversion

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;
        if (canvas.renderMode == RenderMode.WorldSpace || canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            uiCamera = canvas.worldCamera; // Use the camera associated with the Canvas in Camera mode
        }
    }

    void Update()
    {
        switch (EstadoActual)
        {
            case 1:
                HandleOverState();
                break;
            case 2:
                HandleDragState();
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;
            case 3:
                HandlePlayState();
                break;
        }
    }

    public void TransitionToState0()
    {
        EstadoActual = 0;
        rectTransform.localScale = originalScale;
        rectTransform.localRotation = originalRotation;
        rectTransform.localPosition = originalPosition;
        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EstadoActual == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;
            EstadoActual = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EstadoActual == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (EstadoActual == 1)
        {
            EstadoActual = 2;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
            originalPanelLocalPosition = rectTransform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (EstadoActual == 2)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                // Convert screen point to world point for correct positioning in Camera render mode
                Vector3 worldPosition;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, uiCamera, out worldPosition);
                rectTransform.position = Vector3.Lerp(rectTransform.position, worldPosition, lerpFactor);

                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    EstadoActual = 3;
                    playArrow.SetActive(true);
                    rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, playPosition, lerpFactor);
                }
            }
        }
    }

    private void HandleOverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * ElegirEscala;
    }

    private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState()
    {
        rectTransform.localPosition = playPosition;
        rectTransform.localRotation = Quaternion.identity;
        if (Input.mousePosition.y < cardPlay.y)
        {
            EstadoActual = 2;
            playArrow.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0))
        {
            turnos = FindObjectOfType<Turnos>();
            turnos.DatosCarta = gameObject.GetComponent<RefreshCartaDisplay>().DatosCarta;
            if (turnos.TurnosActuales == 0 || turnos.DatosCarta.Costo > turnos.TurnosActuales || turnos.TurnoJugadorVerdadero == false)
            {
                TransitionToState0();
            }
            else
            {
                Mazo mazogestion = FindAnyObjectByType<Mazo>();
                DiscardManager discardManager = FindObjectOfType<DiscardManager>();
                discardManager.AddToDiscard(gameObject.GetComponent<RefreshCartaDisplay>().DatosCarta);

                Jugador jugador = FindObjectOfType<Jugador>();
                jugador.AgregarCartaJugada(gameObject.GetComponent<RefreshCartaDisplay>().DatosCarta);
                mazogestion.CartasActivas.Remove(gameObject);
                mazogestion.ActualizarVisualesMazo();
                Debug.Log("Carta Eliminada del Mazo");
                GameObject.Find("Cartas Jugadas").GetComponent<Jugador>().cartaDescartada = true;
                Destroy(gameObject);
            }
        }
    }
}

