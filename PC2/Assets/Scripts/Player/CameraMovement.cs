using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    PlayerMovement playerMove;
    PlayerJump playerJump;
    GameObject player;
    SpriteRenderer sprite;
    Camera cam;
    //SpeedEffect speed;
    /*public ParticleSystem shortRight;
    public ParticleSystem midRight;
    public ParticleSystem longRight;
    public ParticleSystem shortLeft;
    public ParticleSystem midLeft;
    public ParticleSystem longLeft;*/

    [Header("Camera Stats")]
    public float damping;
    public float longFallDamping;
    public float lookAhead;
    public float standardY;
    public float fallingY;
    public float deadZoneX;
    public float deadZoneY;
    public float airSpeedDeadZoneX;
    public float trainLookAhead;
    public Vector3 standardCamSize;
    public Vector3 trainCamSize;
    public float sizeDamping;

    public float walkBuffer;
    public float landStopBuffer;
    public float maxSize;
    public float minSize;
    public float momentumLookAhead;
    public float speedEffect;
    public float longFallDistance;
    [HideInInspector] public float additionalY;

    [Header("Internal Math")]
    public Vector3 velocity = Vector3.zero;
    public Vector3 offset = new Vector3(0, 0, -10);
    public Vector3 trainOffset;
    //public Train train;
    public Vector3 camSize;
    public Vector3 sizeOffset;

    public float targetY;
    public float groundedTimerRight;
    public float groundedTimerLeft;
    public float groundedTimerStop;

    public bool playingRight;
    public bool playingLeft;
    public bool longFall;

    public bool locked;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        playerMove = player.GetComponent<PlayerMovement>();
        playerJump = player.GetComponent<PlayerJump>();
        sprite = player.GetComponent<SpriteRenderer>();
        camSize = new Vector3(14, 0, 0);
        cam = GetComponent<Camera>();
        groundedTimerRight = 0;
        groundedTimerLeft = 0;
        groundedTimerStop = 0;
        //speed = GetComponentInChildren<SpeedEffect>();

        /*shortRight.Stop();
        midRight.Stop();
        longRight.Stop();
        shortLeft.Stop();
        midLeft.Stop();
        longLeft.Stop();*/
    }

    // Update is called once per frame
    /*void FixedUpdate()
    {
        offset.y = standardY;
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);

        //Vector3 targetCamSize = new Vector3(cam.orthographicSize, 0, 0) + sizeOffset;
        //camSize = Vector3.SmoothDamp(camSize, targetCamSize, ref velocity, sizeDamping);
        camSize = standardCamSize + sizeOffset;
        cam.orthographicSize = camSize.x;

        if (playerMove.momentum > 0.1 || playerMove.momentumToAdd > 0.1)
        {
            Debug.Log("Hey");
            offset.x = trainLookAhead;
            //sizeOffset = trainCamSize;
        }
        else if (playerMove.momentum < -0.1 || playerMove.momentumToAdd < -0.1)
        {
            offset.x = -trainLookAhead;
            //sizeOffset = trainCamSize;
        }
        else
        {
            //targetCamSize = standardCamSize;
            //sizeOffset = Vector3.zero;
            if (playerMove.onGround)
            {
                GroundedCamera(); //Checks for when on ground
            }
            else
            {
                AirbornCamera();
            }
        }
    }*/

    void FixedUpdate()
    {
        if (!locked)
        {
            Vector3 targetPosition = new Vector3(target.position.x + offset.x, targetY + additionalY, -10);
            float dampValue = damping;
            if (longFall)
            {
                dampValue = longFallDamping;
            }
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampValue);
        }

        float Ydistance = target.position.y + standardY - transform.position.y;
        if (-Ydistance > longFallDistance || longFall)
        {
            targetY = target.position.y + fallingY;
            longFall = true;
        }
        if (playerMove.onGround)
        {
            longFall = false;
            targetY = target.position.y + standardY;
            if (playerMove.velocity.x > 0.1)
            {
                groundedTimerLeft = 0;
                if (groundedTimerRight >= walkBuffer) //If we're walking for long enough to activate lookahead
                {
                    if (playerMove.momentum > 0)
                    {
                        offset.x = momentumLookAhead;
                    }
                    else
                    {
                        offset.x = lookAhead;
                    }
                }
                else //If the timer hasn't expired yet
                {
                    groundedTimerRight += Time.deltaTime;
                }
            }
            else if (playerMove.velocity.x < -0.1)
            {
                groundedTimerRight = 0;
                if (groundedTimerLeft >= walkBuffer) //If we're walking for long enough to activate lookahead
                {
                    if (playerMove.momentum < 0)
                    {
                        offset.x = -momentumLookAhead;
                    }
                    else
                    {
                        offset.x = -lookAhead;
                    }
                }
                else //If the timer hasn't expired yet
                {
                    groundedTimerLeft += Time.deltaTime;
                }
            }
            else //If we're standing still
            {
                groundedTimerLeft = 0;
                groundedTimerRight = 0;
                if (groundedTimerStop >= landStopBuffer) //If we're stopping for long enough to activate lookahead
                {
                    offset.x = 0;
                }
                else //If the timer hasn't expired yet
                {
                    groundedTimerStop += Time.deltaTime;
                }
            }
        }
        if (playerMove.transform.parent != null)
        {
            if (playerMove.momentumToAdd > 0)
            {
                offset += new Vector3(4, 0, 0);
                //speed.speed = -speedEffect;
            }
            else
            {
                offset -= new Vector3(4, 0, 0);
                //speed.speed = speedEffect;
            }

            if (cam.orthographicSize < maxSize)
            {
                cam.orthographicSize += Time.deltaTime;
            }
            else
            {
                cam.orthographicSize = maxSize;
            }
        }
        else
        {
            if (cam.orthographicSize > minSize)
            {
                cam.orthographicSize -= Time.deltaTime;
            }
            else
            {
                cam.orthographicSize = minSize;
            }
        }
        if (playerMove.transform.parent != null && playerMove.momentumToAdd > 2 || playerMove.momentum > 2 || playerMove.momentumToAdd > 2)
        {
            if (!playingRight)
            {
                //shortRight.Play();
                //midRight.Play();
                //longRight.Play();
                playingRight = true;
                playingLeft = false;
            }
            //shortLeft.Stop();
            //midLeft.Stop();
            //longLeft.Stop();
            //shortLeft.Clear();
            //midLeft.Clear();
            //longLeft.Clear();
            //Debug.Log("right plays");
        }
        else if (playerMove.transform.parent != null && playerMove.momentumToAdd < -2 || playerMove.momentum < -2 || playerMove.momentumToAdd < -2)
        {
            if (!playingLeft)
            {
                //shortLeft.Play();
                //midLeft.Play();
                //longLeft.Play();
                playingLeft = true;
                playingRight = false;
            }
            //shortRight.Stop();
            //midRight.Stop();
            //longRight.Stop();
            //shortRight.Clear();
            //midRight.Clear();
            //longRight.Clear();
            //Debug.Log("left plays");
        }
        else
        {
            //shortRight.Stop();
            //midRight.Stop();
            //longRight.Stop();
            //shortLeft.Stop();
            //midLeft.Stop();
            //longLeft.Stop();
            //shortRight.Clear();
            //midRight.Clear();
            //longRight.Clear();
            //shortLeft.Clear();
            //midLeft.Clear();
            //longLeft.Clear();
            playingLeft = false;
            playingRight = false;
            //Debug.Log("no plays");
        }
    }




    private void GroundedCamera() //When you are currently Fabian
    {
        if (playerMove.directionX == 0) //If on the ground and not inputting direction
        {
            offset.x = 0;
        }
        if (playerMove.directionX == 1) //If on the ground and inputting right
        {
            offset.x = lookAhead;
        }
        else if (playerMove.directionX == -1) //If on the ground and inputting left
        {
            offset.x = -lookAhead;
        }
    }
    private void AirbornCamera()
    {
        if (playerMove.velocity.x > -airSpeedDeadZoneX && playerMove.velocity.x < airSpeedDeadZoneX) //If in the sky and going right (with deadzone)
        {
            offset.x = 0;
        }
        if (playerMove.velocity.x > airSpeedDeadZoneX) //If in the sky and going right (with deadzone)
        {
            offset.x = lookAhead;
        }
        if (playerMove.velocity.x < -airSpeedDeadZoneX) //If in the sky and going left (with deadzone)
        {
            offset.x = -lookAhead;
        }
    }

    public void Lock(Vector3 pos)
    {
        locked = true;
        transform.position = pos;
    }

    public void Unlock(Vector3 pos)
    {
        locked = false;
        targetY = pos.y + standardY;
        transform.position = pos;
    }
}
