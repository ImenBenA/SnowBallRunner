using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private const float LANE_DISTANCE = 3.0f;
    private const float TURN_SPEED = 0.05f;

    private bool isRunning = false;

    private CharacterController controller;
	private float jumpForce=7f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private int desiredLane = 1;

    //speed
    private float originalSpeed = 7.0f;
    private float speed = 9.0f;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;


    void Start () {
        controller = GetComponent<CharacterController>();
        //ChangeBall();
        //this.GetComponentInChildren<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (!isRunning)
            return; 

        if (Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed  += speedIncreaseAmount;
            GameManager.Instance.UpdateModifier(speed - originalSpeed);
        }

        if (MobileInputs.Instance.SwipeLeft)
            MoveLane(false);
        if (MobileInputs.Instance.SwipeRight)
            MoveLane(true);

        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
            targetPosition += Vector3.left * LANE_DISTANCE;
        else if (desiredLane == 2)
            targetPosition += Vector3.right * LANE_DISTANCE;

        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;
        if (isGrounded())
        {
            verticalVelocity = -0.1f;
            if (MobileInputs.Instance.SwipeUp)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);
            //FastFalling
            if (MobileInputs.Instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime);

        Vector3 dir = controller.velocity;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
    }


    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool isGrounded()
    {
        Ray groundRay = new Ray(new Vector3(
            controller.bounds.center.x,
            (controller.bounds.center.y - controller.bounds.extents.y)+0.2f,
            controller.bounds.center.z),
            Vector3.down
            );
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

        return Physics.Raycast(groundRay, 0.2f + 0.1f) ;

    }

    public void StartRunning()
    {
        isRunning = true;
    }

    private void Crash()
    {
        isRunning = false;
        explode();
        GameManager.Instance.OnDeath();
    }
    private void OnControllerColliderHit (ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
        }
    }
    public void ChangeBall()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Ball")
                child.GetComponent<MeshRenderer>().enabled = false;
            if (child.name == "AtomBall")
                foreach(Transform child2 in child.transform)
                    child2.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }
    public float cubeSize = 0.2f;
    public int cubeInRow = 5; 
    private void explode()
    {
        gameObject.SetActive(false);
        for (int x  = 0; x < cubeInRow; x++)
        {
            for (int y = 0; y < cubeInRow; y++)
            {
                for (int z = 0; z < cubeInRow; z++)
                {
                    createPieces(x, y, z);
                }
            }
        }
    }
    private void createPieces(int x , int y , int z)
    {
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.transform.position = transform.position + new Vector3(cubeSize*x ,cubeSize * y,cubeSize * y );
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
    }
}
