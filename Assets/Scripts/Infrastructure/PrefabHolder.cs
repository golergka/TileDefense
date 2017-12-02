using UnityEngine;

public class PrefabHolder<T> : MonoBehaviour where T : Component
{
	[SerializeField] T prefab;

	T instance;

	public T Instance
	{
		get
		{
			Instantiate();
			return instance;
		}
	}

	void Start()
	{
		Instantiate();
	}

	void Instantiate()
	{
		if (instance == null)
			instance = Instantiate(prefab, transform) as T;
	}

	public static implicit operator T(PrefabHolder<T> holder)
	{
		return holder.Instance;
	}
}
