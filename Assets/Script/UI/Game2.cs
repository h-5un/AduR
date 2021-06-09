using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2 : MonoBehaviour
{
    public int startingPitch = 450;
    public int timeToDecrease = 1000;
    public GameObject testpitch;
    public AudioSource audioSource;

    // Test for Pitch
    // Start is called before the first frame update
    void Start()
    {
        //audioSource.pitch = startingPitch;
        testpitch.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.pitch > 0)
            audioSource.pitch -= Time.deltaTime * startingPitch / timeToDecrease / 2;
    }
}
