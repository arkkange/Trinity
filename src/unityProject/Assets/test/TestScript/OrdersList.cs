using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrdersList : MonoBehaviour {

	public GameObject player;
	public int chosenPlayerIndex;

	public List<GameObject> allPlayersOrderList;

	List<SkillTest> SkillToLaunch = new List<SkillTest>();
	List<Vector3> Directions = new List<Vector3>();
	List<float> Magnitudes = new List<float>();

    [SerializeField]
    TimeLineManager _myTimeLineManager; // visuel de la time line

	//List<List<Object>> FUCKYOUPROJECT = new List<List<Object>>();

	Transform showingActualSkill;

	[SerializeField]
	LineRenderer _myLine;

	void Start()
	{ 
		_myLine.SetVertexCount(0);

		/*for(int i = 0; i< allPlayersOrderList.Count;i++)
		{
			FUCKYOUPROJECT.Add(new List<Object>());
		}*/
	}


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

			if(((thisSkill.getCastTime((showingActualSkill.position - total).magnitude)) + calculateAllTime()) < 10)
			{

				Directions.Add((showingActualSkill.position - total).normalized);
				Magnitudes.Add((showingActualSkill.position - total).magnitude); 
				
				SkillToLaunch.Add(thisSkill);
				
				Destroy((showingActualSkill as Transform).gameObject);
				//showLines();

                // c'est ici qu'on ajoute un nouveau skill en visuel dans la time line

			}			
		}
	}

	private void showLines()
	{ 
		Vector3 total = player.transform.position;
		_myLine.SetVertexCount(0);
		//Debug.Log(SkillToLaunch.Count);
		_myLine.SetVertexCount(SkillToLaunch.Count);
		for(int i = 0; i < SkillToLaunch.Count;i++)
		{
			if(i == 0)
			{
				_myLine.SetPosition(i,total);
			} else {
				total = new Vector3(0,0,0);
				for(int j = 0; j < i; j++)
				{
					total += (Directions[j] * Magnitudes[j]);
				}
				_myLine.SetPosition(i,total);
			}
		}

	}

	private void showLinesWithSkillShow()
	{ 
		Vector3 total = player.transform.position;
		_myLine.SetVertexCount(0);
		Debug.Log(SkillToLaunch.Count);
		_myLine.SetVertexCount(SkillToLaunch.Count);
		for(int i = 0; i < SkillToLaunch.Count;i++)
		{
			if(i == 0)
			{
				_myLine.SetPosition(i,total);
			} else {
				total = new Vector3(0,0,0);
				for(int j = 0; j < i; j++)
				{
					total += (Directions[j] * Magnitudes[j]);
				}
				_myLine.SetPosition(i,total);
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
		showingActualSkill = thisSkill.skillShow(position_clicked);
		//showLines();

        // c'est ici qu'on lance le visuel sur la time line

	}

	// Update is called once per frame
	void OnGUI () {

		for(int i = 0; i < SkillToLaunch.Count ; ++i)
		{
			GUI.Button(new Rect(100 + (i *50), 400, 50, 40), SkillToLaunch[i].name);
		}

		if(GUI.Button(new Rect(200, 500, 100, 50), "Launch !"))
		{
			networkView.RPC("askForData",RPCMode.All);
		}

		if(GUI.Button(new Rect(200,550, 100,50), "Suppress !")) //eveneent de suppression du dernier skill ajouté   todo : a ajouter dans le timeline manager
		{
			SkillToLaunch.RemoveAt(SkillToLaunch.Count - 1); 
			Directions.RemoveAt(Directions.Count - 1);
			Magnitudes.RemoveAt(Magnitudes.Count - 1);
		}
	}

    //remplacement de la gui au dessus
    public void DeleteLastSkillEvent()
    {
        SkillToLaunch.RemoveAt(SkillToLaunch.Count - 1);
        Directions.RemoveAt(Directions.Count - 1);
        Magnitudes.RemoveAt(Magnitudes.Count - 1);
    }

	[RPC]
	void askForData(){
		StartCoroutine(launchAllSkills());
	}
		

	IEnumerator launchAllSkills()
	{
			SkillTest[] SkillsLaunched = new SkillTest[SkillToLaunch.Count];
			SkillToLaunch.CopyTo(SkillsLaunched);
			SkillToLaunch.Clear();
			
			for(int i = 0; i < SkillsLaunched.Length; ++i)
			{
				StartCoroutine(SkillsLaunched[i].skillResolve(player, Directions[i], Magnitudes[i])); 
				yield return new WaitForSeconds(SkillsLaunched[i].getCastTime(Magnitudes[i]));
			}
			
			Directions.Clear();
			Magnitudes.Clear ();	
	}

	
}
