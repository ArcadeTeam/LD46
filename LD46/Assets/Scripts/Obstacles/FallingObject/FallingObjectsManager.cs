using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectsManager : MonoBehaviour
{

    public GameObject[] items;
    private float yOffset = 20f;

    private bool objectSpawned = false;
    private float spawnCooldownTime = 10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("BabyDuck")) {
            InstanceFallingObject(other.transform.position);
        }
    }

    private void InstanceFallingObject(Vector3 position)
    {
        if (items.Length == 0 || objectSpawned) return;

        objectSpawned = true;
        var item = items[Random.Range(0, items.Length)];
        var pos = new Vector3(position.x, position.y + yOffset, position.z);
        Instantiate(item, pos, item.transform.rotation);
        Invoke("SpawnCooldown", spawnCooldownTime);
    }

    private void SpawnCooldown()
    {
        objectSpawned = false;
    }

}
