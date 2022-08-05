using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWallScript : MonoBehaviour
{
    public EasyPing game;
    public int playerNumber;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ball")
        {
            // register the score
            game.RegisterScore(playerNumber);
        }
    }

}
