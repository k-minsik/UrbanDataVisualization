using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonManager : Singleton<BalloonManager>
{
    private Dictionary<string, Balloon> balloonMap = new Dictionary<string, Balloon>();
    private Dictionary<string, GameObject> balloon_GameObjectMap = new Dictionary<string, GameObject>();

    public Balloon CreateBalloon(Vector3 balloonPosition, float sizeOffset, GameObject balloonMesh, int colorIdx, bool isBeforeCovid) {
        string name = "balloon" + balloonMap.Count.ToString();
        Balloon balloon = new Balloon(name,balloonPosition,sizeOffset,balloonMesh,colorIdx,isBeforeCovid);
        //Debug.Log("Created Balloon with name : " + name);
        return balloon;
    }

    public void AddBalloonToMap(Balloon balloon) {
        if (!balloonMap.ContainsKey(balloon.BalloonName)) {
            balloonMap.Add(balloon.BalloonName, balloon);
        }
        else Debug.Log("Identical balloon already exists in balloonMap");
    }

    public Balloon GetBalloonByName(string balloonName) {
        if (balloonMap.ContainsKey(balloonName)) {
            return balloonMap[balloonName];
        }
        throw new System.ArgumentException("Cannot find Balloon with Name",balloonName);
    }

    public void AddBalloon_GameObjectInMap(GameObject instance) {
        if (!balloon_GameObjectMap.ContainsKey(instance.name)) {
            balloon_GameObjectMap.Add(instance.name,instance);
        }
        else Debug.Log("GameObject Balloon with same key already exists");
    }
    public GameObject GetBalloon_GameObjectWithName(string balloonName) {
        if (balloon_GameObjectMap.ContainsKey(balloonName)) {
            return balloon_GameObjectMap[balloonName];
        }
        throw new System.ArgumentException("Cannot find existing Balloon GameObject in Runtime",balloonName);
    }
}
