using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkillResolver : MonoBehaviour
{
    [SerializeField]
    Transform _myTransform;

    List<Transform> _TransformsHits = new List<Transform>();

    //[SerializeField]
    //Transform _myAttacks;   //to change the direction where the skill is casting

    [SerializeField]
    public float _MySpeed;

    [SerializeField]
    Transform _myCircle;

    [SerializeField]
    Transform _myCone;

    [SerializeField]
    Transform _myLine;

    [SerializeField]
    string _playersTag;

    [SerializeField]
    string _minionsTag;


    /******************************************************************************************\
    |   ResolveSkill : creation de la coroutine associée a la resolution du skill en question  |
    \******************************************************************************************/
    public void ResolveSkill(Skill mySkill ){

        switch (mySkill._type)
        {
            case 1: //circle
                StartCoroutine(resolveCircle(mySkill));
                break;

            case 2: //cone
                StartCoroutine(resolveCone(mySkill));
                break;

            case 3: //Line
                StartCoroutine(resolveLine(mySkill));
                break;

            case 4: //move
                
                break;

            case 5: //environment action
                
                break;

            default:
                Debug.Log("skill type error");
                break;
        }

    }

    IEnumerator resolveCircle(Skill mySkill)
    {
        yield return new WaitForFixedUpdate(); //toujours le mettre avant de manipuler un objet (comprenant un rigidbody)
        //Circle
        Vector3 myPosition = this.transform.position;

        //repositionement du cercle (sous forme de transformation vectorielle pour que soit detecté le deplacement dans le trigger)
        _myCircle.Translate( (mySkill._location - _myCircle.position)  , Space.World);
        _myCircle.localScale = new Vector3(mySkill._scaleModifier, mySkill._scaleModifier, mySkill._scaleModifier);

        _myCircle.GetComponentInChildren<MeshRenderer>().enabled = true;

        //waiting for cast Time
        yield return new WaitForSeconds( mySkill._castTime );
        yield return new WaitForFixedUpdate(); //toujours le mettre avant de manipuler un objet (comprenant un rigidbody)

        //get the collisions
        _TransformsHits = _myCircle.GetComponentInChildren<ColliderScript>()._TransformListOfCollisions;


        //resolution concrete
        if (mySkill._affectMinions == false)
        {
            removeMinionsFromTransformsHitsList();
        }

        if (mySkill._affectPlayers == false)
        {
            removePlayersFromTransformsHitsList();
        }

        if (mySkill._isDamage)
        {
            for (int i = 0; i < _TransformsHits.Count; i++)
            {
                if ( _TransformsHits[i] != _myTransform )
                {
                    Debug.Log(_TransformsHits[i].tag);
                    _TransformsHits[i].GetComponent<HealthManager>().damage(mySkill._damageValue);
                }
                
            }
        }
        else
        {
            if (mySkill._isResurection)
            {
                //case resurection
                for (int i = 0; i < _TransformsHits.Count; i++)
                {
                    _TransformsHits[i].GetComponent<HealthManager>().revive(mySkill._healValue);
                }
            }
            else
            {
                //case heal
                for (int i = 0; i < _TransformsHits.Count; i++)
                {
                    _TransformsHits[i].GetComponent<HealthManager>().heal(mySkill._healValue);
                }
            }
        }

        //masque du visiuel
        _myCircle.GetComponentInChildren<MeshRenderer>().enabled = false;

    }

    IEnumerator resolveCone(Skill mySkill)
    {   
        //Cone
        // TODO : coder resolveCone ( penser a modifier le meshcollider en composants de colliders simples )


        //waiting for cast Time
        yield return new WaitForSeconds(mySkill._castTime);
        yield return new WaitForFixedUpdate(); //toujours le mettre avant de manipuler un objet (comprenant un rigidbody)

    }

    IEnumerator resolveLine(Skill mySkill)
    {
        //Line
        // TODO : coder resolveLine  (penser a modifier le meshcollider en composants de colliders simples)


        //waiting for cast Time
        yield return new WaitForSeconds(mySkill._castTime);
        yield return new WaitForFixedUpdate(); //toujours le mettre avant de manipuler un objet (comprenant un rigidbody)
    }


    /******************************************************************************************\
    |   removeMinionsFromTransformsHitsList                                                    |
    \******************************************************************************************/
    private void removeMinionsFromTransformsHitsList()
    {
        for(int i=0; i < (_TransformsHits.Count -1) ; i++){
            if (_TransformsHits[i].tag == _minionsTag)
            {
                _TransformsHits.RemoveAt(i);
            }
        }

    }


    /******************************************************************************************\
    |   removePlayersFromTransformsHitsList                                                    |
    \******************************************************************************************/
    private void removePlayersFromTransformsHitsList()
    {
        for (int i = 0; i < (_TransformsHits.Count - 1); i++)
        {
            if (_TransformsHits[i].tag == _playersTag)
            {
                _TransformsHits.RemoveAt(i);
            }
        }

    }



}
