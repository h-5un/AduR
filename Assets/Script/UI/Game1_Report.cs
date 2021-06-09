using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game1_Report : MonoBehaviour
{

    public GameObject S1Blackbox, S1sol1, S1sol2, S1sol3, S1sol4, S1sol5;
    public GameObject Lbutton, Rbutton, Xbutton, ScoreText;
    public GameObject AllSolobj, Buttons;
    public Text Score_Text;

    int score;

    // 답지 페이지 변수 page
    private int page = 1;

    void Awake()
    {
        // 퀴즈 플레이 이력 불러오기
        score = DataManager.instance.CheckQuiz1_Score();
        Debug.Log(score);

        Score_Text.text = score + " / 5";
    }


    /*
     * [정답 확인하기] 버튼 입력 시
     * Solution 페이지 출력
     */
    public void ShowSol()
    {
        AllSolobj.SetActive(true);
        S1Blackbox.SetActive(true);
        S1sol1.SetActive(true);
        Rbutton.SetActive(true);
        Xbutton.SetActive(true);
        ScoreText.SetActive(false);
        Buttons.SetActive(false);
        page = 1;
    }

    /*
     * X버튼을 눌러 해답페이지 종료
     */
    public void ExitSol()
    {
        AllSolobj.SetActive(false);
        ScoreText.SetActive(true);
        Xbutton.SetActive(false);
        Buttons.SetActive(true);
        page = 1;
    }

    /*
     * Right 버튼을 입력하면 다음 페이지로 넘어가도록 함.
     */
    public void MoveNextSol()
    {
        switch (page)
        {
            case 1:
                S1sol1.SetActive(false);
                S1sol2.SetActive(true);
                Lbutton.SetActive(true);
                Rbutton.SetActive(true);
                page++;
                break;
            case 2:
                S1sol2.SetActive(false);
                S1sol3.SetActive(true);
                page++;
                break;
            case 3:
                S1sol3.SetActive(false);
                S1sol4.SetActive(true);
                page++;
                break;
            case 4:
                S1sol4.SetActive(false);
                S1sol5.SetActive(true);
                Rbutton.SetActive(false);
                page++;
                break;
            default:
                Debug.Log("Page Error");
                break;
        }
    }

    public void MoveBackSol()
    {
        switch (page)
        {
            case 2:
                S1sol2.SetActive(false);
                S1sol1.SetActive(true);
                Lbutton.SetActive(false);
                page--;
                break;
            case 3:
                S1sol3.SetActive(false);
                S1sol2.SetActive(true);
                page--;
                break;
            case 4:
                S1sol4.SetActive(false);
                S1sol3.SetActive(true);
                page--;
                break;
            case 5:
                S1sol5.SetActive(false);
                S1sol4.SetActive(true);
                Rbutton.SetActive(true);
                page--;
                break;
            default:
                Debug.Log("Page Error");
                break;
        }
    }

    /*
     * Left 버튼을 입력하면 다음 페이지로 넘어가도록 함.
     */
    public void MovePrevSol()
    {
        // 여기 스위치문 적절히 변형해서 넣으씨오
    }

    // Start is called before the first frame update
    void Start()
    {
        // 초기화
        page = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
