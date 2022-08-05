using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private AudioSource audio;
    private Rigidbody rb;

    public AudioClip wallSound;
    public AudioClip paddleSound;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            MakeWallSound();

        } else if (collision.gameObject.tag == "Paddle")
        {
            MakePaddleSound();

            if (audio.pitch <= 2.0f) { 
                // increase the pitch
                audio.pitch += 0.1f;

                // increase the speed
                rb.velocity *= 1.1f;
            }

        } else if (collision.gameObject.tag == "BackWall")
        {
            audio.pitch = 1.0f;
            MakeDeathSound();
        }

    }

    private void MakeWallSound() 
    {
        audio.PlayOneShot(wallSound);
    }

    private void MakeDeathSound()
    {
        audio.PlayOneShot(deathSound);
    }

    private void MakePaddleSound()
    {
        audio.PlayOneShot(paddleSound);
    }


}
