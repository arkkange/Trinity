using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    private string _gameName = "Dungeon Trinity";
    private bool _isRefreshingHostList = false;
    private HostData[] _hostList;
    private int _numberOfPlayer = 0;
    private string _mapName = "Map no1";

    
    public bool init = false;
    public GameObject _myPlayer;
    public GameObject _playerPrefab;
    [SerializeField]
    private GameObject _warriorPrefab;
    [SerializeField]
    private GameObject _priestPrefab;
    [SerializeField]
    private GameObject _archerPrefab;
    [SerializeField]
    private GameObject _canvasPlayers;
    [SerializeField]
    private GameObject _canvasReady;
    [SerializeField]
    private GameObject _canvasTimeline;
    [SerializeField]
    private GameObject _canvasActionBar;


    private bool _playerChoose = false;
    private bool _mapChoose = false;
    private bool _isReadytoLaunch = false;
    private bool _connected = false;
    private bool _lobbyToinit = true;
    private bool GUIisAvailable = true;

    private GameObject _buttonWarriorG;
    private Button buttonWarrior;
    private GameObject _buttonPriestG;
    private Button buttonPriest;
    private GameObject _buttonArcherG;
    private Button buttonArcher;
    private GameObject _buttonMap1G;
    private GameObject _buttonMap2G;
    private GameObject _buttonMap3G;
    private GameObject _buttonValG;
    private GameObject _buttonServerG;
    private GameObject _buttonSearchG;

    private GameObject _lobbyName;
    private GameObject _player1name;
    private GameObject _player2name;
    private GameObject _player3name;
    private GameObject _buttonValLobby;

    List<string> _listPlayers = new List<string>();

    /*
    private Component _lobbyNameComp;
    private Component _player1nameComp;
    private Component _player2nameComp;
    private Component _player3nameComp; 
    */

    /*********************************************************************\
    |   Start : Initialisation des variables pour les boutons et UIText	   |
    \*********************************************************************/

    void Start()
    {
        RefreshHostList();

        _canvasTimeline.SetActive(false);
        _canvasActionBar.SetActive(false);
        _canvasReady.SetActive(false);

        //Boutons pour Choisir son joueur
        _buttonWarriorG = _canvasPlayers.transform.GetChild(1).gameObject;
        buttonWarrior = _buttonWarriorG.GetComponent<Button>();
        buttonWarrior.onClick.AddListener(() => { setUpCharacter(1); });
        _buttonWarriorG.GetComponent<Image>().color = Color.green;
        _buttonWarriorG.SetActive(false);


        _buttonPriestG = _canvasPlayers.transform.GetChild(2).gameObject;
        buttonPriest = _buttonPriestG.GetComponent<Button>();
        buttonPriest.onClick.AddListener(() => { setUpCharacter(2); });
        _buttonPriestG.GetComponent<Image>().color = Color.green;
        _buttonPriestG.SetActive(false);


        _buttonArcherG = _canvasPlayers.transform.GetChild(3).gameObject;
        buttonArcher = _buttonArcherG.GetComponent<Button>();
        buttonArcher.onClick.AddListener(() => { setUpCharacter(3); });
        _buttonArcherG.GetComponent<Image>().color = Color.green;
        _buttonArcherG.SetActive(false);


        //Boutons pour Choisir une Map
        _buttonMap1G = _canvasPlayers.transform.GetChild(4).gameObject;
        Button buttonMap1 = _buttonMap1G.GetComponent<Button>();
        buttonMap1.onClick.AddListener(() => { setUpMap(1); });
        _buttonMap1G.SetActive(false);

        /*
        _buttonMap2G = _canvasPlayers.transform.GetChild(4).gameObject;
        Button buttonMap2 = _buttonMap2G.GetComponent<Button>();
        buttonMap2.onClick.AddListener(() => { setUpMap(2); });

        _buttonMap3G = _canvasPlayers.transform.GetChild(5).gameObject;
        Button buttonMap3 = _buttonMap3G.GetComponent<Button>();
        buttonMap3.onClick.AddListener(() => { setUpMap(3); });
        */
        //Bouton pour créer une partie
        _buttonValG = _canvasPlayers.transform.GetChild(5).gameObject;
        Button buttonVal = _buttonValG.GetComponent<Button>();
        buttonVal.onClick.AddListener(() => { UiLobby(); });

        //Bouton pour Rechercher une Partie
        _buttonSearchG = _canvasPlayers.transform.GetChild(6).gameObject;
        Button buttonSearch = _buttonSearchG.GetComponent<Button>();
        buttonSearch.onClick.AddListener(() => { RefreshHostList(); });


        _buttonValLobby = _canvasPlayers.transform.GetChild(7).gameObject;
        Button buttonValLobby = _buttonValLobby.GetComponent<Button>();
        buttonValLobby.onClick.AddListener(() => { StartGame(); });
        _buttonValLobby.SetActive(false);

        /* 
        _lobbyNameComp = _lobbyName.GetComponent<Text>();
        _player1nameComp = _player1name.GetComponent<Text>();
        _player2nameComp = _player2name.GetComponent<Text>();
        _player3nameComp = _player3name.GetComponent<Text>();
        */
    }

    /*********************************************************************\
    |   Update : Rafraichis le tableau des parties disponibles			   |
    \*********************************************************************/
    void Update()
    {
        //rafaichis la liste des parties disponibles
        if (_isRefreshingHostList && MasterServer.PollHostList().Length > 0)
        {
            _isRefreshingHostList = false;
            _hostList = MasterServer.PollHostList();
        }

        //Si on est le serveur et le nombre de joueurs est de 3
        if (Network.isServer && _listPlayers.Count == 3)
        {
            _buttonValLobby.SetActive(true);
        }

        //Si on est le client et que les autres joueurs sont dans la partie
        if (Network.isClient && _isReadytoLaunch)
        {
            spawnPlayer();
            _isReadytoLaunch = false;
        }

        //Envoi de RPC de synchro si on est serveur et qu'un client se connecte
        if (_connected && Network.isServer)
        {
            networkView.RPC("addPlayerCount", RPCMode.All, _numberOfPlayer);
            string stringlistPlayer = string.Join(",", _listPlayers.ToArray());
            networkView.RPC("server_listPlayersSync", RPCMode.Others, stringlistPlayer);
            _connected = false;
        }

        if (_listPlayers.Count >= 1)
        {
            setUpCharacterButtons();
        }
    }

    private void setUpCharacterButtons()
    {
        foreach (var str in _listPlayers)
        {
            if (str == "Player_warrior")
            {
                _buttonWarriorG.GetComponent<Image>().color = Color.red;
                buttonWarrior.onClick.RemoveAllListeners();
            }

            if (str == "Player_priest")
            {
                _buttonPriestG.GetComponent<Image>().color = Color.red;
                buttonPriest.onClick.RemoveAllListeners();
            }

            if (str == "Player_archer")
            {
                _buttonArcherG.GetComponent<Image>().color = Color.red;
                buttonArcher.onClick.RemoveAllListeners();
            }
        }
    }

    /*********************************************************************\
    | setUpCharacter : instancie la variable correspondant au nom du joueur|
    \*********************************************************************/
    public void setUpCharacter(int i)
    {
        if (!_playerChoose)
        {
            switch (i)
            {
                case 1:
                    _playerPrefab = _warriorPrefab;
                    break;
                case 2:
                    _playerPrefab = _priestPrefab;
                    break;
                case 3:
                    _playerPrefab = _archerPrefab;
                    break;
            }
            _listPlayers.Add(_playerPrefab.name);
            if (!Network.isClient)
            {
                StartServer();
            }
            else
            {
                networkView.RPC("listPlayersAdd", RPCMode.Others, _playerPrefab.name);
            }
            _playerChoose = true;
        }
    }

    /*********************************************************************\
    |   setUpMap : instancie la variable correspondant au nom de la map    |
    \*********************************************************************/
    public void setUpMap(int i)
    {
        switch (i)
        {
            case 1:
                _mapName = "Map no1";
                break;
            case 2:
                _mapName = "Map no2";
                break;
            case 3:
                _mapName = "Map no3";
                break;
        }
        _mapChoose = true;

        _buttonMap1G.SetActive(false);
        _buttonWarriorG.SetActive(true);
        _buttonPriestG.SetActive(true);
        _buttonArcherG.SetActive(true);
    }

    /**********************************************************************************\
    |   GetName : Renvoie le nom du perso choisi                                        |
    \**********************************************************************************/
    private string GetName(string namep)
    {
        string name = null;
        switch (namep)
        {
            case "Player_warrior":
                name = "Warrior";
                break;
            case "Player_archer":
                name = "Archer";
                break;
            case "Player_priest":
                name = "Priest";
                break;
        }
        return name;
    }

    private void UiLobby()
    {
        GUIisAvailable = false;
        _buttonMap1G.SetActive(true);
        _buttonSearchG.SetActive(false);
        _buttonValG.SetActive(false);
    }

    /*********************************************************************\
    |   OnGUI Interface de création et de démarage d'une partie			  |
    \*********************************************************************/
    void OnGUI()
    {
        if (GUIisAvailable)
        {
            if (MasterServer.PollHostList().Length > 0)
            {
                _hostList = MasterServer.PollHostList();

                GUILayout.BeginArea(new Rect(630f, 0f, 200f, 180f), GUI.skin.window);
                Vector2 scrollPosition = Vector2.zero;
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
                GUILayout.BeginVertical(GUI.skin.box);
                for (int i = 0; i < _hostList.Length; i++)
                {
                    GUILayout.Label(_hostList[i].gameName);
                    if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                    {
                        if (Input.GetMouseButtonDown(0))
                            JoinServer(_hostList[i]);
                    }
                }

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                GUILayout.EndArea();

            }
        }
    }


    /*********************************************************************\
    |   StartServer : Initialise un serveur avec 3 joueurs maximum		  |         
    |                                                                     |  
    |    Local MasterServer Si celui d'unity est down :                   |
    |                                                                     |
    |    MasterServer.ipAddress = "127.0.0.1";                            |
    |    MasterServer.port = 23466;                                       |      
    |    Network.natFacilitatorIP = "127.0.0.1";                          |      
    |    Network.natFacilitatorPort = 5005;                               |  
    |    Network.InitializeServer(3, 5005,false);                         |  
    |                                                                     |
    \*********************************************************************/
    private void StartServer()
    {
        Network.InitializeServer(3, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(_gameName, _mapName);
    }

    /*********************************************************************\
    |   StartServer : Envoie la demande de Spawn des joueurs		       |
    \*********************************************************************/
    private void StartGame()
    {
        spawnPlayer();
        networkView.RPC("ready_launch_game", RPCMode.All);
    }


    /**********************************************************************************\
    |   OnServerInitialized : Création du Gameobject player and le joueur se connecte  |
    \**********************************************************************************/
    void OnServerInitialized()
    {
        _numberOfPlayer += 1;
    }


    /*********************************************************************\
    |   RefreshHosList : Rafraichis le tableau des parties disponibles	   |
    \*********************************************************************/
    private void RefreshHostList()
    {
        if (!_isRefreshingHostList)
        {
            _isRefreshingHostList = true;
            MasterServer.RequestHostList(_gameName);
        }
    }

    /*********************************************************************\
    |   JoinServer : Connecte le client au serveur						   |
    \*********************************************************************/
    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    /*********************************************************************\
    |   OnConnectedToServer : envoie le nom du joueur connecté au Serveur  |
    \*********************************************************************/
    void OnConnectedToServer()
    {
        GUIisAvailable = false;
        _canvasPlayers.SetActive(true);
        networkView.RPC("onConnect", RPCMode.Server);
        _buttonWarriorG.SetActive(true);
        _buttonArcherG.SetActive(true);
        _buttonPriestG.SetActive(true);
        _buttonValG.SetActive(false);
        _buttonSearchG.SetActive(false);
    }

    /*********************************************************************\
    |server_PlayerJoinRequest : RPC d'envoie du nom du joueur au serveur  |
    \*********************************************************************/
    [RPC]
    void listPlayersAdd(string player)
    {
        _listPlayers.Add(player);
    }


    [RPC]
    void onConnect()
    {
        _connected = true;
    }


    /************************************************************************\
    |server_listPlayersSync : RPC d'envoie du tableau de joueurs aux clients  |
    \************************************************************************/
    [RPC]
    void server_listPlayersSync(string player)
    {
        List<string> newlist = new List<string>();
        foreach (string pl in player.Split(','))
        {
            newlist.Add(pl);
        }
        _listPlayers = newlist;
    }

    /*********************************************************************\
    |ready_launch_game : RPC qui indique que tous les clienst sont prêts   |
    \*********************************************************************/
    [RPC]
    void ready_launch_game()
    {
        _isReadytoLaunch = true;
    }

    /*********************************************************************\
    |addPlayerCount : RPC qui ajoute +1 aux nombres de joueurs             |
    \*********************************************************************/
    [RPC]
    void addPlayerCount(int nb)
    {
        _numberOfPlayer = nb + 1;
    }

    /*********************************************************************\
    |   spawnPlayer : Crée le gameObject du joueur a partir d'un prefab	   |
    \*********************************************************************/
    private void spawnPlayer()
    {
        Vector3 pos = Vector3.up;

        _canvasPlayers.SetActive(false);
        if (_playerPrefab.name == "Player_archer")
        {
            pos = new Vector3(-1.5f, 0f, -4.5f);
        }
        if (_playerPrefab.name == "Player_priest")
        {
            pos = new Vector3(1.5f, 0f, -4.5f);
        }
        if (_playerPrefab.name == "Player_warrior")
        {
            pos = new Vector3(0.5f, 0f, -3.0f);
        }

        _myPlayer = Network.Instantiate(_playerPrefab, pos, Quaternion.identity, 0) as GameObject;
        init = true;
        _canvasReady.SetActive(true);
        _canvasTimeline.SetActive(true);
        _canvasActionBar.SetActive(true);
    }

}