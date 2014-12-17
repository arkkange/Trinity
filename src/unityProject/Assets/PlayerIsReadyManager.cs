using UnityEngine;
using System.Collections;

public class PlayerIsReadyManager : MonoBehaviour {

    [SerializeField]
    public Transform _buttonIsReady;

    [SerializeField]
    public Transform _buttonIsNotReady;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActiveReady()
    {
        _buttonIsReady.active = true;
    }

    public void UnactiveReady()
    {
        _buttonIsReady.active = false;
    }


    public void ActiveNotReady()
    {
        _buttonIsNotReady.active = true;
    }

    public void UnactiveNotReady()
    {
        _buttonIsNotReady.active = false;
    }

}
