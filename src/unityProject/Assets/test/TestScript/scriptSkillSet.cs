using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptSkillSet : MonoBehaviour {
	
	[SerializeField]
	public List<SkillTest> playerSkillSet = new List<SkillTest>();

	[SerializeField]
	public string _name;
}
