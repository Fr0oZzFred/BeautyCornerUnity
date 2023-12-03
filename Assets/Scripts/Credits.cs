using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct ArtistInfo {
    public string name;
    public string portfolioURL;
}

public class Credits : MonoBehaviour {
    [Header("Camera Switcher")]
    [SerializeField] TextMeshProUGUI camerasText;
    [SerializeField] PlayableDirector sliderDirector;
    BeautyCornerCameraSwitcher cameraSwitcher;

    [Header("Artists")]
    [SerializeField] PlayableDirector barDirector;
    [SerializeField] PlayableDirector MainMenuButtonDirector;
    [SerializeField] float textFadeTime = 1.0f;
    [SerializeField] float textFadeSteps = 10.0f;
    [SerializeField] List<ArtistInfo> artistsInfos;
    [SerializeField] List<TextMeshProUGUI> artistsText;



    private void OnValidate() {
        for (int i = 0; i < artistsText.Count; i++) {
            if (!artistsText[i]) continue;

            if (i > artistsInfos.Count - 1) {
                artistsText[i].transform.parent.gameObject.SetActive(false);
                artistsText[i].transform.parent.GetComponent<Button>().interactable = false;
                continue;
            }

            artistsText[i].SetText(artistsInfos[i].name);
            artistsText[i].gameObject.name = artistsInfos[i].name;

            artistsText[i].transform.parent.gameObject.SetActive(true);
            artistsText[i].transform.parent.gameObject.name = artistsInfos[i].name + "Button";
            artistsText[i].transform.parent.GetComponent<Button>().interactable = artistsInfos[i].portfolioURL.Length != 0;
        }
    }
    private void Awake() {
        foreach (var t in artistsText) {
            t.alpha = 0.0f;
            t.transform.parent.GetComponent<Image>().color = Color.clear;
        }
    }
    private void Start() {
        cameraSwitcher = FindObjectOfType<BeautyCornerCameraSwitcher>();
        if (!cameraSwitcher) {
            Debug.LogWarning("Camera Switcher not found");
            return;
        }
        sliderDirector.playableGraph.GetRootPlayable(0).SetSpeed(1.0f/cameraSwitcher.SwitchTimer);
        cameraSwitcher.OnCameraChanged += OnCamChanged;
        OnCamChanged();
    }
    void OnCamChanged() {
        camerasText.SetText(cameraSwitcher.CurrentCameraIndex + 1 + "/" + cameraSwitcher.CameraCount);
    }
    public void ShowArtists() {
        StartCoroutine(PlayTextsAnimation());
    }
    IEnumerator PlayTextsAnimation() {
        float waitTime = textFadeTime / textFadeSteps;
        float fadeRatio = (1.0f / textFadeTime) * waitTime;
        foreach (var t in artistsText) {
            if (!t.transform.parent.gameObject.activeInHierarchy) continue;

            float alpha = 0;
            Image button = t.transform.parent.GetComponent<Image>();
            while (alpha < 1) {
                alpha += fadeRatio;
                t.alpha = alpha;
                button.color = new Color(1.0f, 1.0f, 1.0f, alpha);
                yield return new WaitForSeconds(waitTime);
            }
        }
        barDirector.Play();
        yield return new WaitForSeconds((float)barDirector.duration);
        MainMenuButtonDirector.Play();

    }

    public void OpenPortfolio(int ArtistIndex) {
        Application.OpenURL(artistsInfos[ArtistIndex].portfolioURL);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }
}