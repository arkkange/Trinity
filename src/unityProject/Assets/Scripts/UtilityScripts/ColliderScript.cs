using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderScript : MonoBehaviour
{

    public List<Transform> _TransformListOfCollisions = new List<Transform>();

    /***********************************************************\
    |   OnTriggerEnter : recupère la liste des collisions       |
    \***********************************************************/
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        if (!_TransformListOfCollisions.Contains(other.gameObject.transform))
        {
            _TransformListOfCollisions.Add(other.gameObject.transform);
            if (other.gameObject.tag != "Player")
            {
                Debug.Log("enter : " + other.gameObject.tag);
            }
            
         }
    }

    /***********************************************************\
    |   OnTriggerEnter : recupère la liste des collisions       |
    \***********************************************************/
    void OnTriggerExit(Collider other)
    {
        _TransformListOfCollisions.Remove(other.gameObject.transform);
        if (other.gameObject.tag != "Player")
        {
            Debug.Log("Exit : " + other.gameObject.tag);
        }
    }

    /***********************************************************************\
    |   GetListOfCollisions : Donne la liste des objets dans le collider    |
    \***********************************************************************/
    List<Transform> GetListOfCollisions()
    {
        return _TransformListOfCollisions;
    }

}
