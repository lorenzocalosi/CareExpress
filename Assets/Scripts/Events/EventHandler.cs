using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Event {
	public class EventHandler : Singleton<EventHandler> {
		public Transform display;
		public EventUI eventUI;
		public Image background;

		public Event testEvent;

		[ReadOnly]
		public Actor selectedActor;
		[ReadOnly]
		public Solution selectedItem;
		[ReadOnly]
		public Actor currentActor;

		private void Start() {
			//Debug
			//ShowEvent(testEvent);
		}

		public void ShowEvent(Event newEvent) {
			currentActor = newEvent.actor;
			background.sprite = newEvent.background;
			eventUI.gameObject.SetActive(true);
			eventUI.ShowEventUI(newEvent);
		}

		public void ExitEvent() {
			eventUI.gameObject.SetActive(false);
			eventUI.characterAnimator.SetTrigger("Reset");
		}

		public void Evaluate() {
			if (currentActor.relatedActor == selectedActor && currentActor.relatedSolution == selectedItem) {
				EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(EventManager.ActorState.Success, EventManager.Instance.progress[currentActor].tries);
				eventUI.ShowNonePage();
				eventUI.characterAnimator.SetBool("Happy", true);
			}
			else {
				if (EventManager.Instance.progress[currentActor].tries + 1 >= EventManager.Instance.triesLimit) {
					eventUI.ShowNonePage();
					EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(EventManager.ActorState.Failure, EventManager.Instance.progress[currentActor].tries + 1);
					eventUI.characterAnimator.SetTrigger("Angry");
					//Handle actor fucking off
				}
				else {
					EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(EventManager.ActorState.Pending, EventManager.Instance.progress[currentActor].tries + 1);
					eventUI.characterAnimator.SetTrigger("Confused");
					eventUI.ShowMainPage();
				}
				selectedActor = null;
				selectedItem = null;
			}
		}
	}
}