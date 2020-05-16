using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public Vector3 camStartPos = new Vector3(0, 80, -30);
    public Quaternion camStartRot = Quaternion.Euler(60, 0, 0);
    
    // Camera pan settings
    public float panSpeed = 10000f;
    public Vector2 panLimitX = new Vector2(-50, 180);
    public Vector2 panLimitZ = new Vector2(-250, 0);
    
    // Camera zoom settings
    public float zoomSpeed = 10000f;
    public float minFOV = 30f;
    public float maxFOV = 90f;


    // Variable initializations
    private Vector3 mousePos;
    private Vector3 offset;

    void Start()
    {
        // Set Camera to a default position
        Camera.main.transform.position = camStartPos;
        Camera.main.transform.rotation = camStartRot;
        Camera.main.fieldOfView = 60f;
    }

    // Integrates three functions: giveTileName(), Pan(), Zoom()
    void Update()
    {   
        bool isLeftMouseDown = Input.GetMouseButtonDown(0);
        if (isLeftMouseDown || Input.GetMouseButtonDown(1))
        {
            mousePos = Input.mousePosition;
            if (isLeftMouseDown)
            {
                giveTileName();
            }
        }
        else if (Input.GetMouseButton(1))
        {
            Pan();
        }
        Zoom();
    }

    // Camera panning on XZ-axis
    void Pan()
    {
        Vector3 mouseNewPos = Input.mousePosition;
        Vector3 move = Camera.main.ScreenToViewportPoint(mousePos - mouseNewPos) *
                panSpeed * Time.deltaTime;

        Vector3 pos = Camera.main.transform.position;
        // Move camera only within the boundaries of a terrain
        pos.x = Mathf.Clamp(pos.x + move.x, panLimitX.x, panLimitX.y);
        pos.z = Mathf.Clamp(pos.z + move.y, panLimitZ.x, panLimitZ.y);
        Camera.main.transform.position = pos;
            
        mousePos = mouseNewPos;            
    }

    // Zooming by scrolling the mouse wheel
    void Zoom()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        
        // Zooming within minimum and maximum zoom
        Camera.main.fieldOfView = Mathf.Clamp(
                Camera.main.fieldOfView - Time.deltaTime * zoomSpeed * scrollWheelInput,
                minFOV,
                maxFOV);
    }

    void giveTileName()
    {
        RaycastHit hit;
        Ray selectObj = Camera.main.ScreenPointToRay(mousePos);
        
        // There are only two types of colliders: tiles and objects on tiles
        if(Physics.Raycast(selectObj, out hit))
        {
            // If the tile is clicked (or a non colliding object on the tile)
            // display tile name on console
            if(hit.collider.tag == "Tile")
            {
                Debug.Log(hit.collider.name);
            }
            // Otherwise, a colliding object on a tile was hit. In this case,
            // its parent's name will be displayed, which is the tile 
            else
            {
                Debug.Log(hit.collider.transform.parent.name);
            }
        }
    }
}
