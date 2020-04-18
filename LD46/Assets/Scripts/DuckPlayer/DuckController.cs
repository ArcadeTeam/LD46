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

    void Start()
    {
        _body = GetComponent<Rigidbody>();

    }



    void Update() {

        checkOnFloor();
        
        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        if (_inputs != Vector3.zero)
        {
            
            var cameraPos = Camera.main.GetComponent<CameraFollower>().cameraPositionRelative;

            var camPos2 = new Vector3(cameraPos.x,0,cameraPos.z);
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
        _body.MovePosition(_body.position + orientatedInput * Speed * Time.fixedDeltaTime);
    }
}
