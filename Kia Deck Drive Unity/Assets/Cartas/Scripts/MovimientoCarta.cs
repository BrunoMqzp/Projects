using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using KDDC;
using TMPro.Examples;

public class MovimientoCarta : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
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

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.localRotation;
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
        rectTransform.localScale = originalScale; // Reiniciar Escala
        rectTransform.localRotation = originalRotation; //Reinicioar Rotacion
        rectTransform.localPosition = originalPosition; // Reiniciar Posicion
        glowEffect.SetActive(false); //Desactivar effect glow
        playArrow.SetActive(false); //Desactivar playarrow
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
                rectTransform.position = Vector3.Lerp(rectTransform.position, Input.mousePosition, lerpFactor);
                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    EstadoActual = 3;
                    playArrow.SetActive(true);
                    rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor);
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
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
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