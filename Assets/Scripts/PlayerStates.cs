// Machine que controla los ditintos estados del usuario, de esta forma podemos correr codigo que sea unico para cada estado. Esto tiene varias ventajas
//y aprovecha principios de la programacion orientada a objetos como la polimorfia. --------> https://gameprogrammingpatterns.com/state.html
/* Imports */
using UnityEngine;

/* Enumeracion PlayerStatesEnum */
public enum PlayerStatesEnum
{
    PlayerJumping,
    PlayerGrounded
}

/* Clase  PlayerStates*/
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
        controller.Tr.position +=
            Vector3.right * (movementAmount * Time.deltaTime);
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

/* Clase  PlayerJumping */
public class PlayerJumping : PlayerStates
{
    /* Atributos */
    public int groundedHash = Animator.StringToHash("Grounded");

    public int jumpHash = Animator.StringToHash("Jump");

    private int jumpForce = 4;

    private float jumpingTime;

    public int numberOfJumps;

    public int groundLayer = LayerMask.GetMask("Ground");

    /* Salto */
    public PlayerJumping(PlayerController controller) :
        base(controller)
    {
    }

    /* FixedTick */
    public override void FixedTick()
    {
        base.FixedTick();
        float movementAmount =
            Mathf.Clamp(Input.GetAxis("Vertical") * 20, int.MinValue, 0);
        base.controller.Rb.AddForce(Vector2.up * (movementAmount));
    }

    /* OnBegin */
    public override void OnBegin()
    {
        numberOfJumps = 0;
        jumpingTime = 0;
        DoubleJump();
    }

    /* OnEnd */
    public override void OnEnd()
    {
        base.OnEnd();
    }

    /* Tick */
    public override void Tick()
    {
        base.Tick();
        jumpingTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.W)) DoubleJump();
        if (jumpingTime <= 0.3f) return; // No ground Check needed
        RaycastHit2D hit =
            Physics2D
                .Raycast((base.controller.PlayerFeet.position),
                -base.controller.Tr.up,
                10,
                groundLayer);
        Debug
            .DrawRay((base.controller.PlayerFeet.position),
            -base.controller.Tr.up,
            Color.blue,
            1);
        if (hit)
        {
            if (hit.distance <= 0)
            {
                base.controller.ChangeState(PlayerStatesEnum.PlayerGrounded);
                base.controller.AnimationController.SetTrigger(groundedHash);
            }
        }
    }

    /* DoubleJump */
    private void DoubleJump()
    {
        if (numberOfJumps > 2) return;
        base.controller.Rb.velocity = Vector2.zero;
        base.controller.AnimationController.SetTrigger(jumpHash);
        numberOfJumps++;
        base.controller
            .Rb
            .AddForce(Vector2.up * (jumpForce), ForceMode2D.Impulse);
    }

    /* ToString */
    public override string ToString()
    {
        return base.ToString();
    }
}

/* Clase PlayerGrounded */
public class PlayerGrounded : PlayerStates
{
    /* Atributos */
    int runningAnimHash = Animator.StringToHash("Running");

    /* PlayerGrounded */
    public PlayerGrounded(PlayerController controller) :
        base(controller)
    {
    }

    /* OnBegin */
    public override void OnBegin()
    {
        base.OnBegin();
    }

    /* OnEnd */
    public override void OnEnd()
    {
        base.OnEnd();
    }

    /* Tick */
    public override void Tick()
    {
        base.Tick();
        float movementAmount = Input.GetAxis("Horizontal");
        if (movementAmount != 0)
        {
            base.controller.AnimationController.SetBool(runningAnimHash, true);

            base.controller.StartParticleSystem();
        }
        else
        {
            base.controller.AnimationController.SetBool(runningAnimHash, false);
            base.controller.StopParticleSystem();
        }
        if (Input.anyKey) ProcessInput(movementAmount);
    }

    /* ProcessInput */
    private void ProcessInput(float movementAmount)
    {
        if (Input.GetKeyDown(KeyCode.W))
            base.controller.ChangeState(PlayerStatesEnum.PlayerJumping);
        if (movementAmount != 0)
        {
            base.controller.SwitchPlayerDirection(movementAmount > 0);
        }
    }

    /* ToString */
    public override string ToString()
    {
        return base.ToString();
    }
}
