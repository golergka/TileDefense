using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	
	[SerializeField] Enemy spawned;
	[SerializeField] float radius;
	[SerializeField] float period;

	Health target;
	List<Enemy> spawnedEnemies = new List<Enemy>();

	public void Init(Health target)
	{
		this.target = target;
		StartCoroutine(Spawn());
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, radius);
	}

	IEnumerator Spawn()
	{
		while(true)
		{
			yield return new WaitForSeconds(period);
			var posXY = Random.insideUnitCircle * radius;
			var pos = new Vector3(posXY.x, 0f, posXY.y);
			var enemy = Instantiate(spawned, transform.position + pos, Quaternion.Euler(0f, Random.value * 360f, 0f)) as Enemy;
			enemy.Init(target);
			enemy.GetComponent<Health>().OnDie += delegate
			{
				spawnedEnemies.Remove(enemy);
				OnEnemyDie(enemy);
				Destroy(enemy.gameObject);
			};
			spawnedEnemies.Add(enemy);
		}
	}

	public event System.Action<Enemy> OnEnemyDie = delegate{};

	public void RebuildPaths()
	{
		foreach(var e in spawnedEnemies)
		{
			e.RebuildPath();
		}
	}
}
