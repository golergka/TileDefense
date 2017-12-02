using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Damager))]
public class Enemy : MonoBehaviour
{
	#region Components

	private NavMeshAgent navMeshAgent;
	private NavMeshAgent NavMeshAgent
	{
		get
		{
			return navMeshAgent ?? (navMeshAgent = GetComponent<NavMeshAgent>());
		}
	}

	private Damager damageDealer;
	private Damager Damager
	{
		get
		{
			return damageDealer ?? (damageDealer = GetComponent<Damager>());
		}
	}

	#endregion

	public int GoldReward;

	IDamageReceiver target;
	Transform targetTransform;

	public void Init<T>(T target)
		where T : Component, IDamageReceiver
	{
		this.target = target;
		targetTransform = target.transform;
		currentState = new MovementState(this);
	}

	void OnDrawGizmosSelected()
	{
		if (targetTransform != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, targetTransform.position);
		}
	}

	#region States
	
	EnemyState currentState;

	abstract class EnemyState
	{
		protected readonly Enemy enemy;

		public EnemyState(Enemy enemy)
		{
			this.enemy = enemy;
		}

		public abstract void Update();
		public abstract void RebuildPath();
	}

	class MovementState : EnemyState
	{
		public MovementState(Enemy enemy) : base(enemy)
		{
			RebuildPath();
		}

		public override void Update() 
		{ 
			var targetDelta = enemy.targetTransform.position - enemy.transform.position;
			var closeEnough = targetDelta.sqrMagnitude <= Mathf.Pow(enemy.NavMeshAgent.stoppingDistance, 2f);

			if (closeEnough)
			{
				enemy.currentState = new KamikadzeState(enemy);
			}
		}

		public override void RebuildPath()
		{
			enemy.NavMeshAgent.SetDestination(enemy.targetTransform.position);
		}
	}

	class KamikadzeState : EnemyState
	{
		readonly float startTime;
		const float KAMIKADZE_TIME = 1f;

		public KamikadzeState(Enemy enemy) : base(enemy)
		{
			startTime = Time.time;
			enemy.Damager.DamageInstantly(enemy.target);
		}

		public override void Update()
		{
			if (Time.time - startTime >= KAMIKADZE_TIME)
			{
				Destroy(enemy.gameObject);
			}
		}

		public override void RebuildPath()
		{
			// Do nothing
		}
	}

	#endregion

	#region State delegates

	public void RebuildPath()
	{
		if (currentState != null)
		{
			currentState.RebuildPath();
		}
	}

	void Update()
	{
		if (currentState != null)
		{
			currentState.Update();
		}
	}

	#endregion

}
