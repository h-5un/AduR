/* BalloonBehavior.cs
 * Date(Last Modified) : 2021/5/21
 * Programmer : 주수아
 * 
 * 풍선의 크기와 위치를 결정하는 기압, 온도를 저장하고 관리합니다.
 * 기압, 온도에 따라 변하는 풍선의 크기를 계산하고, 이에 따라 풍선의 크기와 위치를 적절히 변경합니다.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TaskManage : MonoBehaviour
{

    public GameObject VolumeText;
    private Text CurrentVolume;

    //현재 온도. 섭씨 0도로 시작. 공식은 절대온도에 기반하기 때문에 절대온도로 변환하여 저장. +-50도 범위로 움직일 예정.
    public float Temperature = 273f;
    public float MaxTemp = 323f;
    public float MinTemp = 223f;

    //현재 기압. 1 atm으로 시작. +-0.2atm 범위로 움직일 예정.
    public float AtPressure = 1.0f;
    public float MaxAt = 1.2f;
    public float MinAt = 0.8f;

    public float BalloonSize;
    public GameObject Balloon = null; 

    // Start is called before the first frame update
    void Start()
    {
        //풍선 크기 초기 설정. L 단위를 차용한다. 기체 분자의 개수는 0.5몰이라 가정한다.
        BalloonSize = 0.5f * 0.08f * Temperature / AtPressure;
        CurrentVolume = VolumeText.GetComponent<Text>();

        Screen.SetResolution(1920, 1080, true);
}

    // Update is called once per frame
    void Update()
    {

        //손잡이 4개의 작동유무에 따라 기압과 온도를 변경한다.
        if (GetComponent<StickManage>().TemperatureUp)
        {
            if (Temperature < MaxTemp)
            {
                Temperature += 30f * Time.deltaTime;
            }
            else
            {
                Temperature = MaxTemp;
            }
        }

        if (GetComponent<StickManage>().TemperatureDown)
        {
            if (Temperature > MinTemp)
            {
                Temperature -= 30f * Time.deltaTime;
            }
            else
            {
                Temperature = MinTemp;
            }
        }

        if (GetComponent<StickManage>().AtUp)
        {
            if (AtPressure < MaxAt)
            {
                AtPressure += 0.15f * Time.deltaTime;
            }
            else
            {
                AtPressure = MaxAt;
            }
        }

        if (GetComponent<StickManage>().AtDown)
        {
            if (AtPressure > MinAt)
            {
                AtPressure -= 0.15f * Time.deltaTime;
            }
            else
            {
                AtPressure = MinAt;
            }
        }

        //최종 풍선 크기. L 단위를 차용한다.
        BalloonSize = 0.5f * 0.08f * Temperature / AtPressure;

        //풍선 크기에 따른 풍선 사이즈 결정.
        Balloon.transform.localScale = new Vector3(4f, BalloonSize * (2f/10.92f), BalloonSize * (2f / 10.92f));
        Balloon.transform.position = new Vector3(0, 0.5f + (Balloon.transform.lossyScale.y / 2), 0);

        CurrentVolume.text = "현재 부피 : "+ string.Format("{0:0.#}", BalloonSize) +"L";

    }
}
