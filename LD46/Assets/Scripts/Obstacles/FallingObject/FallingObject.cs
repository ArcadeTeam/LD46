﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public bool canKill = true;
    private Projector projector;
    private bool hasBeenHit = false;

    //public AudioClip fallingSound;
    public AudioClip objectSound;
    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        projector = GetComponentInChildren<Projector>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenHit)
        {
            audio.clip = objectSound;
            audio.Play();

            hasBeenHit = true;
            projector.enabled = false;

            if (canKill && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("BabyDuck")))
            {
                var direction = -collision.impulse.normalized;
                var impact = new Vector3(direction.x, 0.2f, direction.z);

                collision.gameObject.GetComponent<Duck>().killDuck(impact, 30f);
            }
        }
    }

}
