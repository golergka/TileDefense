using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthHitAnimator : MonoBehaviour
{
	[SerializeField] ParticleSystemHolder hitAnimationHolder;

	void Start()
	{
		var health = GetComponent<Health>();
		health.OnCurrentChange += delegate(int change)
		{
			var emit = -Mathf.Min(change, 0);
			hitAnimationHolder.Instance.Emit(emit);
		};
	}


}
