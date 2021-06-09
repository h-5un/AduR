using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 각 씬으로 이동하는 기능을 다루는 SceneManager 코드
 */

public class MoveScene : MonoBehaviour
{

    public void Move2GameSelect()
    {
        SceneManager.LoadScene("GameSelect");
    }

    public void Move2Ballon()
    {
        SceneManager.LoadScene("BalloonExp");
    }

    public void Move2ReportSelect()
    {
        SceneManager.LoadScene("ReportSelect");
    }

    public void Move2Main()
    {
        SceneManager.LoadScene("Main");
    }

    public void Move2Game1()
    {
        SceneManager.LoadScene("Game1");
    }

    public void MoveReport1()
    {
        SceneManager.LoadScene("Report_Game1");
    }

    public void Move2Report2()
    {
        SceneManager.LoadScene("Report2");
    }

    public void Move2Quiz1()
    {
        SceneManager.LoadScene("Quiz_Game1");
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
