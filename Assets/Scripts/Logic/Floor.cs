using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Floor : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerEnterHandler
{
	public event Action SnakeActivated = delegate{};

	private Transform sourceTransform;
	private Transform targetTransform;
	private Action rebuildNavMesh;

	private float lastCancelTime = float.NegativeInfinity;

	[SerializeField] Color activeColor;
	[SerializeField] Color passiveColor;
	[SerializeField] SpriteRenderer cancelIcon;
	[SerializeField] Renderer floorRenderer;

	public void Init(Action rebuildNavMesh, Transform sourceTransform, Transform targetTransform)
	{
		this.rebuildNavMesh = rebuildNavMesh;
		this.sourceTransform = sourceTransform;
		this.targetTransform = targetTransform;
		DeactivateSnake();
	}

	private NavMeshModifier modifier;

	public void DeactivateSnake()
	{
		if (modifier != null)
		{
			modifier.enabled = false;
			rebuildNavMesh();
		}
		floorRenderer.material.color = passiveColor;
	}

	public void TryActivateSnake()
	{
		if (modifier != null && modifier.enabled)
			return;

		if (modifier == null)
		{
			modifier = gameObject.AddComponent<NavMeshModifier>();
		}
		modifier.overrideArea = true;
		modifier.area = (int) NavArea.TestingBuilding;
		modifier.enabled = true;

		rebuildNavMesh();
		
		var path = new NavMeshPath();
		var mask = (int) (NavArea.Walkable | NavArea.TestingBuilding);

		if (NavMesh.CalculatePath(sourceTransform.position, targetTransform.position, mask, path) && path.status == NavMeshPathStatus.PathComplete)
		{
			modifier.area = (int) NavArea.NonWalkable;
			floorRenderer.material.color = activeColor;
			SnakeActivated();
		}
		else
		{
			modifier.enabled = false;
			lastCancelTime = Time.time;
		}

		rebuildNavMesh();
	}

	const float CANCEL_ICON_STAY = 0.8f;
	const float CANCEL_ICON_FADE = 0.2f;

	void Update()
	{
		var cancelTime = Time.time - lastCancelTime;
		var alpha = Mathf.Clamp01(1f - ((cancelTime - CANCEL_ICON_STAY) / CANCEL_ICON_FADE));
		var col = cancelIcon.color;
		col.a = alpha;
		cancelIcon.color = col;
	}

	public void OnPointerDown(PointerEventData pointerEventData)
	{
		TryActivateSnake();
		pointerEventData.Use();
	}

	public void OnDrag(PointerEventData pointerEventData)
	{
		TryActivateSnake();
		pointerEventData.Use();
	}

	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		if (pointerEventData.dragging)
		{
			TryActivateSnake();
			pointerEventData.Use();
		}
	}
}
