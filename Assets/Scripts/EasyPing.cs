using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { none, setup, getReady, playing, gameOver, oops};


public class EasyPing : MonoBehaviour
{
    public GameObject ballObject;
    private Rigidbody ballRigidbody;
    private Renderer ballRender;
    private AudioSource audio;

    public AudioClip readySound;
    public AudioClip goSound;

    public float force;
    public Vector3 direction;

    // declare my state variable and set a value
    public GameState myState = GameState.none;

    // score tracking
    private int playerOneScore;
    private int playerTwoScore;

    private int MAX_SCORE = 3;

    // get my ui text values
    public Text overlayMessage;
    public Text scoreMessage;

    


    // Start is called before the first frame update
    void Start()
    {
        // fetch the rigidbody & renderer
        ballRigidbody = ballObject.GetComponent<Rigidbody>();
        ballRender = ballObject.GetComponent<Renderer>();
        audio = ballObject.GetComponent<AudioSource>();

        setGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (myState == GameState.none) { 
            if (Input.GetKeyDown(KeyCode.S))
            {
                // Initilize Round
                InitRound();
            }
        }
        if (myState == GameState.gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Initilize Round
                setGame();
            }
        }
    }

    private void setGame()
    {
        myState = GameState.none;
        playerOneScore = 0;
        playerTwoScore = 0;

        // set the text message to Get Ready!!!
        overlayMessage.text = "Press \"S\" to start...";
        overlayMessage.enabled = true;

        scoreMessage.enabled = false;
    }

    private void InitRound()
    {
        // set the game state
        myState = GameState.setup;

        // reset the ball
        ResetBall();

        // start the coroutine
        StartCoroutine(GetReady());

    }

    public IEnumerator GetReady()
    {
        // set the get ready message
        overlayMessage.enabled = false;
        overlayMessage.text = "Get Ready!!!";

        // hide the score for first round
        if (playerOneScore == 0 && playerTwoScore == 0)
        {
            scoreMessage.enabled = false;
        }

        myState = GameState.getReady; // set the gamestate to get ready

        for (int i=0; i<3; i++)
        {

            audio.PlayOneShot(readySound);
            overlayMessage.text = 3 - i + "!!!!";
            // turn message on, and ball off
            overlayMessage.enabled = true;
            ballRender.enabled = false;

            yield return new WaitForSeconds(0.5f); // wait 1/2 second
            

            // turn message off, and ball on
            overlayMessage.enabled = false;
            ballRender.enabled = true;

            yield return new WaitForSeconds(0.5f); // wait 1/2 second

        }

        overlayMessage.enabled = true;
        overlayMessage.text = "GO!!!!";
        scoreMessage.enabled = true;
        audio.PlayOneShot(goSound);
        yield return new WaitForSeconds(0.5f);
        overlayMessage.enabled = false;
        // start the ball
        StartBall();
    }

    private void ResetBall()
    {
        // make sure we control the ball, not physics
        ballRigidbody.isKinematic = true;

        // move the ball to the start position
        ballObject.transform.position = Vector3.zero;
    }
    
    private void StartBall()
    {
        // turn kinematics off
        ballRigidbody.isKinematic = false;

        // normalize the directional vector
        direction.Normalize();
       
        int xDirection = Random.Range(0, 1);
        if (xDirection == 0)
        {
            xDirection = -1;
        }
        float zDirection = Random.Range(-0.8f, 0.8f);

        direction.Set(xDirection, 0, zDirection);

        // launch the ball
        ballRigidbody.AddForce((direction * force), ForceMode.VelocityChange);

        // update the gamestate
        myState = GameState.playing;
    }

    public void RegisterScore(int playerNumber)
    {
        // stop the action
        ballRigidbody.isKinematic = true;

        // who scored?
        if (playerNumber == 1)
        {
            playerOneScore++;
            StartCoroutine(EnterOops("One"));


        } else if (playerNumber == 2)
        {
            playerTwoScore++;
            StartCoroutine(EnterOops("Two"));

        } else
        {
            Debug.Log("Unassigned player number was submitted");
        }

    }

    public IEnumerator EnterOops(string player)
    {
        Debug.Log("The Score is " + playerOneScore + "-" + playerTwoScore);
        scoreMessage.text = playerOneScore + " : " + playerTwoScore;

        bool gameOverStatus = true;
        
        if (playerOneScore == 3)
        {
            overlayMessage.text = "Player One Wins!!!";
        }
        else if (playerTwoScore == 3)
        {
            overlayMessage.text = "Player Two Wins!!!";
        }
        else
        {
            gameOverStatus = false;
            overlayMessage.text = "Player " + player + " Scores!!!";
        }
        // set the get ready message
        overlayMessage.enabled = true;
        yield return new WaitForSeconds(2f);
        overlayMessage.enabled = false;

        if (gameOverStatus)
        {
            myState = GameState.gameOver;
            overlayMessage.text = "Game Over!\nPress \"R\" to restart...\n Score 3 Rounds to Win!";
            overlayMessage.enabled = true;
        }
        else
        {
            // gamestate is no longer playing
            myState = GameState.setup;

            // start a new round
            InitRound();
        }
    }


}
