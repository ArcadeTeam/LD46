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
    Vector3 cameraPositionRelativeFinal;

    private List<Vector3> filteredPositions = new List<Vector3>();


    private List<GameObject> hits = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        duckObject = FindObjectOfType<DuckController>().gameObject;
        cameraPositionRelativeFinal = cameraPositionRelative;
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

    private void LateUpdate() {
        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");
        float JoystickChange = Input.GetAxis("CameraXBOX");
        if (ScrollWheelChange != 0 || JoystickChange != 0) {
            float R = ScrollWheelChange * -8;
            if (JoystickChange != 0)
                R = JoystickChange;
            float PosX = Camera.main.transform.eulerAngles.x + 90;
            float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);
            PosX = PosX / 180 * Mathf.PI;
            PosY = PosY / 180 * Mathf.PI;
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);
            float Y = R * Mathf.Cos(PosX);
            cameraPositionRelativeFinal = cameraPositionRelative + new Vector3(X, Y, Z);
        }
    }

    public void fastMoveToDuck()
    {
        transform.forward = cameraPositionRelativeFinal;
        var distance = cameraPositionRelativeFinal.magnitude * cameraDistance;
        Vector3 desiredCameraLocation = duckObject.transform.position - cameraPositionRelativeFinal * distance;
        Vector3 difference = (transform.position - desiredCameraLocation);
        transform.position -= difference;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.forward = cameraPositionRelativeFinal;
        var distance = cameraPositionRelativeFinal.magnitude * cameraDistance;
        Vector3 desiredCameraLocation = duckObject.transform.position - cameraPositionRelativeFinal * distance;
        Vector3 difference = (transform.position - desiredCameraLocation);
        transform.position -= difference * cameraSpeed;
    }
}
