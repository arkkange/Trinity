using UnityEngine;
using System.Collections;

public class IaSpawnScript : MonoBehaviour {

	[SerializeField]
	private GameObject _Prefab_enemy;
	[SerializeField]
	private float _f_spawn_radius = 10f;
	[SerializeField]
	private float _f_spawn_delay = 1.5f;
	[SerializeField]
	private float _f_stats_increase_delay = 10f;
	
	private float _f_timer_spawn = 0f;
	private float _f_timer_stats_increase = 0f;
	private bool _b_stats_to_increased = false;
	
	[SerializeField]
	private int _i_enemies_hp_start = 1;
	[SerializeField]
	private float _f_enemies_speed_start = 1f;
	
	
	void Start()
	{
		_Prefab_enemy.GetComponent<SC_enemy>()._i_hp = _i_enemies_hp_start;
		_Prefab_enemy.GetComponent<SC_enemy>()._f_speed = _f_enemies_speed_start;
	}
	
	void Update()
	{
		_f_timer_stats_increase += Time.deltaTime;
		if (_f_timer_stats_increase >= _f_stats_increase_delay)
		{
			_f_timer_stats_increase = 0;
			if (_b_stats_to_increased)
				++_Prefab_enemy.GetComponent<SC_enemy>()._i_hp;
			else
				_Prefab_enemy.GetComponent<SC_enemy>()._f_speed += 0.2f;
			_b_stats_to_increased = !_b_stats_to_increased;
		}
		
		_f_timer_spawn += Time.deltaTime;
		if (_f_timer_spawn >= _f_spawn_delay)
		{
			_f_timer_spawn = 0;
			float f_angle = Random.value * 360;
			f_angle = f_angle * Mathf.Deg2Rad;
			GameObject GO_tmp = Instantiate(_Prefab_enemy, new Vector3(Mathf.Sin(f_angle) * _f_spawn_radius, 0, Mathf.Cos(f_angle) * _f_spawn_radius), Quaternion.identity) as GameObject;
			GO_tmp.transform.parent = transform;
		}
	}
}
