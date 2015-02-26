using UnityEngine;
using System.Collections;

public class ColiderManager : MonoBehaviour {

	public SkillTest skill;

	void OnTriggerEnter(Collider other) {
		scriptSkillSet test = other.gameObject.GetComponent<scriptSkillSet>();
		if(test) {
			skill.giveDamage(test);
		}
		ColiderManager test2 = other.gameObject.GetComponent<ColiderManager>();
		if(test2) {
			skill.giveDamage(other.transform);
		}
	}
}
