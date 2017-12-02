using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Linq;

public class StageBuilder : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] int stageCostUpgrade;

	private List<PeriodicDamager> stages = new List<PeriodicDamager>();

	void Start()
	{
		foreach(Transform child in transform)
		{
			var stage = child.GetComponent<PeriodicDamager>();
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

	int UpgradeCost
	{
		get
		{
			return (CurrentStage + 2) * stageCostUpgrade;
		}
	}

	bool MaxStage
	{
		get
		{
			return CurrentStage >= stages.Count - 1;
		}
	}

	void SetTarget(IDamageReceiver target)
	{
		foreach(var stage in stages)
		{
			stage.Target = target;
		}
	}

	private GoldWallet wallet;

	public void Init(GoldWallet wallet)
	{
		this.wallet = wallet;
	}

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		if (MaxStage)
		{
			OnUpgradeFailure(UpgradeFailureReason.MaxStageReached);
			return;
		}
		if (wallet == null)
		{
			Debug.LogError("Trying to upgrade without a wallet reference!");
			return;
		}
		if (!wallet.CanSpend(UpgradeCost))
		{
			OnUpgradeFailure(UpgradeFailureReason.NotEnoughGold);
			return;
		}

		wallet.Spend(UpgradeCost);
		CurrentStage++;
		OnUpgradeSuccess();
	}

	public enum UpgradeFailureReason
	{
		MaxStageReached,
		NotEnoughGold
	}

	public event Action OnUpgradeSuccess = delegate{};
	public event Action<UpgradeFailureReason> OnUpgradeFailure = delegate{};
}
