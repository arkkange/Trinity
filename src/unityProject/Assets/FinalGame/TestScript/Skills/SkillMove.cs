using UnityEngine;
using System.Collections;

public class SkillMove : SkillTest {

	public override IEnumerator skillResolve (GameObject actualPos, Vector3 Direction, float magnitude)
	{
		float time = getCastTime(magnitude);
		float i = 0;
		while(i < time)
		{
			float factor = Time.deltaTime * _damageValue;
			actualPos.transform.Translate(Direction.x*factor,actualPos.transform.position.y,Direction.z*factor);
			i += Time.deltaTime; 
			yield return null;
		}
	} 

	public override float getSkillMagnitude(Transform hisTransform, Vector3 var)
	{
		return (hisTransform.position - var).magnitude;
	}

	public override Transform skillShow( Vector3 position, Vector3 total)
	{
		Transform show = Instantiate(_prefabsTransform, position, Quaternion.identity) as Transform;
		show.Rotate(new Vector3(90,0,0));
		show.localScale.Scale(new Vector3(3,3,3));

		return show;
	}

	public override float getCastTime(float magnitude)
	{
		return magnitude / _damageValue;
	}

	public override Vector3 getSkillDirection(Transform hisTransform,Vector3 var)
	{
		return (hisTransform.position - var).normalized;
	}

	public override void giveDamage(scriptSkillSet player) 
	{
		
	}

	public override void giveDamage(Transform isSkill)
	{
		
	}
}
