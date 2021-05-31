/* CameraImport.cs
 * Date(Last Modified) : 2021/5/9
 * Programmer : 주수아
 * 
 * 안드로이드 전면 카메라로부터 화면을 받아와 Screen에 덮어 씌웁니다.
 * 동작 감지 함수인 isMoved()에 필요한 최근 프레임 이미지 저장소 또한 이 곳에서 실시간으로 갱신합니다.
 */



using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Android;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;

public class CameraImport : MonoBehaviour
{
    //Renderer 컴포넌트를 포함하고 있는 게임 오브젝트 지정. 해당 오브젝트의 텍스쳐에 카메라 화면을 덮어씌운다.
    public GameObject objectTarget = null;

    //안드로이드 웹캠 디바이스 중 전면 카메라를 사용한다
    WebCamDevice frontCameraDevice;
    WebCamTexture frontCameraTexture;

    //프레임 이미지를 나눠 저장할 Color32 변수 3개. 시간에 따라 담겨있는 프레임 이미지가 달라진다.
    //숫자가 높을수록 최근 프레임. 이 프레임 3개를 통해 동작을 감지한다. 
    public Mat cameraTexture1;
    public Mat cameraTexture2;
    public Mat cameraTexture3;

    Color32[] colors;

    //그리고 세 프레임 중 어디까지가 채워져있는지를 표기하는 int 변수 추가
    public int TextureCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        //카메라 퍼미션이 허용되지않았을때

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            // 카메라 퍼미션 설정
            Permission.RequestUserPermission(Permission.Camera);
        }

        if (WebCamTexture.devices.Length == 0)

        {
            // 디바이스가 없는 경우에는 Return 해준다
            Debug.Log("No devices cameras found");
            return;

        }

        WebCamDevice[] devices = WebCamTexture.devices;

        // 전면 카메라 디바이스
        frontCameraDevice = devices.Last();

        // 전면 카메라 Texture
        frontCameraTexture = new WebCamTexture(frontCameraDevice.name, 1920, 1080);

        // 부드러워보이기 위해 필터 설정
        frontCameraTexture.filterMode = FilterMode.Trilinear;

        //objectTarget으로 카메라가 표시되도록 설정
        if(frontCameraTexture != null)
        {
            
            //objectTarget에 포함된 Renderer
            Renderer render = objectTarget.GetComponent<Renderer>();

            //해당 Renderer의 mainTextrue를 WebCamTexture로 설정
            render.material.mainTexture = frontCameraTexture;
            
            
        }

        frontCameraTexture.Play(); //카메라 재생

        colors = new Color32[frontCameraTexture.width * frontCameraTexture.height];

        //카메라가 인식되었다면 저장인수들의 초깃값 설정
        cameraTexture1 = new Mat(frontCameraTexture.height, frontCameraTexture.width, CvType.CV_8UC4, new Scalar(0, 0, 0, 255));
        cameraTexture2 = new Mat(frontCameraTexture.height, frontCameraTexture.width, CvType.CV_8UC4, new Scalar(0, 0, 0, 255));
        cameraTexture3 = new Mat(frontCameraTexture.height, frontCameraTexture.width, CvType.CV_8UC4, new Scalar(0, 0, 0, 255));
    }

    // Update is called once per frame
    void Update()
    {
        if (frontCameraTexture.isPlaying) //카메라 작동 여부부터 확인
        {
            //프레임이 꽉 차지 않은 경우, 오래된 순서대로 등록한다.
            if (TextureCount == 0)
            {
                Utils.webCamTextureToMat(frontCameraTexture, cameraTexture1, colors);
                TextureCount++;
            }
            else if (TextureCount == 1)
            {
                Utils.webCamTextureToMat(frontCameraTexture, cameraTexture2, colors);
                TextureCount++;
            }
            else if (TextureCount == 2)
            {
                Utils.webCamTextureToMat(frontCameraTexture, cameraTexture3, colors);
                TextureCount++;
            }
            
            //그렇지 않은 경우, 데이터를 한 칸씩 옮길 필요가 있다.
            else
            {
                cameraTexture1 = cameraTexture2;
                cameraTexture2 = cameraTexture3;
                Utils.webCamTextureToMat(frontCameraTexture, cameraTexture3, colors);
                //새로 받아온 프레임을 3번 저장소에 저장. 나머지는 한 칸씩 밀어낸다는 느낌으로 저장.
                //원래 1번 저장소에 있던 프레임은 폐기된다.
            }

        }
        
    }


    void OnDestroy()
    {

        if (frontCameraTexture != null)
        {
            frontCameraTexture.Stop();
            WebCamTexture.Destroy(frontCameraTexture);
            frontCameraTexture = null; //객체가 파괴되는 경우, 불러온 카메라 화면을 사라지게 한다
        }
        

    }
}

