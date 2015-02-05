using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {


	public delegate void MovingCameraHandler (float x, float y, float z);
	public static event MovingCameraHandler cameraMove;

	void OnGUI()
	{
		if (Input.GetKey (KeyCode.DownArrow)) {
			if(cameraMove != null)
			{
				cameraMove(0,-1*Time.deltaTime,0);
			}
		} 

		if (Input.GetKey (KeyCode.UpArrow)) {
			if(cameraMove != null)
			{
				cameraMove(0,1*Time.deltaTime,0);
			}
		}
	}
}
