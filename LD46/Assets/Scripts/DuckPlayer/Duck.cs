using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    protected Rigidbody _body;
    protected bool dead = false;
    void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    public void killDuck(Vector3 impactOrientation, float impactSpeed = 1.0f)
    {
        if (gameObject.CompareTag("Player") && !dead) {
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
        dead = true;
        GetComponent<Rigidbody>().freezeRotation = false;
        _body.velocity = impactOrientation.normalized * impactSpeed;
        _body.AddTorque(new Vector3(0f, 10f, 10f));
    }
}
