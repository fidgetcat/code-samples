using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Vector3 initPos;           //Initial position of the camera

    public float cameraSpeed;         //Camera maximum speed
    public float cameraAccel;         //Camera acceleration
    public float maxZoom;             //Closest zoom setting (lower boundary)
    public float minZoom;             //Furthest zoom setting (higher boundary)
    public float zoomSteps;           //Amount of zoom steps
    public bool edgeMovement;         //Toggles mouse over screen edges movement

    public int frameWidth;            //Thickness of screen edges frame

    public Vector2 worldUpper;        //Upper corner of the world
    public Vector2 worldLower;        //Lower corner of the world

    //Internal variables
    private Camera mainCamera;        //Holds unity's active camera
    private float zoom;               //Current zoom value
    private float zoomScale = 1;      //Linear zoom from 0 to 1
    private float speed;              //Current camera speed
    private float zoomStep;           //Interpolation step

    //Helpers
    private float maxZoomSqrt;        //Max linear zoom for interpolation
    private float minZoomSqrt;        //Min linear zoom for interpolation

    //Input system variables
    private float outputX = 0;        //Smoothened movement on X axis
    private float outputZ = 0;        //Smoothened movement on Z axis
                                      // Use this for initialization
    public float vmod;
    public float hmod;

    public float maxbound;

    public bool IsBusy;

    public Vector3 Origin;
    void Start () {
        IsBusy = false;
        mainCamera = Camera.main;
        transform.position = initPos;
        //Interpolation setup
        maxZoomSqrt = Mathf.Sqrt(maxZoom);
        minZoomSqrt = Mathf.Sqrt(minZoom) - maxZoomSqrt;
        zoomStep = 1 / zoomSteps;
        maxbound = 5.6f;
        mainCamera.orthographicSize = minZoom;
        zoomScale += -0.53f;
        vmod = 0f;
        hmod = 0f;
        //Force zoom update
        UpdateZoom();
        Origin = transform.position;
     }


    void Update () {
        //transform.Translate(MoveDirection() * speed * Time.deltaTime, Space.World);
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");
        if (!IsBusy)
        {
            if (Mathf.Abs(vmod) <= maxbound)
            {
                vmod += Time.deltaTime * 2.2f * ver;
            }
            else if (Mathf.Abs(vmod + ver) <= maxbound)
            {
                vmod += Time.deltaTime * 2.2f * ver;
            }
            if (Mathf.Abs(hmod) <= maxbound)
            {
                hmod += Time.deltaTime * 2.2f * hor;
            }
            else if (Mathf.Abs(hmod + hor) <= maxbound)
            {
                hmod += Time.deltaTime * 2.2f * hor;
            }
        }
        Vector3 newdir = new Vector3();

        if(Input.GetAxis("Horizontal")!= 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!IsBusy)
            {
                newdir = new Vector3(Origin.x + (hmod) + (vmod), Origin.y, Origin.z + (-hmod) + (vmod));
                transform.position = newdir;
            }
        }
        else
        {
            newdir = transform.position;
            hmod = 0f;
            vmod = 0f;
            Origin = newdir;
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            outputX = 0;
            outputZ = 0;
            transform.position = new Vector3(0f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            outputX = 0;
            outputZ = 0;
            transform.position = new Vector3(0.5f, 0f, 0.5f);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (zoomScale < 1)
                zoomScale += zoomStep;

            UpdateZoom();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (zoomScale > 0)
                zoomScale -= zoomStep;

            if (zoomScale < 0)
                zoomScale = 0;

            UpdateZoom();
        }

        /*
        if (Input.GetButtonDown("CameraRot"))
        {
            transform.Rotate(Vector3.up * 90f * Input.GetAxisRaw("CameraRot"), Space.World);
        }*/

        
    }

    public void UpdateZoom()
    {
        //Translates linear zoom step into quadratic zoom value
        zoom = maxZoomSqrt + zoomScale * minZoomSqrt;
        zoom *= zoom;
        speed = zoom * cameraSpeed;
        mainCamera.orthographicSize = zoom;
    }

    private Vector3 MoveDirection()
    {
        //Directional axis input
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        //Mouse over screen edges input
        if (edgeMovement)
        {
            if (Input.mousePosition.x > Screen.width - frameWidth)
                horizontal += 1;
            if (Input.mousePosition.x < frameWidth)
                horizontal += -1;
            if (Input.mousePosition.y > Screen.height - frameWidth)
                vertical += 1;
            if (Input.mousePosition.y < frameWidth)
                vertical += -1;
        }

        //Converting vertical and horizontal inputs into X and Z axis
        float inputX = vertical + horizontal;
        float inputZ = vertical - horizontal;

        //Creating an input vector and normalizing it
        Vector3 input = new Vector3(inputX, 0f, inputZ);
        input.Normalize();

        //Transforming the input to account for the camera rotation
        input = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * input;

        //Cutting input when map borders are reached
        if (input.x > 0 && transform.position.x >= worldUpper.x)
            input.x = 0;
        else if (input.x < 0 && transform.position.x <= worldLower.x)
            input.x = 0;

        if (input.z > 0 && transform.position.z >= worldUpper.y)
            input.z = 0;
        else if (input.z < 0 && transform.position.z <= worldLower.y)
            input.z = 0;

        //Running first order system
        outputX += (input.x - outputX) * cameraAccel * Time.deltaTime;
        outputZ += (input.z - outputZ) * cameraAccel * Time.deltaTime;

        //Preventing micro sliding ad-infinitum
        if (Mathf.Abs(outputX) < 0.001f)
            outputX = 0;
        if (Mathf.Abs(outputZ) < 0.001f)
            outputZ = 0;

        //Protect outputs against lag spikes
        outputX = Mathf.Clamp(outputX, -1, 1);
        outputZ = Mathf.Clamp(outputZ, -1, 1);

        //Creating new vector for output
        Vector3 direction = new Vector3(outputX, 0f, outputZ);

        return direction;
    }

}
