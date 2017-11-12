using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(NavMeshModifier))]
public class Floor : MonoBehaviour, IPointerClickHandler
{
	private NavMeshModifier navMeshModifier;
	private NavMeshModifier NavMeshModifier
	{
		get
		{
			return navMeshModifier ?? (navMeshModifier = GetComponent<NavMeshModifier>());
		}
	}

	private Action rebuildNav;

	public void Init(Action rebuildNav)
	{
		this.rebuildNav = rebuildNav;
	}

	public void OnPointerClick(PointerEventData pointerEventData)
	{
		NavMeshModifier.enabled = true;
		GetComponent<Renderer>().material.color = Color.red;
		rebuildNav();
	}
}
