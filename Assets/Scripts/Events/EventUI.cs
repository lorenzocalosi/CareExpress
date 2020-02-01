using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
				SoundManager.Instance.InterruptSoundTrackAndPlayOther("Talking", "Walking");
			}
		}

		private void SpawnActorButtons()
		{
			bool isFirstButtonSelected = false;
			foreach (Transform t in actorParent) {
				
				if (!isFirstButtonSelected){
				
					SelectButton(t.gameObject);
					isFirstButtonSelected = true;
				}
				t.gameObject.SetActive(false);
			}
			
			foreach (Actor actor in inventory.actors) {
				if (actor != EventHandler.Instance.currentActor) {
					Button button = DrawFromPool(buttonPrefab, actorParent).GetComponent<Button>();
					button.onClick.RemoveAllListeners();
					button.onClick.AddListener(() => PickActor(actor));
					//Change this to an image or whatever you want sweetie ://) <3 <3 <3
					if (!string.IsNullOrEmpty(actor.name)) {
						if (!string.IsNullOrEmpty(actor.name)) {
							Text buttonText = button.GetComponentInChildren<Text>();
							if (buttonText != null) {
								buttonText.text = actor.name;
							}
						}
					}
					if (!isFirstButtonSelected){
						SelectButton(button.gameObject);
						isFirstButtonSelected = true;
					}
					if (actor.icon != null) {
						Image[] buttonImages = button.GetComponentsInChildren<Image>();
						if (buttonImages != null && buttonImages.Length > 0) {
							buttonImages[1].sprite = actor.icon;
							buttonImages[1].color = Color.white;
						}
					}
				}
			}
		}

		void SelectButton(GameObject selectableObject) {
			// Set the currently selected GameObject
			EventSystem.current.SetSelectedGameObject(selectableObject);
		}
		
		private void SpawnItemButtons() {
			foreach (Transform transform in itemsParent) {
				transform.gameObject.SetActive(false);
			}
			
			bool isFirstButtonSelected = false;
			foreach (Solution item in inventory.items) {
				Button button = DrawFromPool(buttonPrefab, itemsParent).GetComponent<Button>();
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(() => PickItem(item));
				//Change this to an image or whatever you want sweetie ://) <3 <3 <3
				if (!string.IsNullOrEmpty(item.name)) {
					Text buttonText = button.GetComponentInChildren<Text>();
					if (buttonText != null) {
						buttonText.text = item.name;
					}
				}
				if (item.sprite != null) {
					Image[] buttonImages = button.GetComponentsInChildren<Image>();
					if (buttonImages != null && buttonImages.Length > 0) {
						buttonImages[1].sprite = item.sprite;
						buttonImages[1].color = Color.white;
					}
				}
				if (!isFirstButtonSelected){
					SelectButton(button.gameObject);
					isFirstButtonSelected = true;
				}
				
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
			if (inventory.actors.Count > 1 && EventManager.Instance.progress[newEvent.actor].actorState == EventManager.ActorState.Pending) {
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
			SelectButton(actorButton.gameObject);
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