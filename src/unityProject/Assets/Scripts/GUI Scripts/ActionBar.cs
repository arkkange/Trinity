using UnityEngine;
using System.Collections;

public class ActionBar : MonoBehaviour {

    [SerializeField]
    RectTransform _timeLinePrefab;

	// Use this for initialization
	void Start () {
        TimeLine _timeLine = _timeLinePrefab.GetComponent<TimeLine>();
	}
	
	// Update is called once per frame
	void Update () {
	

	}

}
