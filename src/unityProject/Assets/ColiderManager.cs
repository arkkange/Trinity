using UnityEngine;
using System.Collections;

public class ColiderManager : MonoBehaviour {

	public SkillTest skill;

	void OnTriggerEnter(Collider other) {
		scriptSkillSet test = other.gameObject.GetComponent<scriptSkillSet>();
		if(test) {
			skill.giveDamage(test);
		}
	}
}
