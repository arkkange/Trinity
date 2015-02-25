using UnityEngine;
using System.Collections;

public class SkillHeal : SkillTest {

	public override IEnumerator skillResolve (GameObject actualPos, Vector3 Direction, float magnitude)
	{
		//var rotate = Quaternion.LookRotation (Direction).eulerAngles;
		Transform attackManager = (Transform)Network.Instantiate(_prefabsTransform, actualPos.transform.position + (Direction*magnitude), Quaternion.identity,0);
		attackManager.collider.isTrigger = false;
		attackManager.Rotate(new Vector3(90,0,0));
		attackManager.localScale.Scale(new Vector3(3,3,3));
		ColiderManager skillToUse = attackManager.GetComponent<ColiderManager>();
		skillToUse.skill = this;
		float time = getCastTime(magnitude);
		float i = 0;
		while(i < time)
		{
			i += Time.deltaTime; 
			yield return null;
		}
		attackManager.collider.isTrigger = true;
		yield return null;
		Network.Destroy((attackManager as Transform).gameObject);
	}
	
	public override void giveDamage(scriptSkillSet player){
		player.ActualHealth+=_damageValue;
		if(player.ActualHealth > player.MaxHealth)
		{
			player.ActualHealth = player.MaxHealth;
		}
		player.callActualizeHealth();
	}
	
	public override float getSkillMagnitude(Transform hisTransform, Vector3 var)
	{
		return (hisTransform.position - var).magnitude;
	}
	
	
	
	public override Transform skillShow(Vector3 position, Vector3 total)
	{
		Transform show = Instantiate(_prefabsTransform, position, Quaternion.identity) as Transform;

		if(((position - total).magnitude) > 10)
		{
			position = (position - total).normalized * 10;
		}
		show.position = position;

		show.Rotate(new Vector3(90,0,0));
		show.localScale.Scale(new Vector3(3,3,3));
		
		return show;
	}
	
	public override float getCastTime(float magnitude)
	{ 
		return _castTime;
	}
	
	public override Vector3 getSkillDirection(Transform hisTransform,Vector3 var)
	{
		return (hisTransform.position - var).normalized;
	}

	public override void giveDamage(Transform isSkill)
	{
		
	}
}
