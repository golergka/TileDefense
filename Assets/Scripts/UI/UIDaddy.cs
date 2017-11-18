using UnityEngine;

public class UIDaddy : MonoBehaviour
{
	[SerializeField] BaseHealth baseHealthUI;

	public void Init(Health baseHealth)
	{
		baseHealthUI.Init(baseHealth);
	}
}
