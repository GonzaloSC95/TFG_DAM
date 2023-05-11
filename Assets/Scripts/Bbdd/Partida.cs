using SQLite4Unity3d;
using System;

public class Partida  {

	[PrimaryKey, AutoIncrement]
	public  int Id { get; set; }
	public int Usuario_Id { get; set; }
	public string Level { get; set; }
	public int Points { get; set; }
	public int Life { get; set; }
	public DateTime Date { get; set; }

	public override string ToString ()
	{
		return string.Format ("[Partida: Id={0}, Usuario_Id={1}, Level={2},  Points={3}, Life={4}, Date={5}]", Id, Usuario_Id, Level, Points, Life, Date);
	}
}
