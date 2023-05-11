using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    /* Atributos */
    private static SessionManager instance;
    private DataService db;
    private Usuario user;
    /* M�todos */
    /* M�todo InicializarComponentes */
    private void InicializarComponentes()
    {
        db = new DataService("DataBase.db");
    }
    /* M�todo Awake */
    private void Awake()
    {
        //Creamos la instancia del objeto GameManager
        if (instance != this && instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        //Inicializamos los componentes
        InicializarComponentes();
        db.CreateDataBase();
        DontDestroyOnLoad(instance);
    }

    /* Getters */
    public static SessionManager Instance { get => instance; }
    public DataService Db { get => db; }
    public Usuario User { get => user; set => user = value; }
}
