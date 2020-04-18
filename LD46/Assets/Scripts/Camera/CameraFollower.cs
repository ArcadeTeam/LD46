using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    private GameObject duckObject;
    public float cameraDistance = 6f;
    public float cameraSpeed = 0.015f;
    public Vector3 cameraPositionRelative = new Vector3(-1f, -1f, 1f);

    private List<Vector3> filteredPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        duckObject = FindObjectOfType<DuckController>().gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.forward = cameraPositionRelative;
        var distance = cameraPositionRelative.magnitude * cameraDistance;
        Vector3 desiredCameraLocation = duckObject.transform.position - cameraPositionRelative* distance;
        Vector3 difference = (transform.position - desiredCameraLocation);
        transform.position -= difference * cameraSpeed;
    }
}
