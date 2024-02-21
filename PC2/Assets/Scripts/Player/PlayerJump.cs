using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour, IAbility
{
    Rigidbody2D rb;
    GroundDetect groundDetect;

    [Header("PlayerJump Stats")]
    public float jumpHeight; //Typically between 0 and 5
    public float timeToJumpApex; //Typically between 0.2 and 2.5
    public float upwardMovementMultiplier = 1;
    public float downwardMovementMultiplier; //Typically between 1 and 10
    public float jumpCutOff; //THIS IS A GRAVITY MULTIPLIER
    public float coyoteTime; //How many seconds until you can't jump anymore when falling off a ledge
    public float jumpBuffer;

    [Header("Internal maths")]
    public bool onGround;
    public bool desiredJump;
    public float gravMultiplier;
    public float jumpSpeed;
    public Vector2 velocity;

    [Header("Variable Jump Additions")]
    public bool pressingJump;
    public bool currentlyJumping;
    //JUMPCUTOFF

    [Header("Coyote Time Additions")]
    //COYOTE TIME
    public float coyoteTimeCounter;

    [Header("Jump Buffer additions")]
    public float jumpBufferCounter;
    //JUMPBUFFER

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundDetect = GetComponent<GroundDetect>();
    }
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            desiredJump = true; //Signal that we want to jump ASAP
            pressingJump = true;
        }
        if (ctx.canceled)
        {
            pressingJump = false; //When we let go of jump we signal that the jump button is not pushed right now
        }

    }
    public void Disable()
    {
        enabled = false;
    }

    public void Enable()
    {
        enabled = true;
    }

    private void Update()
    {

        JumpBuffer();
        onGround = groundDetect.GetOnGround(); //Ground check
        if (!currentlyJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = 0;
        }
        CalculateGravityScale(); //Changes gravity scale making you fall at different speeds
    }

    private void FixedUpdate()
    {
        if (onGround)
        {
            currentlyJumping = false;
        }
        velocity = rb.velocity; //Reads the current speed we're shmoving at to make new calculations with
        if (desiredJump)
        {
            PerformJump(); //Resets jump preparations and calculates a new Y speed to jump with
            rb.velocity = velocity; //Applies new Y speed as well as the X that was read earlier
            currentlyJumping = true; //Tells the code we're jumping now. Used for variable height
            return;
        }
        //if (!onGround && rb.velocity.y > 0.01f) //DID ADDING A GROUND CHECK HERE FIX IT??????
        //{
        //    if (pressingJump && currentlyJumping)
        //    {
        //        gravMultiplier = 1f;
        //    }
        //    else
        //    {
        //        gravMultiplier = jumpCutOff;
        //    }
        //}
        //SwapMultiplier();
        calculateGravity();
    }





    private void PerformJump()
    {
        if ((onGround && velocity.y > -0.1) || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime)) //If grounded or if you still have coyote time
        {
            desiredJump = false;
            jumpBufferCounter = 0;
            velocity.y = 0; //Very brute force fix for super jump I guess...
            CalculateJump();
            velocity.y += jumpSpeed; //Swaps Y speed for the newly calculated one in CalculateJump()
        }
        if (jumpBuffer == 0)
        {
            desiredJump = false;
        }
    }
    public void CalculateGravityScale()
    {
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        rb.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier; //Sets gravity scale based on things. THIS IS CAUSING PROBLEMS
    }
    public void SwapMultiplier() //Sets gravity multiplier depending on your move speed
    {
        if (rb.velocity.y == 0)
        {
            gravMultiplier = 1;

        }
        if (rb.velocity.y < -0.01f) { gravMultiplier = downwardMovementMultiplier; }
    }
    public void CalculateJump()
    {
        jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * rb.gravityScale * jumpHeight);
        if (velocity.y > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
        }
        else if (velocity.y < 0f)
        {
            jumpSpeed += Mathf.Abs(rb.velocity.y);
        }
    }
    private void JumpBuffer()
    {
        if (desiredJump)
        {
            jumpBufferCounter += Time.deltaTime;
            if (jumpBufferCounter > jumpBuffer)
            {
                desiredJump = false;
                jumpBufferCounter = 0;
            }
        }
    }
    private void calculateGravity()
    {
        //We change the character's gravity based on her Y direction

        //If Kit is going up...
        if (rb.velocity.y > 0.01f)
        {
            if (onGround)
            {
                //Don't change it if Kit is stood on something (such as a moving platform)
                gravMultiplier = 1;
            }
            else
            {
                //Apply upward multiplier if player is rising and holding jump
                if (pressingJump && currentlyJumping)
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
                //But apply a special downward multiplier if the player lets go of jump
                else
                {
                    gravMultiplier = jumpCutOff;
                }
            }
        }

        //Else if going down...
        else if (rb.velocity.y < -0.01f)
        {

            if (onGround)
            //Don't change it if Kit is stood on something (such as a moving platform)
            {
                gravMultiplier = 1;
            }
            else
            {
                //Otherwise, apply the downward gravity multiplier as Kit comes back to Earth
                gravMultiplier = downwardMovementMultiplier;
            }

        }
        //Else not moving vertically at all
        else
        {
            if (onGround)
            {
                currentlyJumping = false;
            }

            gravMultiplier = 1;
        }

        //Set the character's Rigidbody's velocity
        //But clamp the Y variable within the bounds of the speed limit, for the terminal velocity assist option
        //rb.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }

    #region unused interface bits
    public void Move(InputAction.CallbackContext c)
    {
        //not used
    }

    public void Interact(InputAction.CallbackContext c)
    {
        //not used
    }
    #endregion
}
//Superjump still seems to happen on occasion...
//Perhaps it's because some things happen on update and some things happen on fixedupdate. Maybe at some point update reads or writes data exactly between when fixedupdate should change it and has changed it
//Tested the theory and it is false. I have however discovered something:
//With the current way that the scene is built, the player always triggers the conditions for a super jump right when they land on the ground.
//You can see gravity multiplier cycle between 6 and 1.1. The 1.1 is there from the excessive gravity multiplier checks I did in an effort to fight the 6.
//You can also see that the y velocity swings wildly between 0.1 and -0.1.
//I'm removing the extra gravity multiplier checks to see if I can find out more.
//Update: It only becomes 6 when the Y velocity is positive