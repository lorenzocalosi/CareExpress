using UnityEngine;

public class ButtonEventHandler : MonoBehaviour {
	public System.Action lateExectution;

	public void Execute() {
		lateExectution?.Invoke();
	}
}
