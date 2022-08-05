using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    public GameObject myBall;
    public GameObject leftPaddle;
    public GameObject rightPaddle;

    public Vector3 direction;
    public float speed;

    public float paddleSpeed;

    private bool clearedRight = false;
    private bool clearedLeft = false;

    // Boundary Variables
    private float FIELD_LEFT = -48.5f;
    private float FIELD_RIGHT = 48.5f;
    private float FIELD_TOP = 23.5f;
    private float FIELD_BOTTOM = -23.5f;

    /* New Variables for Assignment 1 */
    // paddle variables
    private float PADDLE_OFFSET = 5.0f;
    private float PADDLE_RIGHT = 37.5f;
    private float PADDLE_LEFT = -37.5f;

    // Start is called before the first frame update
    void Start()
    {
        myBall.transform.position = new Vector3();  // Reset the ball to 0,0,0
        // normalize the direction vector
        direction.Normalize();

        Time.timeScale = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        // move the ball
        myBall.transform.position = myBall.transform.position + (direction * speed * Time.deltaTime);

        // check the ball position
        CheckBall();
        CheckPaddles();  // check the paddles too...

        // move the paddles
        MovePaddles();

    }

    private void CheckBall()
    {
        Vector3 curPos = myBall.transform.position;

        if (curPos.z >= FIELD_TOP)
        {
            // bounce off of the top
            BounceBall(curPos, "top");
        } else if (curPos.z <= FIELD_BOTTOM)
        {
            // bounce off of the bottom
            BounceBall(curPos, "bottom");
        }

        if (curPos.x <= FIELD_LEFT)
        {
            // bounce off of the left
            BounceBall(curPos, "left");
        } else if (curPos.x >= FIELD_RIGHT)
        {
            // bounce off of the right
            BounceBall(curPos, "right");
        }

    }

    /* New Function for Assignment 1 */
    private void CheckPaddles()
    {
        Vector3 curPos = myBall.transform.position;
        Vector3 leftPadPos = leftPaddle.transform.position;
        Vector3 rightPadPos = rightPaddle.transform.position;

        // Check if paddle line crossed
        if (curPos.x > PADDLE_RIGHT)
        {
            if (!clearedRight) { 

                // paddle line crossed... check the paddle position
                if ((curPos.z <= rightPadPos.z + PADDLE_OFFSET) && (curPos.z >= rightPadPos.z - PADDLE_OFFSET))
                {
                    // process the bounce
                    BounceBall(curPos, "rightPad");
                } else
                {
                    clearedRight = true;
                }
            }

        } else
        {
            clearedRight = false;
        }

        if (curPos.x < PADDLE_LEFT)
        {
            // paddle line crossed... check the paddle position
            if ((curPos.z <= leftPadPos.z + PADDLE_OFFSET) && (curPos.z >= leftPadPos.z - PADDLE_OFFSET))
            {
                // process the bounce
                BounceBall(curPos, "leftPad");
            }
        }

    }

    private void BounceBall(Vector3 bouncePos, string bounceObj)
    {
        switch(bounceObj)
        {
            case "top":
                // bouncing off of the top
                bouncePos.z = FIELD_TOP - (bouncePos.z - FIELD_TOP);
                direction.z = -1.0f * direction.z;
                break;

            case "bottom":
                // bounce off of the bottom
                bouncePos.z = FIELD_BOTTOM - (bouncePos.z - FIELD_BOTTOM);
                direction.z = -direction.z;
                break;

            case "left":
                // bounce off of the left
                bouncePos.x = 2 * FIELD_LEFT - bouncePos.x;
                direction.x = -direction.x;
                break;

            case "right":
                // bounce off of the right
                bouncePos.x = 2 * FIELD_RIGHT - bouncePos.x;
                direction.x *= -1;
                break;

            /* New case option for paddle bounce */
            case "rightPad":
                // bounce off of the right paddle
                bouncePos.x = 2 * PADDLE_RIGHT - bouncePos.x;
                direction.x *= -1;
                break;

            /* New case option for paddle bounce */
            case "leftPad":
                // bounce off of the right paddle
                bouncePos.x = 2 * PADDLE_LEFT - bouncePos.x;
                direction.x *= -1;
                break;


            default:
                Debug.Log("BounceBall Switch reached default");
                break;
        }

        //update the ball position
        myBall.transform.position = bouncePos;

    }

    private void MovePaddles()
    {
        // get the current position of the paddles
        Vector3 leftPadPos = leftPaddle.transform.position;
        Vector3 rightPadPos = rightPaddle.transform.position;

        // adjust that position based on the keys pressed
        if (Input.GetKey("up"))
        {
            rightPadPos.z = rightPadPos.z + (paddleSpeed * Time.deltaTime);
        }
        if (Input.GetKey("down"))
        {
            rightPadPos.z = rightPadPos.z - (paddleSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            leftPadPos.z += (paddleSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            leftPadPos.z -= (paddleSpeed * Time.deltaTime);
        }


        // put the new position back into the object's transform
        rightPaddle.transform.position = rightPadPos;
        leftPaddle.transform.position = leftPadPos;

    }



}
