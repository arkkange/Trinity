using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour {

	[SerializeField]
	public List<GameObject> playerPrefabs = new List<GameObject>();
	public List<bool> IsthisSkillchosen = new List<bool>();

	[SerializeField]
	GameObject _thisPanelGameObject;
	CratePanelPrefab panel;

	GameObject chosenObject;
	int indexChosen = -1;

	bool GameStarted = false;
	bool isClient = false;

	public string IP = "127.0.0.1";
	public int Port = 25001;

	void Start()
	{
		MasterServer.ipAddress = IP;
		MasterServer.port = 23466;
		Network.natFacilitatorIP = IP;
		Network.natFacilitatorPort = Port;

		for(int i = 0; i < playerPrefabs.Count; i++)
		{
			//playerPrefabs[i].renderer.enabled = false;
		}

		for(int i = 0; i < playerPrefabs.Count; i++)
		{
			IsthisSkillchosen.Add(false);
		}
		
	}


	void OnGUI()
	{

			if(GameStarted)
			{
				//Le jeu est lancé plus rien à afficher dans la boucle
			} else {
				if(Network.peerType == NetworkPeerType.Disconnected)
				{
					if (isClient)
					{
						if(GUI.Button(new Rect(100,100,100,25),"Refresh"))
						{
							MasterServer.RequestHostList("aaa");
						}
					HostData[] hosts = MasterServer.PollHostList();

						for(int i = 0; i < hosts.Length; ++i) 
						{
							if(GUI.Button(new Rect(100, 150-(i*100),150,50), hosts[i].gameName))
							{
								Network.Connect(hosts[i]);
							}
						}
					} else {
					if(GUI.Button(new Rect(100,100,100,25),"Start Client"))
					{
						isClient = true;
						
					}
					
					if(GUI.Button(new Rect(100,125,100,25),"Start Server"))
					{
						//Network.InitializeServer(10,Port);
						Network.InitializeServer(3,Port,true);
						MasterServer.RegisterHost("aaa","MyGame", "gnagnagna");
					}
				}
					

				} else {
					if(Network.peerType == NetworkPeerType.Client)
					{
						GUI.Label(new Rect(100,100,100,25), "Client");

					    networkView.RPC("giveMeData",RPCMode.Server);
						drawChosenPlayer();
						
						if(GUI.Button(new Rect(100,125,100,25), "Logout"))
						{
							Network.Disconnect(250);
						}
					}
					if(Network.peerType == NetworkPeerType.Server)
					{
						GUI.Label(new Rect(100,100,100,25), "Server");
						
						drawChosenPlayer();

					    canLaunchGame();
	
						if(GUI.Button(new Rect(100,125,100,25), "Logout"))
						{
							Network.Disconnect(250);
						}
					}
			}
		}
		
	}

	void canLaunchGame() {
		
		bool canLaunch = true;

		/*for(int i = 0; i < IsthisSkillchosen.Count ; i++)
		{
			if(IsthisSkillchosen[i] == false)
			{
				canLaunch = false;
			}
		}*/
		if(canLaunch) {
			if(GUI.Button(new Rect(225,125,100,25), "Lancer Partie"))
			{
				networkView.RPC("launchGameServer",RPCMode.All);
			}
		}
	}



	[RPC]
	void launchGameServer() {

		_thisPanelGameObject = Instantiate(_thisPanelGameObject, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		CratePanelPrefab panel = _thisPanelGameObject.GetComponent<CratePanelPrefab>();
		
		panel.PlayerPrefab = chosenObject;
		if(Network.peerType == NetworkPeerType.Server)
		{
			panel._thisPlayer = (GameObject)Network.Instantiate(panel.PlayerPrefab, new Vector3(0,0,0), Quaternion.identity,0);
		} else {
			panel._thisPlayer = (GameObject)Network.Instantiate(panel.PlayerPrefab, new Vector3(0,0,0), Quaternion.identity,1);
		}
		

		panel.initialize(indexChosen);
		GameStarted = true;
		
	}

	void drawChosenPlayer(){

		scriptSkillSet skill; 

		scriptSkillSet chosedPrefab;

		string chosenName = "";

		if(chosenObject)
		{
			chosedPrefab = chosenObject.GetComponent<scriptSkillSet>();
			chosenName = chosedPrefab._name;
		}
		
		for(int i = 0; i < playerPrefabs.Count; i++)
		{
			skill = playerPrefabs[i].GetComponent<scriptSkillSet>();

			if(!(IsthisSkillchosen[i])){
				if(GUI.Button(new Rect((i * 150)+100,250,100,100), skill._name))
				{
					if(indexChosen == -1) {
						networkView.RPC("choseSkill", RPCMode.All, i, true);
						chosenObject = playerPrefabs[i];
						indexChosen = i;
					} else {
						networkView.RPC("changeSkill", RPCMode.All, indexChosen, i);
						chosenObject = playerPrefabs[i];
						indexChosen = i;
					}
				}
			} else {
				if(indexChosen != i) {
					GUI.Button(new Rect((i * 150)+100,250,100,100), skill._name + " Already");
				} else {
					if(GUI.Button(new Rect((i * 150)+100,250,100,100), "Annuler Selection"))
					{
						networkView.RPC("choseSkill", RPCMode.All, i, false);
						indexChosen = -1;
						chosenObject = null;
					}
				}	
			}
		}
	}

	[RPC]
	void  choseSkill(int index, bool isWhat) {
		IsthisSkillchosen[index] = isWhat;
	}
	
	[RPC]
	void changeSkill(int ancienSkill, int newSkill) {
		IsthisSkillchosen[ancienSkill] = false;
		IsthisSkillchosen[newSkill] = true;
	}

	[RPC]
	void giveMeData() {
		if(Network.isServer)
		{
			for(int i = 0; i < IsthisSkillchosen.Count; i++)
			{
				networkView.RPC("syncDataChosenSkill", RPCMode.Others,i, IsthisSkillchosen[i]);
			}
			
		}
	}

	[RPC]
	void syncDataChosenSkill(int index, bool isthisskillblabla) {
		IsthisSkillchosen[index] = isthisskillblabla;
	}
}
