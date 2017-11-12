using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class LevelGenerator : MonoBehaviour
{
	[SerializeField] Floor tilePrefab;
	[SerializeField] Transform basePrefab;
	[SerializeField] Spawner spawnerPrefab;
	[SerializeField] int width;
	[SerializeField] int height;

	const float TILE_SIZE = 1f;

	private NavMeshSurface navMeshSurface;
	private NavMeshSurface NavMeshSurface
	{
		get
		{
			return navMeshSurface ?? (navMeshSurface = GetComponent<NavMeshSurface>());
		}
	}

	private Spawner spawner;

	void Start()
	{
		for(var x = 0; x < width; x++)
		{
			for(var y = 0; y < height; y++)
			{
				var tile = InstantiateAt(x, y, tilePrefab) as Floor;
				tile.Init(RebuildNavMesh);
			}
		}

		var basePosition = InstantiateAt(0, 0, basePrefab).transform.position;
		spawner = InstantiateAt(width - 1, height - 1, spawnerPrefab) as Spawner;
		spawner.Init(basePosition);

		NavMeshSurface.BuildNavMesh();
	}

	private void RebuildNavMesh()
	{
		NavMeshSurface.BuildNavMesh();
		spawner.RebuildPaths();
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
}
