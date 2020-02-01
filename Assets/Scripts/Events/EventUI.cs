using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Event {
	public class EventUI : MonoBehaviour {

		private enum Page {
			None,
			Main,
			Actor,
			Item
		}

		public Button actorButton;
		public Button itemsButton;

		public Inventory inventory;

		private Stack<Page> pageStack;
		private Page currentPage;

		public Animator animator;
		public Animator characterAnimator;
		public Image characterImage;

		public Transform itemsParent;
		public Transform actorParent;

		[AssetsOnly]
		public GameObject buttonPrefab;

		private void Awake() {
			pageStack = new Stack<Page>();
			actorButton.onClick.AddListener(() => ShowActorPage());
			itemsButton.onClick.AddListener(() => ShowItemsPage());
		}

		//Change to rewired, you know how to do it!!
		private void Update() {
			if (Input.GetKeyDown(KeyCode.Escape) && gameObject.activeSelf) {
				Back();
			}
		}

		private void SpawnActorButtons() {
			foreach (Transform transform in actorParent) {
				transform.gameObject.SetActive(false);
			}
			foreach (Actor actor in inventory.actors) {
				if (actor != EventHandler.Instance.currentActor) {
					Button button = DrawFromPool(buttonPrefab, actorParent).GetComponent<Button>();
					button.onClick.RemoveAllListeners();
					button.onClick.AddListener(() => PickActor(actor));
					//Change this to an image or whatever you want sweetie ://) <3 <3 <3
					button.GetComponentInChildren<Text>().text = actor.name;
					//button.GetComponentInChildren<Image>().sprite = actor.icon;
				}
			}
		}

		private void SpawnItemButtons() {
			foreach (Transform transform in itemsParent) {
				transform.gameObject.SetActive(false);
			}
			foreach (Solution item in inventory.items) {
				Button button = DrawFromPool(buttonPrefab, itemsParent).GetComponent<Button>();
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() => PickItem(item));
				//Change this to an image or whatever you want sweetie ://) <3 <3 <3
				button.GetComponentInChildren<Text>().text = item.name;
				//button.GetComponentInChildren<Image>().sprite = item.sprite;
			}
		}

		private GameObject DrawFromPool(GameObject prefab, Transform parent) {
			GameObject buttonObject = null;
			foreach (Transform child in parent) {
				if (!child.gameObject.activeSelf) {
					buttonObject = child.gameObject;
					buttonObject.SetActive(true);
					break;
				}
			}
			return buttonObject ?? Instantiate(prefab, parent);
		}

		public void ShowEventUI(Event newEvent) {
			characterImage.color = newEvent.actor.spriteColor;
			characterAnimator.runtimeAnimatorController = newEvent.actor.animator;
			if (EventManager.Instance.progress[newEvent.actor].actorState == EventManager.ActorState.Pending) {
				characterAnimator.GetComponent<Image>().enabled = true;
				ShowMainPage();
			}
			else {
				if (EventManager.Instance.progress[newEvent.actor].actorState == EventManager.ActorState.Failure) {
					characterAnimator.GetComponent<Image>().enabled = false;
				}
				else {
					characterAnimator.GetComponent<Image>().enabled = true;
				}
				if (EventManager.Instance.progress[newEvent.actor].actorState == EventManager.ActorState.Success) {
					characterAnimator.SetBool("Happy", true);
				}
				ShowNonePage();
			}
		}

		private void ShowActorPage(bool stack = true) {
			if (stack) {
				pageStack.Push(currentPage);
			}
			SpawnActorButtons();
			currentPage = Page.Actor;
			animator.SetBool("Actor", true);
			animator.SetBool("Main", false);
			animator.SetBool("Items", false);
			animator.SetBool("None", false);
		}

		private void ShowItemsPage(bool stack = true) {
			if (stack) {
				pageStack.Push(currentPage);
			}
			SpawnItemButtons();
			currentPage = Page.Item;
			animator.SetBool("Items", true);
			animator.SetBool("Main", false);
			animator.SetBool("Actor", false);
			animator.SetBool("None", false);
		}

		public void ShowMainPage() {
			pageStack.Clear();
			currentPage = Page.Main;
			animator.SetBool("Main", true);
			animator.SetBool("Items", false);
			animator.SetBool("Actor", false);
			animator.SetBool("None", false);
		}

		public void ShowNonePage() {
			pageStack.Clear();
			currentPage = Page.None;
			animator.SetBool("None", true);
			animator.SetBool("Items", false);
			animator.SetBool("Actor", false);
			animator.SetBool("Main", false);
		}

		private void Back() {
			if (pageStack.Count > 0 && currentPage != Page.None) {
				Page page = pageStack.Pop();
				switch (page) {
					case Page.Actor:
						EventHandler.Instance.selectedActor = null;
						ShowActorPage(false);
						break;
					case Page.Item:
						EventHandler.Instance.selectedItem = null;
						ShowItemsPage(false);
						break;
					case Page.Main:
						ShowMainPage();
						break;
				}
			}
			else {
				EventHandler.Instance.ExitEvent();
			}
		}

		private void PickActor(Actor actor) {
			EventHandler.Instance.selectedActor = actor;
			if (EventHandler.Instance.selectedItem != null) {
				EventHandler.Instance.Evaluate();
			}
			else {
				ShowItemsPage();
			}
		}

		private void PickItem(Solution item) {
			EventHandler.Instance.selectedItem = item;
			if (EventHandler.Instance.selectedActor != null) {
				EventHandler.Instance.Evaluate();
			}
			else {
				ShowActorPage();
			}
		}
	}
}