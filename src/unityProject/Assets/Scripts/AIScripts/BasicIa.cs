using UnityEngine;
using System.Collections;

public class BasicIa : MonoBehaviour {

	[SerializeField]
	scriptSkillSet skills;

	[SerializeField]
	GameObject thisObj;

	public Vector3 startLocation;

	public float speed = 5.0f;

	bool attackIs = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

	void checkMovement()
	{
		float distanceFromStart = Vector3.Distance (transform.position, startLocation);
		Vector3 point = startLocation - transform.position;

		var distance = Vector3.Distance (transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
		Vector3 PlayerIsHere =  GameObject.FindGameObjectWithTag ("Player").transform.position - transform.position;

		var rotate = Quaternion.LookRotation (GameObject.FindGameObjectWithTag ("Player").transform.position - transform.position).eulerAngles;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(rotate), Time.deltaTime * 3.0f);

		Debug.Log (distance);

		if (distance < 20 && distance > 2) {
			PlayerIsHere.y = 0;
			//Debug.Log("comming");
			thisObj.rigidbody.velocity = PlayerIsHere.normalized * speed;
		} else if(distance > 0 && distance < 2) {
			thisObj.rigidbody.velocity = new Vector3(0,0,0);
			//Debug.Log("attacking");
			attackIs = true;
		} else {
			if (distanceFromStart > 0.2f) {
				//Debug.Log("returning");
				point.y = 0;
				thisObj.rigidbody.velocity = point.normalized * speed;
			} else {
				//Debug.Log("don't move");
				thisObj.rigidbody.velocity = new Vector3(0,0,0);
			}
		}
	}

	public void attack() {
		Debug.Log("attacking");
		//hisTransform.rotation * 
			StartCoroutine(skills.playerSkillSet[0].skillResolve(gameObject,transform.rotation *Vector3.forward, 0));
	}

	public IEnumerator IaSetOK()
	{
		float time = 10;
		float i = 0;
		while(i < time)
		{
			checkMovement();
			if(attackIs) {
				Debug.Log("TA GUEULE");
				attackIs = false;
				attack ();
				i += 2.0f;
				yield return new WaitForSeconds(2.0f);
			} else {
				i += Time.deltaTime;
				yield return null;
			}
		}
	}

	enum Stances {
		attack,
		block,
		flee,
		waitInFormation
	}
}
