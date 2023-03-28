// Voy a hacer una state Machine que controle los ditintos estados del usuario, de esta forma podemos correr codigo que sea unico para cada estado. Esto tiene varias ventajas 
//y aprovecha principios de la programacion orientada a objetos como la polimorfia. Te dejo un link de referencia por si quieres estudiar mas sobre esto
//------------> https://gameprogrammingpatterns.com/state.html
//Entre otras cosas nos hace tener un codigo mas limpio y con conocimiento de que se esta ejecutando en cada momento


//Si ves algo que no te suena dime

using UnityEngine;

public enum PlayerStatesEnum{PlayerJumping, PlayerGrounded}
public abstract class PlayerStates
{
    public virtual void Tick(){

    }
    public virtual void OnBegin(){

    }
    public virtual void OnEnd(){

    }

}

public  class PlayerJumping : PlayerStates
{
    public override void OnBegin()
    {
        base.OnBegin();
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
public  class PlayerGrounded : PlayerStates
{
    public override void OnBegin()
    {
        base.OnBegin();
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }

    public override void Tick()
    {
        base.Tick();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}