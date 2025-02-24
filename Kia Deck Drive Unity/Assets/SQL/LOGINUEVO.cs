using JetBrains.Annotations;
using System;
using System.Data;
using System.Data.SqlClient;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class LOGINUEVO : MonoBehaviour
{
    public string Usuario; // To share the variable with onGUI function
    public InputField nameField;
    public InputField passwordField;
    public Button submitButton;
    public string NombreEscena;
    public static string inputUsername;
    public static string numeroempleado; // Static string to store Pers.No.

    void Start()
    {
        Debug.Log("Hello world !!!"); // Show message on the GameObject Console

        // Attach the login logic to the button
        submitButton.onClick.AddListener(ValidateLogin);
    }

    void ValidateLogin()
    {
        string connectionString =
           "Server=BRUNOLAP;" + // Your SQLServer
           "Database=KDD;" + // Your database name
           "User ID=sa;" + // Your SQL user
           "Password=12345;"; // Your database user password

        // Get input from the UI fields
        inputUsername = nameField.text;
        string inputPassword = passwordField.text;

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (IDbCommand dbcmd = connection.CreateCommand())
            {
                // Query to check if the user exists with the provided username and password
                string sql = "SELECT nombre, password, [Pers.No.] FROM Usuarios WHERE nombre = @username AND password = @password";
                dbcmd.CommandText = sql;

                // Adding parameters to prevent SQL injection
                dbcmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar) { Value = inputUsername });
                dbcmd.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar) { Value = inputPassword });

                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    // Check if any rows were returned
                    if (reader.Read())
                    {
                        ChangeScene();

                        // If a user is found, login is successful
                        string firstName = (string)reader["nombre"];
                        string lastName = (string)reader["password"];
                        Usuario = "Name: " + firstName + " " + lastName;
                        numeroempleado = (string)reader["Pers.No."]; // Store Pers.No. in static variable
                        Debug.Log("Login successful: " + Usuario + ", Pers.No.: " + numeroempleado);
                    }
                    else
                    {
                        // No user found with the provided credentials
                        Debug.Log("Invalid username or password.");
                    }
                }
            }
        }
    }

    public void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(NombreEscena);
    }
}
