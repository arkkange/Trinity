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

        // TODO : enlever la serie de tests une fois fonctionel

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _myHealthManagerScript.damage(100.0f);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            _myHealthManagerScript.heal(100.0f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _myHealthManagerScript.revive(100.0f);
        }
       
	}


    void UpdateBar()
    {

        float lifepercentage = (_myHealthManagerScript._ActualLife / _myHealthManagerScript._MaxLife);
            _myImage.anchorMax = new Vector2(lifepercentage, 1);

    }

}
