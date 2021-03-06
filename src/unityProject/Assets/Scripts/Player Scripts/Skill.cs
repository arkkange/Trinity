﻿using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

    public int              _type;              // 1:circle     2:cone      3:line      4:movement      5:special action
    public float            _scaleModifier;
    public Vector3          _location;
    public Vector3          _initialLocation;
    public Quaternion       _rotation;
    public float            _castTime;
    public int              _powerValue;
    public float            _damageValue;
    public bool             _isDamage;
    public float            _healValue;
    public bool             _isResurection;
    public bool             _affectPlayers;
    public bool             _affectMinions;
    public bool             _isMovement;


    /***********************************************************\
    |       Constructeur (damage or heal skill)                 |
    \***********************************************************/
    public Skill(int type, Vector3 initialLocation, Vector3 skillLocation, Quaternion rotation, float scaleModifier, float castTime, int powerValue, int damageValue, bool isDamage, int healValue, bool isResurection, bool affectPlayers, bool affectMinions)
    {
        _type               = type;
        _scaleModifier      = scaleModifier;
        _initialLocation    = initialLocation;
        _location           = skillLocation;
        _rotation           = rotation;
        _castTime           = castTime;
        _powerValue         = powerValue;

        _damageValue        = damageValue;
        _isDamage           = isDamage;
        _healValue          = healValue;
        _isResurection      = isResurection;
        _affectPlayers      = affectPlayers;
        _affectMinions      = affectMinions;
        _isMovement         = false;
    }

    /***********************************************************\
    |       Constructeur (mouvement)                            |
    \***********************************************************/
    public Skill(Vector3 initialLocation, Vector3 location, float castTime)    //type Movement de par son constructeur
    {
        _type = 4;
        _isMovement = false;
        _initialLocation = initialLocation;
        _location = location;
        _castTime = castTime;
    }

    public void skillDebug(){
        Debug.Log("skillDebug : " + _type + " , " + _initialLocation + " , " + _location + " , " + _castTime);
    }

}