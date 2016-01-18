using UnityEngine;
using System.Collections;

public enum GameState {
    Menu,
    Playing,
    End
}

public class GameController : MonoBehaviour {
//	public int fontSize = 18;
    public GameObject startUI;
    public GameObject gameoverUI;
	public bool restart = false;


    public static int[] xOffsets = new int[3]{
                                  -14,0,14
                              };


    public static GameState gameState = GameState.Menu;
	public int coin;
    private int score;
    private float startZ;
    private Transform player;

    void Awake() {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        startZ = player.position.z;
    }

    void Update() {
        if (GameController.gameState == GameState.Menu) {
            if (Input.GetMouseButtonDown(0)) {
                gameState = GameState.Playing;
                startUI.SetActive(false);
            }
        }
        if (GameController.gameState == GameState.End) {
            gameoverUI.SetActive(true);
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel(Application.loadedLevel);
				gameoverUI.SetActive(false);
				restart = true;
				gameState = GameState.Menu;
			}
        }
        score = (int)(player.position.z - startZ);
    }


    void OnGUI() {
        GUILayout.Label("Score : " + score);
		GUILayout.Label ("Coins : " + coin);
		if (GameController.gameState == GameState.End) {
			GUILayout.Label("Press R to START again");
		}
    }

}
