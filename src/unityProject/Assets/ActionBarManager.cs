using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionBarManager : MonoBehaviour {

    public int ActiveButton = 0;

    //ImageSprites
    [SerializeField]
    Image mySpriteSkill1;

    [SerializeField]
    Image mySpriteSkill2;

    [SerializeField]
    Image mySpriteSkill3;

    [SerializeField]
    Image mySpriteSkill4;

    [SerializeField]
    Image mySpriteSkill5;

    //myButtons
    [SerializeField]
    Image Button1Image;

    [SerializeField]
    Image Button2Image;

    [SerializeField]
    Image Button3Image;

    [SerializeField]
    Image Button4Image;

    [SerializeField]
    Image Button5Image;

    [SerializeField]
    GameObject myValidateButton;

    //Colors
    [SerializeField]
    Color ButtonNormalColor;
    [SerializeField]
    Color ButtonSelectedColor;


    //test des fonctions ci après
    void Start()
    {
        
    }


    //Evenements des boutons
    public void OnclickButton(int number)
    {
        switch (number)
        {
            case 1:
                Debug.Log("clicked on button 1 of the actionBar");
                break;
            case 2:
                Debug.Log("clicked on button 2 of the actionBar");
                break;
            case 3:
                Debug.Log("clicked on button 3 of the actionBar");
                break;
            case 4:
                Debug.Log("clicked on button 4 of the actionBar");
                break;
            case 5:
                Debug.Log("clicked on button 5 of the actionBar");
                break;

            default:
                Debug.LogError("le skill "+ number + " n'existe pas!");
                break;
        }

        //set the actual button number
        ActiveButton = number;

        //validate button on
        myValidateButton.SetActive(true);

        //colors changes
        resetAllButtonColors();
        SetButtonHighlightedColor(number);

    }

    public void changeSkillImage(int number, Sprite newSprite){

        switch (number)
        {
            case 1:
                mySpriteSkill1.sprite = newSprite;
                break;
            case 2:
                mySpriteSkill2.sprite = newSprite;
                break;
            case 3:
                mySpriteSkill3.sprite = newSprite;
                break;
            case 4:
                mySpriteSkill4.sprite = newSprite;
                break;
            case 5:
                mySpriteSkill5.sprite = newSprite;
                break;

            default:
                Debug.LogError("le skill " + number + " n'existe pas!");
                break;
        }

    }

    //evenement de validation du skill
    public void Validate()
    {
        Debug.Log("Validate button " + ActiveButton);
        resetAllButtonColors();
        ActiveButton = 0;
        myValidateButton.SetActive(false);
    }


    //color gestion for buttons
    void SetButtonHighlightedColor(int number){
        switch (number)
        {
            case 1:
                Button1Image.color = ButtonSelectedColor;
                break;
            case 2:
                Button2Image.color = ButtonSelectedColor;
                break;
            case 3:
                Button3Image.color = ButtonSelectedColor;
                break;
            case 4:
                Button4Image.color = ButtonSelectedColor;
                break;
            case 5:
                Button5Image.color = ButtonSelectedColor;
                break;

            default:
                Debug.LogError("le skill " + number + " n'existe pas!");
                break;
        }
    }

    void resetAllButtonColors() {
        Button1Image.color = ButtonNormalColor;
        Button2Image.color = ButtonNormalColor;
        Button3Image.color = ButtonNormalColor;
        Button4Image.color = ButtonNormalColor;
        Button5Image.color = ButtonNormalColor;
    }
}
;