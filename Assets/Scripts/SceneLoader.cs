using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
	public string sceneToLoad;

	public void OnEnable() {
		LoadScene();
	}

	public void LoadScene() {
		SceneManager.LoadScene(sceneToLoad);
	}
}