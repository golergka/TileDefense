using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIBaseHealth : MonoBehaviour
{
	private Text text;
	private Text Text
	{
		get
		{
			return text ?? (text = GetComponent<Text>());
		}
	}

	private Health health;

	public void Init(Health health)
	{
		this.health = health;
		health.OnCurrentChange += UpdateText;
	}

	void UpdateText()
	{
		Text.text = "Base: " + health.Current + "/" + health.Max;
	}
}
