using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 movementVector;

    public Rigidbody rb;
    public CapsuleCollider collider;

    bool grounded;

    public int accelerationCurve;
    public float groundMovementSpeed;
    public float maxMovementSpeed;
    public float accelerationTime;
    public Quaternion direction;

    public void FixedUpdate()
    {
        Debug.Log("fixedUpdate");
        direction = transform.rotation;
        movementVector = ControlManager.instance.playerControls.GeneralMovement.MovementDirection.ReadValue<Vector2>();
        CheckGrounded();
        if (movementVector.magnitude == 0)
        {
            rb.velocity = Vector3.zero;
            return; //dont run if not holding inputs
        }
        if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude >= maxMovementSpeed) return; //dont run if above max speed   WILL NEED TO INCLUDE FOR HOLDING OPPOSITE DIRECTION
        if (grounded)
        {
            if (accelerationCurve == 0)
            {
                float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
                Vector2 direction2D = movementVector;
                float curveX = (speed / (maxMovementSpeed / accelerationTime)) + Time.deltaTime;
                speed = curveX * (maxMovementSpeed / accelerationTime);
                Debug.Log(speed);
                rb.velocity = new Vector3((speed * direction2D).x, rb.velocity.y, (speed * direction2D).y);
                Debug.Log(new Vector3((speed * direction2D).x, rb.velocity.y, (speed * direction2D).y));
            }
            else if (accelerationCurve == 1)
            {
                float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
                Vector2 direction2D = movementVector;
                float curveX = (Mathf.Pow(speed / maxMovementSpeed, 2) * accelerationTime) + Time.deltaTime;
                speed = maxMovementSpeed * Mathf.Sqrt(curveX / accelerationTime);
                Debug.Log(speed);
                rb.velocity = new Vector3((speed * direction2D).x, rb.velocity.y, (speed * direction2D).y);
                Debug.Log("hi + " + new Vector3((speed * direction2D).x, rb.velocity.y, (speed * direction2D).y));
            }
            else if (accelerationCurve == 2)
            {
                float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
                Vector2 direction2D = movementVector;
                float curveX = Mathf.Sqrt(speed / (maxMovementSpeed / Mathf.Pow(accelerationTime, 2))) + Time.deltaTime;
                speed = maxMovementSpeed / Mathf.Pow(accelerationTime, 2) * Mathf.Pow(curveX, 2);
                Debug.Log(speed);
                rb.velocity = new Vector3((speed * direction2D).x, rb.velocity.y, (speed * direction2D).y);
                Debug.Log(new Vector3((speed * direction2D).x, rb.velocity.y, (speed * direction2D).y));
            }
        }

    }

    public void CheckGrounded()
    {
        grounded = Physics.Linecast(new Vector3(transform.position.x, transform.position.y - collider.height / 2 + 0.001f, transform.position.z), new Vector3(transform.position.x, transform.position.y - collider.height / 2 - 0.01f, transform.position.z));
    }

}
