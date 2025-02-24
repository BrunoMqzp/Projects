using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AIPRUEBA : MonoBehaviour
{
    public Turnos turns;
    public Enemigo enemy;
    public Jugador player;
    public SaludEnemigo s_health;
    public EscudoEnemigo s_shield;
    public Salud health;
    public Escudo escudo; // Reference to the player's shield
    public int Rondas = 0;
    // Costs associated with actions
    public int AttackCost = 1;
    public int HealCost = 1;
    public int ShieldCost = 1;

    //ANIMACION
    private Animator anim;
    AudioSource efectos;

    public AudioClip[] Sonidos;
    public GameObject objetoEnemigo;

    void Start()
    {
        efectos = GetComponent<AudioSource>();
        anim = objetoEnemigo.GetComponent<Animator>();
    }

    // Function to perform Minimax decision-making for the AI
    public void PerformAction()
    {
        player = FindObjectOfType<Jugador>();
        enemy = FindObjectOfType<Enemigo>();
        turns = FindObjectOfType<Turnos>();
        s_shield = FindObjectOfType<EscudoEnemigo>();
        s_health = FindObjectOfType<SaludEnemigo>();
        health = FindObjectOfType<Salud>();
        escudo = FindObjectOfType<Escudo>(); // Initialize the player's shield

        if (!turns.TurnoJugadorVerdadero)
        {
            // Execute the Minimax decision-making process
            MinimaxDecision();

            // End AI's turn if it cannot take any action
            if (turns.TurnosActualesMaquina <= 0)
            {
                turns.TurnoJugadorVerdadero = true; // Change to player's turn
                turns.TurnosActualesMaquina += 2; // Add turns for the next round
                Debug.Log("AI ends its turn and gains 2 turns for the next round.");
                Rondas++;
            }
        }
    }

    private void MinimaxDecision()
    {
        int bestScore = int.MinValue;
        int bestAction = -1;

        // Evaluate each possible action for the AI
        for (int action = 0; action < 3; action++) // 0: Attack, 1: Heal, 2: Increase Shield
        {
            int currentScore = MinimaxEvaluate(action, 0);
            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestAction = action;
            }
        }

        // Execute the best action found
        ExecuteAction(bestAction);
    }

    private int MinimaxEvaluate(int action, int depth)
    {
        // Base case: If the depth is too high or the game is over, return a score
        if (depth > GetDynamicDepth() || IsGameOver())
        {
            return EvaluateGameState();
        }

        int score;
        if (turns.TurnoJugadorVerdadero)
        {
            // Maximizing player's score
            score = int.MinValue;
            for (int a = 0; a < 3; a++)
            {
                if (IsActionValid(a))
                {
                    ExecuteAction(a);
                    score = Mathf.Max(score, MinimaxEvaluate(a, depth + 1));
                    UndoAction(a);
                }
            }
        }
        else
        {
            // Minimizing AI's score
            score = int.MaxValue;
            for (int a = 0; a < 3; a++)
            {
                if (IsActionValid(a))
                {
                    ExecuteAction(a);
                    score = Mathf.Min(score, MinimaxEvaluate(a, depth + 1));
                    UndoAction(a);
                }
            }
        }
        return score;
    }

    private bool IsGameOver()
    {
        // Check if the game is over (either the player or AI is defeated)
        return health.salud <= 0 || s_health.salud <= 0;
    }

    private int EvaluateGameState()
    {
        // Enhanced heuristic: score the game state
        int score = s_health.salud - health.salud; // AI health - Enemy health

        // Evaluate based on health ratios
        float aiHealthRatio = (float)s_health.salud / 20; // Assuming max health is 20
        float playerHealthRatio = (float)health.salud / 20;

        score += (int)((aiHealthRatio - playerHealthRatio) * 10); // Weight health ratios

        // Adjust score based on the enemy's shield and player's shield
        if (escudo != null && escudo.escudo > 0)
        {
            score -= escudo.escudo; // Penalize for player shield
        }

        // Reward if the AI has high health and the enemy has low health
        if (s_health.salud > 10 && health.salud < 10)
        {
            score += 5; // Favorable condition for AI
        }

        // More complex heuristics can be added here
        return score;
    }

    private bool IsActionValid(int action)
    {
        switch (action)
        {
            case 0: // Attack
                return turns.TurnosActualesMaquina >= AttackCost && (escudo == null || escudo.escudo <= 0); // Check if player shield is active
            case 1: // Heal
                return turns.TurnosActualesMaquina >= HealCost;
            case 2: // Increase Shield
                return turns.TurnosActualesMaquina >= ShieldCost;
            default:
                return false;
        }
    }

    private void ExecuteAction(int action)
    {
        switch (action)
        {
            case 0:
                AttackEnemy();
                break;
            case 1:
                HealIA();
                break;
            case 2:
                IncreaseShield();
                break;
        }
    }

    private void UndoAction(int action)
    {
        // Implement the logic to undo the action if necessary
        // This requires keeping track of the state before the action was executed
    }

    // Function to get dynamic depth based on game state
    private int GetDynamicDepth()
    {
        if (s_health.salud < 5) // If AI health is low
        {
            return 5; // Deeper search for safety
        }
        return 3; // Normal depth
    }

    // Function to attack the enemy
    void AttackEnemy()
    {
        if (!turns.TurnoJugadorVerdadero)
        {
            if (escudo.escudo > 0)
            {
                escudo.escudo -= 2;
                if (escudo.escudo < 0)
                {
                    if (health.salud > 0)
                    {
                        health.salud += escudo.escudo; // Adjust health if shield goes below zero
                    }
                    escudo.escudo = 0;
                }
                Debug.Log("Dano Maximo aplicado al escudo");
            }
            else
            {
                health.salud -= 1; // Adjust damage as necessary
                Debug.Log("AI attacks the enemy. Enemy health: " + health.salud);
                turns.TurnosActualesMaquina -= AttackCost; // Deduct turn cost
            }
            anim.Play("Ataque_enemigo");
            efectos.clip = Sonidos[0];
            efectos.Play();
        }
    }

    // Function to heal the AI
    void HealIA()
    {
        if (!turns.TurnoJugadorVerdadero)
        {
            s_health.salud += 1; // Adjust healing as necessary
            Debug.Log("AI heals. AI life: " + s_health.salud);
            s_health.salud = Mathf.Clamp(s_health.salud, 0, 20);
            turns.TurnosActualesMaquina -= HealCost; // Deduct turn cost
            anim.Play("Vida_enemigo");
            efectos.clip = Sonidos[1];
            efectos.Play();
        }
    }

    // Function to increase the AI's shield
    void IncreaseShield()
    {
        if (!turns.TurnoJugadorVerdadero)
        {
            s_shield.escudo += 1; // Adjust shield amount as necessary
            Debug.Log("AI increases its shield. AI shield: " + s_shield.escudo);
            s_shield.escudo = Mathf.Clamp(s_shield.escudo, 0, 20);
            turns.TurnosActualesMaquina -= ShieldCost; // Deduct turn cost
            anim.Play("Escudo_enemigo");
            efectos.clip = Sonidos[2];
            efectos.Play();
        }
    }
}
