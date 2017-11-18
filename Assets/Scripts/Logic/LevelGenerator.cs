using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class LevelGenerator : MonoBehaviour
{
	#region Prefabs

	[SerializeField] Floor tilePrefab;
	[SerializeField] StageBuilder turretPrefab;
	[SerializeField] Health basePrefab;
	[SerializeField] Spawner spawnerPrefab;

	#endregion

	#region Level settings

	[SerializeField] int width;
	[SerializeField] int height;
	[SerializeField] float turretProbability;

	#endregion

	#region Constants

	const float TILE_SIZE = 1f;

	#endregion

	#region Components

	private NavMeshSurface navMeshSurface;
	private NavMeshSurface NavMeshSurface
	{
		get
		{
			return navMeshSurface ?? (navMeshSurface = GetComponent<NavMeshSurface>());
		}
	}

	#endregion

	#region Created objects

	public Spawner Spawner { get; private set; }
	public Health BaseHealth { get; private set; }

	#endregion

	#region Public methods

	public void Init()
	{
		for(var x = 0; x < width; x++)
		{
			for(var y = 0; y < height; y++)
			{
				if (Random.value < turretProbability)
				{
					InstantiateAt(x, y, turretPrefab);
				}
				else
				{
					var tile = InstantiateAt(x, y, tilePrefab) as Floor;
					tile.Init(RebuildNavMesh);
				}
			}
		}

		BaseHealth = InstantiateAt(0, 0, basePrefab) as Health;
		Spawner = InstantiateAt(width - 1, height - 1, spawnerPrefab) as Spawner;
		Spawner.Init(BaseHealth);

		NavMeshSurface.BuildNavMesh();
	}

	#endregion

	#region Private helper methods

	private void RebuildNavMesh()
	{
		NavMeshSurface.BuildNavMesh();
		Spawner.RebuildPaths();
	}

	private Component InstantiateAt(int x, int y, Component prefab)
	{
		var tilePosition = TilePosition(x, y);
		var result = Instantiate(prefab, transform) as Component;
		result.transform.localPosition = tilePosition;
		return result;
	}

	private Vector3 TilePosition(int x, int y)
	{
		return new Vector3(x * TILE_SIZE, 0f, y * TILE_SIZE);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		var size = new Vector3(width * TILE_SIZE, 0f, height * TILE_SIZE);
		var center = transform.position + size * 0.5f;
		Gizmos.DrawWireCube(center, size);
	}

	#endregion
}
