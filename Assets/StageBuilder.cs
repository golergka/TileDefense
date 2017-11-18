using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class StageBuilder : MonoBehaviour, IPointerClickHandler
{
	private List<DamageDealer> stages = new List<DamageDealer>();

	void Start()
	{
		foreach(Transform child in transform)
		{
			var stage = child.GetComponent<DamageDealer>();
			if (stage != null)
			{
				stages.Add(stage);
			}
		}
		CurrentStage = -1;
	}

	int currentStage;
	int CurrentStage
	{
		get { return currentStage; }
		set
		{
			currentStage = value;
			for(var i = 0; i < stages.Count; i++)
			{
				stages[i].gameObject.SetActive(i == currentStage);
			}
		}
	}

	void SetTarget(IDamageReceiver target)
	{
		foreach(var stage in stages)
		{
			stage.Target = target;
		}
	}

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (CurrentStage < stages.Count - 1)
		{
			CurrentStage++;
		}
	}
}
