using UnityEngine;
using System.Collections;

public class HealthBarManager : MonoBehaviour {

    [SerializeField]
    RectTransform _myImage;

    [SerializeField]
    Transform _myHealthManager;

    HealthManager _myHealthManagerScript;

    // Use this for initialization
    void Start()
    {
        _myHealthManagerScript = _myHealthManager.GetComponent<HealthManager>();
    }

    /***********************************************************\
    |   Update : Fonction apellée une fois par frame            |
    \***********************************************************/
    void Update () {

        UpdateBar();
       
	}


    void UpdateBar()
    {

        float lifepercentage = (_myHealthManagerScript._ActualLife / _myHealthManagerScript._MaxLife);
            _myImage.anchorMax = new Vector2(lifepercentage, 1);

    }

}
