using SQLite4Unity3d;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq.Expressions;

public class DataService  {

	/* Atributos */
	private SQLiteConnection _connection;

	/* Constructor */
	public DataService(string DatabaseName)
	{
		#if UNITY_EDITOR
		var dbPath = string.Format("{0}/DataBase/{1}", Application.dataPath, DatabaseName);
		#else
		var dbPath = string.Format("{0}/DataBase/{1}", Application.dataPath, DatabaseName);
		var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);
		dbPath = (!File.Exists(dbPath)) ? filepath : dbPath;
		#endif
		_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
		Debug.Log("DataBase PATH: " + dbPath);     
	}

	/* Métodos */
	/* Método CreateDataBase */
	public void CreateDataBase()
	{
		_connection.CreateTable<Usuario>();
		_connection.CreateTable<Partida>();
	}

	/* ------------------------ Métos para la persistencia y consulta de la tabla Partida --------------------------------- */
	/* Método GetAllPartidas */
	public IEnumerable<Partida> GetAllPartidas()
	{
		return _connection.Table<Partida>();
	}

	/* Método GetAllPartidasOrderBy */
	public IEnumerable<Partida> GetAllPartidasOrderBy(string campoOrderBy)
	{
		var lambda = GetPartidaLambdaExpesion(campoOrderBy);
		return _connection.Table<Partida>().OrderByDescending(lambda);
	}

	/* Método GetPartidas by Usuario_Id */
	public IEnumerable<Partida> GetPartidas(int usuario_id)
	{
		return _connection.Table<Partida>().Where(x => x.Usuario_Id == usuario_id);
	}

	/* Método GetPartidasOrderBy */
	public IEnumerable<Partida> GetPartidasOrderBy(int usuario_id, string campoOrderBy)
	{
		var lambda = GetPartidaLambdaExpesion(campoOrderBy);
		return _connection.Table<Partida>().Where(x => x.Usuario_Id == usuario_id).OrderByDescending(lambda);
	}

	/* Método GetFirstPartida */
	public Partida GetFirstPartida(int usuario_id)
	{
		return _connection.Table<Partida>().Where(x => x.Usuario_Id == usuario_id).FirstOrDefault();
	}

	/* Método GetLastPartida */
	public Partida GetLastPartida(int usuario_id)
	{
		return _connection.Table<Partida>().Where(x => x.Usuario_Id == usuario_id).OrderByDescending(x => x.Id).FirstOrDefault();
	}

	/* Método CreatePartida */
	public Partida CreatePartida(int usuario_id, string level, int points, int life, DateTime date)
	{
		var p = new Partida
		{
			Usuario_Id = usuario_id,
			Level = level,
			Points = points,
			Life = life,
			Date = date,
		};
		_connection.Insert(p);
		return p;
	}

	/* Método UpdatePartidaField */
	public int UpdatePartidaField(Partida p, string campo, object valor)
	{
		PropertyInfo propertyInfo = p.GetType().GetProperty(campo);
		if (propertyInfo != null && propertyInfo.CanWrite)
		{
			propertyInfo.SetValue(p, valor, null);
		}
		return _connection.Update(p);
	}

	/* Método UpdatePartida */
	public int UpdatePartida(Partida p)
	{
		return _connection.Update(p);
	}

	/* Método DeletePartida */
	public int DeletePartida(Partida p)
	{
		return _connection.Delete(p);
	}

	/* Método DeleteAllPartidas */
	public int DeleteAllPartidas()
	{
		return _connection.DeleteAll<Partida>();
	}

	/* Método DeletePartidasFromUsuario */
	public int DeletePartidasFromUsuario(int usuario_id)
	{
		return _connection.Execute("DELETE FROM PARTIDA WHERE USUARIO_ID = ?", usuario_id);
	}

	/* ----------------- Métos para la persistencia y consulta de la tabla Usuario ------------------------- */
	/* Método GetPartidas */
	public IEnumerable<Usuario> GetAllUsuarios()
	{
		return _connection.Table<Usuario>();
	}

	/* Método GetAllPartidasOrderBy */
	public IEnumerable<Usuario> GetAllUsuariosOrderBy(string campoOrderBy)
	{
		var lambda = GetUsuarioLambdaExpesion(campoOrderBy);
		return _connection.Table<Usuario>().OrderByDescending(lambda);
	}

	/* Método GetUsuarios by Usuario_Id */
	public IEnumerable<Usuario> GetUsuarios(string name)
	{
		return _connection.Table<Usuario>().Where(x => x.Name == name);
	}

	/* Método GetUsuariosOrderBy */
	public IEnumerable<Usuario> GetUsuariosOrderBy(string name, string campoOrderBy)
	{
		var lambda = GetUsuarioLambdaExpesion(campoOrderBy);
		return _connection.Table<Usuario>().Where(x => x.Name == name).OrderByDescending(lambda);
	}

	/* Método GetFirstUsuario */
	public Usuario GetFirstUsuario(string name)
	{
		return _connection.Table<Usuario>().Where(x => x.Name == name).FirstOrDefault();
	}

	/* Método GetLastUsuario */
	public Usuario GetLastUsuario(string name)
	{
		return _connection.Table<Usuario>().Where(x => x.Name == name).OrderByDescending(x => x.Id).FirstOrDefault();
	}

	/* Método CreateUsuario */
	public Usuario CreateUsuario(string name, string psw)
	{
		var u = new Usuario
		{
			Name = name,
			Psw = psw,
		};
		_connection.Insert(u);
		return u;
	}

	/* Método UpdateUsuarioField */
	public int UpdateUsuarioField(Usuario p, string campo, object valor)
	{
		PropertyInfo propertyInfo = p.GetType().GetProperty(campo);
		if (propertyInfo != null && propertyInfo.CanWrite)
		{
			propertyInfo.SetValue(p, valor, null);
		}
		return _connection.Update(p);
	}

	/* Método UpdateUsuario */
	public int UpdateUsuario(Usuario u)
	{
		return _connection.Update(u);
	}

	/* Método DeleteUsuario */
	public int DeleteUsuario(Usuario u)
	{
		return _connection.Delete(u);
	}

	/* Método DeleteAllUsuarios */
	public int DeleteAllUsuarios()
	{
		return _connection.DeleteAll<Usuario>();
	}

	/* Método DeleteUsuarios */
	public int DeleteUsuarios(string name)
	{
		return _connection.Execute("DELETE FROM USUARIO WHERE NAME = '?'", name);
	}

	/* ------------------------------ Métodos privados ----------------------------- */
	/* Método GetPartidaLambdaExpesion */
	private Expression<Func<Partida, object>> GetPartidaLambdaExpesion(string campo)
    {
		// Obtener un objeto PropertyInfo a partir del nombre del campo
		var propertyInfo = typeof(Partida).GetProperty(campo);
		if (propertyInfo == null)
		{
			Debug.LogError($"No se encontró el campo '{campo}' en la clase Partida.");
			return null;
		}
		// Usar el objeto PropertyInfo para construir una expresión lambda que usa el valor del campo como criterio de ordenación
		var parameter = Expression.Parameter(typeof(Partida), "x");
		var property = Expression.Property(parameter, propertyInfo);
		var lambda = Expression.Lambda<Func<Partida, object>>(Expression.Convert(property, typeof(object)), parameter);
		return lambda;
	}
	/* Método GetUsuarioLambdaExpesion */
	private Expression<Func<Usuario, object>> GetUsuarioLambdaExpesion(string campo)
	{
		// Obtener un objeto PropertyInfo a partir del nombre del campo
		var propertyInfo = typeof(Partida).GetProperty(campo);
		if (propertyInfo == null)
		{
			Debug.LogError($"No se encontró el campo '{campo}' en la clase Partida.");
			return null;
		}
		// Usar el objeto PropertyInfo para construir una expresión lambda que usa el valor del campo como criterio de ordenación
		var parameter = Expression.Parameter(typeof(Partida), "x");
		var property = Expression.Property(parameter, propertyInfo);
		var lambda = Expression.Lambda<Func<Usuario, object>>(Expression.Convert(property, typeof(object)), parameter);
		return lambda;
	}

	/* Getters */
	public TableMapping TableUsuarios { get => _connection.GetMapping(typeof(Usuario));}
	public TableMapping TablePartidas { get => _connection.GetMapping(typeof(Partida));}
}
