using SQLite4Unity3d;
using System;

public class Usuario
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string Name { get; set; }
	public string Psw { get; set; }

	public override string ToString()
	{
		return string.Format("[Usuario: Id={0}, Name={1},  Psw={2}]", Id, Name, Psw);
	}
}
