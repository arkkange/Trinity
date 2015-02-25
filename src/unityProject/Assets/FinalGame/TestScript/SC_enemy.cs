using UnityEngine;
using System.Collections;

public class SC_enemy : MonoBehaviour {

	[SerializeField]
	private NavMeshAgent _agent;

	public int _i_hp = 1;
	public float _f_speed = 3f;



	void Start()
	{
		transform.localRotation = Quaternion.LookRotation(-transform.position);
		_agent.speed = _f_speed;
		_agent.destination = Vector3.zero;
	}

	void OnEnable()
	{
		_agent.destination = Vector3.zero;
	}

	public void DealDamage(int i_damage)
	{
		_i_hp -= i_damage;
		if (_i_hp < 1)
		{
			//SC_game_manager._instance.EnemyKilled();
			Destroy(gameObject);
		}
	}
}
