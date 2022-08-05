using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    public string upButton;
    public string downButton;

    private float speed = 50.0f;
    public float offset = 10.0f;

    private EasyPing game;

    // Start is called before the first frame update
    void Start()
    {
        // get the game manager object
        GameObject gameManager = GameObject.Find("GameManager");
        Debug.Log("Game Manager object is: " + gameManager);

        // set the easy ping script
        game = gameManager.GetComponent<EasyPing>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // is the game playing...
        if ( (game.myState == GameState.playing) || (game.myState == GameState.getReady)) {
            MovePaddles();
        }
        
        
    }

    private void MovePaddles()
    {
        // get the current position
        Vector3 currentPosition = transform.position;

        // check the "up" key
        if (Input.GetKey(upButton))
        {
            currentPosition.z += speed * Time.deltaTime;
        }

        // check the "down" key
        if (Input.GetKey(downButton))
        {
            currentPosition.z -= speed * Time.deltaTime;
        }

        // check the range of the z position
        currentPosition.z = Mathf.Clamp(currentPosition.z, -offset, offset);


        // replace the adjusted position
        transform.position = currentPosition;
    }

}
