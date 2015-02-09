using UnityEngine;
using System.Collections;

public abstract class SkillTest : MonoBehaviour {

	[SerializeField]
	public Transform _prefabsTransform;

    [SerializeField]
    Sprite _mySprite;

	public float            _castTime;
	public int              _powerValue;
	public float            _damageValue;
	public string           _name;
	
	
	/***********************************************************\
    |       Constructeur (damage or heal skill)                 |
    \***********************************************************/
	/*protected SkillTest()
	{
		
	}*/
	public abstract Transform skillShow(Vector3 position);

	public abstract IEnumerator skillResolve(GameObject actualPos, Vector3 Direction, float magnitude);

	public abstract float getCastTime(float magnitude);

}
