using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 해상도 설정을 다루는 ScreenManager Code
*/
public class ScreenManager : MonoBehaviour
{
    void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
