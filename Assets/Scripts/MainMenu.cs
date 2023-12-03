using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SceneInfos {
    public string name;
    public string sceneName;
}
public class MainMenu : MonoBehaviour {
    [SerializeField] List<SceneInfos> scenesInfos;
    [SerializeField] List<TextMeshProUGUI> scenesText;

    private void OnValidate() {
        for (int i = 0; i < scenesText.Count; i++) {
            if (!scenesText[i]) continue;

            if (i > scenesInfos.Count - 1) {
                scenesText[i].transform.parent.gameObject.SetActive(false);
                scenesText[i].transform.parent.GetComponent<Button>().interactable = false;
                continue;
            }

            scenesText[i].SetText(scenesInfos[i].name);
            scenesText[i].gameObject.name = scenesInfos[i].name;

            scenesText[i].transform.parent.gameObject.SetActive(true);
            scenesText[i].transform.parent.gameObject.name = scenesInfos[i].name + "Button";
            scenesText[i].transform.parent.GetComponent<Button>().interactable = true;
        }
    }

    public void LoadScene(int index) {
        SceneManager.LoadScene(scenesInfos[index].sceneName);
    }
    public void Quit() {
        Application.Quit();
    }
}