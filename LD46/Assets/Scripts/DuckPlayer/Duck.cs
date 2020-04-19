using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    protected Rigidbody _body;
    protected bool dead = false;

    public ParticleSystem splash;
    private bool canSplah = true;
    private float waterSplashCooldown = 1f;

    void Awake()
    {
        splash.Stop();
        _body = GetComponent<Rigidbody>();
    }

    public void killDuck(Vector3 impactOrientation, float impactSpeed = 1.0f)
    {
        if (gameObject.CompareTag("Player") && !dead) {
           // gameObject.transform.FindChild("")
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
        dead = true;
        GetComponent<Rigidbody>().freezeRotation = false;
        _body.velocity = impactOrientation.normalized * impactSpeed;
        _body.AddTorque(new Vector3(0f, 10f, 10f));
    }

    public void WaterSplash()
    {
        splash.Play();
        /*if (canSplah)
        {
            canSplah = false;
            splash.Play();
            Invoke("WaterSplashCooldown", waterSplashCooldown);
        }*/
    }

    private void WaterSplashCooldown()
    {
        canSplah = true;
    }
}
