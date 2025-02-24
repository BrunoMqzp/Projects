using JetBrains.Annotations;
using System;
using System.Data;
using System.Data.SqlClient;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;
public class REGISTRO : MonoBehaviour
{
    //public string Usuario; //to share the variable with onGUI function
    public InputField nameField;
    public InputField passwordField;
    public Button registerButton;
    public InputField vpasswordField;
    public InputField numeroempleado;

    void Start()
    {
        Debug.Log("Registro AVA"); // Show message on the GameObject Console

        // Attach the login logic to the button
        registerButton.onClick.AddListener(ValidateRegistro);
    }

    void ValidateRegistro()
    {
        string connectionString =
           "Server=BRUNOLAP;" + //Your SQLServer, use double slashes for the path, e.g., MYPC\\SQLEXPRESS
           "Database=KDD;" + //Your database name
           "User ID=sa;" + //Your SQL user as SA or other
           "Password=12345;"; //Your database user password

        // Get input from the UI fields
        string inputUsername = nameField.text;
        string inputPassword = passwordField.text;
        string VerificarPassword = vpasswordField.text;
        string inputNumeroEmpleado = numeroempleado.text;
        if (string.IsNullOrEmpty(inputUsername)) throw new System.ArgumentNullException(nameof(inputUsername), "Usuario incompleto");
        if (string.IsNullOrEmpty(inputPassword)) throw new System.ArgumentNullException(nameof(inputPassword), "Ingresar clave");
        if (inputPassword == VerificarPassword){
            IDbConnection Rconnection;
            using (Rconnection = new SqlConnection(connectionString))
            {
                Rconnection.Open();
                using (IDbCommand dbcmd = Rconnection.CreateCommand())
                {
                string Verificarsql = "SELECT nombre FROM Usuario WHERE nombre = @username";
                dbcmd.CommandText = Verificarsql;
                string checkUserSql = "SELECT COUNT(*) FROM Usuario WHERE nombre = @username";
                dbcmd.CommandText = checkUserSql;
                SqlParameter nameParam = new SqlParameter("@username", SqlDbType.VarChar);
                nameParam.Value = inputUsername; // Assuming 'name' is the same as 'username' for simplicity
                dbcmd.Parameters.Add(nameParam);

                int userCount = (int)dbcmd.ExecuteScalar();
                if (userCount > 0)
                    {
                        // User already exists
                        Debug.Log("Username already taken.");
                    }
                    else
                    {
                    // Proceed to register the new user
                    string registerSql = "INSERT INTO Usuario (nombre, password, loginStatus) VALUES (@username, @password, @loginS)";
                    dbcmd.CommandText = registerSql;

                    // Add parameters for username, password, and name
                    SqlParameter passwordParam = new SqlParameter("@password", SqlDbType.VarChar);
                        passwordParam.Value = inputPassword;
                        dbcmd.Parameters.Add(passwordParam);

                        //string registerSql = "INSERT INTO Usuario (loginStatus) VALUES (@loginS)";
                        //dbcmd.CommandText = registerSql;
                        SqlParameter LoginParameter = new SqlParameter("@loginS", SqlDbType.VarChar);
                        LoginParameter.Value = "True";
                        dbcmd.Parameters.Add(LoginParameter);

                    // Execute the insert command
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
            }
        }
        else
        {
            Debug.Log("Password no coincide");
        }
    }
}