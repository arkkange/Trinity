using UnityEngine;
using System.Collections;

public class DelegateScript : MonoBehaviour {

	delegate void MyDelegate(int num);
	MyDelegate myDelegate;

	// Use this for initialization
	void Start () {
		myDelegate = printNum;
		myDelegate (50);

		myDelegate = DoubleNum;
		myDelegate (50);
	}

	void printNum(int num)
	{
		print ("print Num : " + num);
	}

	void DoubleNum(int num)
	{
		print ("Double Num : " + num * 2);
	}
}

