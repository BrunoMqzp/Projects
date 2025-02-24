using JetBrains.Annotations;
using System;
using System.Data;
using System.Data.SqlClient;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class REGISTRONUEVO : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;
    public Button registerButton;
    public InputField vpasswordField;
    public InputField numeroempleado;

    void Start()
    {
        Debug.Log("Registro AVA"); // Show message on the GameObject Console

        // Attach the registration logic to the button
        registerButton.onClick.AddListener(ValidateRegistro);
    }

    void ValidateRegistro()
    {
        string connectionString =
            "Server=BRUNOLAP;" + // Your SQL Server
            "Database=KDD;" + // Your database name
            "User ID=sa;" + // Your SQL user
            "Password=12345;"; // Your database password

        // Get input from the UI fields
        string inputUsername = nameField.text;
        string inputPassword = passwordField.text;
        string verifyPassword = vpasswordField.text;
        string inputNumeroEmpleado = numeroempleado.text;

        if (string.IsNullOrEmpty(inputUsername)) throw new ArgumentNullException(nameof(inputUsername), "Usuario incompleto");
        if (string.IsNullOrEmpty(inputPassword)) throw new ArgumentNullException(nameof(inputPassword), "Ingresar clave");
        if (string.IsNullOrEmpty(inputNumeroEmpleado)) throw new ArgumentNullException(nameof(inputNumeroEmpleado), "Ingresar número de empleado");

        if (inputPassword == verifyPassword)
        {
            using (IDbConnection Rconnection = new SqlConnection(connectionString))
            {
                Rconnection.Open();

                // Check if the employee number already exists in the database
                using (IDbCommand dbcmd = Rconnection.CreateCommand())
                {
                    string checkEmployeeSql = "SELECT COUNT(*) FROM EmpleadosKIA WHERE [Pers.No.] = @numeroEmpleado";
                    dbcmd.CommandText = checkEmployeeSql;
                    dbcmd.Parameters.Add(new SqlParameter("@numeroEmpleado", SqlDbType.VarChar) { Value = inputNumeroEmpleado });

                    int employeeCount = (int)dbcmd.ExecuteScalar();
                    if (employeeCount > 0)
                    {
                        // Employee number already exists, register the user with this employee number
                        string checkUserSql = "SELECT COUNT(*) FROM Usuarios WHERE nombre = @username";
                        dbcmd.CommandText = checkUserSql;
                        dbcmd.Parameters.Clear();
                        dbcmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar) { Value = inputUsername });

                        int userCount = (int)dbcmd.ExecuteScalar();
                        if (userCount > 0)
                        {
                            Debug.Log("Username already taken.");
                        }
                        else
                        {
                            // Insert the new user associated with the employee number
                            string registerSql = "INSERT INTO Usuarios (nombre, password, [Pers.No.], loginStatus) VALUES (@username, @password, @numeroEmpleado, @loginS)";
                            dbcmd.CommandText = registerSql;
                            dbcmd.Parameters.Clear();
                            dbcmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar) { Value = inputUsername });
                            dbcmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = inputPassword });
                            dbcmd.Parameters.Add(new SqlParameter("@numeroEmpleado", SqlDbType.VarChar) { Value = inputNumeroEmpleado });
                            dbcmd.Parameters.Add(new SqlParameter("@loginS", SqlDbType.VarChar) { Value = "True" });

                            int rowsAffected = dbcmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Debug.Log("User registered successfully.");
                            }
                            else
                            {
                                Debug.Log("Failed to register the user.");
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Employee number not found.");
                    }
                }
            }
        }
        else
        {
            Debug.Log("Passwords do not match.");
        }
    }
}

