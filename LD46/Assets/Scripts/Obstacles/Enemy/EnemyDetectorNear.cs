using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectorNear : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GetComponentInParent<EnemyController>().HitInit();
        }
    }
}
