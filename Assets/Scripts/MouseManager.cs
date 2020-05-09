using UnityEngine;

public class MouseManager : MonoBehaviour
{
    // Corners of rectangle surrounding Tiles
    public Vector3 BottomLeft = new Vector3(-5, 0, -8);
    public Vector3 BottomRight = new Vector3(5, 0, -8);
    public Vector3 UpperLeft = new Vector3(-5, 0, 58);
    public Vector3 UpperRight = new Vector3(5, 0, 58);

    // Mouse setups
    public float mouseSensitivity = 10000.0f;
    private Vector3 mouseStart;

    void Start()
    {
        Camera.main.transform.position = new Vector3(0, 40, -30);
        Quaternion camsetup = Quaternion.Euler(50, 0, 0);
        Camera.main.transform.rotation = camsetup;
    }

    void Update()
    {   
        if (Input.GetMouseButtonDown(1))
        {
            mouseStart = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition) - mouseStart;
            offset = mouseSensitivity * offset * Time.deltaTime;
            Camera.main.transform.Translate(offset.x, 0, offset.y, Space.World);            
            if(CheckBounds())
            {
                Camera.main.transform.Translate(-offset.x, 0, -offset.y, Space.World);
            }
            mouseStart = Camera.main.ScreenToViewportPoint(Input.mousePosition);            
        }     
//            Camera.main.transform.position = new Vector3
//            (
//                Mathf.Clamp(Camera.main.transform.position.x, leftLimit, rightLimit);
//                Mathf.Clamp(transform.position.)
//            );
    }

    bool CheckBounds()
    {
        Debug.Log(Camera.main.WorldToViewportPoint(BottomRight).y);
        return (Camera.main.WorldToViewportPoint(BottomLeft).x < 0 ||  
                Camera.main.WorldToViewportPoint(BottomLeft).y < 0 ||
                Camera.main.WorldToViewportPoint(BottomRight).x > 1 ||                
                Camera.main.WorldToViewportPoint(BottomRight).y < 0 ||
                Camera.main.WorldToViewportPoint(UpperLeft).x < 0 ||                
                Camera.main.WorldToViewportPoint(UpperLeft).y > 1 ||
                Camera.main.WorldToViewportPoint(UpperRight).x > 1 ||                
                Camera.main.WorldToViewportPoint(UpperRight).y > 1
        );
    }
}
