using UnityEngine;
using System.Collections;

public abstract class SkillTest : MonoBehaviour {

	[SerializeField]
	public Transform _prefabsTransform;

    [SerializeField]
    Sprite _mySprite;

    public Color _ColorTimeLineSkill1;

	public float            _castTime;      //changer en int ?
	public int              _powerValue;
	public float            _damageValue;
	public string           _name;
	
	
	/***********************************************************\
    |       Constructeur (damage or heal skill)                 |
    \***********************************************************/
	/*protected SkillTest()
	{
		
	}*/
	public abstract Transform skillShow(Vector3 position, Vector3 total);

	public abstract IEnumerator skillResolve(GameObject actualPos, Vector3 Direction, float magnitude);

	public abstract float getCastTime(float magnitude);

	public abstract Vector3 getSkillDirection(Transform hisTransform, Vector3 var);

	public abstract float getSkillMagnitude(Transform hisTransform, Vector3 var);

	public abstract void giveDamage(scriptSkillSet player);
}
