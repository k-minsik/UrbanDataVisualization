using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon
{   
    // Constructor with minimum equiered informations
    public Balloon(string _balloonName,Vector3 _balloonPosition, float _sizeOffset, GameObject _balloonMesh, int _colorIdx, bool _isBeforeCovid) {
        this.BalloonName = _balloonName;
        this.BalloonPosition = _balloonPosition;
        this.SizeOffset = _sizeOffset;
        this.BalloonMesh = _balloonMesh;
        this.ColorIdx = _colorIdx;
        this.IsBeforeCovid = _isBeforeCovid;
    }
    
    public string BalloonName {get;}
    public Vector3 BalloonPosition {get;}
    public float SizeOffset {get;}
    public GameObject BalloonMesh {get;} 
    public int ColorIdx {get;}

    public bool IsBeforeCovid {get;}

}