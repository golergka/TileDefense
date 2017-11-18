using UnityEngine;

public class LevelDaddy : MonoBehaviour
{
	[SerializeField] LevelGenerator generator;
	[SerializeField] UIDaddy uiDaddy;

	void Start()
	{
		var goldWallet = new GoldWallet(100);

		generator.Init(goldWallet);

		generator.BaseHealth.OnCurrentChange += 
			() => Debug.Log("Base health: " + generator.BaseHealth.Current);
		generator.BaseHealth.OnDie += 
			() => Debug.Log("Game over!");

		generator.Spawner.OnEnemyDie +=
			enemy => goldWallet.Add(enemy.GoldReward);

		foreach(var turret in generator.Turrets)
		{
			turret.OnUpgradeSuccess +=
				() => Debug.Log("Turret upgraded successfully!");
			turret.OnUpgradeFailure +=
				reason => Debug.Log("Can't upgrade turret: " + reason);
		}

		uiDaddy.Init(generator.BaseHealth, goldWallet);
	}
}
