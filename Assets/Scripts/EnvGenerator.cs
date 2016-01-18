using UnityEngine;
using System.Collections;

public class EnvGenerator : MonoBehaviour {

    public GameObject[] forests;

    private int forestCount=2;

    public Forest forest1;
    public Forest forest2;

	// Use this for initialization
	void Start () {

	}

    Forest GenerateForest() {
        forestCount++;
        float z = 3000 * forestCount;
        int index = Random.Range(0, 3);//0 1 2 
        GameObject go = GameObject.Instantiate(forests[index], new Vector3(0, 0, z), Quaternion.identity) as GameObject;
        return go.GetComponent<Forest>();
    }

    public void UpdateForest() {
        forest1 = forest2;
        forest2 = GenerateForest();
    }

	
}
