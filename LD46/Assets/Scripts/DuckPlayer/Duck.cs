using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Duck : MonoBehaviour
{
    protected Rigidbody _body;
    protected bool dead = false;

    public ParticleSystem splash;

    protected bool inWater = false;
    public GameObject featherParticleEffect;

    void Awake()
    {
        splash.Stop();
        _body = GetComponent<Rigidbody>();
    }

    public void killDuck(Vector3 impactOrientation, float impactSpeed = 1.0f)
    {

        if (gameObject.CompareTag("BabyDuck")) {

            featherParticleEffect.SetActive(true);
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.transform.localScale = new Vector3(1, 0.1f, 1);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponentInChildren<Animator>().enabled = false;
        }
        if (gameObject.CompareTag("Player") && !dead) {
            GetComponent<Rigidbody>().freezeRotation = false;
            _body.velocity = impactOrientation.normalized * impactSpeed;
            _body.AddTorque(new Vector3(0f, 10f, 10f));
            /* gameObject.GetComponent<Animator>().enabled = false;
             gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);*/
            GetComponent<Rigidbody>().freezeRotation = true;
            //GetComponent<CapsuleCollider>().enabled = false;
            GameObject.Find("DuckCamera").GetComponent<CameraFollower>().enabled = false;
            Debug.Log("EEEEEEh");
            gameObject.GetComponent<RagdollController>().ActivateRagdoll();
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
        dead = true;
    }

    public void EnterWater()
    {
        inWater = true;
        WaterSplash();
    }

    public void ExitWater()
    {
        inWater = false;
        WaterSplash();
    }

    private void WaterSplash()
    {
        splash.Play();
    }
}
