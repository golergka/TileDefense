using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIGoldWallet : MonoBehaviour
{
	private Text text;
	private Text Text
	{
		get
		{
			return text ?? (text = GetComponent<Text>());
		}
	}

	private GoldWallet goldWallet;

	public void Init(GoldWallet goldWallet)
	{
		this.goldWallet = goldWallet;
		goldWallet.OnCurrentChange += UpdateText;
	}

	void UpdateText()
	{
		Text.text = "Gold: " + goldWallet.Current;
	}
}
