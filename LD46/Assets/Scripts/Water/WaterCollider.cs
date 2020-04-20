using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("BabyDuck"))
        {
            collision.gameObject.GetComponent<Duck>().EnterWater();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("BabyDuck"))
        {
            collision.gameObject.GetComponent<Duck>().ExitWater();
        }
    }


}
