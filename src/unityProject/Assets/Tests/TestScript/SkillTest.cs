using UnityEngine;
using System.Collections;

public abstract class SkillTest : MonoBehaviour {

	[SerializeField]
	public Transform _prefabsTransform;

	//public int              _type;             // 1:circle     2:cone      3:line      4:movement      5:special action
	public float            _scaleModifier;
	public Vector3          _direction;
	public float            _magnitude;
	public float            _castTime;
	public int              _powerValue;
	public float            _damageValue;
	public bool             _isDamage;
	public float            _healValue;
	public bool             _isResurection;
	public bool             _affectPlayers;
	public bool             _affectMinions;
	public bool             _isMovement;
	public string           _name;
	
	
	/***********************************************************\
    |       Constructeur (damage or heal skill)                 |
    \***********************************************************/
	/*protected SkillTest()
	{
		
	}*/
	public abstract void skillShow(Transform actualPos, Vector3 position);

	public abstract IEnumerator skillResolve(GameObject actualPos, Vector3 Direction, float magnitude);

	public abstract float getCastTime(float magnitude);
}
