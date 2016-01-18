using UnityEngine;
using System.Collections;

public class PlayerSmallCollider : MonoBehaviour {

    private PlayerMove playerMove;
	private GameController gameController;
    void Awake() {
        playerMove = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerMove>();
		gameController = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<GameController> ();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == Tags.obstacles && playerMove.isSliding) {
            GameController.gameState = GameState.End;
		} else if(other.tag == "Coin"){
			gameController.coin += 1;
			Destroy(other.gameObject);
		}
    }

}
