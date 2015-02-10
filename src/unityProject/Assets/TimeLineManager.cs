using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeLineManager : MonoBehaviour {


    List<RectTransform> PanelList = new List<RectTransform>();
    List<SkillTest> skillTestList = new List<SkillTest>();      //listes de skills gérée uniquement dans time line pour récupérer les infos;

    [SerializeField]
    RectTransform PanelPrefab;

    [SerializeField]
    RectTransform _myPanelOfPanelsArea;  //le panel qui contient les autres zones de couleur

    [SerializeField]
    RectTransform _myText;

    [SerializeField]
    RectTransform myCancelButton;
    
    //panel position in %
    float   _myActualAnchorPosition = 0;
    int     _myActualTimeValue;


    void addSkillTimeline(SkillTest newSkill)
    {
        float SkillCastTime = newSkill._castTime;

        if ((_myActualTimeValue + SkillCastTime) <= 10)
        {

            RectTransform _newPortion = Instantiate(PanelPrefab) as RectTransform;
            _newPortion.parent = _myPanelOfPanelsArea;

            //reset Achors of our portion to no border
            _newPortion.localPosition = new Vector3(0, 0, 0);
            _newPortion.offsetMax = new Vector2(0, 0);
            _newPortion.offsetMin = new Vector2(0, 0);
            _newPortion.anchorMin = new Vector2(_myActualAnchorPosition, 0);
            _newPortion.anchorMax = new Vector2(_myActualAnchorPosition + (SkillCastTime/10), 1);

            //MAj anchors and time value
            _myActualAnchorPosition += SkillCastTime / 10;
            _myActualTimeValue += (int)SkillCastTime;

            //changement de couleur
            _newPortion.GetComponent<Image>().color = newSkill._ColorTimeLineSkill1;

            //CHangement de texte
            _newPortion.GetComponentInChildren<Text>().text = _myActualTimeValue.ToString();

            //ajout a la liste de portions
            PanelList.Add(_newPortion);
            //ajout a la liste de skill
            skillTestList.Add(newSkill);

            //active cancel button
            myCancelButton.gameObject.SetActive(true);
        }

        Debug.Log(_myActualTimeValue);

        
    }



    public void removeLastSkillFromTimeline()
    {

        int lastPanelIndex = PanelList.Count - 1;
        int lastSkillIndex = skillTestList.Count - 1;

        if (PanelList.Count != 0 && skillTestList.Count != 0)
        {

            //PanelList gestion
            RectTransform LastPanel = PanelList[lastPanelIndex];
            Destroy(LastPanel.gameObject);
            PanelList.RemoveAt(lastPanelIndex);

            //calculate new actual anchor
            _myActualAnchorPosition -= (float)(skillTestList[lastSkillIndex]._castTime / 10);

            //actual time Value MAJ
            _myActualTimeValue -= (int)skillTestList[lastSkillIndex]._castTime;
            Debug.Log(_myActualTimeValue);

            //remove the skill form list
            skillTestList.RemoveAt(skillTestList.Count - 1);

            //event of the click
            cancelSkillEvent();

            if (_myActualTimeValue == 0)
            {
                myCancelButton.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log(" tous différents de 0");
            myCancelButton.gameObject.SetActive(false);
        }


    }




    //evenement de suppression du dernier skill
    void cancelSkillEvent()
    {

    }

}