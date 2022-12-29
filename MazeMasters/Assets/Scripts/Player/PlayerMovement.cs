using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movementVector;

    private Rigidbody rb;
    private CapsuleCollider collider;

    bool grounded;

    public float mouseSensitivity;

    [Header("Movement Values")]
    [Space]
    public Vector2 maxVelocity;
    public float gravityScale;

    #region Ground Walk Values
    [Space]
    [Header("Ground Values")]
    [Tooltip("A value of 0/1/2 to represent different acceleration curves to test.")]
    public int groundAccelerationCurve;
    public float maxGroundMovementSpeed;
    public float groundAccelerationTime;
    [Tooltip("The time it takes to slow down the player from full speed when not holding any inputs.")]
    public float groundDecelerationTime;
    [Tooltip("Ignores acceleration curve and acceleration time and on holding an input instantly puts the player to maximum speed")]
    public bool instantGroundAcceleration;
    [Tooltip("Ignores deceleration time and stops speed instantly after no inputs are being held")]
    public bool instantGroundDeceleration;
    [Tooltip("Time it takes to turn 90 degrees")]
    public float groundTurnTime;
    [Tooltip("Turning turns the vector and doesnt slow it down")]
    public bool groundTurnTurnsVector;
    public bool groundTurnDirect;
    [Tooltip("Ignores Turn Time and keeps speed and instantly changes it to the desired direction when turning")]
    public bool instantGroundTurn;
    string groundTurnType;
    #endregion

    #region Air Values
    [Space]
    [Header("Air Values")]
    public bool allowAirControl;
    public bool fullControlInAir;
    public bool allowAirAcceleration;
    [Tooltip("A value of 0/1/2 to represent different acceleration curves to test.")]
    public int airAccelerationCurve;
    public float maxAirMovementSpeed;
    public float airAccelerationTime;
    [Tooltip("The time it takes to slow down the player from full speed when not holding any inputs.")]
    public float airDecelerationTime;
    [Tooltip("Ignores acceleration curve and acceleration time and on holding an input instantly puts the player to maximum speed")]
    public bool instantAirAcceleration;
    [Tooltip("Ignores deceleration time and stops speed instantly after no inputs are being held")]
    public bool instantAirDeceleration;
    [Tooltip("Time it takes to turn 90 degrees")]
    public float airTurnTime;
    [Tooltip("Turning turns the vector and doesnt slow it down")]
    public bool airTurnTurnsVector;
    public bool airTurnDirect;
    [Tooltip("Ignores Turn Time and keeps speed and instantly changes it to the desired direction when turning")]
    public bool instantAirTurn;
    string airTurnType;
    #endregion

    #region Crouch Values
    [Space]
    [Header("Crouch Values")]
    public bool crouchSameWalkControls;
    public float crouchAdjustFromWalkFactor;
    [Tooltip("A value of 0/1/2 to represent different acceleration curves to test.")]
    public int crouchAccelerationCurve;
    public float maxCrouchMovementSpeed;
    public float crouchAccelerationTime;
    [Tooltip("The time it takes to slow down the player from full speed when not holding any inputs.")]
    public float crouchDecelerationTime;
    [Tooltip("Ignores acceleration curve and acceleration time and on holding an input instantly puts the player to maximum speed")]
    public bool instantCrouchAcceleration;
    [Tooltip("Ignores deceleration time and stops speed instantly after no inputs are being held")]
    public bool instantCrouchDeceleration;
    [Tooltip("Time it takes to turn 90 degrees")]
    public float crouchTurnTime;
    [Tooltip("Turning turns the vector and doesnt slow it down")]
    public bool crouchTurnTurnsVector;
    public bool crouchTurnDirect;
    [Tooltip("Ignores Turn Time and keeps speed and instantly changes it to the desired direction when turning")]
    public bool instantCrouchTurn;
    public float crouchHeight;
    public float crouchSizeChangeTime;
    Coroutine sizeChange;
    string crouchTurnType;
    bool crouchHeld;
    bool crouching;
    #endregion

    #region Slide Values
    [Space]
    [Header("Slide Values")]
    [Tooltip("The speed at which crouching transitions to sliding")]
    public float slideSpeedMinimum;

    public float slideSpeedDecay;
    public float initialSlideSpeedGain;
    public float slideCancelSpeedLoss;
    bool sliding;
    bool slidingLastFrame;
    Vector3 slideDirection;
    #endregion

    #region Jump Values
    [Space]
    [Header("Jumping")]
    [Tooltip("How long before touching the ground you can press jump and still successfully jump upon touching the ground")]
    public float jumpBufferTime;
    [Tooltip("The velocity added to the player the instant they jump")]
    public float jumpInitialVelocity;
    [Tooltip("The velocity per second to add the player while holding the jump key, excluding the first frame")]
    public float jumpAdditionalVelocity;
    [Tooltip("The maximum amount of time the player can hold the jump key to get additional velocity for")]
    public float jumpHoldMaximumTime;
    bool jumpQueued = false;
    bool jumping = false;
    Coroutine jumpBuffer;
    InputAction.CallbackContext jumpContext;
    bool inJumpHoldTime;
    Coroutine JumpTimeCounter;

    public bool fallingIncreasesGravity;
    [Tooltip("Range in which gravity adjust factor takes place, x = lower y = higher")]
    public Vector2 gravityAdjustRange;
    public float gravityAdjustFactor;
    #endregion

    #region Sprint Region
    [Space]
    [Header("Sprinting")]
    public bool sprintAdjustWalk;
    public float sprintAdjustFactor;
    [Tooltip("normalized range of where sprinting is possible 1 (forwards) to -1 (backwards) min/max of direction that you can sprint at")]
    public Vector2 forwardSprintRange;
    [Tooltip("(most likely want min/max to be *-1 of eachother) normalized range of where sprinting is possible 1 (right) to -1 (left) min/max of direction that you can sprint at")]
    public Vector2 leftRightSprintRange;

    public int sprintAccelerationCurve;
    public float maxSprintMovementSpeed;
    public float sprintAccelerationTime;
    [Tooltip("Ignores acceleration curve and acceleration time and on holding an input instantly puts the player to maximum speed")]
    public bool instantSprintAcceleration;
    [Tooltip("Ignores deceleration time and stops speed instantly after no inputs are being held")]
    public bool instantSprintDeceleration;
    [Tooltip("Time it takes to turn 90 degrees")]
    public float sprintTurnTime;
    [Tooltip("Turning turns the vector and doesnt slow it down")]
    public bool sprintTurnTurnsVector;
    public bool sprintTurnDirect;
    [Tooltip("Ignores Turn Time and keeps speed and instantly changes it to the desired direction when turning")]
    public bool instantSprintTurn;
    string sprintTurnType;
    bool sprintHeld;
    bool sprinting;
    #endregion

    #region roll
    [Space]
    [Header("Roll Values")]
    public float rollSpeed;
    public float rollTime;
    public bool keepMomentum;
    public float momentumAfterRoll;
    Coroutine rollCoroutine;

    public float rollHitboxHeight;
    public bool allowAirRolling;
    public bool stopYVelocity;
    bool rolling;
    #endregion

    #region fallstun
    [Space]
    [Header("Fallstun")]
    public float fallstunApplyMin;
    public Vector2 normalYVectorRange;
    public float fallStunDuration;
    public float fallstunMovementSpeed;
    bool inFallstun;
    Coroutine fallstunCoroutine;
    #endregion

    float colliderNormalHeight;
    float colliderNormalRadius;
    bool settingSize = false;

    public void Awake()
    {

        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        ControlManager.instance.playerControls.GeneralMovement.Jump.performed += ctx => QueueJump(ctx);
        ControlManager.instance.playerControls.GeneralMovement.Crouch.performed += ctx => Crouch(ctx);
        ControlManager.instance.playerControls.GeneralMovement.Crouch.canceled += ctx => Crouch(ctx);
        ControlManager.instance.playerControls.GeneralMovement.Sprint.performed += ctx => Sprint(ctx);
        ControlManager.instance.playerControls.GeneralMovement.Sprint.canceled += ctx => Sprint(ctx);
        ControlManager.instance.playerControls.GeneralMovement.Roll.performed += ctx => Roll();

        colliderNormalHeight = transform.localScale.y;
        colliderNormalRadius = transform.localScale.z;

    }

    public void FixedUpdate()
    {
        
        if (ErrorCheck()) return;
        if (inFallstun)
        {
            Fallstun();
            return;
        }

        CheckGrounded();
        //check if crouching or sliding
        if (grounded && crouchHeld)
        {
            if (new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude >= slideSpeedMinimum)
            {
                sliding = true;
                crouching = false;
            }
            else
            {
                crouching = true;
                sliding = false;
            }
        }
        else
        {
            sliding = false;
            crouching = false;
        }
        //check if sprinting
        if (sprintHeld && !crouching && grounded)
        {
            Vector3 velocityDirection = Quaternion.Inverse(transform.rotation) * new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;
            if ((velocityDirection.z >= forwardSprintRange.x && velocityDirection.z <= forwardSprintRange.y) && (velocityDirection.x >= leftRightSprintRange.x && velocityDirection.x <= leftRightSprintRange.y))
            {
                sprinting = true;
            }
            else
            {
                sprinting = false;
            }
        }
        else
        {
            sprinting = false;
        }

        if (!grounded && !settingSize && transform.localScale.y != colliderNormalHeight && !rolling)
        {
            if (sizeChange != null) StopCoroutine(sizeChange);
            settingSize = true;
            sizeChange = StartCoroutine(SizeChange(colliderNormalHeight, colliderNormalRadius, 0.1f));
        }
        if (!rolling && !sliding) Movement();
        Slide();
        if ((jumpQueued && grounded) || jumping) Jump();

        if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude > maxVelocity.x) rb.velocity = new Vector3(0, rb.velocity.y, 0) + new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * maxVelocity.x; //set maximum x/z speed
        if (Mathf.Abs(rb.velocity.y) > maxVelocity.y) rb.velocity = new Vector3(rb.velocity.x, maxVelocity.y * Mathf.Sign(rb.velocity.y), 0); //set maximum y speed

        if (fallingIncreasesGravity && (rb.velocity.y > gravityAdjustRange.x && rb.velocity.y < gravityAdjustRange.y) && !grounded && rb.useGravity)
        {
            rb.AddForce(Physics.gravity * rb.mass * (gravityScale * gravityAdjustFactor - 1));
        }
        else
        {
            rb.AddForce(Physics.gravity * rb.mass * (gravityScale - 1));
        }
        slidingLastFrame = sliding;
    }

    private bool ErrorCheck()
    {
        bool errorsfound = false;

        if (groundAccelerationCurve != 0 && groundAccelerationCurve != 1 && groundAccelerationCurve != 2)
        {
            errorsfound = true;
            Debug.LogError("Ground Acceleration curve must be set to 0, 1 or 2");
        }
        if (groundTurnDirect && (instantGroundTurn || groundTurnTurnsVector) || (instantGroundTurn && (groundTurnDirect || groundTurnTurnsVector)))
        {
            errorsfound = true;
            Debug.LogError("multiple type of turn types are enabled");
        }
        if (gravityAdjustRange.x > gravityAdjustRange.y && fallingIncreasesGravity)
        {
            errorsfound = true;
            Debug.LogError("gravity adjust range has the values the wrong way");
        }
        return errorsfound;
    }

    public void Movement()
    {
        movementVector = ControlManager.instance.playerControls.GeneralMovement.MovementDirection.ReadValue<Vector2>();
        Vector3 direction = transform.rotation * new Vector3(movementVector.x, 0, movementVector.y);
        if (movementVector.magnitude == 0 && !sliding)
        {
            if (grounded && !crouching)
            {
                if (instantGroundDeceleration || groundDecelerationTime == 0)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    return;
                }
                float speedOffStopped = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
                Vector3 decelAmount = (new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * (maxGroundMovementSpeed / groundDecelerationTime)) * Time.deltaTime;
                rb.velocity -= Mathf.Abs(speedOffStopped) <= Mathf.Abs(decelAmount.magnitude) ? new Vector3(rb.velocity.x, 0, rb.velocity.z) : decelAmount;
            }
            if (!grounded)
            {
                if (instantAirDeceleration || airDecelerationTime == 0)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    return;
                }
                float speedOffStopped = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
                Vector3 decelAmount = (new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * (maxAirMovementSpeed / airDecelerationTime)) * Time.deltaTime;
                rb.velocity -= Mathf.Abs(speedOffStopped) <= Mathf.Abs(decelAmount.magnitude) ? new Vector3(rb.velocity.x, 0, rb.velocity.z) : decelAmount;
            }
            if (crouching)
            {
                if (crouchSameWalkControls)
                {
                    if (instantGroundDeceleration || groundDecelerationTime == 0)
                    {
                        rb.velocity = new Vector3(0, rb.velocity.y, 0);
                        return;
                    }
                    float speedOffStopped = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
                    Vector3 decelAmount = (new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * (maxGroundMovementSpeed * crouchAdjustFromWalkFactor / (groundDecelerationTime * crouchAdjustFromWalkFactor))) * Time.deltaTime;
                    rb.velocity -= Mathf.Abs(speedOffStopped) <= Mathf.Abs(decelAmount.magnitude) ? new Vector3(rb.velocity.x, 0, rb.velocity.z) : decelAmount;
                }
                else
                {
                    if (instantCrouchDeceleration || crouchDecelerationTime == 0)
                    {
                        rb.velocity = new Vector3(0, rb.velocity.y, 0);
                        return;
                    }
                    float speedOffStopped = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
                    Vector3 decelAmount = (new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * (maxCrouchMovementSpeed / crouchDecelerationTime)) * Time.deltaTime;

                    rb.velocity -= Mathf.Abs(speedOffStopped) <= Mathf.Abs(decelAmount.magnitude) ? new Vector3(rb.velocity.x, 0, rb.velocity.z) : decelAmount;
                }
            }
            return; //dont run if not holding inputs
        }
        
        if ((grounded && !crouching && !sliding) || (allowAirControl && fullControlInAir && !grounded) || (crouching && crouchSameWalkControls) || (sprinting && sprintAdjustWalk))
        {
            if (groundTurnDirect)
            {
                groundTurnType = "turnDirect";
            }
            else if (instantGroundTurn)
            {
                groundTurnType = "instantTurn";
            }
            else if (groundTurnTurnsVector)
            {
                groundTurnType = "turnTurnsVector";
            }
            if (crouching)
            {
                TurnSpeed(Accelerate(groundAccelerationCurve, groundAccelerationTime, maxGroundMovementSpeed * crouchAdjustFromWalkFactor, instantGroundAcceleration, groundDecelerationTime), groundTurnType, groundTurnTime);
            }
            else if (sprinting)
            {
                TurnSpeed(Accelerate(groundAccelerationCurve, groundAccelerationTime, maxGroundMovementSpeed * sprintAdjustFactor, instantGroundAcceleration, groundDecelerationTime), groundTurnType, groundTurnTime);
            }
            else
            {
                TurnSpeed(Accelerate(groundAccelerationCurve, groundAccelerationTime, maxGroundMovementSpeed, instantGroundAcceleration, groundDecelerationTime), groundTurnType, groundTurnTime);
            }
        }
        else if (allowAirControl && grounded == false)
        {
            if (airTurnDirect)
            {
                airTurnType = "turnDirect";
            }
            else if (instantAirTurn)
            {
                airTurnType = "instantTurn";
            }
            else if (airTurnTurnsVector)
            {
                airTurnType = "turnTurnsVector";
            }

            if (allowAirAcceleration)
            {
                TurnSpeed(Accelerate(airAccelerationCurve, airAccelerationTime, maxAirMovementSpeed, instantAirAcceleration, airDecelerationTime), airTurnType, airTurnTime);
            }
            else
            {

                TurnSpeed(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude, airTurnType, airTurnTime);
            }
        }
        else if (crouching)
        {
            if (crouchTurnDirect)
            {
                crouchTurnType = "turnDirect";
            }
            else if (instantCrouchTurn)
            {
                crouchTurnType = "instantTurn";
            }
            else if (crouchTurnTurnsVector)
            {
                crouchTurnType = "turnTurnsVector";
            }

            TurnSpeed(Accelerate(crouchAccelerationCurve, crouchAccelerationTime, maxCrouchMovementSpeed, instantCrouchAcceleration, crouchDecelerationTime), crouchTurnType, crouchTurnTime);
        }
        else if (sprinting)
        {
            if (sprintTurnDirect)
            {
                sprintTurnType = "turnDirect";
            }
            else if (instantSprintTurn)
            {
                sprintTurnType = "instantTurn";
            }
            else if (sprintTurnTurnsVector)
            {
                sprintTurnType = "turnTurnsVector";
            }
            TurnSpeed(Accelerate(sprintAccelerationCurve, sprintAccelerationTime, maxSprintMovementSpeed, instantSprintAcceleration, groundDecelerationTime), sprintTurnType, sprintTurnTime);
        }
    }

    public float Accelerate(int accelerationCurve, float accelerationTime, float maxMovementSpeed, bool instantAcceleration, float decelerationTime)
    {
        movementVector = ControlManager.instance.playerControls.GeneralMovement.MovementDirection.ReadValue<Vector2>();
        Vector3 direction = transform.rotation * new Vector3(movementVector.x, 0, movementVector.y);

        float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;

        if (speed > maxMovementSpeed)
        {
            speed = Decelerate(speed, decelerationTime, maxMovementSpeed);
            return speed;
        }
        //calculate speed then adjust speed to direction
        if (instantAcceleration)
        {
            speed = maxMovementSpeed;
            return speed;
        }

        if (accelerationCurve == 0)
        {
            float curveX = (speed / (maxMovementSpeed / accelerationTime)) + Time.deltaTime;
            speed = curveX * (maxMovementSpeed / accelerationTime);
        }
        else if (accelerationCurve == 1)
        {
            float curveX = (Mathf.Pow(speed / maxMovementSpeed, 2) * accelerationTime) + Time.deltaTime;
            speed = maxMovementSpeed * Mathf.Sqrt(curveX / accelerationTime);
        }
        else if (accelerationCurve == 2)
        {
            float curveX = Mathf.Sqrt(speed / (maxMovementSpeed / Mathf.Pow(accelerationTime, 2))) + Time.deltaTime;
            speed = maxMovementSpeed / Mathf.Pow(accelerationTime, 2) * Mathf.Pow(curveX, 2);
        }
        if (speed > maxMovementSpeed) speed = maxMovementSpeed;
        return speed;
    }

    public float Decelerate(float speed, float decelerationTime, float maxMovementSpeed)
    {
        if (decelerationTime == 0)
        {
            return 0f;
        }
        float speedOffStopped = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        speed -= maxMovementSpeed / decelerationTime * Time.deltaTime;

        return speed;
    }

    public void TurnSpeed(float speed, string turnType, float turnTime)
    {
        float initialSpeed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        movementVector = ControlManager.instance.playerControls.GeneralMovement.MovementDirection.ReadValue<Vector2>();
        Vector3 direction = transform.rotation * new Vector3(movementVector.x, 0, movementVector.y);
        if (turnType == "instantTurn")
        {

            rb.velocity = new Vector3((direction * speed).x, rb.velocity.y, (direction * speed).z);
            return;
        }
        if (turnType == "turnTurnsVector")
        {
            Vector3 angleDiff = Vector3.RotateTowards(new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized, direction, 90 / turnTime * Mathf.PI / 180 * Time.deltaTime, 0);
            if (angleDiff == Vector3.zero) angleDiff = direction;
            rb.velocity = new Vector3((angleDiff * speed).x, rb.velocity.y, (angleDiff * speed).z);
        }
        else if (turnType == "turnDirect")
        {
            float addedSpeed = Mathf.Abs(speed - initialSpeed);
            speed -= addedSpeed;
            Vector3 angleDiff = Vector3.MoveTowards(new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * speed, direction * speed, (maxGroundMovementSpeed / turnTime) * Time.deltaTime);

            Vector3 adjustAmount = addedSpeed * direction;
            rb.velocity = new Vector3(angleDiff.x, rb.velocity.y, angleDiff.z) + adjustAmount;
        }
    }

    public void Jump()
    {
        if (jumpContext.canceled) Debug.Log("jump input canceled");
        
        if (!jumping)
        {
            StopCoroutine(BufferJump());
            jumpQueued = false;
            jumping = true;

            if (JumpTimeCounter != null) StopCoroutine(JumpTimeCounter);
            JumpTimeCounter = StartCoroutine(JumpHoldTimer());

            rb.velocity += new Vector3(0, jumpInitialVelocity, 0);
        }
        else if (jumpContext.performed)
        {
            if (inJumpHoldTime)
            {
                rb.velocity += new Vector3(0, jumpAdditionalVelocity * Time.deltaTime, 0);
            }
            else
            {
                jumping = false;
            }
        }
        else
        {
            jumping = false;
            StopCoroutine(JumpTimeCounter);
        }


    }

    public IEnumerator JumpHoldTimer()
    {
        inJumpHoldTime = true;
        yield return new WaitForSeconds(jumpHoldMaximumTime);
        inJumpHoldTime = false;
    }

    public void QueueJump(InputAction.CallbackContext ctx)
    {
        jumpContext = ctx;
        if (jumpBuffer != null)
        {
            StopCoroutine("JumpBuffer");
        }
        jumpBuffer = StartCoroutine("BufferJump");
    }

    public IEnumerator BufferJump()
    {
        jumpQueued = true;
        yield return new WaitForSeconds(jumpBufferTime);
        jumpQueued = false;
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (sizeChange != null) StopCoroutine(sizeChange);
            crouchHeld = true;
            if (grounded) sizeChange = StartCoroutine(SizeChange(crouchHeight, colliderNormalRadius, crouchSizeChangeTime));
        }
        if (ctx.canceled)
        {
            if (sizeChange != null) StopCoroutine(sizeChange);
            crouchHeld = false;
            if (grounded) sizeChange = StartCoroutine(SizeChange(colliderNormalHeight, colliderNormalRadius, crouchSizeChangeTime));
        }
    } 

    public void Sprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            sprintHeld = true;
        }
        if (ctx.canceled)
        {
            sprintHeld = false;
        }
    }

    public IEnumerator SizeChange(float targetHeight, float targetRadius, float changeTime)
    {
        if (changeTime == 0)
        {
            transform.localScale = new Vector3(targetRadius, targetHeight, targetRadius);
        }
        float startingHeight = transform.localScale.y;
        float startingRadius = transform.localScale.x;
        for (; transform.localScale.y != targetHeight || transform.localScale.x != targetRadius;)
        {
            if (transform.localScale.y != targetHeight)
            {
                float amountNeeded = targetHeight - transform.localScale.y;
                float heightChange = (targetHeight - startingHeight) * Time.deltaTime / changeTime;
                transform.position += new Vector3(0, Mathf.Abs(amountNeeded) <= Mathf.Abs(heightChange) ? amountNeeded / 2 * collider.height : heightChange / 2 * collider.height, 0); ;
                transform.localScale += new Vector3(0, Mathf.Abs(amountNeeded) <= Mathf.Abs(heightChange) ? amountNeeded : heightChange, 0);
            }
            if (transform.localScale.x != targetRadius)
            {
                float amountNeeded = targetRadius - transform.localScale.x;
                float radiusChange = (targetRadius - startingRadius) * Time.deltaTime / changeTime;
               transform.localScale += new Vector3(Mathf.Abs(amountNeeded) <= Mathf.Abs(radiusChange) ? amountNeeded : radiusChange, 0, Mathf.Abs(amountNeeded) <= Mathf.Abs(radiusChange) ? amountNeeded : radiusChange);
            }
            yield return new WaitForFixedUpdate();
        }
        settingSize = false;
    }

    private void LateUpdate()
    {
        ChangeViewingDirection();
    }

    public void ChangeViewingDirection()
    {
        transform.localEulerAngles += new Vector3(0, ControlManager.instance.playerControls.GeneralMovement.ChangeLookDirection.ReadValue<Vector2>().x) * Time.deltaTime * mouseSensitivity;
    }

    public void CheckGrounded()
    {
        grounded = Physics.Linecast(new Vector3(transform.position.x, transform.position.y - collider.height / 2 * transform.localScale.y  + 0.001f, transform.position.z), new Vector3(transform.position.x, transform.position.y - collider.height / 2 * transform.localScale.y - 0.01f, transform.position.z));
    }

    public void Roll()
    {
        if (!allowAirRolling && !grounded) return;
        crouching = false;
        sprinting = false;
        movementVector = ControlManager.instance.playerControls.GeneralMovement.MovementDirection.ReadValue<Vector2>();
        Vector3 direction = transform.rotation * new Vector3(movementVector.x, 0, movementVector.y);
        rollCoroutine = StartCoroutine(Rolling(direction));
    }

    public IEnumerator Rolling(Vector3 direction)
    {
        rolling = true;
        ControlManager.instance.DisableMovementControls();
        transform.localScale = new Vector3(transform.localScale.x, rollHitboxHeight, transform.localScale.z);
        transform.position -= new Vector3(0, (colliderNormalHeight - rollHitboxHeight) * collider.height / 2, 0);
        for (float timer = 0; timer < rollTime; timer+= Time.deltaTime)
        {
            if (stopYVelocity)
            {
                rb.useGravity = false;
                rb.velocity = new Vector3(direction.x * rollSpeed, 0, direction.z * rollSpeed);

            }
            else
            {
                rb.velocity = new Vector3(direction.x * rollSpeed, rb.velocity.y, direction.z * rollSpeed);
            }
            yield return new WaitForFixedUpdate();
        }
        ControlManager.instance.EnableMovementControls();
        if (!keepMomentum)
        {
            rb.velocity = momentumAfterRoll * direction;
        }
        if (stopYVelocity) rb.useGravity = true;
        transform.localScale = new Vector3(transform.localScale.x, colliderNormalHeight, transform.localScale.z);
        transform.position -= new Vector3(0, (rollHitboxHeight - colliderNormalHeight) * collider.height / 2, 0);
        rolling = false;
    }

    public void Slide()
    {
        if (sliding)
        {
            if (!slidingLastFrame)
            {
                slideDirection = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;
                rb.velocity += slideDirection * initialSlideSpeedGain;

            }
            else
            {
                rb.velocity -= slideDirection * slideSpeedDecay * Time.deltaTime;
            }
        }
        else
        {
            if (slidingLastFrame)
            {
                if(new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude > slideSpeedMinimum)
                {
                    float totalSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
                    rb.velocity -= totalSpeed >= slideCancelSpeedLoss ? slideDirection * slideCancelSpeedLoss : totalSpeed * slideDirection;
                }

            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if ((collision.contacts[0].normal.y >= normalYVectorRange.x && collision.contacts[0].normal.y <= normalYVectorRange.y) && (collision.relativeVelocity.y > fallstunApplyMin) && (!rolling))
        {
            fallstunCoroutine = StartCoroutine(FallstunTimer());
        }
    }
    public IEnumerator FallstunTimer()
    {
        inFallstun = true;
        yield return new WaitForSeconds(fallStunDuration);
        inFallstun = false;
    }

    public void Fallstun()
    {
        movementVector = ControlManager.instance.playerControls.GeneralMovement.MovementDirection.ReadValue<Vector2>();
        Vector3 direction = transform.rotation * new Vector3(movementVector.x, 0, movementVector.y);
        rb.velocity = direction * fallstunMovementSpeed;
    }
}