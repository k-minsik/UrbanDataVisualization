using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static CanvasGroup zoomedIn, overView, infoPanel;
    void Awake() {
        PlanetManager.Instance.enabled = true;
        BalloonManager.Instance.enabled = true;
        InputManager.Instance.enabled = true;
        CameraManager.Instance.enabled = true;
        ColorManager.Instance.enabled = true;
    }

    // Initialize data
    void Start() {
        ColorManager.Instance.Init();

        InitializeData.InitializePlanetData();

        
        // Instantiate all planets
        PlanetManager.Instance.InstantiateAllPlanets();

        PlanetManager.Instance.InstantiateAllPlanets_AfterCovid();

        // Access Instantiated Planets / Balloons
        //PlanetManager.Instance.GetPlanet_GameObjectWithName("samplePlanet1").GetComponent<MeshRenderer>().material.color = Color.red;
        
        zoomedIn = GameObject.Find("Panel_FocusedView").GetComponent<CanvasGroup>();
        overView = GameObject.Find("Panel_PlanetsView").GetComponent<CanvasGroup>();
        infoPanel = GameObject.Find("Panel_InfoPanel").GetComponent<CanvasGroup>();
        ChangeGameState();



    }
    public static State currentState = State.OverView;
    public static void ChangeGameState() {
        Debug.Log("Changed Gamestate");
        switch (currentState) {
            case State.OverView:
                GameManager.overView.alpha = 1.0f;
                GameManager.overView.interactable = true;
                GameManager.zoomedIn.alpha = 0.0f;
                GameManager.zoomedIn.interactable = false;
                GameManager.infoPanel.alpha = 0.0f;
                break;
            case State.ZoomIn:
                GameManager.zoomedIn.alpha = 1.0f;
                GameManager.zoomedIn.interactable = true;
                GameManager.overView.alpha = 0.0f;
                GameManager.overView.interactable = false;
                break;
        }
    }

    public enum State {
        OverView,
        ZoomIn
    }
}