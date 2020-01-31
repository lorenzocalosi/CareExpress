using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Event {
	public class EventHandler : Singleton<EventHandler> {
		public Transform display;
		public List<Button> buttons;

		public Event testEvent;

		private GameObject currentInstance;
		private List<Actor> currentActors;

		private void Start() {
			ShowEvent(testEvent);
		}

		public void ShowEvent(Event newEvent) {
			int correctIndex = Random.Range(0, buttons.Count);
			int incorrectIndex = 0;
			List<Solution> incorrectSolutions = SolutionCollection.Instance.GetRandomSolutions(newEvent.solution);
			for (int i = 0; i < buttons.Count; i++) {
				if (i == correctIndex) {
					buttons[i].GetComponentInChildren<Image>().sprite = newEvent.solution.sprite;
					buttons[i].onClick.AddListener(() => Success(newEvent));
				}
				else {
					buttons[i].GetComponentInChildren<Image>().sprite = incorrectSolutions[incorrectIndex].sprite;
					buttons[i].onClick.AddListener(() => Failure(newEvent));
					incorrectIndex++;
				}
			}
			currentActors = null;
			DisplayEvent(newEvent, newEvent.basic);
		}

		private void DisplayEvent(Event newEvent, GameObject gameObject, List<Actor> actors = null) {
			if (currentInstance != null) {
				Destroy(currentInstance);
				currentInstance = null;
			}
			currentInstance = Instantiate(gameObject, display);
			currentInstance.name = currentInstance.name.Replace("(Clone)", "");
			EventScreen eventScreen = currentInstance.GetComponent<EventScreen>();
			eventScreen.BuildScreen(currentActors ?? GetActors(newEvent));
		}

		private List<Actor> GetActors(Event newEvent) {
			List<Actor> actors = new List<Actor>();
			Actor checkActor;
			foreach (Event.ActorGroup actorGroup in newEvent.actors) {
				do {
					checkActor = actorGroup.possibleActors[Random.Range(0, actorGroup.possibleActors.Count)];
				}
				while (actors.Contains(checkActor));
				actors.Add(checkActor);
			}
			currentActors = actors;
			return actors;
		}

		private void Success(Event newEvent) {
			DisplayEvent(newEvent, newEvent.success);
		}

		private void Failure(Event newEvent) {
			DisplayEvent(newEvent, newEvent.failure);
		}
	}
}