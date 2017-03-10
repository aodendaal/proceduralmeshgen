using UnityEngine;

public class MouseRotate : MonoBehaviour
{
    public enum MouseState
    {
        Idle,
        Limbo,
        Drag
    }

    [SerializeField]
    private MouseState currentState = MouseState.Idle;
    private float startTime;
    private Vector3 startPosition;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentState == MouseState.Idle)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTime = Time.time;
                startPosition = Input.mousePosition;
                currentState = MouseState.Drag;
            }           
        }

        if (currentState == MouseState.Drag)
        {
            if (Input.GetMouseButtonUp(0))
            {
                currentState = MouseState.Idle;
            }

            var xDistance = startPosition.x - Input.mousePosition.x;
            var yDistance = startPosition.y - Input.mousePosition.y;

            transform.RotateAround(Vector3.zero, Vector3.up, xDistance * Time.deltaTime);
            transform.RotateAround(Vector3.zero, Vector3.right, yDistance * Time.deltaTime);

        }
    }
}