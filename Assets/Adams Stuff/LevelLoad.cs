using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

// public class LevelLoad : MonoBehaviour {
//     public IEnumerator LoadLevel(string sceneName, GameObject fade) {
//         Fading fadeScript = fade.GetComponent<Fading>();
//         yield return new WaitForSecondsRealtime(0.1f);
//         AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
//         asyncLoad.allowSceneActivation = false;
//         fadeScript.DoFade();
//         while (asyncLoad.progress < 0.9f || fadeScript.fading == true) {
//             yield return null;
//         }
//         asyncLoad.allowSceneActivation = true;
//     }
// }