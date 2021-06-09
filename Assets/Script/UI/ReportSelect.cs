using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportSelect : MonoBehaviour
{
    public GameObject Game1, Game2;
    int Q1_flag = 0;
    // int Q2_Flag = 0;

    void Awake()
    {
        // 퀴즈 플레이 이력 불러오기
        Q1_flag = DataManager.instance.CheckQuiz1_Flag();
        Debug.Log(Q1_flag);
    }

    // Start is called before the first frame update
    void Start()
    {

        // 이전에 퀴즈를 완료했을 경우, 버튼 활성화
        if (Q1_flag == 1)
        {
            Game1.SetActive(true);
        }
        /*
        if (Quiz2_Flag == 1)
        {
            Game2.SetActive(true);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {

    }
}
