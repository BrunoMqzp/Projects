using System;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using TMPro;
public class GenerateLabels : MonoBehaviour
{
    public string UserName; //to share the variable with onGUI function
    public LOGINUEVO LOGIN;
    public LoginManager LoginManager;
    public TMP_Text Tmp;
    void Start()
    {
        Debug.Log("Hello world !!!"); // Show message on the GameObject Console
        string connectionString =
           "Server=BRUNOLAP;" + //Your SQLServer use with doble slash \\ for path as: MYPC\\SQLEXPRESS
           "Database=KDD;" + //Your database name
           "User ID=sa;" + //Your SQL user as SA or other
           "Password=12345;"; //Your database user password
        IDbConnection dbcon;
        using (dbcon = new SqlConnection(connectionString))
        {
            dbcon.Open();
            using (IDbCommand dbcmd = dbcon.CreateCommand())
            {
                string sql = "SELECT nombre FROM Usuarios"; //To select only 1 user
                dbcmd.CommandText = sql;
                using (IDataReader reader = dbcmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string FirstName = (string)reader["nombre"];
                        Debug.Log("prueba");
                        if (FirstName == LoginManager.username)
                        {
                            Debug.Log("Usuario encontrado: " + FirstName);
                            //string LastName = (string)reader["UserPrincipalName"];
                            //UserName = "Name: " + FirstName + " " + LastName;
                            UserName = FirstName;
                            Debug.Log(UserName); //Display username in GameObject Console
                        }
                    }
                }
            }
        }
        Show();
    }
    //void OnGUI()
    //{
    //    //Show database values on GameObject Scene
    //    Rect position = new Rect(60, 20, 400, 60);
    //    GUI.Label(position, UserName);
    //    //You can use a LIST to record all the Selected results from MSSQL database
    //}

    void Show()
    {
        Tmp.text = UserName;
    }
}