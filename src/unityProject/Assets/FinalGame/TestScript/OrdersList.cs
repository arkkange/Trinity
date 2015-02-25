using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrdersList : MonoBehaviour {

	public GameObject player;
	public int chosenPlayerIndex;

	//public List<GameObject> allPlayersOrderList;

	public List<SkillTest> SkillToLaunch = new List<SkillTest>();
	public List<Vector3> Directions = new List<Vector3>();
	public List<float> Magnitudes = new List<float>();

	[SerializeField]
	public List<GameObject> ennemys = new List<GameObject>();

    /*[SerializeField]
    TimeLineManager _myTimeLineManager;*/ // visuel de la time line


	Transform showingActualSkill; 

	[SerializeField]
	LineRenderer _myLine;
	[SerializeField]
	public 
	void Start()
	{ 
		placeEnemies();
		_myLine.SetVertexCount(0);
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

				Directions.Add(thisSkill.getSkillDirection(showingActualSkill, total));
				Magnitudes.Add(thisSkill.getSkillMagnitude(showingActualSkill, total)); 
				
				SkillToLaunch.Add(thisSkill);
				
				Destroy((showingActualSkill as Transform).gameObject);
				showLines();
				
                // c'est ici qu'on ajoute un nouveau skill en visuel dans la time line

			}			
		}
	}

	private void showLines()
	{ 
		Vector3 total = player.transform.position;
		_myLine.SetVertexCount(0);
		
		_myLine.SetVertexCount(SkillToLaunch.Count+1);
		for(int i = 0; i < SkillToLaunch.Count+1;i++)
		{
			if(i == 0)
			{
				_myLine.SetPosition(i,total);
			} else {
				total = player.transform.position;
				for(int j = 0; j < i; j++)
				{
					total += (Directions[j] * Magnitudes[j]);
				}
				_myLine.SetPosition(i,total);
			}

		}

	}

	private void showLinesWithSkillShow(Vector3 position_clicked)
	{ 
		
		Vector3 total = player.transform.position;
		_myLine.SetVertexCount(0);

		_myLine.SetVertexCount(SkillToLaunch.Count+2);
		for(int i = 0; i < SkillToLaunch.Count+1;i++)
		{
			if(i == 0)
			{
				_myLine.SetPosition(i,total);
			} else {
				total = player.transform.position;
				for(int j = 0; j < i; j++)
				{
					total += (Directions[j] * Magnitudes[j]);
				}
				_myLine.SetPosition(i,total);
			}

			if(i == SkillToLaunch.Count)
			{
				_myLine.SetPosition(i+1,position_clicked);
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
		Vector3 result = player.transform.position;
		for(int i = 0; i < Directions.Count; ++i)
		{
			result += (Directions[i] * Magnitudes[i]);
		}

		return result; 
	}

	void skillShows(SkillTest thisSkill, Vector3 position_clicked)  
	{
		Vector3 total = player.transform.position;
		
		if(Directions.Count > 0)
		{
			total = calculateDirectionsAndMagnitudes();
		}

		if(showingActualSkill)
		{
			Destroy((showingActualSkill as Transform).gameObject);
		}
		showLinesWithSkillShow(position_clicked);
		showingActualSkill = thisSkill.skillShow(position_clicked,total);
		
	}

	// Update is called once per frame
	void OnGUI () {

		for(int i = 0; i < SkillToLaunch.Count ; ++i)
		{
			GUI.Button(new Rect(100 + (i *50), 400, 50, 40), SkillToLaunch[i].name);
		}

		if(GUI.Button(new Rect(200, 500, 100, 50), "Launch !"))
		{
			if(showingActualSkill) {
				Destroy((showingActualSkill as Transform).gameObject); 
			}
			
			_myLine.SetVertexCount(0);
			networkView.RPC("askForData",RPCMode.All);
			for(int i = 0; i < ennemys.Count;++i)
			{
				BasicIa ia = ennemys[i].GetComponent<BasicIa>();
				
				ia.startLocation = ia.transform.position;
				StartCoroutine(ia.IaSetOK());
			}
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
	void askForData() {
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

	void placeEnemies() 
	{
		if(Network.isServer)
		{
			for(int i = 0; i < ennemys.Count;++i)
			{
				GameObject enemytospawn = (GameObject)Network.Instantiate(ennemys[i], new Vector3(0,0,0), Quaternion.identity,0);
				ennemys[i] = enemytospawn;
				switch(i) {
					case 0: 
						enemytospawn.transform.position = new Vector3(4.40f,0,27.40f);
						break;
					case 1:
						enemytospawn.transform.position = new Vector3(-2.40f,0,27.40f);
						break;
					case 2: 
						enemytospawn.transform.position = new Vector3(-2.20f,0,32.80f);
						break;
					case 3: 
						enemytospawn.transform.position = new Vector3(4.40f,0,33.80f);
						break;
				}
			}
		}
	}
}
