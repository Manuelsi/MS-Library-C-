using System.Collections.Generic;

using UnityEngine;

public static class InputManager {

	private static Dictionary<string, bool> downStates = new Dictionary<string, bool>();
	private static Dictionary<string, bool> upStates = new Dictionary<string, bool>();

	private static Dictionary<string, float> deadZones = new Dictionary<string, float>();
	private const float DEFAULT_DEADZONE = 0.4f;

	private static bool IsDown(string axis) => downStates[axis];
	private static bool IsUp(string axis) => upStates[axis];

	public static bool TriggerDown(string axis) {
		TryInitialize(axis);

		float trigger = Input.GetAxisRaw(axis); //This also takes keyboard input
		if (IsDown(axis))
		{
			UpdateDownState(axis, trigger);
			return false;
		}
		if (trigger > deadZones[axis])
		{
			UpdateDownState(axis, trigger);
			return true;
		}
		return false;
	}

	public static bool TriggerHeld(string axis) =>
		Input.GetAxisRaw(axis) > deadZones[axis];

	public static bool TriggerUp(string axis) {
		TryInitialize(axis);

		float trigger = Input.GetAxisRaw(axis); //This also takes keyboard input
		if (IsUp(axis))
		{
			UpdateUpState(axis, trigger);
			return false;
		}

		if (trigger < deadZones[axis])
		{
			UpdateUpState(axis, trigger);
			return true;
		}
		return false;
	}
	public static void SetDeadzone(string axis, float value) =>
		deadZones[axis] = value;

	private static void UpdateDownState(string axis, float axisValue) =>
		downStates[axis] = axisValue > deadZones[axis];

	private static void UpdateUpState(string axis, float axisValue) =>
		upStates[axis] = axisValue < deadZones[axis];

	private static void TryInitialize(string axis) {
		if (!downStates.ContainsKey(axis))
			downStates[axis] = false;
		if (!upStates.ContainsKey(axis))
			upStates[axis] = false;
		if (!deadZones.ContainsKey(axis))
			deadZones[axis] = DEFAULT_DEADZONE;
	}

	public static void Debug_PrintAxis(string axis) =>
		Debug.Log($"{axis}: Value: {Input.GetAxisRaw(axis)}. Down: {downStates[axis]}. Up: {upStates[axis]}.");

	public static float GetAxis(string axis) => Input.GetAxisRaw(axis);
}