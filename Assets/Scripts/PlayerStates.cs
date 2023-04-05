// Machine que controla los ditintos estados del usuario, de esta forma podemos correr codigo que sea unico para cada estado. Esto tiene varias ventajas
//y aprovecha principios de la programacion orientada a objetos como la polimorfia. --------> https://gameprogrammingpatterns.com/state.html
/* Imports */
using UnityEngine;

/* Enumeracion PlayerStatesEnum */
public enum PlayerStatesEnum
{
    //Estados del player
    PlayerJumping,
    PlayerGrounded
}

/* Clase  PlayerStates*/
public abstract class PlayerStates
{
    /* Atributos */
    protected PlayerController controller;

    /* Constructor */
    protected PlayerStates(PlayerController controller)
    {
        this.controller = controller;
    }

    /* Métodos */
    /* Método Tick */
    public virtual void Tick()
    {
        //Movimiento horizontal
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

/* Clase  PlayerJumping */
public class PlayerJumping : PlayerStates
{
    /* Atributos */
    //Hashes para las animaciones
    public int groundedHash = Animator.StringToHash("Grounded");
    public int jumpHash = Animator.StringToHash("Jump");
    //Fuerza de salto
    private int jumpForce = 4;
    //Tiempo de salto
    private float jumpingTime;
    //Numero de saltos
    public int numberOfJumps;
    //Capa de suelo
    public int groundLayer = LayerMask.GetMask("Ground");

    //Constructor
    public PlayerJumping(PlayerController controller) : base(controller)
    {
    }

    //Métodos
    /* Método FixedTick */
    public override void FixedTick()
    {
        //Movimiento vertical
        base.FixedTick();
        float movementAmount = Mathf.Clamp(Input.GetAxis("Vertical") * 20, int.MinValue, 0);
        base.controller.Rb.AddForce(Vector2.up * (movementAmount));
    }

    /* Método OnBegin */
    public override void OnBegin()
    {
        //Reiniciamos el numero de saltos
        numberOfJumps = 0;
        jumpingTime = 0;
        DoubleJump();
    }

    /* Método OnEnd */
    public override void OnEnd()
    {
        base.OnEnd();
    }

    /* Método Tick */
    public override void Tick()
    {
        //Comprobamos si el jugador esta en el suelo
        base.Tick();
        jumpingTime += Time.deltaTime;
        //Si el jugador pulsa la tecla W, saltamos
        if (Input.GetKeyDown(KeyCode.W)) DoubleJump();
        // Si el tiempo de salto es inferior o igual a 0.2f, el código no se ejecutará 
        //Esto sirve para que el jugador no pueda saltar infinitamente
        if (jumpingTime <= 0.2f) return;
        //Raycast para comprobar si el jugador esta en el suelo
        RaycastHit2D hit = Physics2D.Raycast((base.controller.PlayerFeet.position), -base.controller.Tr.up, 10, groundLayer);
        //Debug
        Debug.DrawRay((base.controller.PlayerFeet.position),-base.controller.Tr.up,Color.blue,1);
        //Si el raycast ha colisionado con el suelo, cambiamos el estado a PlayerGrounded
        if (hit)
        {
            if (hit.distance <= 1) //0 es un valor demasiado bajo para detectar la colision
            {
                base.controller.ChangeState(PlayerStatesEnum.PlayerGrounded);
                base.controller.AnimationController.SetTrigger(groundedHash);
            }
        }
    }

    /* Método DoubleJump */
    private void DoubleJump()
    {
        //Si el numero de saltos es mayor que 2, el código no se ejecutará
        if (numberOfJumps > 2) return;
        
        base.controller.Rb.velocity = Vector2.zero;
        //Animacion de salto
        base.controller.AnimationController.SetTrigger(jumpHash);
        //Aumentamos el numero de saltos
        numberOfJumps++;
        //Se ejecuta el salto
        base.controller.Rb.AddForce(Vector2.up * (jumpForce), ForceMode2D.Impulse);
    }

    /* Método ToString */
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

    /* Constructor */
    public PlayerGrounded(PlayerController controller) : base(controller)
    {
    }

    /* Métodos */
    /* Método OnBegin */
    public override void OnBegin()
    {
        base.OnBegin();
    }

    /* Método OnEnd */
    public override void OnEnd()
    {
        base.OnEnd();
    }

    /* Método Tick */
    public override void Tick()
    {
        //Movimiento horizontal
        base.Tick();
        float movementAmount = Input.GetAxis("Horizontal");
        //Si el jugador se mueve horizontamente se inicia la animacion de correr junto con el sistema de particulas
        if (movementAmount != 0)
        {
            base.controller.AnimationController.SetBool(runningAnimHash, true);

            base.controller.StartParticleSystem();
        }
        else //Si el jugador no se mueve, se detiene la animacion de correr junto con el sistema de particulas
        {
            base.controller.AnimationController.SetBool(runningAnimHash, false);
            base.controller.StopParticleSystem();
        }
        
        if (Input.anyKey) ProcessInput(movementAmount);
    }

    /* Método ProcessInput */
    private void ProcessInput(float movementAmount)
    {
        //Si el jugador pulsa la tecla W, cambiamos el estado a PlayerJumping
        if (Input.GetKeyDown(KeyCode.W))
        {
            base.controller.ChangeState(PlayerStatesEnum.PlayerJumping);
        }
        //Si el jugador pulsa la tecla A o D, cambiamos la direccion del jugador   
        if (movementAmount != 0)
        {
            base.controller.SwitchPlayerDirection(movementAmount > 0);
        }
    }

    /* Método ToString */
    public override string ToString()
    {
        return base.ToString();
    }
}
