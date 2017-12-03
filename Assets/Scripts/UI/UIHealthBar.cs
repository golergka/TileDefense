using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIHealthBar : MonoBehaviour
{
	CanvasGroup group;
	CanvasGroup Group
	{
		get
		{
			return group ?? (group = GetComponent<CanvasGroup>());
		}
	}

	[SerializeField] RectTransform fill;

	private float lastUpdate = float.NegativeInfinity;
	private Health health;

	public void Init(Health health)
	{
		this.health = health;
		UpdateHealthValue();
		health.OnCurrentChange += _ =>
		{
			lastUpdate = Time.time;
			UpdateHealthValue();
			UpdatePositionAlpha();
		};

		UpdatePositionAlpha();
	}

	private void UpdateHealthValue()
	{
		var percent = ((float) health.Current / health.Max);
		var anchorMax = fill.anchorMax;
		anchorMax.x = percent;
		fill.anchorMax = anchorMax;
	}

	const float BAR_STAY = 1f;
	const float BAR_FADE = 0.3f;

	private void UpdatePositionAlpha()
	{
		var cancelTime = Time.time - lastUpdate;
		Group.alpha = Mathf.Clamp01(1f - ((cancelTime - BAR_STAY) / BAR_FADE));

		gameObject.SetActive(Group.alpha > 0f);

		transform.position = Camera.main.WorldToScreenPoint(health.transform.position);
	}

	private void Update()
	{
		UpdatePositionAlpha();
	}
}
