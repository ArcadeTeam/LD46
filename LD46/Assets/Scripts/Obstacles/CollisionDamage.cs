using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("BabyDuck"))
        {
            var direction = -collision.impulse.normalized;
            var impact = new Vector3(direction.x, 0.2f, direction.z);

            collision.gameObject.GetComponent<Duck>().killDuck(impact, 30f);
        }
    }
}
