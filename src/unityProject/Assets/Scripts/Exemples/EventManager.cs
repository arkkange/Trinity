using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

	[SerializeField]
	GameObject camera;

	void Update()
	{
		int multiply = 3;
		if(Input.GetKey(KeyCode.RightShift))
		{
			multiply = 3*3;
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			camera.transform.position += new Vector3(0,0,-multiply*Time.deltaTime);
		} 

		if (Input.GetKey (KeyCode.UpArrow)) {
			camera.transform.position += new Vector3(0,0,multiply*Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.LeftArrow)) {
			camera.transform.position += new Vector3(-multiply*Time.deltaTime,0,0);
		} 
		
		if (Input.GetKey (KeyCode.RightArrow)) {
			camera.transform.position += new Vector3(multiply*Time.deltaTime,0,0);
		}
	}
}
