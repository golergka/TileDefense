using UnityEngine;

public class UIDaddy : MonoBehaviour
{
	[SerializeField] UIBaseHealth uiBaseHealth;
	[SerializeField] UIGoldWallet uiGoldWallet;

	public void Init(Health baseHealth, GoldWallet goldWallet)
	{
		uiBaseHealth.Init(baseHealth);
		uiGoldWallet.Init(goldWallet);
	}
}
