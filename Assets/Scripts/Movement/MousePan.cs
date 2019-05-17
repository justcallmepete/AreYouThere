using UnityEngine;
using System.Collections;

public class MousePan : MonoBehaviour 
{	
	public float turnSpeed = -4.0f;		// Speed of camera turning when mouse moves in along an axis
	
	private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
	private Vector3 lastMouseCoordinate;
	private Vector3 mouseDelta;
	private bool isRotating;	// Is the camera being rotated?
	
	void Update () 
	{
		mouseDelta = Input.mousePosition - lastMouseCoordinate;

		// Get the left mouse button
		if(Input.GetMouseButtonDown(0))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}
		
		// Disable movements on button release
		if (!Input.GetMouseButton(0)) isRotating=false;
		
		// Rotate camera along X and Y axis
		if (isRotating)
		{
			if(mouseDelta.x == 0 && mouseDelta.y == 0) return;

	        	Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

			transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
			transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
		}
		lastMouseCoordinate = Input.mousePosition;
	}
}