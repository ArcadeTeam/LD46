using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    protected Rigidbody _body;
    protected bool dead = false;

    public ParticleSystem splash;


    void Awake()
    {
        splash.Stop();
        _body = GetComponent<Rigidbody>();
    }

    public void killDuck(Vector3 impactOrientation, float impactSpeed = 1.0f)
    {
        GetComponent<Rigidbody>().freezeRotation = false;
        _body.velocity = impactOrientation.normalized * impactSpeed;
        _body.AddTorque(new Vector3(0f, 10f, 10f));
        dead = true;
        if (gameObject.CompareTag("Player") && !dead) {
            gameObject.GetComponent<Animator>().enabled = false;
            gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            GetComponent<Rigidbody>().freezeRotation = true;
            //GetComponent<CapsuleCollider>().enabled = false;
            GameObject.Find("DuckCamera").GetComponent<CameraFollower>().enabled = false;
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }


    }

    public void WaterSplash()
    {
        splash.Play();
    }
}
