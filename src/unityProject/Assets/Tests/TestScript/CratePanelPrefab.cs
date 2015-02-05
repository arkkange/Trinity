﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CratePanelPrefab : MonoBehaviour {

	[SerializeField]
	GameObject PlayerPrefab;
	GameObject _thisPlayer;

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
		if(Input.GetMouseButton(0))
		{
			Plane playerPlane = new Plane(Vector3.up, _thisPlayer.transform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float hitdist = 0.0f;
			if (playerPlane.Raycast(ray, out hitdist))
			{
				Vector3 _lastPositionClicked = ray.GetPoint(hitdist);
				Debug.Log(_lastPositionClicked.ToString());
				skillShow(chosenSkill, _lastPositionClicked);
			}
		}
	}

	// Use this for initialization
	void Start () {
		_thisPlayer = Instantiate(PlayerPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		
		

		scriptSkillSet skills = _thisPlayer.GetComponent<scriptSkillSet>();
		
		for(int i = 0; i < skills.playerSkillSet.Count; ++i)
		{
			GUI.Button(new Rect(5, i * 50, 150, 40), skills.playerSkillSet[i].name);
		}

		timeLine = Instantiate(TimeLinePrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;

		OrdersList orders = timeLine.GetComponent<OrdersList>();

		orders.player = _thisPlayer;
	}
	
	void OnGUI(){

		scriptSkillSet skills = _thisPlayer.GetComponent<scriptSkillSet>();

		for(int i = 0; i < skills.playerSkillSet.Count; ++i)
		{
			if(GUI.Button(new Rect(5, i * 50, 150, 40), skills.playerSkillSet[i].name))
			{
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
