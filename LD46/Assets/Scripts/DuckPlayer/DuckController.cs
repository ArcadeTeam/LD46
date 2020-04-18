using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    public float Speed = 1f;
    public float JumpHeight = 4f;

    public float DashForce = 5f;
    public float timeBetweenDash = 3f;

    public LayerMask Ground;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Vector3 groundOrientation = Vector3.up;
    private Vector3 orientatedInput;

    private bool dead = false;
    private CameraFollower cameraFollower;

    public List<BabyDuckController> nearBabies;
    private float lastDash = -999f;

    private Vector3 ori1;
    private Vector3 ori2;
    private float oriTime2;

    private Vector3 lastOrientationWhenGrounded = Vector3.forward;

    private Boolean planning = false;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        cameraFollower = Camera.main.GetComponent<CameraFollower>();
        _body.useGravity = false;
        resetDuckAlignment();
    }

    public void killDuck(Vector3 impactOrientation, float impactSpeed = 1.0f )
    {
        dead = true;
        GetComponent<Rigidbody>().freezeRotation = false;
        _body.velocity = impactOrientation.normalized * impactSpeed;
        _body.AddTorque(new Vector3(0f,10f, 10f));
    }

    public void resetDuck(Vector3 position)
    {
        dead = false;
        GetComponent<Rigidbody>().freezeRotation = true;
        transform.position = position;
        resetDuckAlignment();
        cameraFollower.fastMoveToDuck();
        ori1 = position;
        ori2 = position;
        oriTime2 = 0;
    }

    private void resetDuckAlignment()
    {
        var cameraPos = cameraFollower.cameraPositionRelative;
        var camPos2 = new Vector3(cameraPos.x, 0, cameraPos.z);
        transform.forward = camPos2;
    }

    void Update() {


        if (!dead)
        {
            //applying gravity
            if (planning)
            {
                if (_body.velocity.y > -0.1f) _body.AddForce(0.2f * Physics.gravity * (_body.mass * _body.mass));
            }
            else
            {
                _body.AddForce(Physics.gravity * (_body.mass * _body.mass));
            }

            checkOnFloor();

            //orientation
            _inputs = Vector3.zero;
            _inputs.x = Input.GetAxis("Horizontal");
            _inputs.z = Input.GetAxis("Vertical");
            if (_inputs != Vector3.zero )
            {
                var cameraPos = cameraFollower.cameraPositionRelative;
                var camPos2 = new Vector3(cameraPos.x, 0, cameraPos.z);
                transform.forward = camPos2;
                orientatedInput = transform.localRotation * _inputs;

                if ((Time.fixedTime - oriTime2) * 10 > 1)
                {
                    ori1 = ori2;
                    ori2 = orientatedInput;
                    oriTime2 = Time.fixedTime;
                }

                transform.forward = Vector3.Slerp(ori1, ori2, (Time.fixedTime- oriTime2) *10);
            }
            else orientatedInput = Vector3.zero;

            if (_isGrounded) lastOrientationWhenGrounded = orientatedInput;

            //planning detection
            if (Input.GetButton("Jump") && _body.velocity.y <= 0)
            {
                planning = true;
            }
            else
            {
                planning = false;
            }

            //jump detection
            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            }

            //dash logic
            if (Input.GetButtonDown("Fire1")) {
                if (Time.fixedTime - lastDash >= timeBetweenDash)
                {
                    lastDash = Time.fixedTime;
                    _body.AddForce(transform.forward.normalized * DashForce, ForceMode.Impulse);
                }
            }

            //quack logic
            if (Input.GetButtonDown("Fire2")) {
                foreach (BabyDuckController baby in nearBabies) {
                    baby.GoWithMom(transform);
                }
            }
        }
    }

    private void checkOnFloor() 
    {
        float rayDistance = 100;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, rayDistance))
        {
            groundOrientation = hit.transform.up;
            var distanceToFloor = GetComponent<CapsuleCollider>().height * 0.5f;

            if (hit.distance <= distanceToFloor + 0.1f) _isGrounded = true;
            else _isGrounded = false;
        }  else  _isGrounded = false;
    }


    void FixedUpdate()
    {
        if (!dead)
        {
            var orientation = orientatedInput;
            if (!_isGrounded) orientation = Vector3.Lerp(orientation, lastOrientationWhenGrounded, 0.25f);
            _body.MovePosition(_body.position + orientation * Speed * Time.fixedDeltaTime);
        }
        else
        {
            if (_body.velocity.y == 0) _body.velocity = new Vector3(_body.velocity.x * 0.9f, 0f, _body.velocity.z * 0.9f);
        }

    }


}
