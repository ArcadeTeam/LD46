using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float yOffset = 0.2f;
    public float velocity = 0.08f;

    private Vector3 initialPosition;
    private Vector3 upPosition;
    private Vector3 downPosition;
    private bool goUp = true;

    private void Start()
    {
        initialPosition = transform.position;
        upPosition = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);
        downPosition = new Vector3(initialPosition.x, initialPosition.y - yOffset, initialPosition.z);
    }

    private void Update()
    {
        CheckNextPosition();
        Move();
    }

    private void CheckNextPosition()
    {
        if (goUp)
        {
            if (transform.position.y > upPosition.y)
                goUp = false;
        }
        else
        {
            if (transform.position.y < downPosition.y)
                goUp = true;
        }
    }

    private void Move()
    {
        if (goUp) transform.Translate(new Vector3(0, Time.deltaTime * velocity, 0));
        else transform.Translate(new Vector3(0, Time.deltaTime * -velocity, 0));
    }


}
