using UnityEngine;
using System.Collections;

public class ShieldSkill : SkillTest {

	public override IEnumerator skillResolve (GameObject actualPos, Vector3 Direction, float magnitude)
	{
		var rotate = Quaternion.LookRotation (Direction).eulerAngles;
		Transform attackManager = (Transform)Network.Instantiate(_prefabsTransform, actualPos.transform.position +(Direction*0.75f), Quaternion.Euler(rotate),0);
		attackManager.collider.isTrigger = true;
		ColiderManager skillToUse = attackManager.GetComponent<ColiderManager>();
		skillToUse.skill = this;
		float time = getCastTime(magnitude);
		float i = 0;
		while(i < time)
		{
			i += Time.deltaTime; 
			yield return null;
		}
		Network.Destroy((attackManager as Transform).gameObject);
	}
	
	public override void giveDamage(scriptSkillSet player){
		player.ActualHealth-=_damageValue;
		if(player.ActualHealth <= 0)
		{
			player.die();
		}
	}

	public override void giveDamage(Transform isSkill)
	{

	}
	
	public override float getSkillMagnitude(Transform hisTransform, Vector3 var)
	{
		return 0;
	}
	
	
	
	public override Transform skillShow(Vector3 position, Vector3 total)
	{
		var rotate = Quaternion.LookRotation (position - total).eulerAngles;
		
		return Instantiate(_prefabsTransform, total + ((position - total).normalized*0.75f), Quaternion.Euler(rotate)) as Transform;
	}
	
	
	public override float getCastTime(float magnitude)
	{ 
		return _castTime;
	}
	
	public override Vector3 getSkillDirection(Transform hisTransform,Vector3 var)
	{
		return hisTransform.rotation * Vector3.forward;
	}	

}
