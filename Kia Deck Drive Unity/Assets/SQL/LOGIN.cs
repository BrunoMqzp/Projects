using JetBrains.Annotations;
using System;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.SceneManagement;
public class LOGIN : MonoBehaviour
{
    public string Usuario; //to share the variable with onGUI function
    public InputField nameField;
    public InputField passwordField;
    public Button submitButton;
    public string NombreEscena;
    public static string inputUsername;

    void Start()
    {
        Debug.Log("Hello world !!!"); // Show message on the GameObject Console

        // Attach the login logic to the button
        submitButton.onClick.AddListener(ValidateLogin);
    }

    void ValidateLogin()
    {
        string connectionString =
           "Server=BRUNOLAP;" + //Your SQLServer, use double slashes for the path, e.g., MYPC\\SQLEXPRESS
           "Database=KDD;" + //Your database name
           "User ID=sa;" + //Your SQL user as SA or other
           "Password=12345;"; //Your database user password

        // Get input from the UI fields
        inputUsername = nameField.text;
        string inputPassword = passwordField.text;
        //if (inputUsername == null) throw new System.ArgumentNullException(nameof(inputUsername), "Favor de ingresar un nombre de Usuario");
        //if (inputPassword == null) throw new System.ArgumentNullException(nameof(inputPassword), "Favor de ingresar una clave");
        IDbConnection connection;
        using (connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (IDbCommand dbcmd = connection.CreateCommand())
            {
                // Query to check if the user exists with the provided username and password
                string sql = "SELECT nombre, password FROM Usuarios WHERE nombre = @username AND password = @password";
                dbcmd.CommandText = sql;

                // Adding parameters to prevent SQL injection
                SqlParameter usernameParam = new SqlParameter("@username", SqlDbType.VarChar);
                usernameParam.Value = inputUsername;
                dbcmd.Parameters.Add(usernameParam);

                SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.VarChar);
                passwordParam.Value = inputPassword;
                dbcmd.Parameters.Add(passwordParam);

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
                        Debug.Log("Login successful: " + Usuario);
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