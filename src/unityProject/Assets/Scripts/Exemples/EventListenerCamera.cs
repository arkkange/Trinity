using UnityEngine;
using System.Collections;

public class EventListenerCamera : MonoBehaviour {

	private Transform cameraTransform;

	void Start()
	{
		cameraTransform = transform;
		}

	void OnEnable()
	{
		//EventManager.cameraMove += cameraMove;
	}

	void OnDisable()
	{
		//EventManager.cameraMove -= cameraMove;
	}

	void cameraMove(float x, float y, float z)
	{
		//cameraTransform.position += new Vector3 (x, y, z);
		cameraTransform.Translate (new Vector3(x,y,z));
		//cameraTransform.position = Vector3.Lerp (cameraTransform.position, cameraTransform.position + new Vector3(x,y,z), 1);
	}
}
