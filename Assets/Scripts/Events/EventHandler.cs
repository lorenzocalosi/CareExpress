using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Event {
	public class EventHandler : Singleton<EventHandler> {
		public Transform display;
		public EventUI eventUI;
		public Image background;
		
		public Event testEvent;

		public event Action GameEnded;
		[ReadOnly]
		public Actor selectedActor;
		[ReadOnly]
		public Solution selectedItem;
		[ReadOnly]
		public Actor currentActor;
		[ShowInInspector]
		private int questCompleted;
		private const int MaxQuest = 3;
		[ShowInInspector] private List<Actor> endingActors = new List<Actor>();

		private bool gameEnded;
		private void Start() {
			//Debug
			//ShowEvent(testEvent);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.O))
			{
				StartCoroutine(EventManager.Instance.endingSettings.ShowImages(endingActors));
			}
			
			if (questCompleted >= MaxQuest)
			{
				if (gameEnded) return;
				//end game state
				Debug.Log($"Game ending");
				GameEnded?.Invoke();
				EventManager.Instance.endingSettings.PlayEndingMusic();
				StartCoroutine(EventManager.Instance.endingSettings.ShowImages(endingActors));
				gameEnded = true;
			}
		}

		public void ShowEvent(Event newEvent) {
			currentActor = newEvent.actor;
			background.sprite = newEvent.background;
			eventUI.gameObject.SetActive(true);
			eventUI.ShowEventUI(newEvent);
			selectedActor = EventManager.Instance.progress[EventHandler.Instance.currentActor].guessedActor ? EventHandler.Instance.selectedActor : null;
			selectedItem = EventManager.Instance.progress[EventHandler.Instance.currentActor].guessedItem ? EventHandler.Instance.selectedItem : null;
		}

		public void ExitEvent() {
			CameraHandler.Instance.FadeOut(() =>
			{
				eventUI.errorTransform.gameObject.SetActive(false);
				eventUI.gameObject.SetActive(false);
				eventUI.characterAnimator.SetTrigger("Reset");
				PlayerMovement.Instance.canMove = true;
			});
		}

		public void Evaluate() {
			if (currentActor.relatedActor == selectedActor) {
				EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(
					EventManager.Instance.progress[currentActor].actorState,
					EventManager.Instance.progress[currentActor].tries,
					true,
					EventManager.Instance.progress[currentActor].guessedItem);
			}
			if (currentActor.relatedSolution == selectedItem) {
				EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(
					EventManager.Instance.progress[currentActor].actorState,
					EventManager.Instance.progress[currentActor].tries,
					EventManager.Instance.progress[currentActor].guessedActor,
					true);
			}
			if (EventManager.Instance.progress[currentActor].guessedActor && EventManager.Instance.progress[currentActor].guessedItem) {
				eventUI.errorTransform.gameObject.SetActive(false);
				EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(EventManager.ActorState.Success, EventManager.Instance.progress[currentActor].tries);
				EventManager.Instance.progress[currentActor.relatedActor] = new EventManager.ActorStateTries(EventManager.ActorState.Success, EventManager.Instance.progress[currentActor.relatedActor].tries);
				eventUI.ShowNonePage();
				eventUI.characterAnimator.SetBool("Happy", true);
				SoundManager.Instance.PlayAudioClipOneShot("Happy");
				//
				endingActors.Add(currentActor);
				//
				eventUI.inventory.actors.Remove(currentActor);
				eventUI.inventory.actors.Remove(currentActor.relatedActor);
				eventUI.inventory.items.Remove(currentActor.relatedSolution);
				//
				questCompleted++;
			}
			else {
				selectedActor = EventManager.Instance.progress[EventHandler.Instance.currentActor].guessedActor ? EventHandler.Instance.selectedActor : null; ;
				selectedItem = EventManager.Instance.progress[EventHandler.Instance.currentActor].guessedItem ? EventHandler.Instance.selectedItem : null; ;
				if (EventManager.Instance.progress[currentActor].tries + 1 >= EventManager.Instance.triesLimit) {
					eventUI.ShowNonePage();
					EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(
						EventManager.ActorState.Failure,
						EventManager.Instance.progress[currentActor].tries + 1,
						EventManager.Instance.progress[currentActor].guessedActor,
						EventManager.Instance.progress[currentActor].guessedItem);
					eventUI.characterAnimator.SetTrigger("Angry");
					SoundManager.Instance.PlayAudioClipOneShot("Angry");
					eventUI.inventory.actors.Remove(currentActor);
					//
					questCompleted++;
				}
				else {
					EventManager.Instance.progress[currentActor] = new EventManager.ActorStateTries(
						EventManager.ActorState.Pending,
						EventManager.Instance.progress[currentActor].tries + 1,
						EventManager.Instance.progress[currentActor].guessedActor,
						EventManager.Instance.progress[currentActor].guessedItem);
					eventUI.characterAnimator.SetTrigger("Confused");
					SoundManager.Instance.PlayAudioClipOneShot("Confused");
					eventUI.ShowMainPage();
				}
				eventUI.ShowErrors();
			}
		}
	}
}