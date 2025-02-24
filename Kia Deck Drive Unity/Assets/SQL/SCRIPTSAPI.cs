using System;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using TMPro;
public class SCRIPTAPI : MonoBehaviour
{
    public string UserName; //to share the variable with onGUI function
    public LOGINUEVO LOGIN;
    public LoginManager loginManager;
    public TMP_Text Tmp;
    void Start()
    {
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
        UserName = LoginManager.username;
        Tmp.text = UserName;
    }
}
