using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckController : MonoBehaviour
{
    public float Speed = 1f;
    public float JumpHeight = 2f;
    public LayerMask Ground;

    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Vector3 groundOrientation = Vector3.up;
    private Vector3 orientatedInput;

    private bool dead = false;
    private CameraFollower cameraFollower;


    void Start()
    {
        _body = GetComponent<Rigidbody>();
        cameraFollower = Camera.main.GetComponent<CameraFollower>();
        resetDuckAlignment();
        StartCoroutine(waiter());
    }


    IEnumerator waiter()
    {

        //Wait for 4 seconds
        yield return new WaitForSeconds(4);
        killDuck(new Vector3(-1f,1f,0f), 20f);
        yield return new WaitForSeconds(4);
        resetDuck(new Vector3(66.03f, 2.33f, 129.67f));
    }


    void killDuck(Vector3 impactOrientation, float impactSpeed = 1.0f )
    {
        dead = true;
        GetComponent<Rigidbody>().freezeRotation = false;
        _body.velocity = impactOrientation.normalized * impactSpeed;
        _body.AddTorque(new Vector3(0f,10f, 10f));
    }

    void resetDuck(Vector3 position)
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
            checkOnFloor();

            _inputs = Vector3.zero;
            _inputs.x = Input.GetAxis("Horizontal");
            _inputs.z = Input.GetAxis("Vertical");
            if (_inputs != Vector3.zero)
            {
                var cameraPos = cameraFollower.cameraPositionRelative;
                var camPos2 = new Vector3(cameraPos.x, 0, cameraPos.z);
                transform.forward = camPos2;
                orientatedInput = transform.localRotation * _inputs;
                transform.forward = orientatedInput;

            }
            else orientatedInput = Vector3.zero;

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
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
            _body.MovePosition(_body.position + orientatedInput * Speed * Time.fixedDeltaTime);
        }
        else
        {
            if (_body.velocity.y == 0) _body.velocity = new Vector3(_body.velocity.x * 0.9f, 0f, _body.velocity.z * 0.9f);
        }

    }
}
