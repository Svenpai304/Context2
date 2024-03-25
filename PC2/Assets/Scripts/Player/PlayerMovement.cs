using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IAbility
{
    Rigidbody2D rb;
    GroundDetect groundDetect;
    BoxCollider2D coll;
    SpriteRenderer sprite;
    Animator anim;

    [Header("Movement Stats")]
    public float maxSpeed; //0 to 20 DETERMINES HOW FAST YOU CAN GO IN TOTAL
    public float maxAcceleration; //the rest is 0 to 100
    public float maxAirAcceleration;
    public float maxDeceleration;
    public float maxAirDeceleration;
    public float maxTurnSpeed;
    public float maxAirTurnSpeed;

    [Header("Platform parenting")]
    public float extraHeight;
    public LayerMask movingPlatformMask;

    [Header("Internal math shit")]
    public Vector2 moveInput;
    public float directionX;
    public Vector2 desiredVelocity; //When landing on normal ground this should be set back to maxSpeed. This can become negative
    public bool onGround;
    public float acceleration;
    public float deceleration;
    public float turnSpeed;
    public Vector2 velocity;
    public float maxSpeedChange;

    [Header("Momentum additions")]
    public float momentum;
    //public TrainFront currentTrain;
    public float momentumToAdd = 0;
    public bool leavingTrain;
    public LayerMask wallMask;
    public float aboveGroundDistance; //Moves the raycasts used for wall detection off the floor

    [Header("Sounds")]
    public AudioSource skidSound;
    public bool playSkid;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetect = GetComponent<GroundDetect>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        directionX = Mathf.Round(ctx.ReadValue<Vector2>().x); //Boils down movement inputs to left right or no
    }

    public void Disable()
    {
        enabled = false;
        rb.velocity = Vector2.zero;
        directionX = 0;
    }

    public void Enable()
    {
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        ParentToPlatform();
        onGround = groundDetect.GetOnGround(); //Ground check
        //TrainBehaviour(); //Detects and saves the speed of any train you stand on. Also deletes any memory of them when leaving except for the speed that is yet to be applied. It gives the signal to FixedUpdate to start applying that speed
        if (!onGround && momentumToAdd != 0)  //When there's momentum about to be added to your character
        {
            //Here it can become negative
            momentum = momentumToAdd; //copy the momentum to a new variable that will stick around longer and you can manipulate
        }
        //This check accounts for it being negative and actively works with that
        /*if (Mathf.Sign(directionX) != Mathf.Sign(momentum)) //If going against the movement of a platform
        {
            //This check and what we do after are wrong for some reason
            desiredVelocity = new Vector2(directionX, 0f) * (maxSpeed - Mathf.Abs(momentum)); //How fast do we want to go in total? Mathf.abs is used because maxSpeed is never negative
        }
        else //If going with the movement of a platform
        {
            desiredVelocity = new Vector2(directionX, 0f) * (maxSpeed + Mathf.Abs(momentum)); //How fast do we want to go in total?
        }*/
        if (Mathf.Sign(momentum) == 1) //If platform goes positive
        {
            if (directionX > 0) //if platform is positive AND you are positive
            {
                desiredVelocity = new Vector2(directionX * maxSpeed + momentum, 0f); //1 * 5 = 5. 10 + 5 = 15
            }
            else if (directionX < 0) //if platform is positive BUT you are negative. Momentum should counteract you resulting in less positive number
            {
                desiredVelocity = new Vector2(directionX * maxSpeed + momentum, 0f); //-1 * 5 = -5. 10 + -5 = 5
            }
            else
            {
                desiredVelocity = new Vector2(momentum, 0f); //If the input is nothing just move at the platform's speed
            }
        }
        if (Mathf.Sign(momentum) == -1) //Pretending the momentum is plus or minus 10 and your speed 5
        {
            if (directionX > 0) //if platform is negative BUT you are positive. Momentum should counteract you resulting in less negative number
            {
                desiredVelocity = new Vector2(directionX * maxSpeed + momentum, 0f); //1 * 5 = 5. -10 + 5 = -5
            }
            else if (directionX < 0) //if platform is negative AND you are negative
            {
                desiredVelocity = new Vector2(directionX * maxSpeed + momentum, 0f); //-1 * 5 = -5. -10 + -5 = -15
            }
            else
            {
                desiredVelocity = new Vector2(momentum, 0f); //If the input is nothing just move at the platform's speed
            }
        }
        Animations();
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity; //Get current velocity. Not in the webpage but absolutely needed. It makes sure we don't get a Y speed of 0 mid jump/ Just like jump we're calculating using current speeds
        onGround = groundDetect.GetOnGround(); //Ground check so we know wether to use the ground acceleration stuff or the air versions
        SwapLandAir(); //When on the ground we use ground acceleration. When in air the variables get swapped out
        MomentumReduction();
        CalculateMaxSpeedChange(); //Basically automatically calculates what to do with the various acceleration variables. Makes you go faster or slower faster or slower. Not relevant for momentum!!
        



        if (!leavingTrain) //Check if we're currently leaving a train. If so, the momentum of the train should be added
        {
            if (transform.parent != null && momentum != 0)
            {
                
                
                
                    velocity.x = Mathf.MoveTowards(velocity.x - momentum, desiredVelocity.x, maxSpeedChange); //This version happens when landing on a moving platform. It deletes all your added momentum and then whipes momentum data
                    momentum = 0;
                

            }
            
                velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange); //Your actual speed moves towards your chosen maximum speed at the rate of whatever acceleration is on now. For momentum only velocity.x and desiredvelocity change
            

        }
        else
        {
            //velocity.x = 0;  //Prevents the player from moving a little to the right after a neutral jump in exchange for needing to build up speed again...
            
                velocity.x = Mathf.MoveTowards(velocity.x + momentumToAdd, desiredVelocity.x, maxSpeedChange); //Same thing but we're adding the train's speed too. This one isn't abs because it alters velocity directly which can go negative
            

            leavingTrain = false; //Prevent the system from doing it again until we jump off another moving train
            momentumToAdd = 0; //Prevent the speed from being used multiple times
        }
        rb.velocity = velocity; //Input not just the X speed but also the Y speed to the player rigidbody. Because we read the Y we don't input 0
    }





    private void SwapLandAir()
    {
        acceleration = onGround ? maxAcceleration : maxAirAcceleration; //Some switches that swap between land and air variables
        deceleration = onGround ? maxDeceleration : maxAirDeceleration;
        turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;
    }
    private void CalculateMaxSpeedChange()
    {
        if (directionX != 0) //If we're trying to move
        {
            if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x)) //If your calculated velocity is not on the same side of a 0
            {
                maxSpeedChange = turnSpeed * Time.deltaTime; //Set your speedchange to turnspeed instead of normal acceleration
            }
            else
            {
                maxSpeedChange = acceleration * Time.deltaTime; //If you are going the way you intend then use normal acceleration
            }
        }
        else //If you're trying to stop moving
        {
            maxSpeedChange = deceleration * Time.deltaTime;
        }
    }
    //private void TrainBehaviour()
    //{
        //if (transform.parent != null) //Are we on a moving platform?
        //{
            //if (currentTrain == null) //Is the current platform/train not registered yet?
            //{
            //    currentTrain = transform.parent.gameObject.GetComponent<TrainFront>(); //Register the train
            //    momentumToAdd = currentTrain.velocity.x; //Register the speed of the train
            //}
        //}
        //else //If we're not on a train
        //{
            //if (currentTrain != null) //If there's still a train registered
            //{
            //    leavingTrain = true; //We're ditching this train
            //}
            //currentTrain = null; //Tell the system to forget that train ever existed
        //}
    //}
    private void ParentToPlatform()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, extraHeight, movingPlatformMask);
        if (raycastHit.collider != null)
        {
            transform.parent = raycastHit.collider.transform;
        }
        else
        {
            transform.parent = null;
        }
    }
    private void MomentumReduction()
    {
        if (momentum != 0 && directionX != Mathf.Sign(momentum) && directionX != 0) //Whenever I have momentum, and I'm inputting the opposite direction
        {
            if (Mathf.Sign(momentum) == 1 && Mathf.Sign(velocity.x) == 1) //If the momentum is positive
            {
                momentum = velocity.x - maxSpeed; //My new momentum is my current speed - my maxSpeed. That's turning 10 into speed - 12
            }
            if (Mathf.Sign(momentum) == -1 && Mathf.Sign(velocity.x) == -1) //If the momentum is negative
            {
                momentum = velocity.x + maxSpeed; //My new momentum is my current speed + my maxSpeed. That's turning -10 into speed + 12
            }
        }
        if (momentum != 0 && transform.parent == null && onGround && directionX != Mathf.Sign(momentum)) //When landing on normal ground but going opposite of momentum
        {
            momentum = 0; //Delete momentum
        } //So the intent must have been to let momentum remain the same when airborn going against a train. It should return to 0 when landing so you can rebuild speed
        //Then the rest must be exclusively for trying to change direction a little bit. So I'm going 

        RaycastHit2D raycastHitBottomRight = Physics2D.BoxCast(coll.bounds.center - new Vector3(0, coll.bounds.size.y / 2 - aboveGroundDistance, 0), coll.bounds.size, 0f, Vector2.right, extraHeight, wallMask);
        if (raycastHitBottomRight.collider != null)
        {
            Debug.Log("Bottom right");
            momentum = 0;
        }
        RaycastHit2D raycastHitTopRight = Physics2D.BoxCast(coll.bounds.center + new Vector3(0, coll.bounds.size.y / 2, 0), coll.bounds.size, 0f, Vector2.right, extraHeight, wallMask);
        if (raycastHitTopRight.collider != null)
        {
            Debug.Log("Top right");
            momentum = 0;
        }
        RaycastHit2D raycastHitBottomLeft = Physics2D.BoxCast(coll.bounds.center - new Vector3(0, coll.bounds.size.y / 2 - aboveGroundDistance, 0), coll.bounds.size, 0f, Vector2.left, extraHeight, wallMask);
        if (raycastHitBottomRight.collider != null)
        {
            Debug.Log("Bottom left");
            momentum = 0;
        }
        RaycastHit2D raycastHitTopLeft = Physics2D.BoxCast(coll.bounds.center + new Vector3(0, coll.bounds.size.y / 2, 0), coll.bounds.size, 0f, Vector2.left, extraHeight, wallMask);
        if (raycastHitTopRight.collider != null)
        {
            Debug.Log("Top left");
            momentum = 0;
        }
    }

    #region unused interface bits
    public void Jump(InputAction.CallbackContext c)
    {
        //not used
    }

    public void Interact(InputAction.CallbackContext c)
    {
        //not used
    }

    #endregion
    private void Animations()
    {

        anim.SetBool("Slide", false);
        if (onGround && velocity.x < 0) //If we're moving on the ground
        {
            anim.SetBool("Stand", false);
            sprite.flipX = true; //Flip depending on direction
            if (directionX == Mathf.Sign(velocity.x))
            {
                anim.SetBool("Skid", false);
                anim.SetBool("Run", true); //If not skidding run
                playSkid = true;
            }
            else
            {
                anim.SetBool("Skid", true);
                if (playSkid)
                {
                    skidSound.Play();
                    playSkid = false;
                }
                anim.SetBool("Run", false);
            }
        }
        else if (onGround && velocity.x > 0)
        {
            anim.SetBool("Stand", false);
            sprite.flipX = false;
            anim.SetBool("Run", true);
            if (directionX == Mathf.Sign(velocity.x))
            {
                anim.SetBool("Skid", false);
                anim.SetBool("Run", true); //If not skidding run
                playSkid = true;
            }
            else
            {
                anim.SetBool("Skid", true);
                if (playSkid)
                {
                    skidSound.Play();
                    playSkid = false;
                }
                anim.SetBool("Run", false);
            }
        }
        else
        {
            anim.SetBool("Skid", false);
            anim.SetBool("Run", false);

            anim.SetBool("Stand", true);
            playSkid = true;
        }
        if (!onGround)
        {
            playSkid = true;
            anim.SetBool("Stand", false);
            anim.SetBool("Run", false);
            anim.SetBool("Skid", false);
            if (Mathf.Sign(velocity.y) == 1)
            {
                anim.SetBool("Jump", true);
                anim.SetBool("Fall", false);
            }
            else
            {
                anim.SetBool("Jump", false);
                anim.SetBool("Fall", true);
            }
        }
        else
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
        }

    }
}
