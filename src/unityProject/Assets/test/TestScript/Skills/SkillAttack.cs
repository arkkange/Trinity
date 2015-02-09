using UnityEngine;
using System.Collections;

public class SkillAttack : SkillTest {
	

	public override IEnumerator skillResolve (GameObject actualPos, Vector3 Direction, float magnitude)
	{
		yield return null;
	}


	public override Transform skillShow(Vector3 position)
	{
		return Instantiate(_prefabsTransform, position, Quaternion.identity) as Transform;
	}


	public override float getCastTime(float magnitude)
    { 
		return 0;
    }

}