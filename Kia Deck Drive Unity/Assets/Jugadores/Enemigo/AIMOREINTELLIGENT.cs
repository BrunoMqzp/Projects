using System.Collections.Generic;
using UnityEngine;

public class AIMOREINTELLIGENT : MonoBehaviour
{
    public Turnos turns;
    public Enemigo enemy;
    public Jugador player;
    public SaludEnemigo s_health;
    public EscudoEnemigo s_shield;
    public Salud health;

    // Variables to control the probability of each action
    public float cureprobability = 0.1f;
    public float shieldprobability = 0.2f;
    public float attackprobability = 0.4f;

    // Costs associated with actions
    public int AttackCost = 1;
    public int HealCost = 1;
    public int ShieldCost = 1;

    // Function to perform Minimax decision-making for the AI
    public void PerformAction()
    {
        player = FindObjectOfType<Jugador>();
        enemy = FindObjectOfType<Enemigo>();
        turns = FindObjectOfType<Turnos>();
        s_shield = FindObjectOfType<EscudoEnemigo>();
        s_health = FindObjectOfType<SaludEnemigo>();
        health = FindObjectOfType<Salud>();

        if (!turns.TurnoJugadorVerdadero)
        {
            bool actionPerformed = false;

            // Check for possible actions based on current turn count
            if (turns.TurnosActualesMaquina > 0)
            {
                // Decision-making process (Minimax can be implemented here)
                actionPerformed = MinimaxDecision();

                // If no action was performed, end the turn
                if (!actionPerformed)
                {
                    turns.TurnoJugadorVerdadero = true; // End AI's turn without any action
                    Debug.Log("AI cannot perform any action and ends turn.");
                }
            }
            else
            {
                // No moves left, change to player's turn
                turns.TurnoJugadorVerdadero = true;
                Debug.Log("AI has no turns left and ends turn.");
            }
        }
    }

    private bool MinimaxDecision()
    {
        float random = UnityEngine.Random.value;

        if (random < cureprobability && turns.TurnosActualesMaquina >= HealCost)
        {
            HealIA();
            return true;
        }
        else if (random < cureprobability + shieldprobability && turns.TurnosActualesMaquina >= ShieldCost)
        {
            IncreaseShield();
            return true;
        }
        else if (random < cureprobability + shieldprobability + attackprobability && turns.TurnosActualesMaquina >= AttackCost)
        {
            AttackEnemy();
            return true;
        }

        // If no valid move can be made
        return false;
    }

    // Function to attack the enemy
    void AttackEnemy()
    {
        if (!turns.TurnoJugadorVerdadero)
        {
            health.salud -= 1; // Adjust damage as necessary
            Debug.Log("AI attacks the enemy. Enemy health: " + health.salud);
            turns.TurnosActualesMaquina -= AttackCost; // Deduct turn cost
            EndTurnIfNoActionsLeft();
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
            EndTurnIfNoActionsLeft();
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
            EndTurnIfNoActionsLeft();
        }
    }

    private void EndTurnIfNoActionsLeft()
    {
        // Check if the AI has no turns left and should end its turn
        if (turns.TurnosActualesMaquina <= 0)
        {
            turns.TurnoJugadorVerdadero = true; // Change to player's turn
            turns.TurnosActualesMaquina += 2; // Add turns for the next round
            Debug.Log("AI ends its turn and gains 2 turns for the next round.");
        }
    }
}

