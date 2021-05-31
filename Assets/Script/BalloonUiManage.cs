using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BalloonUiManage : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject TaskManager;
    public GameObject TemperatureBar;
    public GameObject AtPressureBar;

    float Temperature;
    float AtPressure;

    float MaxTemp;
    float MinTemp;
    float MaxAt;
    float MinAt;

    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {

        MaxTemp = TaskManager.GetComponent<TaskManage>().MaxTemp;
        MinTemp = TaskManager.GetComponent<TaskManage>().MinTemp;
        MaxAt = TaskManager.GetComponent<TaskManage>().MaxAt;
        MinAt = TaskManager.GetComponent<TaskManage>().MinAt;

        PausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TemperatureBar.GetComponent<Image>().fillAmount = (TaskManager.GetComponent<TaskManage>().Temperature - MinTemp) / (MaxTemp - MinTemp);
        AtPressureBar.GetComponent<Image>().fillAmount = (TaskManager.GetComponent<TaskManage>().AtPressure - MinAt) / (MaxAt - MinAt);

        if (isPaused)
        {
            //일시정지할 경우 카메라는 계속해서 작동하나, 불투명도 100%의 패널로 화면 전체를 덮을 예정.
            PausePanel.SetActive(true);
            Time.timeScale = 0;
        }


    }

    public void Restart()
    {
        isPaused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
