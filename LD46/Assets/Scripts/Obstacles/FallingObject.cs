using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{

    private Projector projector;
    private bool hasBeenHit = false;

    void Start()
    {
        projector = GetComponentInChildren<Projector>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenHit)
        {
            hasBeenHit = true;
            projector.enabled = false;
        }
    }

}
