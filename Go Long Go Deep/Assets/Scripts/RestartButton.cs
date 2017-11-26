using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour {
	// Restart the current level
	public void ReloadLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		// Return to default timescale (is paused by gameover)
		Time.timeScale = 1.0f;
	}
}
