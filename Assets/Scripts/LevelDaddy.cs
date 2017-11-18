using UnityEngine;

public class LevelDaddy : MonoBehaviour
{
	[SerializeField] LevelGenerator generator;
	[SerializeField] UIDaddy uiDaddy;

	void Start()
	{
		generator.Init();
		generator.BaseHealth.OnCurrentChange += 
			() => Debug.Log("Base health: " + generator.BaseHealth.Current);
		generator.BaseHealth.OnDie += () => Debug.Log("Game over!");
		uiDaddy.Init(generator.BaseHealth);
	}
}
