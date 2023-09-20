using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputManager : Singleton<InputManager>
{
    public Button buttonR, buttonL, buttonB, buttonBeforeCorona, buttonAfterCorona;
    public CanvasGroup infoPanel;
    private TextMeshProUGUI planetName;
    private TextMeshProUGUI d01, d02, d03, d04, d05, d06, d07, d08, d09, d10;
    private bool corutineRunning, isAfterCorona = false;
    private Vector3 moveVector = new Vector3(0f,0f,0.1f);
    // Start is called before the first frame update
    void Start()
    {
        buttonR = GameObject.Find("Button_ToRight").GetComponent<Button>(); 
        buttonL = GameObject.Find("Button_ToLeft").GetComponent<Button>();
        buttonB = GameObject.Find("Button_Back").GetComponent<Button>();
        buttonBeforeCorona = GameObject.Find("Button_BeforeCorona").GetComponent<Button>();
        buttonAfterCorona = GameObject.Find("Button_AfterCorona").GetComponent<Button>();

        infoPanel = GameObject.Find("Panel_InfoPanel").GetComponent<CanvasGroup>();

        planetName = GameObject.Find("Text_CurrentPlanetName").GetComponent<TextMeshProUGUI>();

        d01 = GameObject.Find("Data_01").GetComponent<TextMeshProUGUI>();
        d02 = GameObject.Find("Data_02").GetComponent<TextMeshProUGUI>();
        d03 = GameObject.Find("Data_03").GetComponent<TextMeshProUGUI>();
        d04 = GameObject.Find("Data_04").GetComponent<TextMeshProUGUI>();
        d05 = GameObject.Find("Data_05").GetComponent<TextMeshProUGUI>();
        d06 = GameObject.Find("Data_06").GetComponent<TextMeshProUGUI>();
        d07 = GameObject.Find("Data_07").GetComponent<TextMeshProUGUI>();
        d08 = GameObject.Find("Data_08").GetComponent<TextMeshProUGUI>();
        d09 = GameObject.Find("Data_09").GetComponent<TextMeshProUGUI>();
        d10 = GameObject.Find("Data_10").GetComponent<TextMeshProUGUI>();

        buttonR.onClick.AddListener(()=>InputManager.Instance.RotatePlanetsR());
        buttonL.onClick.AddListener(()=>InputManager.Instance.RotatePlanetsL());
        buttonB.onClick.AddListener(()=>CameraManager.Instance.CameraToDefaultPosition());

        buttonAfterCorona.onClick.AddListener(()=>InputManager.Instance.ShowAfterCoronaMesh());
        buttonBeforeCorona.onClick.AddListener(()=>InputManager.Instance.ShowBeforeCoronaMesh());
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.currentState == GameManager.State.ZoomIn) {
            infoPanel.alpha = 1.0f;
        }
        if (Input.GetMouseButtonDown(0) && GameManager.currentState == GameManager.State.OverView) {
            CameraManager.Instance.CameraToZoomPosition();
        }

        if (Input.mouseScrollDelta.y != 0 && GameManager.currentState == GameManager.State.OverView) {
            if (Input.mouseScrollDelta.y > 0) {
                if (!corutineRunning) {
                    StartCoroutine(RotateObject(PlanetManager.Instance.parentObj_Planets, 0.6f, 360.0f/PlanetManager.Instance.GetPlanetsNumber()));
                }
            }
            else if (Input.mouseScrollDelta.y < 0) {
                if (!corutineRunning) {
                    StartCoroutine(RotateObject(PlanetManager.Instance.parentObj_Planets, 0.6f, -360.0f/PlanetManager.Instance.GetPlanetsNumber()));
                }
            }
        }
        else if (Input.mouseScrollDelta.y != 0 && GameManager.currentState == GameManager.State.ZoomIn) {
            if (Input.mouseScrollDelta.y > 0) {
                CameraManager.Instance.mainCamera.transform.position += moveVector;
            }                                                           
            else if (Input.mouseScrollDelta.y < 0) {                    
                CameraManager.Instance.mainCamera.transform.position -= moveVector;
            }
        }

    }

    public void RotatePlanetsR() {
        Debug.Log("Button pressed");
        if (!corutineRunning) {
            StartCoroutine(RotateObject(PlanetManager.Instance.parentObj_Planets, 0.6f, 360.0f/PlanetManager.Instance.GetPlanetsNumber()));
        }
    }
    public void RotatePlanetsL() {
        if (!corutineRunning) {
            StartCoroutine(RotateObject(PlanetManager.Instance.parentObj_Planets, 0.6f, -360.0f/PlanetManager.Instance.GetPlanetsNumber()));
        }
    }

    public void ChangePlanetNameText(string writetext) {
        planetName.text = writetext;
    }
    public void ChangeInfoPanelText(string[] textlist) {
        d01.text = textlist[0];
        d02.text = textlist[1];
        d03.text = textlist[2];
        d04.text = textlist[3];
        d05.text = textlist[4];
        d06.text = textlist[5];
        d07.text = textlist[6];
        d08.text = textlist[7];
        d09.text = textlist[8];
        d10.text = textlist[9];
    }
    
    public void ShowAfterCoronaMesh() {
        if (isAfterCorona == false) {
            isAfterCorona = true;
            string currentPlanetName = CameraManager.Instance.GetCurrentPlanetName();
            GameObject currentPlanetObject = PlanetManager.Instance.GetPlanet_GameObjectWithName(currentPlanetName);
            GameObject nextPlanetObject = PlanetManager.Instance.GetPlanet_GameObjectWithName(currentPlanetName + "AfterCovid");
            Visualize.DisableChildMesh(currentPlanetObject);
            Visualize.EnableChildMesh(nextPlanetObject);
            currentPlanetObject.GetComponent<SphereCollider>().enabled = false;
            nextPlanetObject.GetComponent<SphereCollider>().enabled = true;
        }
    }

    public void ShowBeforeCoronaMesh() {
        if (isAfterCorona == true) {
            isAfterCorona = false;
            string currentPlanetName = CameraManager.Instance.GetCurrentPlanetName();
            GameObject currentPlanetObject = PlanetManager.Instance.GetPlanet_GameObjectWithName(currentPlanetName);
            GameObject nextPlanetObject = PlanetManager.Instance.GetPlanet_GameObjectWithName(currentPlanetName.TrimEnd("AfterCovid".ToCharArray()));
            Visualize.DisableChildMesh(currentPlanetObject);
            Visualize.EnableChildMesh(nextPlanetObject);
            currentPlanetObject.GetComponent<SphereCollider>().enabled = false;
            nextPlanetObject.GetComponent<SphereCollider>().enabled = true;
        }
    }

    private IEnumerator RotateObject(GameObject movingObj, float movetime, float angle) {

        if (isAfterCorona == true) {
            ShowBeforeCoronaMesh();
        }
        float elapsedtime = 0f;
        var rotate_from = movingObj.transform.rotation;
        var rotate_to = movingObj.transform.rotation * Quaternion.Euler(0f,angle,0f);
        corutineRunning = true;

        while (elapsedtime < movetime) {
            float lerpamount = elapsedtime / movetime;
            movingObj.transform.rotation = Quaternion.Slerp(rotate_from, rotate_to, lerpamount);
            elapsedtime += Time.deltaTime;
            //Debug.Log("Time eplapsed" + elapsedtime);
            yield return null;
        }
        movingObj.transform.rotation = rotate_to;
        corutineRunning = false;
        string plntName = CameraManager.Instance.GetCurrentPlanetName();
        ChangePlanetNameText(plntName);
        ChangeInfoPanelText(PlanetManager.Instance.GetPlanetByName(plntName).DataString);

        // in zoomin state, reset zoom if rotate
        if (GameManager.currentState == GameManager.State.ZoomIn) {
            CameraManager.Instance.CameraToZoomPosition();
        }
    }

}
