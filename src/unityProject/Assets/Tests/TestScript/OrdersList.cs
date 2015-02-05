using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrdersList : MonoBehaviour {

	public GameObject player;

	List<SkillTest> SkillToLaunch = new List<SkillTest>();
	List<Vector3> Directions = new List<Vector3>();
	List<float> Magnitudes = new List<float>();

	Transform showingActualSkill;
	LineRenderer _myLine;


	void OnEnable() {
		CratePanelPrefab.skillAdd += addSkill;
		CratePanelPrefab.skillShow += skillShows;
	}

	void OnDisable() {
		CratePanelPrefab.skillAdd -= addSkill;
		CratePanelPrefab.skillShow -= skillShows;
	}

	void addSkill(SkillTest thisSkill)
	{
		
		if(showingActualSkill)
		{
			if(Directions.Count > 0)
			{
				Directions.Add((showingActualSkill.position - calculateDirectionsAndMagnitudes()).normalized);
				Magnitudes.Add((showingActualSkill.position - calculateDirectionsAndMagnitudes()).magnitude);
				Debug.Log("pouet2");
				
			} else {
				Directions.Add((showingActualSkill.position - player.transform.position).normalized);
				//Debug.Log(showingActualSkill.position - player.transform.position);
				//Debug.Log ((showingActualSkill.position - player.transform.position).magnitude);
				Magnitudes.Add((showingActualSkill.position - player.transform.position).magnitude); 
				Debug.Log("pouet3");
			}
			SkillToLaunch.Add(thisSkill);

			
			Destroy((showingActualSkill as Transform).gameObject);
		}
	}

	private Vector3 calculateDirectionsAndMagnitudes(){
		Vector3 result = new Vector3();
		for(int i = 0; i < Directions.Count; ++i)
		{
			result += (Directions[i] * Magnitudes[i]);
		}

		return result;
	}

	void skillShows(SkillTest thisSkill, Vector3 position_clicked)
	{
		if(showingActualSkill)
		{
			Destroy((showingActualSkill as Transform).gameObject);
		}
		//Destroy((_myLine as LineRenderer).gameObject);
		showingActualSkill = Instantiate(thisSkill._prefabsTransform, position_clicked, Quaternion.identity) as Transform;
		showingActualSkill.Rotate(new Vector3(90,0,0));
		showingActualSkill.localScale.Scale(new Vector3(3,3,3));
	}

	// Update is called once per frame
	void OnGUI () {

		for(int i = 0; i < SkillToLaunch.Count ; ++i)
		{
			GUI.Button(new Rect(100 + (i *50), 400, 50, 40), SkillToLaunch[i].name);
		}

		if(GUI.Button(new Rect(200, 500, 50, 50), "Launch !"))
		{
			launchAllSkills();
		}

		if(GUI.Button(new Rect(200,550, 50,50), "Suppress !"))
		{
			SkillToLaunch.RemoveAt(SkillToLaunch.Count - 1);
			Directions.RemoveAt(Directions.Count - 1);
			Magnitudes.RemoveAt(Magnitudes.Count - 1);
		}
	}

	void launchAllSkills()
	{
		SkillTest[] SkillsLaunched = new SkillTest[SkillToLaunch.Count];
		SkillToLaunch.CopyTo(SkillsLaunched);
		SkillToLaunch.Clear();
		
		for(int i = 0; i < SkillsLaunched.Length; ++i)
		{
			Debug.Log("lool" + i + " taile " + SkillsLaunched.Length + " " + Directions.Count + " " + Magnitudes.Count);
			Debug.Log(Directions[i] + " " + Magnitudes[i]);
			SkillsLaunched[i].skillResolve(player, Directions[i], Magnitudes[i]); 
			//player.transform.Translate(new Vector3(0,10,10)); 
		}

		Directions.Clear();
		Magnitudes.Clear ();
	}

	
}
