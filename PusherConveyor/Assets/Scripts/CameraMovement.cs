using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour 
{
    [HideInInspector]
    public bool cameraLocked = true;
    private Vector3 lastMousePosition;

    #region Drag Camera variables
    // Source: https://forum.unity.com/threads/click-drag-camera-movement.39513/

    public float dragSpeed = 2;
    #endregion

    #region Flying Camera variables
    // Source: https://gist.github.com/gunderson/d7f096bd07874f31671306318019d996

    float mainSpeed = 50.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private float totalRun = 1.0f;
    #endregion

    void Start () 
	{
		
	}
	
	void Update ()
    {
        if (cameraLocked)
            DragCamera();
        else
            FlyingCamera();
        if (Input.GetButtonDown("Switch Camera"))
        {
            cameraLocked = !cameraLocked;
            lastMousePosition = Input.mousePosition;
        }
            

    }

    #region Drag Camera Method

    private void DragCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - lastMousePosition);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        transform.Translate(move, Space.World);
    }

    #endregion

    #region Flying Camera Methods
    private void FlyingCamera()
    {
        lastMousePosition = Input.mousePosition - lastMousePosition;
        lastMousePosition = new Vector3(-lastMousePosition.y * camSens, lastMousePosition.x * camSens, 0);
        lastMousePosition = new Vector3(transform.eulerAngles.x + lastMousePosition.x, transform.eulerAngles.y + lastMousePosition.y, 0);
        transform.eulerAngles = lastMousePosition;
        lastMousePosition = Input.mousePosition;
        //Mouse  camera angle done.  

        //Keyboard commands
        //float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
    #endregion


}
