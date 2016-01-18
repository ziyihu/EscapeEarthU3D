using UnityEngine;
using System.Collections;

public enum TouchDir {
    None,
    Left,
    Right,
    Top,
    Bottom
}

public class PlayerMove : MonoBehaviour {
    public AudioSource footLand;
    public float jumpHeight = 30f;
    public float speed = 10;
    public float horizontalMoveSpeed = 0.2f;
    public float jumpSpeed = 0.2f;
    public float minTouchLength = 50;
    public Transform prisoner;

    public bool isJumping = false;
    public float slideTime = 1.4f;

    public int nowLaneIndex = 1;
    public int targetLaneIndex = 1;

    public bool isSliding = false;

    private float slideTimer=0;
    private Animation animation;
    private float targetJumpHeight = 0;
    private EnvGenerator env;
    private Vector3 moveDownPos = Vector3.zero;
    private float moveHorizontal = 0;

	public KeyCode keyCode;

    void Awake() {
        env = Camera.main.GetComponent<EnvGenerator>();
        animation = transform.Find("Prisoner").animation;
    }
	
	// Update is called once per frame
	void Update () {
        if (GameController.gameState == GameState.Playing) {
            Vector3 pos = transform.position;
            Vector3 nextWayPoint = env.forest1.GetNextWayPoint();
            nextWayPoint = new Vector3(nextWayPoint.x + GameController.xOffsets[targetLaneIndex], nextWayPoint.y, nextWayPoint.z);
            Vector3 dir = nextWayPoint - transform.position;
            Vector3 moveDir = dir.normalized * speed * Time.deltaTime;
            this.transform.position += moveDir;
            transform.rotation = Quaternion.LookRotation(new Vector3(nextWayPoint.x, transform.position.y, nextWayPoint.z)-transform.position, Vector3.up);
            //transform.LookAt(nextWayPoint);

            if (targetLaneIndex != nowLaneIndex) {
                float move = moveHorizontal * horizontalMoveSpeed;
                moveHorizontal -= moveHorizontal * horizontalMoveSpeed;
                this.transform.position = new Vector3(transform.position.x + move, transform.position.y, transform.position.z);
                if (Mathf.Abs(moveHorizontal) < 0.5f) {
                    this.transform.position = new Vector3(transform.position.x + moveHorizontal, transform.position.y, transform.position.z);
                    nowLaneIndex = targetLaneIndex;
                }
            }
            if (isJumping) {
                if(targetJumpHeight>0){
                    float yJump = jumpSpeed * Time.deltaTime;
                    targetJumpHeight -= yJump;
                    this.prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y + yJump, prisoner.position.z);
                    if (Mathf.Abs(targetJumpHeight) < 0.5f) {
                        this.prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y + targetJumpHeight, prisoner.position.z);
                        targetJumpHeight = -jumpHeight;
                    }
                }else{
                    float yJump = -jumpSpeed * Time.deltaTime;
                    targetJumpHeight -= yJump;
                    this.prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y + yJump, prisoner.position.z);
                    if (Mathf.Abs(targetJumpHeight) < 0.5f) {
                        this.prisoner.position = new Vector3(prisoner.position.x, prisoner.position.y + targetJumpHeight, prisoner.position.z);
                        isJumping = false;
                        footLand.Play();
                    }
                }
            }

            if(isSliding){
                slideTimer += Time.deltaTime;
                if (slideTimer > slideTime) {
                    isSliding = false;
                    slideTimer = 0;
                }
            }

            MoveControll();
//			KeyboardControll();
            
        }
	}

    void FixedUpdate() {
        
    }

//	void KeyboardControll(){
//		if (Input.GetKeyDown (keyCode)) {
//						if (keyCode == KeyCode.A) {
//								targetLaneIndex++;
//								moveHorizontal = 14;
//						}
//		
//		
//						if (keyCode == KeyCode.D) {
//								targetLaneIndex--;
//								moveHorizontal = -14;
//						}
//		
//						if (keyCode == KeyCode.W) {
//								isJumping = true;
//								targetJumpHeight = jumpHeight;
//						}
//		
//						if (keyCode == KeyCode.S) {
//								if (!isJumping) {
//										isSliding = true;
//								}
//						}
//				}
//	}

    void MoveControll() {
        TouchDir touchDir = GetTouchDir();
        switch (touchDir) {
            case TouchDir.None:
                break;
            case TouchDir.Right:
                if (targetLaneIndex < 2) {
                    targetLaneIndex++;
                    moveHorizontal = 14;
                }
                break;
            case TouchDir.Left:
                if (targetLaneIndex > 0) {
                    targetLaneIndex--;
                    moveHorizontal = -14;
                }
                break;
            case TouchDir.Top:
                if (isJumping == false) {
                    isJumping = true;
                    targetJumpHeight = jumpHeight;
                }
                break;
            case TouchDir.Bottom:
                if (!isJumping) {
                    isSliding=true;
                }
                break;
        }

    }


    TouchDir GetTouchDir() {
        if (Input.GetMouseButtonDown(0)) {
            moveDownPos = Input.mousePosition;
            return TouchDir.None;
        }
		if (Input.GetKeyDown (KeyCode.W)) {
			return TouchDir.Top;
		} else if(Input.GetKeyDown(KeyCode.S)){
			return TouchDir.Bottom;
		} else if(Input.GetKeyDown(KeyCode.A)){
			return TouchDir.Left;
		} else if(Input.GetKeyDown(KeyCode.D)){
			return TouchDir.Right;
		}
        if (Input.GetMouseButtonUp(0)) {
            Vector3 moveOffset = Input.mousePosition - moveDownPos;
            if( Mathf.Abs( moveOffset.y ) > Mathf.Abs( moveOffset.x ) && moveOffset.y > minTouchLength){
                return TouchDir.Top;
            }

            if (Mathf.Abs(moveOffset.y) > Mathf.Abs(moveOffset.x) && moveOffset.y < -minTouchLength) {
                return TouchDir.Bottom;
            }

            if (Mathf.Abs(moveOffset.y) < Mathf.Abs(moveOffset.x) && moveOffset.x > minTouchLength) {
                return TouchDir.Right;
            }

            if (Mathf.Abs(moveOffset.y) < Mathf.Abs(moveOffset.x) && moveOffset.x < -minTouchLength) {
                return TouchDir.Left;
            }

        }
            return TouchDir.None;
    }

}
