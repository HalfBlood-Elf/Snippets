using UnityEngine;
using InputHandlers;

public class SimpleTransformMoverOnEvent : MonoBehaviour
{
	[SerializeField] private InputHandlers.ThreeAxisInput _inputHandler;
	[SerializeField] private float _moveSpeed;

#if UNITY_EDITOR
	private void Reset()
	{
		_moveSpeed = 5f;
	}
#endif

	private void Start()
	{
		_inputHandler.OnInputUpdated += OnInputUpdated;
	}

	private void OnInputUpdated(Vector3 input)
	{
		var delta = Time.deltaTime * _moveSpeed;
		transform.position += input * delta;

	}

}
