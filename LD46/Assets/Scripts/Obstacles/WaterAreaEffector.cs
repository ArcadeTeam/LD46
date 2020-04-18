using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAreaEffector : MonoBehaviour
{
    public float waterForce = 2.5f;
    private Vector3 forward;
    private void Start()
    {
        forward = transform.TransformDirection(Vector3.forward);
        Debug.Log(forward);
    }

    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<Rigidbody>().AddForce(forward * waterForce);
    }

}
