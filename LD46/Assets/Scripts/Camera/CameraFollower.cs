using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    private GameObject duckObject;
    public float cameraDistance = 6f;
    public float cameraSpeed = 0.015f;
    public Vector3 cameraPositionRelative = new Vector3(-1f, -1f, 1f);

    private List<Vector3> filteredPositions = new List<Vector3>();


    private List<GameObject> hits = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        duckObject = FindObjectOfType<DuckController>().gameObject;
    }

    void Update()
    {



        Ray topLeft = Camera.main.ScreenPointToRay(new Vector3(0, 0, 0));
        Ray topRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, 0, 0));
        Ray botRight = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0));
        Ray botLeft = Camera.main.ScreenPointToRay(new Vector3(0, Screen.height, 0));


        var newHits = Physics.RaycastAll(duckObject.transform.position, (this.transform.position - duckObject.transform.position), Vector3.Distance(duckObject.transform.position, this.transform.position)).Select(i => i.transform.gameObject).ToList();
        
        List<GameObject> hits2 = Physics.RaycastAll(topLeft.origin+ topLeft.direction * Vector3.Distance(this.transform.position, duckObject.transform.position), topLeft.origin, Vector3.Distance(this.transform.position, duckObject.transform.position)).Select(i => i.transform.gameObject).ToList();
        List<GameObject> hits3 = Physics.RaycastAll(topRight.origin + topRight.direction * Vector3.Distance(this.transform.position, duckObject.transform.position), topRight.origin, Vector3.Distance(this.transform.position, duckObject.transform.position)).Select(i => i.transform.gameObject).ToList();
        List<GameObject> hits4 = Physics.RaycastAll(botRight.origin + botRight.direction * Vector3.Distance(this.transform.position, duckObject.transform.position), botRight.origin, Vector3.Distance(this.transform.position, duckObject.transform.position)).Select(i => i.transform.gameObject).ToList();
        List<GameObject> hits5 = Physics.RaycastAll(botLeft.origin + botLeft.direction * Vector3.Distance(this.transform.position, duckObject.transform.position), botLeft.origin, Vector3.Distance(this.transform.position, duckObject.transform.position)).Select(i => i.transform.gameObject).ToList();


        var realHits = newHits.Where(hit =>
            {
                var corners = 0f;
                if (hits2.Contains(hit)) corners++;
                if (hits3.Contains(hit)) corners++;
                if (hits4.Contains(hit)) corners++;
                if (hits5.Contains(hit)) corners++;
                return (corners > 0);
            }
        ).ToList();

        //Debug.Log(realHits.Count);
        //newHits.ForEach(hit => Debug.Log(hit.name));


        realHits.ForEach(hit =>
        {
            Renderer r = hit.GetComponent<Renderer>();
            if (r)
            {
                r.enabled = false;
            }
        });
        
        foreach (var hit in hits)
        {
            if (!newHits.Contains(hit))
            {
                Renderer r = hit.GetComponent<Renderer>();
                if (r)
                {
                    r.enabled = true;
                }
            }
        }
        
        hits = newHits;

    }


    public void fastMoveToDuck()
    {
        transform.forward = cameraPositionRelative;
        var distance = cameraPositionRelative.magnitude * cameraDistance;
        Vector3 desiredCameraLocation = duckObject.transform.position - cameraPositionRelative * distance;
        Vector3 difference = (transform.position - desiredCameraLocation);
        transform.position -= difference;
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
