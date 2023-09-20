using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualize : MonoBehaviour
{

    public static void InstantiatePlanet(Planet planet, Transform parent) {

        GameObject instantiatedPlanet = Instantiate(planet.PlanetMesh,parent.position + planet.PlanetPosition, Quaternion.identity, parent);

        //Debug.Log("Instantiated Planet : " + planet.PlanetName);
        instantiatedPlanet.name = planet.PlanetName;
        PlanetManager.Instance.AddPlanet_GameObjectToMap(instantiatedPlanet);

        foreach (Balloon balloon in planet.Balloons) {
            InstantiateBalloon(balloon, instantiatedPlanet.transform);
        }
    }

    private static void InstantiateBalloon(Balloon balloon, Transform parent) {

        GameObject instantiatedBalloon = Instantiate(balloon.BalloonMesh, parent.position + balloon.BalloonPosition, Quaternion.identity, parent);

        //set balloon color with balloon.BalloonColor 
        instantiatedBalloon.GetComponent<MeshRenderer>().material = ColorManager.Instance.GetMaterial(balloon.ColorIdx);

        //set balloon scale with balloon.SizeOffset
        instantiatedBalloon.GetComponent<Transform>().localScale = Vector3.one * balloon.SizeOffset;

        instantiatedBalloon.name = balloon.BalloonName;
        BalloonManager.Instance.AddBalloon_GameObjectInMap(instantiatedBalloon);

        //Debug.Log("Instantiated Balloon : " + balloon.BalloonName);

    }
    public static void DisableChildMesh(GameObject obj) {
        foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>()) {
            renderer.enabled = false;
        }
    
    }
    public static void EnableChildMesh(GameObject obj) {
        foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>()) {
            renderer.enabled = true;
        }
    
    }

}
