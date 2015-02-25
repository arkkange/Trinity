using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scriptSkillSet : MonoBehaviour {
	
	[SerializeField]
	public List<SkillTest> playerSkillSet = new List<SkillTest>();

	[SerializeField]
	public string _name;

	[SerializeField]
	public float MaxHealth;
	[SerializeField]
	public float ActualHealth;

	[SerializeField]
	GameObject player;

	public delegate void SkillTrigger(scriptSkillSet player);
	public static event SkillTrigger playerTriggerSkill;

	public void die(){
		Network.Destroy(player);
	}

	/*void OnSerializeNetworkView(BitStream stream, 
	                                NetworkMessageInfo info) {
		float health = 0;
		if (stream.isWriting) {
			health = ActualHealth;
			stream.Serialize(ref health);
		} else {
			stream.Serialize(ref health);
			ActualHealth = health;
		}	
	}*/

	public void callActualizeHealth()
	{
		networkView.RPC("ActualizeHealth",RPCMode.All,ActualHealth);
	}

	[RPC]
	void ActualizeHealth(float health)
	{
		ActualHealth = health;
	}

	//public event Action<Collider> SkillTrigger;

	/*void OnTriggerEnter(Collider other) {
		Debug.Log(other.name);
	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("this one works");
	}

	void OnTriggerStay(Collider other) {
		Debug.Log ("It works");
	}*/
}
