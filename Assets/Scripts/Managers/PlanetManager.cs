using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : Singleton<PlanetManager>
{
    private Dictionary<string, Planet> planetMap = new Dictionary<string, Planet>();
    private Dictionary<string, Planet> planetMap_AfterCovid = new Dictionary<string, Planet>();
    private Dictionary<string, GameObject> planet_GameObjectMap = new Dictionary<string, GameObject>();

    public GameObject parentObj_Planets;

    void Awake() {
        parentObj_Planets = new GameObject();
        parentObj_Planets.name = "Planets";
    }

    public Planet CreatePlanet(string planetName, Vector3 planetPosition, GameObject planetMesh, List<Balloon> balloonlist) {
        Planet planet = new Planet(planetName, planetPosition, planetMesh, balloonlist);
        Debug.Log("Created Planet : " + planetName);
        return planet;
    }

    public void AddPlanetToMap(Planet planet) {
        if (!planetMap.ContainsKey(planet.PlanetName)) {
            planetMap.Add(planet.PlanetName, planet);
        }
        else Debug.Log("Identical planet already exists in planetMap");
    }

    public void AddPlanet_AfterCovidToMap(Planet planet) {
        if (!planetMap_AfterCovid.ContainsKey(planet.PlanetName)) {
            planetMap_AfterCovid.Add(planet.PlanetName, planet);
            Debug.Log("AfterCovid :" + planet.PlanetName);
        }
        else Debug.Log("Identical planet already exists in planetMap_AfterCovid");
    }

    public Planet GetPlanetByName(string planetName) {
        if (planetMap.ContainsKey(planetName)) {
            return planetMap[planetName];
        }
        throw new System.ArgumentException("Cannot find Planet with Name",planetName);
    }
    public Planet GetPlanet_AfterCovidByName(string planetName) {
        if (planetMap_AfterCovid.ContainsKey(planetName)) {
            return planetMap_AfterCovid[planetName];
        }
        throw new System.ArgumentException("Cannot find AfterCovid Planet with Name",planetName);
    }

    public int GetPlanetsNumber(){
        return planetMap.Count;
    }

    public void InstantiateAllPlanets() {
        foreach (Planet planet in planetMap.Values) {
            Visualize.InstantiatePlanet(planet,parentObj_Planets.transform);
        }
        //parentObj_Planets.transform.Rotate(0f,0f,16f);
    }
    public void InstantiateAllPlanets_AfterCovid() {
        foreach (Planet planet in planetMap_AfterCovid.Values) {
            Visualize.InstantiatePlanet(planet,parentObj_Planets.transform);

            GameObject instGameObject = GetPlanet_GameObjectWithName(planet.PlanetName);
            instGameObject.GetComponent<SphereCollider>().enabled = false;
            Visualize.DisableChildMesh(instGameObject);
        }
        parentObj_Planets.transform.Rotate(0f,0f,16f);
    }

    public void AddPlanet_GameObjectToMap(GameObject instance) {
        if (!planet_GameObjectMap.ContainsKey(instance.name)) {
            planet_GameObjectMap.Add(instance.name,instance);
        }
        else Debug.Log("GameObject Planet with same key already exists");
    }

    public GameObject GetPlanet_GameObjectWithName(string planetName) {
        if (planet_GameObjectMap.ContainsKey(planetName)) {
            return planet_GameObjectMap[planetName];
        }
        throw new System.ArgumentException("Cannot find existing Planet GameObject in Runtime",planetName);
    }

    public void TurOffPlanetMeshRenderer(string planetName) {

    }
}
