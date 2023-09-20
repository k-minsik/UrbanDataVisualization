using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : Singleton<CameraManager>
{
    public Camera mainCamera;
    private Vector3 defaultPosition = new Vector3(0.0f,0.0f,-20f);
    private Vector3 zoomPosition = new Vector3(0.0f,0.0f,-12f);
    private bool corutineRunning = false;
    private Ray myRay;
    private RaycastHit hitray;
    void Awake() {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Transform movingObj = mainCamera.transform;
        //Vector3 targetPos = TefaultPosititon;
        //movingObj.position = Vector3.Lerp(movingObj.position, targetPos, 2 * Time.deltaTime);
        if (Input.GetMouseButton(0) && GameManager.currentState == GameManager.State.ZoomIn) {
            //GameObject plnt = PlanetManager.Instance.GetPlanet_GameObjectWithName(GetCurrentPlanetName());
            //Debug.Log("Got GameObject" + plnt.name);
            //Debug.Log("Mouse1 down");
            PlanetManager.Instance.GetPlanet_GameObjectWithName(GetCurrentPlanetName()).transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"),0f) * 300 *  Time.deltaTime);
        }
        else if (GameManager.currentState == GameManager.State.ZoomIn) {
            PlanetManager.Instance.GetPlanet_GameObjectWithName(GetCurrentPlanetName()).transform.Rotate(new Vector3(0f,0.2f,0f),Space.World);
        }
    }

    public void CameraToZoomPosition(){
        if (!corutineRunning) {
            StartCoroutine(ChangePos(mainCamera.transform, zoomPosition, 1.6f));

            GameManager.currentState = GameManager.State.ZoomIn;
            GameManager.ChangeGameState();
        }
    }
    public void CameraToDefaultPosition(){
        if (!corutineRunning) {
            InputManager.Instance.ShowBeforeCoronaMesh();
            StartCoroutine(ChangePos(mainCamera.transform, defaultPosition, 1.6f));

            GameManager.currentState = GameManager.State.OverView;
            GameManager.ChangeGameState();
        }
    }

    public string GetCurrentPlanetName(){
        myRay = mainCamera.ScreenPointToRay(new Vector2(Screen.width/2.0f, Screen.height/2.0f));
        if (Physics.Raycast(myRay, out hitray)) {
            return hitray.collider.gameObject.name;
        }
        throw new System.ArgumentException("no object collided with ray");
    }

    private IEnumerator ChangePos(Transform movingObj, Vector3 finalpos, float movetime) {
        float elapsedtime = 0f;
        corutineRunning = true;

        while (elapsedtime < movetime) {
            movingObj.position = Vector3.Lerp(movingObj.position, finalpos, 5 * Time.deltaTime);
            elapsedtime += Time.deltaTime;
            //Debug.Log("Time eplapsed" + elapsedtime);
            yield return null;
        }
        movingObj.position = finalpos;
        corutineRunning = false;
        InputManager.Instance.ChangePlanetNameText(GetCurrentPlanetName());
    }
}
