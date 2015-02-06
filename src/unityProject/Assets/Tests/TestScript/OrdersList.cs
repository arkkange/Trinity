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

	/*[SerializeField]
	Transform   MessageManager;*/


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
			Vector3 total = player.transform.position;
			if(Directions.Count > 0)
			{
				total = calculateDirectionsAndMagnitudes();
			}
			
			Debug.Log(((thisSkill.getCastTime((showingActualSkill.position - total).magnitude)) + calculateAllTime()));

			

			if(((thisSkill.getCastTime((showingActualSkill.position - total).magnitude)) + calculateAllTime()) < 10)
			{
				if(Directions.Count > 0)
				{
					
					Directions.Add((showingActualSkill.position - total).normalized);
					Magnitudes.Add((showingActualSkill.position - total).magnitude);
					
				} else {
					Directions.Add((showingActualSkill.position - total).normalized);
					Magnitudes.Add((showingActualSkill.position - total).magnitude); 
					
				}
				SkillToLaunch.Add(thisSkill);
				
				
				Destroy((showingActualSkill as Transform).gameObject);

			} else {
				//MessageManager.GetComponent<MessageManager>().CreateShortMessage(2, "Vous ne pouvez pas ajouter cette compétence a votre TimeLine");
			}
			
			
		}
	}

	private float calculateAllTime()
	{
		float total = 0;
		for(int i = 0; i < SkillToLaunch.Count; i++)
		{
			total += SkillToLaunch[i].getCastTime(Magnitudes[i]);
		}
		return total;
	} 

	private Vector3 calculateDirectionsAndMagnitudes(){
		Vector3 result = new Vector3();
		for(int i = 0; i < Directions.Count; ++i)
		{
			/*Debug.Log(i + " taile " + Directions.Count + " " + Magnitudes.Count);
			Debug.Log(Directions[i] + " " + Magnitudes[i]); */
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
			StartCoroutine(launchAllSkills());
		}

		if(GUI.Button(new Rect(200,550, 50,50), "Suppress !"))
		{
			SkillToLaunch.RemoveAt(SkillToLaunch.Count - 1); 
			Directions.RemoveAt(Directions.Count - 1);
			Magnitudes.RemoveAt(Magnitudes.Count - 1);
		}
	}

	IEnumerator launchAllSkills()
	{
		SkillTest[] SkillsLaunched = new SkillTest[SkillToLaunch.Count];
		SkillToLaunch.CopyTo(SkillsLaunched);
		SkillToLaunch.Clear();
		
		for(int i = 0; i < SkillsLaunched.Length; ++i)
		{
			//Debug.Log("1");
			StartCoroutine(SkillsLaunched[i].skillResolve(player, Directions[i], Magnitudes[i])); 
			//yield return SkillsLaunched[i].getCastTime(Magnitudes[i]);
			Debug.Log(string.Format(Time.deltaTime.ToString()));
			yield return new WaitForSeconds(SkillsLaunched[i].getCastTime(Magnitudes[i]));
		}

		Directions.Clear();
		Magnitudes.Clear ();
	}

	
}
