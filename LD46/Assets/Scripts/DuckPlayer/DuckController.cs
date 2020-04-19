using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DuckController : Duck
{
    private float Speed = 6f;
    private float JumpHeight = 4f;

    private float SprintSpeed;
    private float rotationSpeed = 4f;

    public LayerMask Ground;
    
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Vector3 groundOrientation = Vector3.up;
    private Vector3 orientatedInput;

    private CameraFollower cameraFollower;

    public HashSet<BabyDuckController> nearBabies;

    private Vector3 lastOrientationWhenGrounded = Vector3.forward;

    private Boolean planning = false;

    private bool isSprinting = false;
    void Start()
    {
        SprintSpeed = Speed * 2f;

        nearBabies = new HashSet<BabyDuckController>();
        cameraFollower = Camera.main.GetComponent<CameraFollower>();
        _body.useGravity = false;
        resetDuckAlignment();
    }

    public void resetDuck(Vector3 position)
    {
        dead = false;
        GetComponent<Rigidbody>().freezeRotation = true;
        transform.position = position;
        resetDuckAlignment();
        cameraFollower.fastMoveToDuck();
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
                _body.AddForce(0.2f * Physics.gravity * (_body.mass * _body.mass));
                if (_body.velocity.y < -0.7f)
                {
                    _body.velocity = new Vector3(_body.velocity.x, -0.7f, _body.velocity.z);
                }
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
                Quaternion lookOnLook = Quaternion.LookRotation(-_inputs);
                transform.rotation =  Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * rotationSpeed);
                orientatedInput = transform.forward;
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
            if (Input.GetButtonDown("Fire1") && _isGrounded) {
                isSprinting = true;
                rotationSpeed = 2f;
            } else if (!_isGrounded || Input.GetButtonUp("Fire1"))
            {
                isSprinting = false;
                rotationSpeed = 4f;
            }

            //quack logic
            if (Input.GetButtonDown("Fire2")) {
                foreach (BabyDuckController baby in nearBabies) {
                    baby.GoWithMom(transform);
                }
                getCloseHumans().ForEach(human => human.duckQuacked(transform.position));
            }

        }
        else
        {
            planning = false;
            _body.AddForce(Physics.gravity * (_body.mass * _body.mass));
        }


    }

    private List<HumanController> getCloseHumans()
    {
        List<HumanController> humans = Physics.OverlapSphere(transform.position, 6f)
            .Select(hit => hit.transform.gameObject.GetComponent<HumanController>())
            .Where(hc => hc != null && hc != this)
            .ToList();


        return humans;
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
            var speed = Speed;
            if (isSprinting) speed = SprintSpeed;

            var orientation = orientatedInput;
            if (!_isGrounded) orientation = Vector3.Lerp(orientation, lastOrientationWhenGrounded, 0.25f);
            _body.MovePosition(_body.position + orientation * speed * Time.fixedDeltaTime);
        }
        else
        {
            if (_body.velocity.y == 0) _body.velocity = new Vector3(_body.velocity.x * 0.9f, 0f, _body.velocity.z * 0.9f);
            if (_body.velocity.magnitude < 1f) _body.angularVelocity = Vector3.zero;
        }

    }

    public void AddBaby(BabyDuckController baby)
    {
        nearBabies.Add(baby);
    }

    public void RemoveBaby(BabyDuckController baby)
    {
        nearBabies.Remove(baby);
    }
}
