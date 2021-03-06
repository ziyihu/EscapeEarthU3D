﻿using UnityEngine;
using System.Collections;

public class PoliceCar : MonoBehaviour {

    public float followSpeed=1;
    public AudioSource squealing;

    private Transform player;
    private Vector3 offset;
    private bool havePlaySquealing = false;
	private GameController gameController;

    void Awake() {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		gameController = Camera.main.GetComponent<GameController> ();
        offset = transform.position - player.position;
    }


	// Update is called once per frame
	void Update () {
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        if (GameController.gameState == GameState.End) {
            if (havePlaySquealing == false) {
                squealing.Play();
                havePlaySquealing = true;
            }
        }
//		if (gameController.restart == true) {
//			transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z-10);
//		}

	}


}
