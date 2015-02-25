using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

	public delegate RectTransform SkillTimeLineCall(SkillTest newSkill);
	public static event SkillTimeLineCall skillCallTime;

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
		Debug.Log(skills.playerSkillSet.Count);
		for(int i = 0; i < skills.playerSkillSet.Count; ++i)
		{
			changeSkillImage(i, skills.playerSkillSet[i]._mySprite);
		}

		timeLine = Instantiate(TimeLinePrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;

		OrdersList orders = timeLine.GetComponent<OrdersList>();

		orders.player = _thisPlayer;
		orders.chosenPlayerIndex = index;

		/*if(Network.isServer)
		{
			GameObject IA = (GameObject)Network.Instantiate(thisFuckingTHING, new Vector3(0,0,0), Quaternion.identity,0);
		}*/
	}
	
	/*void OnGUI(){

		

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
				
			}
		}
	}*/

	//Gestion de l'ACTION BAR
	/*



    */

	public int ActiveButton = 0;
	
	[SerializeField]
	List<Image> listButton = new List<Image>();
	
	
	[SerializeField]
	GameObject myValidateButton;
	
	//Colors
	[SerializeField]
	Color ButtonNormalColor;
	[SerializeField]
	Color ButtonSelectedColor;
	
	
	//test des fonctions ci après
	void Start()
	{
		
	}
	
	
	//Evenements des boutons
	public void OnclickButton(int number)
	{
		scriptSkillSet skills = _thisPlayer.GetComponent<scriptSkillSet>();
		
		chosenSkill = skills.playerSkillSet[number-1];
		
		//set the actual button number
		ActiveButton = number;
		
		//validate button on
		myValidateButton.SetActive(true);
		
		//colors changes
		resetAllButtonColors();
		SetButtonHighlightedColor(number);
	}
	
	public void changeSkillImage(int number, Image newSprite){
		listButton[number] = newSprite;
		
	}
	
	//evenement de validation du skill
	public void Validate()
	{
		skillAdd(chosenSkill);
		resetAllButtonColors();
		//ActiveButton = 0;
		//myValidateButton.SetActive(false);
	}
	
	
	//color gestion for buttons
	void SetButtonHighlightedColor(int number){
		//listButton[number].color = ButtonSelectedColor;
	}
	
	void resetAllButtonColors() {
		scriptSkillSet skills = _thisPlayer.GetComponent<scriptSkillSet>();
		for(int i = 0; i < skills.playerSkillSet.Count; ++i)
		{
			//listButton[i].color = ButtonNormalColor;
			changeSkillImage(i,skills.playerSkillSet[i]._mySprite);
		}
	}

	//Gestion de la TimeLine
	/*



    */

	/*List<RectTransform> PanelList = new List<RectTransform>();
	//List<SkillTest> skillTestList = new List<SkillTest>();      //listes de skills gérée uniquement dans time line pour récupérer les infos;
	
	[SerializeField]
	RectTransform PanelPrefab;
	
	[SerializeField]
	RectTransform _myPanelOfPanelsArea;  //le panel qui contient les autres zones de couleur
	
	[SerializeField]
	RectTransform _myText;
	
	[SerializeField]
	RectTransform myCancelButton;
	
	//panel position in %
	float   _myActualAnchorPosition = 0;
	int     _myActualTimeValue;
	
	
	void addSkillTimeline(SkillTest newSkill)
	{
		float SkillCastTime = newSkill._castTime;
		
		if ((_myActualTimeValue + SkillCastTime) <= 10)
		{
			
			RectTransform _newPortion = Instantiate(PanelPrefab) as RectTransform;
			_newPortion.parent = _myPanelOfPanelsArea;
			
			//reset Achors of our portion to no border
			_newPortion.localPosition = new Vector3(0, 0, 0);
			_newPortion.offsetMax = new Vector2(0, 0);
			_newPortion.offsetMin = new Vector2(0, 0);
			_newPortion.anchorMin = new Vector2(_myActualAnchorPosition, 0);
			_newPortion.anchorMax = new Vector2(_myActualAnchorPosition + (SkillCastTime/10), 1);
			
			//MAj anchors and time value
			_myActualAnchorPosition += SkillCastTime / 10;
			_myActualTimeValue += (int)SkillCastTime;
			
			//changement de couleur
			_newPortion.GetComponent<Image>().color = newSkill._ColorTimeLineSkill1;
			
			//CHangement de texte
			_newPortion.GetComponentInChildren<Text>().text = _myActualTimeValue.ToString();
			
			//ajout a la liste de portions
			PanelList.Add(_newPortion);
			//ajout a la liste de skill
			skillTestList.Add(newSkill);
			
			//active cancel button
			myCancelButton.gameObject.SetActive(true);
		}
		
		Debug.Log(_myActualTimeValue);
		
		
	}
	
	
	
	public void removeLastSkillFromTimeline()
	{
		
		int lastPanelIndex = PanelList.Count - 1;
		int lastSkillIndex = skillTestList.Count - 1;
		
		if (PanelList.Count != 0 && skillTestList.Count != 0)
		{
			
			//PanelList gestion
			RectTransform LastPanel = PanelList[lastPanelIndex];
			Destroy(LastPanel.gameObject);
			PanelList.RemoveAt(lastPanelIndex);
			
			//calculate new actual anchor
			_myActualAnchorPosition -= (float)(skillTestList[lastSkillIndex]._castTime / 10);
			
			//actual time Value MAJ
			_myActualTimeValue -= (int)skillTestList[lastSkillIndex]._castTime;
			Debug.Log(_myActualTimeValue);
			
			//remove the skill form list
			skillTestList.RemoveAt(skillTestList.Count - 1);
			
			//event of the click
			cancelSkillEvent();
			
			if (_myActualTimeValue == 0)
			{
				myCancelButton.gameObject.SetActive(false);
			}
		}
		else
		{
			Debug.Log(" tous différents de 0");
			myCancelButton.gameObject.SetActive(false);
		}
		
		
	}
	
	
	
	
	//evenement de suppression du dernier skill
	void cancelSkillEvent()
	{
		
	}*/
}
