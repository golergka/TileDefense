using UnityEngine;

public class UIDaddy : MonoBehaviour
{
	[SerializeField] UIBaseHealth uiBaseHealth;
	[SerializeField] UIGoldWallet uiGoldWallet;

	[SerializeField] UIHealthBar healthBarPrefab;

	public void Init(Health baseHealth, GoldWallet goldWallet)
	{
		uiBaseHealth.Init(baseHealth);
		uiGoldWallet.Init(goldWallet);
	}

	public void RegisterHealth(Health health)
	{
		var healthBar = Instantiate(healthBarPrefab, transform) as UIHealthBar;
		healthBar.Init(health);
		health.OnDie += () => Destroy(healthBar.gameObject);
	}
}
