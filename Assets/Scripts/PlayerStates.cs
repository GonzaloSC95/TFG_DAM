// Voy a hacer una state Machine que controle los ditintos estados del usuario, de esta forma podemos correr codigo que sea unico para cada estado. Esto tiene varias ventajas 
//y aprovecha principios de la programacion orientada a objetos como la polimorfia. Te dejo un link de referencia por si quieres estudiar mas sobre esto
//------------> https://gameprogrammingpatterns.com/state.html
//Entre otras cosas nos hace tener un codigo mas limpio y con conocimiento de que se esta ejecutando en cada momento


//Si ves algo que no te suena dime

using UnityEngine;

public enum PlayerStatesEnum { PlayerJumping, PlayerGrounded }
public abstract class PlayerStates
{
    protected PlayerController controller;
    protected PlayerStates(PlayerController controller)
    {
        this.controller = controller;
    }

    public virtual void Tick()
    {
        float movementAmount = Input.GetAxis("Horizontal");
        controller.Tr.position += Vector3.right * (movementAmount * Time.deltaTime);
    }
    public virtual void OnBegin()
    {

    }
    public virtual void OnEnd()
    {

    }

    public virtual void FixedTick()
    {

    }

}

public class PlayerJumping : PlayerStates
{
       int groundedHash = Animator.StringToHash("Grounded"), jumpHash = Animator.StringToHash("Jump");
    private int jumpForce=4;
    private float jumpingTime;
    int numberOfJumps;

    public PlayerJumping(PlayerController controller) : base(controller)
    {
    }

    public override void FixedTick()
    {
        base.FixedTick();

        float movementAmount = Mathf.Clamp(Input.GetAxis("Vertical") * 20,int.MinValue,0) ;

        base.controller.Rb.AddForce(Vector2.up * (movementAmount));

    }

    public override void OnBegin()
    {
        numberOfJumps=0;
        jumpingTime=0;
        DoubleJump();
    }

    public override void OnEnd()
    {
        base.OnEnd();
    }

    public override void Tick()
    {
        base.Tick();
        jumpingTime += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.W))DoubleJump();
        if (jumpingTime <= 0.3f) return;// No ground Check needed
        RaycastHit2D hit = Physics2D.Raycast((base.controller.Tr.position - Vector3.up * 0.5f), -base.controller.Tr.up);
        if (hit)
        {
            if (hit.distance <= 0)
            {
                base.controller.ChangeState(PlayerStatesEnum.PlayerGrounded);
                 base.controller.AnimationController.SetTrigger(groundedHash);
            }
        }

    }
    private void DoubleJump(){
        if(numberOfJumps>2) return;
         base.controller.Rb.velocity=Vector2.zero;
         base.controller.AnimationController.SetTrigger(jumpHash);
        numberOfJumps++;
        base.controller.Rb.AddForce(Vector2.up * (jumpForce), ForceMode2D.Impulse);
        
    }


    public override string ToString()
    {
        return base.ToString();
    }
}
public class PlayerGrounded : PlayerStates
{
    int runningAnimHash = Animator.StringToHash("Running");
    public PlayerGrounded(PlayerController controller) : base(controller)
    {
    }
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
        float movementAmount = Input.GetAxis("Horizontal");
        if (movementAmount != 0)
        {
            base.controller.AnimationController.SetBool(runningAnimHash, true);

            base.controller.Tr.localScale = new Vector3(movementAmount / Mathf.Abs(movementAmount), 1, 1);
            base.controller.StartParticleSystem();
        }
        else
        {
            base.controller.AnimationController.SetBool(runningAnimHash, false);
            base.controller.StopParticleSystem();
        }
        if (Input.GetKeyDown(KeyCode.W)) base.controller.ChangeState(PlayerStatesEnum.PlayerJumping);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}