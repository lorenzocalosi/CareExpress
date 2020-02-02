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

		public Sprite baseActor;
		public Sprite baseItem;

		public Sprite buttonDone;
		public Sprite buttonSelected;

		public Image actorButtonImage;
		public Image itemButtonImage;

		public Button okButton;

		public Transform errorTransform;
		[AssetsOnly]
		public GameObject errorImage;

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
			okButton.onClick.AddListener(() => EventHandler.Instance.Evaluate());
		}

		//Change to rewired, you know how to do it!!
		private void Update() {
			if (PlayerMovement.Instance.IsPressingCancel && gameObject.activeSelf) {
				Back();
				if (MapChanger.Instance.currentMap == 1) {
					SoundManager.Instance.InterruptSoundTrackAndPlayOther("Talking", "Walking");
				}
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
					button.onClick.AddListener(() => button.GetComponent<ButtonEventHandler>().lateExectution = () => PickActor(actor));
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
				button.onClick.AddListener(() => button.GetComponent<ButtonEventHandler>().lateExectution = () => PickItem(item));
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

		private void DrawFromIssuePool(GameObject prefab, Transform parent) {
			foreach (Transform child in parent) {
				if (!child.GetChild(0).gameObject.activeSelf) {
					child.GetChild(0).gameObject.SetActive(true);
					break;
				}
			}
		}

		private GameObject DrawFromPool(GameObject prefab, Transform parent) {
			GameObject newObject = null;
			foreach (Transform child in parent) {
				if (!child.gameObject.activeSelf) {
					newObject = child.gameObject;
					newObject.SetActive(true);
					break;
				}
			}
			return newObject ?? Instantiate(prefab, parent);
		}

		public void ShowErrors() {
			Debug.Log("Error");
			errorTransform.gameObject.SetActive(true);
			foreach (Transform transform in errorTransform) {
				transform.GetChild(0).gameObject.SetActive(false);
			}
			for (int i = 0; i < EventManager.Instance.progress[EventHandler.Instance.currentActor].tries; i++) {
				DrawFromIssuePool(errorImage, errorTransform);
			}
		}

		public void ShowEventUI(Event newEvent) {
			if (!inventory.actors.Contains(newEvent.actor)) {
				inventory.actors.Add(newEvent.actor);
			}
			characterImage.color = newEvent.actor.spriteColor;
			characterAnimator.runtimeAnimatorController = newEvent.actor.animator;
			EventHandler.Instance.selectedActor = null;
			EventHandler.Instance.selectedItem = null;
			if (inventory.actors.Count > 1 && EventManager.Instance.progress[newEvent.actor].actorState == EventManager.ActorState.Pending) {
				characterAnimator.GetComponent<Image>().enabled = true;
				ShowMainPage();
				ShowErrors();
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

		private void CallbackActor() {
			actorButton.GetComponent<ButtonEventHandler>().lateExectution = () => ShowActorPage();
		}
		
		private void CallbackItem() {
			itemsButton.GetComponent<ButtonEventHandler>().lateExectution = () => ShowItemsPage();
		}

		public void ShowMainPage() {
			actorButton.onClick.RemoveListener(CallbackActor);
			itemsButton.onClick.RemoveListener(CallbackItem);
			if (!EventManager.Instance.progress[EventHandler.Instance.currentActor].guessedActor) {
				actorButtonImage.sprite = EventHandler.Instance.selectedActor == null ? baseActor : EventHandler.Instance.selectedActor.icon;
				actorButton.onClick.AddListener(CallbackActor);
				actorButton.transition = Selectable.Transition.Animation;
				actorButton.animator.enabled = true;
			}
			else {
				actorButtonImage.sprite = EventHandler.Instance.currentActor.relatedActor.icon;
				actorButton.animator.enabled = false;
				actorButton.transition = Selectable.Transition.None;
				actorButton.GetComponent<Image>().sprite = buttonSelected;
			}
			if (!EventManager.Instance.progress[EventHandler.Instance.currentActor].guessedItem) {
				itemButtonImage.sprite = EventHandler.Instance.selectedItem == null ? baseItem : EventHandler.Instance.selectedItem.sprite;
				itemsButton.onClick.AddListener(CallbackItem);
				itemsButton.transition = Selectable.Transition.Animation;
				itemsButton.animator.enabled = true;
			}
			else {
				itemButtonImage.sprite = EventHandler.Instance.currentActor.relatedSolution.sprite;
				itemsButton.animator.enabled = false;
				itemsButton.transition = Selectable.Transition.None;
				itemsButton.GetComponent<Image>().sprite = buttonSelected;
			}
			if (EventHandler.Instance.selectedActor != null && EventHandler.Instance.selectedItem != null) {
				okButton.gameObject.SetActive(true);
			}
			else {
				okButton.gameObject.SetActive(false);
			}
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
						EventHandler.Instance.selectedActor = EventManager.Instance.progress[EventHandler.Instance.currentActor].guessedActor ? EventHandler.Instance.selectedActor : null;
						ShowActorPage(false);
						break;
					case Page.Item:
						EventHandler.Instance.selectedItem = EventManager.Instance.progress[EventHandler.Instance.currentActor].guessedItem ? EventHandler.Instance.selectedItem : null;
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
			ShowMainPage();
		}

		private void PickItem(Solution item) {
			EventHandler.Instance.selectedItem = item;
			ShowMainPage();
		}
	}
}