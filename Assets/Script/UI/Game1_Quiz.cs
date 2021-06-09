using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game1_Quiz : MonoBehaviour
{
    // 데이터 저장 변수
    int data_Quiz1_Flag = 0;   // Quiz1을 플레이 한 여부가 저장되는 flag(flag = 1일 시 report가 등록됨)
    int data_Quiz1_Score = -1;   // Quiz1의 점수
    int before_score;           // 이전에 사용자가 풀었던 퀴즈의 점수

    // 사용자가 선택한 각 문항의 정답이 저장되는 변수
    public int Q1ans1, Q1ans2, Q1ans3, Q1ans4, Q1ans5 = 0;
    public int score = 0;                               // 채점 결과
    int[] Q1solution = new int[5] { 3, 1, 1, 3, 2 };    // 정답이 저장되는 배열
    int[] Q1answer = new int[5] { 0, 0, 0, 0, 0 };    // 사용자가 고른 답이 저장되는 배열

    int ans; // 선택된 버튼을 확인하기 위한 변수 ans
    int page = 0; // 현재 사용자가 풀고 있는 문항(0부터 start, 0~4)

    // 토글을 제어하기 위한 배열
    public Toggle[] Q1toggles1, Q1toggles2, Q1toggles3, Q1toggles4, Q1toggles5;

    // in Unity
    public GameObject Q1layout, Q2layout, Q3layout, Q4layout, Q5layout;
    public GameObject EndLayout, EndUI;
    public Text Score_Text;

    // 3개의 보기 중, 사용자가 선택한 보기를 탐색하는 ansSearch 함수
    int ansSearch(Toggle[] arr)
    {
        ans = 0; //초기화

        for (int i = 0; i < 3; i++)
        {
            if (arr[i].isOn == true)    // 선택된 보기가 탐지되었을 경우
                ans = i + 1;              // 해당 보기의 index+1을 ans에 저장한다.
        }
        return ans;                    // 저장된 ans를 return한다
        // (만약 사용자가 보기를 선택하지 않았을 경우, 0 리턴)
    }


    /*
    * 다음 문항으로 넘어가는 것을 제어하는 moveNext 함수
    * - 사용자가 아무 문항도 선택하지 않고 다음으로 넘어가려 시도할 경우, 넘어가지 않도록 한다.
    * - 사용자가 문항을 선택하고 다음으로 넘어가면 선택한 문항을 Q1answer 배열에 저장한다
    * 만약 마지막 문항이었다면 점수를 저장하고 보고서를 생성하도록 한다.
    */
    public void moveNext()
    {
        switch (page)
        {
            case 0:
                Q1answer[0] = ansSearch(Q1toggles1);
                if (Q1answer[0] == 0) return;
                Q1layout.SetActive(false);
                Q2layout.SetActive(true);
                page++;
                break;
            case 1:
                Q1answer[1] = ansSearch(Q1toggles2);
                if (Q1answer[1] == 0) return;
                Q2layout.SetActive(false);
                Q3layout.SetActive(true);
                page++;
                break;
            case 2:
                Q1answer[2] = ansSearch(Q1toggles3);
                if (Q1answer[2] == 0) return;
                Q3layout.SetActive(false);
                Q4layout.SetActive(true);
                page++;
                break;
            case 3:
                Q1answer[3] = ansSearch(Q1toggles4);
                if (Q1answer[3] == 0) return;
                Q4layout.SetActive(false);
                Q5layout.SetActive(true);
                page++;
                break;
            case 4:
                Q1answer[4] = ansSearch(Q1toggles5);
                if (Q1answer[4] == 0) return;
                // 퀴즈 완료! 팝업창 출력
                Time.timeScale = 0;
                score = CalScore();                         // 퀴즈 점수 계산
                Debug.Log(score);
                EndLayout.SetActive(true);
                EndUI.SetActive(true);
                Score_Text.text = score + " / 5";           // 점수 출력
                data_Quiz1_Flag = 1;                       // flag를 1로 전환
                DataManager.instance.Quiz1_FlagSet(data_Quiz1_Flag); // 플래그 저장
                data_Quiz1_Score = score;                        // 점수 데이터       
                DataManager.instance.Quiz1_Save(data_Quiz1_Score); // 데이터 저장
                break;
            default:
                Debug.Log("Page Error");
                break;
        }
    }


    /*
     * 사용자의 점수를 계산한다
     */
    public int CalScore()
    {
        int calscore = 0;
        for (int i = 0; i < 5; i++)
        {
            if (Q1answer[i] == Q1solution[i]) // 사용자가 입력한 답과 정답이 같을 경우
                calscore++;                      // 정답으로 채점
        }
        return calscore;
    }

    // '닫기' 버튼 클릭 시 각 팝업을 종료하는 함수
    public void QuitPopUp()
    {
        //Q4layout.SetActive(false);
    }

    public void QuizEnd()
    {
        SceneManager.LoadScene("Main");
    }


    // (배경음악) SoundManager의 instance 호출 - Slider에 적용
    /*public void BGsoundSlider(float value)
    {
        DataManager.instance.BGSoundVolume(value);
        PlayerPrefs.SetFloat("bgVol", value);
    }*/

    // Start is called before the first frame update
    void Start()
    {
        // 초기화
        ans = 0;
        page = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 유저가 조정한 Slider의 값을 저장, 적용
        // bgSlideVal = PlayerPrefs.GetFloat("bgVol", bgSlideVal);     // 배경음악 Slider의 값을 PlayerPrefs에 저장
        // bgSlider.value = bgSlideVal;                                // PlayerPrefs에 저장된 값을 Slider에 적용
    }
}
