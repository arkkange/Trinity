using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptSkillSet : MonoBehaviour {
	
	[SerializeField]
	public List<SkillTest> playerSkillSet = new List<SkillTest>();

	[SerializeField]
	public string _name;

	[SerializeField]
	public float MaxHealth;
	[SerializeField]
	public float ActualHealth;

	public delegate void SkillTrigger(scriptSkillSet player);
	public static event SkillTrigger playerTriggerSkill;

	//public event Action<Collider> SkillTrigger;

	/*void OnTriggerEnter(Collider other) {
		Debug.Log(other.name);
	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("this one works");
	}

	void OnTriggerStay(Collider other) {
		Debug.Log ("It works");
	}*/
}
