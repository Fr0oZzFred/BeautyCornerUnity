using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct SceneInfos {
    public string name;
    public string sceneName;
    public Sprite image;
}
public class ScenePreview : MonoBehaviour {

    [SerializeField] SceneInfos sceneInfos;
    [SerializeField] TextMeshProUGUI sceneNameText;
    [SerializeField] Image sceneImage;

    private void OnValidate() {
        SetInfos(sceneInfos);
    }
    public void SetInfos(SceneInfos infos) {
        if (sceneNameText == null || sceneImage == null) return;

        sceneNameText.SetText(infos.name);
        sceneImage.sprite = infos.image;
        sceneNameText.gameObject.name = infos.name + "Text";
        sceneImage.gameObject.name = infos.name + "Image";
        this.gameObject.name = infos.name + "ScenePreview";
    }
    public void LoadScene() {
        SceneManager.LoadScene(sceneInfos.sceneName);
    }
}
