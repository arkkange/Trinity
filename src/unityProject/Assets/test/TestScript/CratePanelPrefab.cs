using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CratePanelPrefab : MonoBehaviour {

	public List<GameObject> AllPlayers = new List<GameObject>();
	public GameObject PlayerPrefab;

	List<GameObject> AllPlayersInitiated = new List<GameObject>();
	public GameObject _thisPlayer;

	[SerializeField]
	GameObject TimeLinePrefab;
	GameObject timeLine;

	SkillTest chosenSkill;


	public delegate void SkillAddingHandler(SkillTest thisSkill);
	public static event SkillAddingHandler skillAdd;

	public delegate void SkillShowHandler(SkillTest thisSkill, Vector3 position_clicked);
	public static event SkillShowHandler skillShow;

	void Update()
	{
		if(Input.GetMouseButton(1))
		{
			Plane playerPlane = new Plane(Vector3.up, _thisPlayer.transform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit; 
			float hitdist = 10.0f;
			if (Physics.Raycast(ray, out hit, 100))
			{
				Vector3 _lastPositionClicked = hit.point;

				skillShow(chosenSkill, _lastPositionClicked);
			}

		}
	}

	// Use this for initialization
	public void initialize (int index) {
		
		scriptSkillSet skills = _thisPlayer.GetComponent<scriptSkillSet>();
		
		for(int i = 0; i < skills.playerSkillSet.Count; ++i)
		{
			GUI.Button(new Rect(5, i * 50, 150, 40), skills.playerSkillSet[i].name);
		}

		timeLine = Instantiate(TimeLinePrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;

		OrdersList orders = timeLine.GetComponent<OrdersList>();

		orders.player = _thisPlayer;
		orders.chosenPlayerIndex = index;

		/*for(int i = 0; i < AllPlayers.Count;i++)
		{
			orders.allPlayersOrderList.Add(AllPlayersInitiated[i]);
		}*/
	}
	
	void OnGUI(){

		scriptSkillSet skills = _thisPlayer.GetComponent<scriptSkillSet>();

		for(int i = 0; i < skills.playerSkillSet.Count; ++i)
		{
			if(GUI.Button(new Rect(5, i * 50, 150, 40), skills.playerSkillSet[i].name))
			{
				//To do : Prndre le rectangle correspondant au skill;
				//skills.playerSkillSet[i].skillResolve(_thisPlayer.transform.position);
				chosenSkill = skills.playerSkillSet[i];
			}
		}

		if(GUI.Button(new Rect(5, 500, 100,50), "validate skill")){
			if(chosenSkill != null)
			{
				skillAdd(chosenSkill);
			}
		}
	}
}
