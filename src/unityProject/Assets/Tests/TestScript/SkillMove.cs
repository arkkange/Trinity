using UnityEngine;
using System.Collections;

public class SkillMove : SkillTest {

	/*public SkillMove(float magnitude, bool isMovement, string name, Transform prefabTransform) : base()
	{
		_prefabsTransform = prefabTransform;
		_magnitude          = magnitude;
		_isMovement         = isMovement;
		_name               = name;
	}*/

	public override void skillResolve (GameObject actualPos, Vector3 Direction, float magnitude)
	{
		//GameObject _thisGo = actualPos;
		actualPos.transform.Translate(magnitude * Direction);
		Debug.Log ("Lil");
	} 

	public override void skillShow(Transform actualPos, Vector3 position)
	{

	}
}
