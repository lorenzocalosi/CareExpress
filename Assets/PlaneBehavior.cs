using UnityEngine;

public class PlaneBehavior : MonoBehaviour {

	public Animator transitionAnimation;

	public void PlaneAnimation() {
		transitionAnimation.SetTrigger("Go");
	}

	public void PlaneReturned() {
		MapChanger.Instance.PlacePlayer();
	}
}
