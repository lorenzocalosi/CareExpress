using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

	public string sceneToLoad;

	private void Start() {
		Button button = GetComponent<Button>();
		button.onClick.AddListener(() => button.GetComponent<ButtonEventHandler>().lateExectution = () => SceneManager.LoadScene(sceneToLoad));
	}
}
