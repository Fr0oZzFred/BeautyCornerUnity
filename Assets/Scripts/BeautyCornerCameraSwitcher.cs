using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class BeautyCornerCameraSwitcher : MonoBehaviour {
    [SerializeField] CinemachineVirtualCamera freeCam;
    [SerializeField] float camMovementSpeed = 10.0f;
    [SerializeField] float camRotationSpeed = 30.0f;
    bool playerInFreeCam;

    [SerializeField]
    float switchTimer = 8.0f;

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

    bool doOnce = true;

    private void OnValidate() {
        if (cameras.Count <= 0) return;
        ChangeCam();
    }
    private IEnumerator Start() {
        if (doOnce) {
            doOnce = false;
            OnCameraChanged += ChangeCam;
        }


        do {
            yield return new WaitForSeconds(switchTimer);
            SetCurrentCam(curCamIndex + 1);
        } while (true);

    }
    private void LateUpdate() {
        if (!playerInFreeCam) return;

        MoveFreeCam();
    }
    public void SetCurrentCam(int camIndex) {
        curCamIndex = (camIndex + CameraCount) % CameraCount;
        OnCameraChanged();
        StopAllCoroutines();
        StartCoroutine(Start());
    }

    public void ChangeCam() {
        foreach (var cam in cameras) {
            if (cam == null || !cam.IsValid) return;
            cam.Priority = 0;
        }
        freeCam.Priority = 0;
        cameras[CurrentCameraIndex].Priority = 1;
    }
    public bool ToogleFreeCam() {
        playerInFreeCam = !playerInFreeCam;
        if (playerInFreeCam) {
            StopAllCoroutines();
            freeCam.transform.SetPositionAndRotation(
                cameras[CurrentCameraIndex].transform.position,
                cameras[CurrentCameraIndex].transform.rotation);
            freeCam.Priority = 10;
        } else {
            SetCurrentCam(CurrentCameraIndex);
        }
        return playerInFreeCam;
    }

    void MoveFreeCam() {
        //Set Speed
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        camMovementSpeed += mouseScroll * 10.0f;
        camMovementSpeed = Mathf.Clamp(camMovementSpeed, 0.0f, 100.0f);


        //SetPos
        Vector3 movement = new(Input.GetAxis("Horizontal"), Input.GetAxis("Z"), Input.GetAxis("Vertical"));
        movement *= Time.deltaTime * camMovementSpeed;

        Vector3 nPos = freeCam.transform.position;
        nPos += freeCam.transform.TransformVector(movement);


        //Set Rotation
        Quaternion nRot = freeCam.transform.rotation;
        bool isClicking = Input.GetMouseButton(1);
        Cursor.visible = !isClicking;
        if (isClicking) {
            Vector3 rotation = new(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0.0f);
            rotation *= Time.deltaTime * camRotationSpeed;
            freeCam.transform.eulerAngles -= rotation;
            nRot = freeCam.transform.rotation;
        }


        //Apply
        freeCam.transform.SetPositionAndRotation(nPos, nRot);
    }
}
