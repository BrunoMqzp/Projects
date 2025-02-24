using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControlador : MonoBehaviour
{
    public ENDGAME ENDGAME;
    public ENDGAME ENDGAME2;
    public ENDGAME ENDGAME3;
    public SistemaPuntos sistemaPuntos;
    public void GameOver()
    {
        ENDGAME.Setup();
    }

    public void Pausa()
    {
        ENDGAME.Setup();
    }

    public void Victory()
    {
        ENDGAME3.Setup();
    }
}
