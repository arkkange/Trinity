using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderScript : MonoBehaviour
{

    //remarque : les colliders des objets de type trigger ne collident qu'avec les minions et les player

    public List<Transform> _TransformListOfCollisions = new List<Transform>();

    /***********************************************************\
    |   OnTriggerEnter : recupère la liste des collisions       |
    \***********************************************************/
    void OnTriggerEnter(Collider other)
    {
        if (!_TransformListOfCollisions.Contains(other.gameObject.transform))
        {
            _TransformListOfCollisions.Add(other.gameObject.transform);          
         }
    }

    /***********************************************************\
    |   OnTriggerEnter : recupère la liste des collisions       |
    \***********************************************************/
    void OnTriggerExit(Collider other)
    {
        _TransformListOfCollisions.Remove(other.gameObject.transform);
    }

    /***********************************************************************\
    |   GetListOfCollisions : Donne la liste des objets dans le collider    |
    \***********************************************************************/
    List<Transform> GetListOfCollisions()
    {
        return _TransformListOfCollisions;
    }

}
