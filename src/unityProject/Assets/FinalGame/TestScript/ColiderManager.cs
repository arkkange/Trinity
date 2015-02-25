using UnityEngine;
using System.Collections;

public class ColiderManager : MonoBehaviour {

	public SkillTest skill;

	void OnTriggerEnter(Collider other) {
		scriptSkillSet test = other.gameObject.GetComponent<scriptSkillSet>();
		if(test) {
			skill.giveDamage(test);
		}
		SkillTest test2 = other.gameObject.GetComponent<SkillTest>();
		if(test2) {
			Debug.Log ("Flute");
			skill.giveDamage(other.transform);

		}
	}
}
