using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    // 싱글톤 패턴
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            Quiz1_Load(); // 기존에 저장된 값을 불러오며 시작
            Quiz2_Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 데이터 저장 변수
    public int Quiz1_Flag = 0;   // Quiz1을 플레이 한 여부가 저장되는 flag(flag = 1일 시 report가 등록됨)
    public int Quiz1_Score = -1;   // Quiz1의 점수
    public int before_score = -1;       // 이전에 사용자가 풀었던 퀴즈의 점수
    int checkflag1;
    int checkscore1;

    public int Quiz2_Flag = 0;   // Quiz1을 플레이 한 여부가 저장되는 flag(flag = 1일 시 report가 등록됨)
    public int Quiz2_Score = -1;   // Quiz1의 점수
    public int before_score_2 = -1;       // 이전에 사용자가 풀었던 퀴즈의 점수
    int checkflag2;
    int checkscore2;


    // Quiz 1
    // 이전 flag 및 점수 불러오기
    public void Quiz1_Load()
    {
        Quiz1_Flag = PlayerPrefs.GetInt("Q1_Flag", 0);
        Quiz1_Score = PlayerPrefs.GetInt("Q1_Score", 0);
    }

    // 퀴즈 완료 유무(flag) 저장
    public void Quiz1_FlagSet(int flag)
    {
        PlayerPrefs.SetInt("Q1_Flag", flag);
    }

    // 데이터 값 저장
    public void Quiz1_Save(int new_score_1)
    {
        Debug.Log("데이터 저장 시작...");

        // 기존에 저장된 데이터가 있다면
        if (Quiz1_Flag == 1)
        {
            before_score = PlayerPrefs.GetInt("Q1_Score"); // 이전 퀴즈 점수 불러옴
            // 이전 데이터와의 점수 비교
            if (before_score >= new_score_1)
            {
                PlayerPrefs.SetInt("Q1_Score", before_score);       // 기존 점수 유지
                Debug.Log("Data 유지...");
            }
            else
            {
                PlayerPrefs.SetInt("Q1_Score", new_score_1);       //Update
                Debug.Log("Data Saved");
            }
        }

        // 기존에 저장된 데이터가 없다면
        if (Quiz1_Flag != 1)
        {
            Quiz1_Flag = 1;         // flag를 1로 전환


            PlayerPrefs.SetInt("Q1_Flag", Quiz1_Flag); // 전환된 flag 저장
            PlayerPrefs.SetInt("Q1_Score", new_score_1);
            Debug.Log("Data Saved");
        }
    }


    // 이전 flag 판별
    public int CheckQuiz1_Flag()
    {
        checkflag1 = PlayerPrefs.GetInt("Q1_Flag");
        return checkflag1;
    }

    // 이전 score 판별
    public int CheckQuiz1_Score()
    {
        checkscore1 = PlayerPrefs.GetInt("Q1_Score");
        return checkscore1;
    }



    // Quiz 2
    // 이전 flag 및 점수 불러오기
    public void Quiz2_Load()
    {
        Quiz2_Flag = PlayerPrefs.GetInt("Q2_Flag", 0);
        Quiz2_Score = PlayerPrefs.GetInt("Q2_Score", 0);
    }

    // 퀴즈 완료 유무(flag) 저장
    public void Quiz2_FlagSet(int flag)
    {
        PlayerPrefs.SetInt("Q2_Flag", flag);
    }

    // 데이터 값 저장
    public void Quiz2_Save(int new_score_2)
    {
        Debug.Log("데이터 저장 시작...");

        // 기존에 저장된 데이터가 있다면
        if (Quiz2_Flag == 1)
        {
            before_score_2 = PlayerPrefs.GetInt("Q2_Score"); // 이전 퀴즈 점수 불러옴
            // 이전 데이터와의 점수 비교
            if (before_score_2 >= new_score_2)
            {
                PlayerPrefs.SetInt("Q2_Score", before_score_2);       // 기존 점수 유지
                Debug.Log("Data 유지...");
            }
            else
            {
                PlayerPrefs.SetInt("Q2_Score", new_score_2);       //Update
                Debug.Log("Data Saved");
            }
        }

        // 기존에 저장된 데이터가 없다면
        if (Quiz2_Flag != 1)
        {
            Quiz2_Flag = 1;         // flag를 1로 전환


            PlayerPrefs.SetInt("Q2_Flag", Quiz2_Flag); // 전환된 flag 저장
            PlayerPrefs.SetInt("Q2_Score", new_score_2);
            Debug.Log("Data Saved");
        }
    }


    // 이전 flag 판별
    public int CheckQuiz2_Flag()
    {
        checkflag2 = PlayerPrefs.GetInt("Q2_Flag");
        return checkflag2;
    }

    // 이전 score 판별
    public int CheckQuiz2_Score()
    {
        checkscore2 = PlayerPrefs.GetInt("Q2_Score");
        return checkscore2;
    }

}





