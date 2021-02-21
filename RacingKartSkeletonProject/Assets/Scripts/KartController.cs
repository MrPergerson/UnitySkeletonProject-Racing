using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartController : MonoBehaviour
{
    [Header("References")]
    public Transform kartModel;
    public Transform kartNormal;
    public Rigidbody sphere;

    [Header("Bools")]
    public bool drifting;
    public bool isGrounded;
    public bool isDriving;

    [Header("Parameters")]
    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;

    private float speed, currentSpeed;
    private float rotate, currentRotate;
    private int driftDirection;

    void Update()
    {
        // Set position of kart model to position of sphere. Subtract y pos of kart model by sphere radius 
        transform.position = sphere.transform.position - new Vector3(0, .6f, 0);

        if(isGrounded)
        {
            HandleAcceleration();
            HandleSteering();
            HandleDrift();

            currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f); 
            currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
            speed = 0f;
            rotate = 0f;  

            AnimateKart();
        }
    }

    private void FixedUpdate()
    {
        //Forward Acceleration
        if (!drifting)
            sphere.AddForce(kartModel.transform.forward * currentSpeed, ForceMode.Acceleration);
        else
            sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        //gravity
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        RaycastHit hitNear;

        if(Physics.Raycast(transform.position, Vector3.down, out hitNear, .5f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


        //Normal Rotation
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);

    }

    private void HandleAcceleration()
    {
        if (Input.GetButton("Accelerate"))
        {
            speed = acceleration;
            isDriving = true;
        }

        if(Input.GetButtonUp("Accelerate"))
        {
            isDriving = false;
        }
    }

    private void HandleSteering()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            int dir = (int)Input.GetAxisRaw("Horizontal");
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            rotate = (steering * dir) * amount;
        }
    }

    private void HandleDrift()
    {
        if (Input.GetButtonDown("Drift") && !drifting && Input.GetAxis("Horizontal") != 0)
        {
            drifting = true;
            driftDirection = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
        }

        if (drifting)
        {
            float control = (driftDirection == 1) ? RemapValues(Input.GetAxis("Horizontal"), -1, 1, 0, 2) : RemapValues(Input.GetAxis("Horizontal"), -1, 1, 2, 0);
            rotate = (steering * driftDirection) * control;
        }

        if (Input.GetButtonUp("Drift") && drifting)
        {
            drifting = false;
        }
    }

    private void AnimateKart()
    {
        if (!drifting)
        {

            Quaternion currentRotation = kartModel.localRotation;
            Quaternion newRotation = Quaternion.Euler(new Vector3(0, (Input.GetAxis("Horizontal") * 15), kartModel.localEulerAngles.z));

            kartModel.localRotation = Quaternion.Lerp(currentRotation, newRotation, .2f);
        }
        else
        {
            float control;
            if (driftDirection == 1)
            {
                control = RemapValues(Input.GetAxis("Horizontal"), -1, 1, .5f, 2);
            }
            else
            {
                control = RemapValues(Input.GetAxis("Horizontal"), -1, 1, 2, .5f);
            }

            kartModel.localRotation = Quaternion.Euler(
                0,
                Mathf.LerpAngle(kartModel.localEulerAngles.y, (control * 15) * driftDirection, .2f),
                0);
        }
    }

    private float RemapValues(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - (transform.up * .5f));
    }
}
