using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    public Planet(string _planetName, Vector3 _planetPosition, GameObject _planetMesh, List<Balloon> _balloons) {
        this.PlanetName = _planetName;
        this.PlanetPosition = _planetPosition;
        this.PlanetMesh = _planetMesh;
        this.Balloons = _balloons;
    }

    public string PlanetName {get;}
    public Vector3 PlanetPosition {get;}
    public List<Balloon> Balloons {get;}
    public GameObject PlanetMesh {get;}


    public void SetPlanetMetaData(string[] _dataString) {
        this.DataString = _dataString;
    }
    public string[] DataString {get; set;}
}