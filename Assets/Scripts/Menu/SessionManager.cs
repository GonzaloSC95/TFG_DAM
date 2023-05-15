using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    /* Atributos */
    private static SessionManager instance;
    private DataService db;
    private Usuario user;
    /* Métodos */
    /* Método InicializarComponentes */
    private void InicializarComponentes()
    {
        db = new DataService("DataBase.db");
    }
    /* Método Awake */
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
        StartCoroutine(CreateDatabase());
        DontDestroyOnLoad(instance);
    }

    /* Método CreateDatabase */
    private IEnumerator CreateDatabase()
    {
        if((db.TablePartidas != null) && (db.TableUsuarios != null))
        {
            db.CreateDataBase();
            yield return new WaitUntil(() => ((db.TableUsuarios != null) && (db.TablePartidas != null)));
        }
    }

    /* Getters */
    public static SessionManager Instance { get => instance; }
    public DataService Db { get => db; }
    public Usuario User { get => user; set => user = value; }
}
