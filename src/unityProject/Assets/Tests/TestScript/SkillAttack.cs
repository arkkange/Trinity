using UnityEngine;
using System.Collections;

public class SkillAttack : SkillTest {

	/*public SkillAttack(float magnitude, Vector3 direction, float scaleModifier, float castTime, int powerValue, int damageValue, bool isDamage, int healValue, bool isResurection, bool affectPlayers, bool affectMinions, bool isMovement, string name) : base()
	{
		_scaleModifier      = scaleModifier;
		_castTime           = castTime;
		_powerValue         = powerValue;
		
		_damageValue        = damageValue;
		_isDamage           = isDamage;
		_healValue          = healValue;
		_isResurection      = isResurection;
		_affectPlayers      = affectPlayers;
		_affectMinions      = affectMinions;
		_isMovement         = isMovement;
		_name               = name;
	}*/
	
	public override void skillResolve (GameObject actualPos, Vector3 Direction, float magnitude)
	{
		
	}

	public override void skillShow(Transform actualPos, Vector3 position)
	{
		
	}
}
