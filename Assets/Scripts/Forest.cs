using UnityEngine;
using System.Collections;

public class Forest : MonoBehaviour {

    public float startLength = 10;
    public float minLength = 10;
    public float maxLength = 100;

    public Obstacles[] obstacles;

    private Transform player;
    public WayPoints waypoints;
    private int targetWayPointIndex = 0;

    void Awake() {
        GameObject playerGo = GameObject.FindGameObjectWithTag(Tags.player);
        if (playerGo != null) {
            player = playerGo.transform;
        }
        waypoints = transform.Find("waypoints").GetComponent<WayPoints>();
        targetWayPointIndex = waypoints.waypoints.Length - 2;
    }

    void Start() {
        GenerateObstacles();
		GenerateCoins ();
    }



    public Vector3 GetNextWayPoint() {
        while (true) {

            if (waypoints.waypoints[targetWayPointIndex].position.z - player.position.z < 0.5f) {
                targetWayPointIndex--;
                if (targetWayPointIndex < 0) {
                    targetWayPointIndex = 0;

                    Destroy(this.gameObject);
                    Camera.main.SendMessage("UpdateForest", SendMessageOptions.DontRequireReceiver);
                    return waypoints.waypoints[0].position;
                }
            } else {
                return waypoints.waypoints[targetWayPointIndex].position;
            }
        }
    }

    void GenerateObstacles() {
        float z = startLength;
        while (true) {
            float length = Random.RandomRange(minLength, maxLength);
            z += length;
            if (z > 3000) break;
            Vector3 waypoint = GetWayPoint(z);
            GenerateObstacles(waypoint);
        }
    }

    void GenerateObstacles(Vector3 position) {
        int index = Random.Range(0, obstacles.Length-1);
        Obstacles obs = (GameObject.Instantiate(obstacles[index]) as Obstacles);
        obs.InitSelf(position,this.transform);
    }

	void GenerateCoins() {
		float z = startLength;
		while (true) {
			float length = 40;
			z += length;
			if (z > 3000) break;
			Vector3 waypoint = GetWayPoint(z);
			GenerateCoins(waypoint);
		}
	}

	void GenerateCoins(Vector3 position){
		Obstacles obs = (GameObject.Instantiate (obstacles [obstacles.Length-1]) as Obstacles);
		obs.CoinInitSelf (position, this.transform);
	}



    Vector3 GetWayPoint(float z) {
        Transform[] wps = waypoints.waypoints;
        int index = GetIndex(z);

        return Vector3.Lerp(wps[index + 1].position, wps[index].position, (z + wps[wps.Length-1].position.z - wps[index + 1].position.z) / (wps[index].position.z - wps[index + 1].position.z));
    }

    int GetIndex(float z) {
        Transform[] wps = waypoints.waypoints;
        float startZ = wps[wps.Length - 1].position.z;
        int index = 0;
        for (int i = 0; i < wps.Length; i++) {
            if (wps[i].position.z - startZ >= z) {
                index = i;
            } else {
                break;
            }
        }
        return index;
    }


}
