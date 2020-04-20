using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var orientation = transform.position - other.transform.position;
            other.GetComponentInParent<Duck>().killDuck(orientation.normalized);
        }
    }
}
