using UnityEngine;
using System.Collections;
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
    bool _activeAction;

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

    //visual objects
    [SerializeField]
    Transform _myLineRenderObject;
    LineRenderer _myLine;

    [SerializeField]
    Transform _endMovementRingPrefab;
    Transform _endMovementRing;
    Text _endMovementRingText;

	// Use this for initialization
	void Start () {

        //getcomponents a récupérer pour économiser la mémoire
        _mytimeLine = _myTimeLineObject.GetComponent<TimeLine>();
        _myLine = _myLineRenderObject.GetComponent<LineRenderer>();
        _myPlayerSKillResolver = _myPlayer.GetComponent<PlayerSkillResolver>();

        _activeSkill1   = false;
        _activeSkill2   = false;
        _activeSkill3   = false;
        _activeMovement = false;
        _activeAction   = false;

        _fallowActiveSkill1 = false;
        _fallowActiveSkill2 = false;
        _fallowActiveSkill3 = false;
        _fallowActiveMovement = false;
        _fallowActiveAction = false;

	}
	
	// Update is called once per frame
	void Update () {

	    //case skill 1 active
        if (_activeSkill1)
        {

        }

        //case skill 2 active
        if (_activeSkill2)
        {

        }

        //case skill 3 active
        if (_activeSkill3)
        {

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

                    float PlayerSpeed = _myPlayerSKillResolver._MySpeed;        //distance parcourue en 1s par le personnage
                    Vector3 myMove = _lastPositionClicked - _myPlayer.position; //vecteur de deplacement du personnage

                    float length = myMove.magnitude;
                    float rest = length % PlayerSpeed;                          //le surplus de distance à parcourir
                    float numberOfMove = (length - rest) / PlayerSpeed;         //le nombre de vitesses parcourues pour le mouvement actuel
                    if(rest > 0)numberOfMove ++;

                    Vector3 myMoveResult = myMove.normalized * PlayerSpeed * numberOfMove;

                    //coix des positions de la ligne
                    _myLine.SetPosition(0, _myPlayer.position);
                    _myLine.SetPosition(1, _lastPositionClicked);

                    //gestion de la ring
                    if(!_endMovementRing){
                        _endMovementRing = Instantiate(_endMovementRingPrefab) as Transform;
                        _endMovementRing.GetComponentInChildren<BarFallowCamera>()._myCamera = _myCamera;
                        _endMovementRingText = _endMovementRing.GetComponentInChildren<Text>();
                    }
                    _endMovementRing.position = _lastPositionClicked;
                    _endMovementRingText.text = numberOfMove.ToString();

                    //upadate the actual Skill
                    _CurrentSkillChoice = new Skill(4, _lastPositionClicked, numberOfMove);
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

        //case

        if (_activeAction)
        {

        }

	}

    /***********************************************************\
    |   switchSkill1Active
    \***********************************************************/
    public void switchSkill1Active()
    {
        if (_activeSkill1)
        {
            //rends inactif
            _buttonAction1.active = true;
            _buttonAction1_cancel.active = false;
            _activeSkill1 = false;
            _buttonValidate.active = false;
            // TODO : evenements de suppression du skill
        }
        else
        {
            //rends actif
            _buttonValidate.active = true;
            // TODO : le boutton ci dessus ne s'active pas malheuruesement a voir

            _buttonAction1.active = false;
            _buttonAction1_cancel.active = true;
            _activeSkill1 = true;
            _buttonValidate.active = false;

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
            _activeAction = false;

        }
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
            // TODO : evenements de suppression du skill
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
            _activeAction = false;

        }
    }

    /***********************************************************\
    |   switchSkill3Active
    \***********************************************************/
    public void switchSkill3Active()
    {
        if (_activeSkill3) //est-il actif ? oui/non
        {
            //rends inactif
            _buttonAction3.active = true;
            _buttonAction3_cancel.active = false;
            _activeSkill3 = false;
            _buttonValidate.active = false;
            // TODO : evenements de suppression du skill
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
            _activeAction = false;

        }
    }

    /***********************************************************\
    |   switchMoveActive
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
            _activeAction = false;
        }
    }

    /***********************************************************\
    |   specialAction
    \***********************************************************/
    public void switchSpecialAction()
    {
        if (_activeAction)
        {
            //rends inactif
            _buttonSpecialAction.active = true;
            _buttonSpecialAction_cancel.active = false;
            _activeAction = false;
            _buttonValidate.active = false;
        }
        else
        {
            //rends actif
            _buttonValidate.active = true;

            _buttonSpecialAction.active = false;
            _buttonSpecialAction_cancel.active = true;
            _activeAction = true;

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

    /************************************************************\
    |   _debugBarStates : affiche les différents etats de la bar |
    \************************************************************/
    public void _debugBarStates()
    {
        Debug.Log("etat du skill 1 : "+_activeSkill1);
        Debug.Log("etat du skill 2 : "+_activeSkill2);
        Debug.Log("etat du skill 3 : "+_activeSkill3);
        Debug.Log("etat du mouvement : "+_activeMovement);
        Debug.Log("etat du special : "+_activeAction);
    }

    /***********************************************************\
    |   SendActualSkillToTimeLine() : a validation function who send the skill to timeLine
    \***********************************************************/
    public void SendActualSkillToTimeLine()
    {
        //TODO : coder SendActualSkillToTimeLine()

    }

}
