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

	public override IEnumerator skillResolve (GameObject actualPos, Vector3 Direction, float magnitude)
	{

		Vector3 startPos = actualPos.transform.position;
		float time = getCastTime(magnitude);
		
		float i = 0;
		while(i < time)
		{
			//Debug.Log(i + " time : " + time);
			//v = Vector3.Lerp(startPos, Direction*magnitude, Time.fixedDeltaTime * _damageValue);
			float factor = Time.deltaTime * _damageValue;
			actualPos.transform.Translate(Direction.x*factor,actualPos.transform.position.y,Direction.z*factor);
			i += Time.deltaTime; 
			//actualPos.transform.position = v;
			yield return null;
		}
		

		

		//actualPos.transform.position = v;
	} 

	public override void skillShow(Transform actualPos, Vector3 position)
	{
		
	}

	public override float getCastTime(float magnitude)
	{
		return magnitude / _damageValue;
	}
}
