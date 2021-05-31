/* BalloonBehavior.cs
 * Date(Last Modified) : 2021/5/21
 * Programmer : 주수아
 * 
 * 네 개의 스틱의 동작에 대한 함수와 변수를 관리합니다. OpenCV import도 이 곳에서 이루어집니다.
 * 스틱에 움직임이 감지되면, 온도와 기압을 올리거나 내립니다.
 * 이에 따라 스틱이 움직이는 애니메이션을 재생합니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Runtime;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;

public class StickManage : MonoBehaviour
{
    
    public GameObject Manager;

    //스틱 근처에서 움직임이 인식되었는지를 계산해낸다
    public bool isMovedLeftUp;
    bool LeftUpMovingUp = false;
    bool LeftUpMovingDown = false;
    public bool TemperatureUp = false;
    float LeftUpFalseCount = 0;


    public bool isMovedLeftDown;
    bool LeftDownMovingUp = false;
    bool LeftDownMovingDown = false;
    public bool TemperatureDown = false;
    float LeftDownFalseCount = 0;

    public bool isMovedRightUp;
    bool RightUpMovingUp = false;
    bool RightUpMovingDown = false;
    public bool AtUp = false;
    float RightUpFalseCount = 0;

    public bool isMovedRightDown;
    bool RightDownMovingUp = false;
    bool RightDownMovingDown = false;
    public bool AtDown = false;
    float RightDownFalseCount = 0;

    //레버 회전을 위한 오브젝트를 불러온다. 회전은 회전축에서 이루어짐.
    public GameObject LeftUp;
    public GameObject LeftDown;
    public GameObject RightUp;
    public GameObject RightDown;

    float LeftUpDegree = 0;
    float LeftDownDegree = 0;
    float RightUpDegree = 0;
    float RightDownDegree = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    
    void Update()
    {
        /*레버 동작에 관한 연산은 이 곳에서 한다
         * 4가지 bool 변수를 통해 레버 근처에서 움직임이 있었는지를 알아낸다.
         * 한 번 움직이기 시작한 레버는 30도 기울어질때까지 계속해서 올라가며, 0.3초간 움직임이 한 번도 감지되지 않으면 다시 내려간다.
         */


        //1 : 왼쪽 위 레버 동작 연산
        if ((isMovedLeftUp || LeftUpMovingUp) && !LeftUpMovingDown)
        {
            LeftUpFalseCount = 0;

            if (!LeftUpMovingUp)
            {
                LeftUpMovingUp = true;
            }

            TemperatureUp = true;

            if (LeftUpDegree < 30)
            {
                LeftUpDegree += 60f * Time.deltaTime;
                LeftUp.transform.Rotate(0, 0, 60f * Time.deltaTime);
            }
            else
            {
                LeftUpMovingUp = false;
            }

        }
        else if (!isMovedLeftUp || LeftUpMovingDown)
        {

            TemperatureUp = false;

            if (LeftUpFalseCount < 0.3f)
            {
                LeftUpFalseCount += Time.deltaTime;
            }
            else
            {

                if (!LeftUpMovingDown)
                {
                    LeftUpMovingDown = true;
                }

                if (LeftUpDegree > 0f)
                {
                    LeftUpDegree += -60f * Time.deltaTime;
                    LeftUp.transform.Rotate(0, 0, -60f * Time.deltaTime); //동작이 인식되지 않은 경우, 레버를 원래 위치로 돌린다.
                }
                else
                {
                    LeftUpMovingDown = false;
                    LeftUpFalseCount = 0;
                }
            }

        }
        //왼쪽 위 레버 연산 종료

        //2 : 왼쪽 아래 레버 동작 연산
        if ((isMovedLeftDown || LeftDownMovingDown) && !LeftDownMovingUp)
        {
            LeftDownFalseCount = 0;

            if (!LeftDownMovingDown)
            {
                LeftDownMovingDown = true;
            }

            TemperatureDown = true;

            if (LeftDownDegree > -30)
            {
                LeftDownDegree += -60f * Time.deltaTime;
                LeftDown.transform.Rotate(0, 0, -60f * Time.deltaTime);
            }
            else
            {
                LeftDownMovingDown = false;
            }

        }
        else if (!isMovedLeftDown || LeftDownMovingUp)
        {

            TemperatureDown = false;

            if (LeftDownFalseCount < 0.3f)
            {
                LeftDownFalseCount += Time.deltaTime;
            }
            else
            {

                if (!LeftDownMovingUp)
                {
                    LeftDownMovingUp = true;
                }

                if (LeftDownDegree < 0f)
                {
                    LeftDownDegree += 60f * Time.deltaTime;
                    LeftDown.transform.Rotate(0, 0, 60f * Time.deltaTime); //동작이 인식되지 않은 경우, 레버를 원래 위치로 돌린다.
                }
                else
                {
                    LeftDownMovingUp = false;
                    LeftDownFalseCount = 0;
                }
            }

        }
        //왼쪽 아래 레버 연산 종료

        //3 : 오른쪽 위 레버 동작 연산
        if ((isMovedRightUp || RightUpMovingDown) && !RightUpMovingUp)
        {
            RightUpFalseCount = 0;
            if (!RightUpMovingDown)
            {
                RightUpMovingDown = true;
            }

            AtUp = true;

            if (RightUpDegree > -30)
            {
                RightUpDegree += -60f * Time.deltaTime;
                RightUp.transform.Rotate(0, 0, -60f * Time.deltaTime);
            }
            else
            {
                RightUpMovingDown = false;
            }

        }
        else if (!isMovedRightUp || RightUpMovingUp)
        {

            AtUp = false;

            if (RightUpFalseCount < 0.3f)
            {
                RightUpFalseCount += Time.deltaTime;
            }
            else
            {

                if (!RightUpMovingUp)
                {
                    RightUpMovingUp = true;
                }

                if (RightUpDegree < 0f)
                {
                    RightUpDegree += 60f * Time.deltaTime;
                    RightUp.transform.Rotate(0, 0, 60f * Time.deltaTime); //동작이 인식되지 않은 경우, 레버를 원래 위치로 돌린다.
                }
                else
                {
                    RightUpMovingUp = false;
                    RightUpFalseCount = 0;
                }
            }

        }
        //오른쪽 위 레버 연산 종료

        //4 : 오른쪽 아래 레버 동작 연산
        if ((isMovedRightDown || RightDownMovingUp) && !RightDownMovingDown)
        {
            RightDownFalseCount = 0;
            if (!RightDownMovingUp)
            {
                RightDownMovingUp = true;
            }

            AtDown = true;

            if (RightDownDegree < 30)
            {
                RightDownDegree += 60f * Time.deltaTime;
                RightDown.transform.Rotate(0, 0, 60f * Time.deltaTime);
            }
            else
            {
                RightDownMovingUp = false;
            }

        }
        else if (!isMovedRightDown || RightDownMovingDown)
        {

            AtDown = false;

            if (RightDownFalseCount < 0.3f)
            {
                RightDownFalseCount += Time.deltaTime;
            }
            else
            {

                if (!RightDownMovingDown)
                {
                    RightDownMovingDown = true;
                }

                if (RightDownDegree > 0f)
                {
                    RightDownDegree += -60f * Time.deltaTime;
                    RightDown.transform.Rotate(0, 0, -60f * Time.deltaTime); //동작이 인식되지 않은 경우, 레버를 원래 위치로 돌린다.
                }
                else
                {
                    RightDownMovingDown = false;
                    RightDownFalseCount = 0;
                }
            }

        }
        //왼쪽 위 레버 연산 종료

    }
}
