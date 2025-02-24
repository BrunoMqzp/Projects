using System;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;

public class ActualizarPuntuaciones : MonoBehaviour
{
    public static string nombre; // Player's name
    public static string numeroEmpleado; // Employee number (Pers.No)
    public int duracion; // Duration of the game
    public int puntaje; // Player's score (Puntaje)
    public PUNTUACIONES puntuaciones;
    public Turnos turnos;
    public void GuardarPuntaje()
{
    // Find necessary components
    turnos = FindObjectOfType<Turnos>();
    puntuaciones = FindObjectOfType<PUNTUACIONES>();

    // Ensure that none of the components are null
    if (turnos == null || puntuaciones == null)
    {
        Debug.LogError("Missing component(s). Ensure that Turnos and PUNTUACIONES are assigned.");
        return;
    }

    // Assign variables from static class properties
    nombre = LOGINUEVO.inputUsername;
    numeroEmpleado = LOGINUEVO.numeroempleado;
    duracion = turnos.Rondas;
    puntaje = puntuaciones.score;

    string connectionString =
        "Server=BRUNOLAP;" +
        "Database=KDD;" +
        "User ID=sa;" +
        "Password=12345;";

    using (IDbConnection Rconnection = new SqlConnection(connectionString))
    {
        Rconnection.Open();

        // Insert the score and other game details into the 'Partidas' table
        using (IDbCommand dbcmd = Rconnection.CreateCommand())
        {
            string insertScoreSql = "INSERT INTO Partidas ([Pers.No.], Duracion, Puntaje, nombre) VALUES (@numeroEmpleado, @duracion, @puntaje, @nombre)";
            dbcmd.CommandText = insertScoreSql;

            // Add parameters for the employee number, duration, score, and name
            dbcmd.Parameters.Add(new SqlParameter("@numeroEmpleado", SqlDbType.VarChar) { Value = numeroEmpleado });
            dbcmd.Parameters.Add(new SqlParameter("@duracion", SqlDbType.Int) { Value = duracion });
            dbcmd.Parameters.Add(new SqlParameter("@puntaje", SqlDbType.Int) { Value = puntaje });
            dbcmd.Parameters.Add(new SqlParameter("@nombre", SqlDbType.VarChar) { Value = nombre });

            try
            {
                int rowsAffected = dbcmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Debug.Log("Score saved successfully.");
                }
                else
                {
                    Debug.Log("Failed to save the score.");
                }
            }
            catch (SqlException ex)
            {
                Debug.LogError($"SQL Error: {ex.Message}");
                // Handle specific SQL exceptions as needed
            }
        }
    }
}

}

