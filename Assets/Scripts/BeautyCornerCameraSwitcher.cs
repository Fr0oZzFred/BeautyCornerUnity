using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class BeautyCornerCameraSwitcher : MonoBehaviour {
    [SerializeField]
    float switchTimer = 5.0f;

    [SerializeField]
    [Range(0, 5)]
    public int curCamIndex = 0;

    [SerializeField] 
    List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();


    public delegate void OnCameraChangedDelegate();
    public OnCameraChangedDelegate OnCameraChanged;

    public int CameraCount => cameras.Count;
    public int CurrentCameraIndex => curCamIndex;
    public float SwitchTimer => switchTimer;


    private void OnValidate() {
        if (cameras.Count <= 0) return;
        ChangeCam();
    }
    private IEnumerator Start() {
        OnCameraChanged += ChangeCam;

        do {
            yield return new WaitForSeconds(switchTimer);
            SetCurrentCam(curCamIndex + 1);
        } while (true);

    }
    public void SetCurrentCam(int camIndex) {
        curCamIndex = (camIndex + CameraCount) % CameraCount;
        OnCameraChanged();
    }

    public void ChangeCam() {
        foreach (var cam in cameras) {
            if (cam == null || !cam.IsValid) return;
            cam.Priority = 0;
        }
        cameras[CurrentCameraIndex].Priority = 1;
    }
}
