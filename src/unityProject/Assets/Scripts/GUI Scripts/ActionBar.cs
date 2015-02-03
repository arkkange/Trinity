using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour {

    [SerializeField]
    Transform _myCamera;

    [SerializeField]
    RectTransform   _myTimeLineObject;
    TimeLine        _mytimeLine;

    [SerializeField]
    Transform           _myPlayer;
    PlayerSkillResolver _myPlayerSKillResolver;

    //[SerializeField]
    //Transform _dungeonPlane;

    //real time modified skill
    Skill _CurrentSkillChoice;

    //Skill states
    bool _activeSkill1;
    bool _activeSkill2;
    bool _activeSkill3;
    bool _activeMovement;
    bool _activeSpecialAction;

    bool _fallowActiveSkill1;
    bool _fallowActiveSkill2;
    bool _fallowActiveSkill3;
    bool _fallowActiveMovement;
    bool _fallowActiveAction;

    //bouttons
    [SerializeField]
    Transform _buttonAction1;
    [SerializeField]
    Transform _buttonAction2;
    [SerializeField]
    Transform _buttonAction3;
    [SerializeField]
    Transform _buttonMove;
    [SerializeField]
    Transform _buttonSpecialAction;

    [SerializeField]
    Transform _buttonAction1_cancel;
    [SerializeField]
    Transform _buttonAction2_cancel;
    [SerializeField]
    Transform _buttonAction3_cancel;
    [SerializeField]
    Transform _buttonMove_cancel;
    [SerializeField]
    Transform _buttonSpecialAction_cancel;

    [SerializeField]
    Transform _buttonValidate;

    //click events position
    Vector3 _lastPositionClicked;
    List<Vector3> _MoveList = new List<Vector3>();    //listes des vecteurs de deplacements du personnage
    // TODO : après resolution de la time line, il faut reset la position initiale du joueur dans cette liste

    //visual objects
    [SerializeField]
    Transform _myLineRenderObject;
    LineRenderer _myLine;

    [SerializeField]
    Transform _CirclePrefab;
    Transform _mycircle;

    [SerializeField]
    Transform _LinePrefab;

    [SerializeField]
    Transform _ConePrefab;
	Transform _mycone;

    [SerializeField]
    Transform _endMovementRingPrefab;
    Transform _endMovementRing;
    Text _endMovementRingText;
    List<Transform> _listOfRingsOfMovements = new List<Transform>();

    GameObject _mynetwork;
    NetworkManager _mynetworkmanager;
    bool _init = true;


	// Use this for initialization
	void Start () {
        _mynetwork = GameObject.Find("NetworkManager");

        //getcomponents a récupérer pour économiser la mémoire
        _mytimeLine = _myTimeLineObject.GetComponent<TimeLine>();
        _myLine = _myLineRenderObject.GetComponent<LineRenderer>();

        _activeSkill1           = false;
        _activeSkill2           = false;
        _activeSkill3           = false;
        _activeMovement         = false;
        _activeSpecialAction    = false;

        _fallowActiveSkill1     = false;
        _fallowActiveSkill2     = false;
        _fallowActiveSkill3     = false;
        _fallowActiveMovement   = false;
        _fallowActiveAction     = false;

	}
	
	// Update is called once per frame
    void Update()
    {
        if (_mynetwork.GetComponent<NetworkManager>().init && _init)
        {
            _myPlayer = _mynetwork.GetComponent<NetworkManager>()._myPlayer.transform;
            _myPlayerSKillResolver = _myPlayer.GetComponent<PlayerSkillResolver>();
            _MoveList.Add(_myPlayer.position);
            _init = false;
        }

        //case skill 1 active
        if (_activeSkill1)
        {
			if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))    //clic droit
			{
				//Creation d'un plan destiné a récupérer la position de l'objet
				Plane playerPlane = new Plane(Vector3.up, _myPlayer.position);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				float hitdist = 0.0f;
				if (playerPlane.Raycast(ray, out hitdist))
				{
					
					_lastPositionClicked = ray.GetPoint(hitdist);
					
					if (!_mycone)
					{
						_mycone = Instantiate(_ConePrefab) as Transform;
					}
					if (_MoveList.Count > 0)
					{
						_mycone.position = _MoveList[_MoveList.Count - 1];
						Debug.Log ("LOOOOL");
					}
					else
					{
						_mycone.position = _myPlayer.position;
					}
					_mycone.localScale = new Vector3(1, 1, 1);
					//_mycone.position = _myPlayer.position;
					var rotate = Quaternion.LookRotation ( _lastPositionClicked - _mycone.position).eulerAngles;

					_mycone.rotation = Quaternion.Euler(rotate);
					
					_CurrentSkillChoice = new Skill(0, _mycone.position, _mycone.position, _mycone.rotation, 1, 2, 0, 100, true, 200, false, true, true);
				}
				
			}
        }

        //case skill 2 active
        if (_activeSkill2)//sort de zone
        {

            //cas de l'activation ud skill
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))    //clic droit
            {
                //Creation d'un plan destiné a récupérer la position de l'objet
                Plane playerPlane = new Plane(Vector3.up, _myPlayer.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist))
                {

                    _lastPositionClicked = ray.GetPoint(hitdist);

                    if (!_mycircle)
                    {
                        _mycircle = Instantiate(_CirclePrefab) as Transform;
                    }
                    _mycircle.localScale = new Vector3(1, 1, 1);
                    _mycircle.position = _lastPositionClicked;

                    _CurrentSkillChoice = new Skill(1, _myPlayer.position, _lastPositionClicked, Quaternion.identity, 1, 2, 10, 0, true, 200, false, true, true);
                }

            }
        }
        //gestionaire d'evenement de activeSkill3
        if ((_fallowActiveSkill2 != _activeSkill2))
        {
            if (_activeSkill2 == true)
            {
                _fallowActiveSkill2 = true;
            }
            else
            {
                //cas ou on desactive la fonctionalité
                _fallowActiveSkill2 = false;

                if (_mycircle)
                {
                    Destroy(_mycircle.gameObject);
                }

            }
        }



        //case skill 3 active
        if (_activeSkill3)  //potion
        {

            //cas de l'activation du skill
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))    //clic droit
            {
                //Creation d'un plan destiné a récupérer la position de l'objet
                Plane playerPlane = new Plane(Vector3.up, _myPlayer.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist))
                {
                    
                    _lastPositionClicked = ray.GetPoint(hitdist);

                    if (!_mycircle)
                    {
                        _mycircle = Instantiate(_CirclePrefab) as Transform;
                    }
                    _mycircle.localScale = new Vector3( 1,1,1);
                    _mycircle.position = _lastPositionClicked;
                    
                    _CurrentSkillChoice = new Skill(1, _myPlayer.position, _lastPositionClicked, Quaternion.identity, 1, 4, 10, 0, false, 200, false, true, true);
                }

            }

        }
        //gestionaire d'evenement de activeSkill3
        if ((_fallowActiveSkill3 != _activeSkill3))
        {
            if (_activeSkill3 == true)
            {
                _fallowActiveSkill3 = true;
            }
            else
            {
                //cas ou on desactive la fonctionalité
                _fallowActiveSkill3 = false;

                if (_mycircle)
                {
                    Destroy(_mycircle.gameObject);
                }
                    
            }
        }



        //case skill movePLayer active
        if (_activeMovement)
        {
            //cas de l'activation ud skill
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))    //clic droit
            {
                //Creation d'un plan destiné a récupérer la position de l'objet
                Plane playerPlane = new Plane(Vector3.up, _myPlayer.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist))
                {
                    _lastPositionClicked = ray.GetPoint(hitdist);

                    float PlayerSpeed = _myPlayerSKillResolver._MySpeed;                //distance parcourue en 1s par le personnage
                    Vector3 myMove;
                    if (_MoveList.Count > 0)
                    {
                        myMove = _lastPositionClicked - _MoveList[_MoveList.Count - 1]; //vecteur de deplacement du personnage
                    }
                    else
                    {
                        myMove = _lastPositionClicked - _myPlayer.position;             //vecteur de deplacement du personnage
                    }


                    float length = myMove.magnitude;
                    float rest = length % PlayerSpeed;                          //le surplus de distance à parcourir
                    float numberOfMove = (length - rest) / PlayerSpeed;         //le nombre de vitesses parcourues pour le mouvement actuel
                    if (rest > 0) numberOfMove++;

                    Vector3 myMoveResult = myMove.normalized * PlayerSpeed * numberOfMove;

                    //choix des positions de la ligne
                    if (_MoveList.Count > 0)
                    {
                        _myLine.SetPosition(0, _MoveList[_MoveList.Count - 1]);
                    }
                    else
                    {
                        _myLine.SetPosition(0, _myPlayer.position);
                    }
                    _myLine.SetPosition(1, _lastPositionClicked);

                    //gestion de la ring
                    if (!_endMovementRing)
                    {

                        _endMovementRing = Instantiate(_endMovementRingPrefab) as Transform;
                        _endMovementRing.GetComponentInChildren<BarFallowCamera>()._myCamera = _myCamera;
                        _endMovementRingText = _endMovementRing.GetComponentInChildren<Text>();
                    }
                    _endMovementRing.position = _lastPositionClicked;
                    _endMovementRingText.text = numberOfMove.ToString();

                    //upadate the actual Skill
                    if (_MoveList.Count > 0)
                    {
                        _CurrentSkillChoice = new Skill(_MoveList[_MoveList.Count - 1], _lastPositionClicked, numberOfMove);
                    }
                    else
                    {
                        _CurrentSkillChoice = new Skill(_myPlayer.position, _lastPositionClicked, numberOfMove);
                    }

                }
            }

        }

        //gestionaire d'evenement de activeMovement
        if ((_fallowActiveMovement != _activeMovement))
        {
            if (_activeMovement == true)
            {
                _fallowActiveMovement = true;
            }
            else
            {
                //cas ou on desactive la fonctionalité
                _fallowActiveMovement = false;

                if (_endMovementRing)
                    Destroy(_endMovementRing.gameObject);
                Vector3 nullPosition = new Vector3(0, 0, 0);
                _myLine.SetPosition(0, nullPosition);
                _myLine.SetPosition(1, nullPosition);
            }
        }




        //case special action

        if (_activeSpecialAction)
        {

        }


    }

    /******************************\
    |   switchSkill1Active         |
    \******************************/
    public void switchSkill1Active()
    {
        if (_activeSkill1)
        {
            //rends inactif
            _buttonAction1.active = true;
            _buttonAction1_cancel.active = false;
            _activeSkill1 = false;

            _buttonValidate.active = false;
            Debug.Log(_buttonValidate.active);
        }
        else
        {
            //rends actif
            _buttonValidate.active = true;
            // TODO : le boutton ci dessus ne s'active pas malheuruesement a voir

            _buttonAction1.active = false;
            _buttonAction1_cancel.active = true;
            _activeSkill1 = true;

            //rends inactif 2 3 move et special action
            _buttonAction2.active = true;
            _buttonAction2_cancel.active = false;
            _activeSkill2 = false;

            _buttonAction3.active = true;
            _buttonAction3_cancel.active = false;
            _activeSkill3 = false;

            _buttonMove.active = true;
            _buttonMove_cancel.active = false;
            _activeMovement = false;

            _buttonSpecialAction.active = true;
            _buttonSpecialAction_cancel.active = false;
            _activeSpecialAction = false;

        }
		Debug.Log(_buttonValidate.active);
		Debug.Log("FUCK");
    }

    /***********************************************************\
    |   switchSkill2Active
    \***********************************************************/
    public void switchSkill2Active()
    {
        if (_activeSkill2)
        {
            //rends inactif
            _buttonAction2.active = true;
            _buttonAction2_cancel.active = false;
            _activeSkill2 = false;
            _buttonValidate.active = false;
        }
        else
        {
            //rends actif
            _buttonValidate.active = true;

            _buttonAction2.active = false;
            _buttonAction2_cancel.active = true;
            _activeSkill2 = true;

            //rends inactif, 1, 3, special et move
            _buttonAction1.active = true;
            _buttonAction1_cancel.active = false;
            _activeSkill1 = false;

            _buttonAction3.active = true;
            _buttonAction3_cancel.active = false;
            _activeSkill3 = false;

            _buttonMove.active = true;
            _buttonMove_cancel.active = false;
            _activeMovement = false;

            _buttonSpecialAction.active = true;
            _buttonSpecialAction_cancel.active = false;
            _activeSpecialAction = false;

        }
		Debug.Log("FUCK");
    }

    /***********************************************************\
    |   switchSkill3Active
    \***********************************************************/
    public void switchSkill3Active()
    {
        if (_activeSkill3)
        {
            //rends inactif
            _buttonAction3.active = true;
            _buttonAction3_cancel.active = false;
            _activeSkill3 = false;
            _buttonValidate.active = false;
        }
        else
        {
            //rends actif
            _buttonValidate.active = true;

            _buttonAction3.active = false;
            _buttonAction3_cancel.active = true;
            _activeSkill3 = true;

            //rends inactif 1, 2, special et move
            _buttonAction1.active = true;
            _buttonAction1_cancel.active = false;
            _activeSkill1 = false;

            _buttonAction2.active = true;
            _buttonAction2_cancel.active = false;
            _activeSkill2 = false;

            _buttonMove.active = true;
            _buttonMove_cancel.active = false;
            _activeMovement = false;

            _buttonSpecialAction.active = true;
            _buttonSpecialAction_cancel.active = false;
            _activeSpecialAction = false;

        }
    }

    /***********************************************************\
    |   switchMoveActive                                        |
    \***********************************************************/
    public void switchMoveActive()
    {
        if (_activeMovement)
        {
            //rends inactif
            _buttonMove.active = true;
            _buttonMove_cancel.active = false;
            _activeMovement = false;
            _buttonValidate.active = false;
        }
        else
        {
            //rends actif
            _buttonValidate.active = true;

            _buttonMove.active = false;
            _buttonMove_cancel.active = true;
            _activeMovement = true;

            //rends inactif 1,2,3 et special
            _buttonAction1.active = true;
            _buttonAction1_cancel.active = false;
            _activeSkill1 = false;

            _buttonAction2.active = true;
            _buttonAction2_cancel.active = false;
            _activeSkill2 = false;

            _buttonAction3.active = true;
            _buttonAction3_cancel.active = false;
            _activeSkill3 = false;

            _buttonSpecialAction.active = true;
            _buttonSpecialAction_cancel.active = false;
            _activeSpecialAction = false;
        }
    }

    /***********************************************************\
    |   specialAction                                           |
    \***********************************************************/
    public void switchSpecialAction()
    {
        if (_activeSpecialAction)
        {
            //rends inactif
            _buttonSpecialAction.active = true;
            _buttonSpecialAction_cancel.active = false;
            _activeSpecialAction = false;
            _buttonValidate.active = false;
        }
        else
        {
            //rends actif
            _buttonValidate.active = true;

            _buttonSpecialAction.active = false;
            _buttonSpecialAction_cancel.active = true;
            _activeSpecialAction = true;

            //rends inactif 1,2,3 et move
            _buttonAction1.active = true;
            _buttonAction1_cancel.active = false;
            _activeSkill1 = false;

            _buttonAction2.active = true;
            _buttonAction2_cancel.active = false;
            _activeSkill2 = false;

            _buttonAction3.active = true;
            _buttonAction3_cancel.active = false;
            _activeSkill3 = false;

            _buttonMove.active = true;
            _buttonMove_cancel.active = false;
            _activeMovement = false;
        }
    }

    /**************************************************************************************\
    |   SendActualSkillToTimeLine() : a validation function who send the skill to timeLine |
    \**************************************************************************************/
    public void SendActualSkillToTimeLine()
    {
        //TODO : coder SendActualSkillToTimeLine()
        bool isPossible = _mytimeLine.addSkill(_CurrentSkillChoice);

        if (isPossible)
        {
            if (_CurrentSkillChoice._type == 4)
            {
                _MoveList.Add(_CurrentSkillChoice._location);

				Transform MovementRing;

				if (_CurrentSkillChoice._type == 4)
					{
	                MovementRing = Instantiate(_endMovementRingPrefab) as Transform;
					MovementRing.GetComponentInChildren<BarFallowCamera>()._myCamera = _myCamera;
					MovementRing.GetComponentInChildren<Text>().text = _endMovementRingText.text.ToString();
					Destroy ((_endMovementRing as Transform).gameObject); 
					MovementRing.position = _CurrentSkillChoice._location;
					_listOfRingsOfMovements.Add(MovementRing);
				}
				if (_CurrentSkillChoice._type == 1) {
					MovementRing = Instantiate(_ConePrefab) as Transform;
					Destroy ((_mycone as Transform).gameObject); 
					MovementRing.position = _CurrentSkillChoice._location;
					_listOfRingsOfMovements.Add(MovementRing);
				}

				if (_CurrentSkillChoice._type == 2) {
					MovementRing = Instantiate(_CirclePrefab) as Transform;
					Destroy ((_mycircle as Transform).gameObject); 
					MovementRing.position = _CurrentSkillChoice._location;
					_listOfRingsOfMovements.Add(MovementRing);
				}


				if (_CurrentSkillChoice._type == 3)
				{

				}
                
			}
 
        }

    }

    public void clearActions()  //called when the timeLine is resolved
    {
        //suppression des positions et reset de la position du joueur
        for (int i = (_MoveList.Count - 1); i >= 0; i--)
        {
            _MoveList.RemoveAt(i);
        }
        _MoveList.Add(_myPlayer.position);

        //supression de toutes les positions de mouvements
        for (int i = (_listOfRingsOfMovements.Count - 1); i >= 0; i--)
        {
            Destroy(_listOfRingsOfMovements[i].gameObject);
            _listOfRingsOfMovements.RemoveAt(i);
        }

        _myLine.SetPosition(0, new Vector3(0, 0, 0));
        _myLine.SetPosition(1, new Vector3(0, 0, 0));

    }

	public void removeLastSkill(){

		_MoveList.RemoveAt (_MoveList.Count - 1); 

		Vector3 allMoves = new Vector3 (0,0,0);

		for (int i = 0; i < _MoveList.Count; i++) {
			allMoves += _MoveList[i];
				}

		_lastPositionClicked = _myPlayer.position + allMoves;


		Transform endRingtoDestroy;
		endRingtoDestroy = _listOfRingsOfMovements [_MoveList.Count - 1];
		_listOfRingsOfMovements.RemoveAt (_MoveList.Count - 1);
		Destroy ((endRingtoDestroy as Transform).gameObject); 

	}


}
