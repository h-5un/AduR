

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;


namespace OpenCVForUnity
{
    [RequireComponent(typeof(WebCamTextureToMatHelper))]
    public class CameraMatManager : MonoBehaviour
    {

        public GameObject Manager;
        public GameObject UIManager;

        /// <summary>
        /// The texture.
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The webcam texture to mat helper.
        /// </summary>
        WebCamTextureToMatHelper webCamTextureToMatHelper;

        public Mat cameraTexture1;
        public Mat cameraTexture2;
        public Mat cameraTexture3;

        public Mat test1;
        public Mat test2;

        public Mat final;

        int TextureCount = 0;

        int LeftUpX;
        int LeftUpY;

        int LeftDownX;
        int LeftDownY;

        int RightUpX;
        int RightUpY;

        int RightDownX;
        int RightDownY;

        CoreModule.Rect LeftUpRect;
        CoreModule.Rect LeftDownRect;
        CoreModule.Rect RightUpRect;
        CoreModule.Rect RightDownRect;

        CoreModule.Rect PauseRect;

        // Start is called before the first frame update
        void Start()
        {
            webCamTextureToMatHelper = gameObject.GetComponent<WebCamTextureToMatHelper>();
            webCamTextureToMatHelper.requestedWidth = 1920;
            webCamTextureToMatHelper.requestedHeight = 1080;

            webCamTextureToMatHelper.Initialize(true);
        }

        public void OnWebCamTextureToMatHelperInitialized()
        {
            Debug.Log("OnWebCamTextureToMatHelperInitialized");

            Mat webCamTextureMat = webCamTextureToMatHelper.GetMat();

            texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);
            Utils.fastMatToTexture2D(webCamTextureMat, texture);

            gameObject.GetComponent<Renderer>().material.mainTexture = texture;

            cameraTexture1 = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC4);
            cameraTexture2 = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC4);
            cameraTexture3 = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC4);

            test1 = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC1);
            test2 = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC1);
            final = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC1);

            /*이후 레버 조작 인식에 쓰일 좌표
             * *카메라(1920x1080)상의 좌표 : ((x+11.52)*250/3, (y-12.465)*250/3)
             * *
             * *레버 좌표
             * *공통 사이즈 : 1.971496x0.45. 대략 2x0.45
             * *1.왼쪽 위 : (-9.2, 6.85) + (-1, 0.225) = (-10.2, 7.075)
             * *2.왼쪽 아래 : (-9.2, 1.9) + (-1, 0.225) = (-10.2, 2.125)
             * *3.오른쪽 위 : (9.2, 6.85) + (-1, 0.225) = (8.2, 7.075)
             * *4.오른쪽 아래 : (9.2, 1.9) + (-1, 0.225) = (8.2, 2.125)
             */

            LeftUpX = calStickX(-10.2f);
            LeftUpY = calStickY(7.075f);

            LeftDownX = calStickX(-10.2f);
            LeftDownY = calStickY(2.125f);

            RightUpX = calStickX(8.2f);
            RightUpY = calStickY(7.075f);

            RightDownX = calStickX(8.2f);
            RightDownY = calStickY(2.125f);

            LeftUpRect = new CoreModule.Rect(LeftUpX, LeftUpY, 167, 38);
            LeftDownRect = new CoreModule.Rect(LeftDownX, LeftDownY, 167, 38);
            RightUpRect = new CoreModule.Rect(RightUpX, RightUpY, 167, 38);
            RightDownRect = new CoreModule.Rect(RightDownX, RightDownY, 167, 38);

            PauseRect = new CoreModule.Rect(710, 75, 300, 120);

        }

        public void OnWebCamTextureToMatHelperDisposed()
        {
            Debug.Log("OnWebCamTextureToMatHelperDisposed");

            if (texture != null)
            {
                Texture2D.Destroy(texture);
                texture = null;
            }
        }

        /// <summary>
        /// Raises the webcam texture to mat helper error occurred event.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        public void OnWebCamTextureToMatHelperErrorOccurred(WebCamTextureToMatHelper.ErrorCode errorCode)
        {
            Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
        }

        //int calStickX/calStickY
        //유니티 상의 x, y좌표를 1920*1080 크기의 화상 내 좌표로 바꿔준다. 소숫점은 버림한다.(1픽셀 차이는 크지 않을 것으로 예상했음)
        int calStickX(float x)
        {

            return (int)((x + 11.52f) * 250f / 3f);

        }

        int calStickY(float y)
        {
            return (int)((12.96 - (y + 0.5)) * 250f / 3f);

        }


        // Update is called once per frame
        void Update()
        {
            if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
            {
                Mat rgbaMat = webCamTextureToMatHelper.GetMat();

                if (TextureCount < 3)
                {
                    TextureCount++;
                }

                //새로 받아온 프레임을 3번 저장소에 저장. 나머지는 한 칸씩 밀어낸다는 느낌으로 저장.
                //원래 1번 저장소에 있던 프레임은 폐기된다.
                cameraTexture2.copyTo(cameraTexture1);
                cameraTexture3.copyTo(cameraTexture2);
                rgbaMat.copyTo(cameraTexture3);
                
                //Screen에 화면 출력
                Utils.fastMatToTexture2D(rgbaMat, texture);

                if(TextureCount >= 3)
                {
                    Process1();
                    Process2();

                    Imgproc.threshold(test1, test1, 25, 255, Imgproc.THRESH_BINARY);
                    Imgproc.threshold(test2, test2, 25, 255, Imgproc.THRESH_BINARY);

                    Core.bitwise_and(test1, test2, final);
                    //움직임 감지 인식을 위한 이미지 가공 완료

                    Mat LeftUpcutFinal = new Mat(final, LeftUpRect);
                    Mat LeftDowncutFinal = new Mat(final, LeftDownRect);
                    Mat RightUpcutFinal = new Mat(final, RightUpRect);
                    Mat RightDowncutFinal = new Mat(final, RightDownRect);

                    Mat PauseCut = new Mat(final, PauseRect); //일시정지 인식을 위한 부분 Mat

                    //Mat의 일부에 얼마만큼의 강도의 움직임이 감지되었는지를 검사
                    if (Core.countNonZero(LeftUpcutFinal) > 50)
                    {
                        Manager.GetComponent<StickManage>().isMovedLeftUp = true;
                    }
                    else
                    {
                        Manager.GetComponent<StickManage>().isMovedLeftUp = false;
                    }

                    if (Core.countNonZero(LeftDowncutFinal) > 50)
                    {
                        Manager.GetComponent<StickManage>().isMovedLeftDown = true;
                    }
                    else
                    {
                        Manager.GetComponent<StickManage>().isMovedLeftDown = false;
                    }

                    if (Core.countNonZero(RightUpcutFinal) > 50)
                    {
                        Manager.GetComponent<StickManage>().isMovedRightUp = true;
                    }
                    else
                    {
                        Manager.GetComponent<StickManage>().isMovedRightUp = false;
                    }

                    if (Core.countNonZero(RightDowncutFinal) > 50)
                    {
                        Manager.GetComponent<StickManage>().isMovedRightDown = true;
                    }
                    else
                    {
                        Manager.GetComponent<StickManage>().isMovedRightDown = false;
                    }

                    if(Core.countNonZero(PauseCut) > 450)
                    {
                        UIManager.GetComponent<BalloonUiManage>().isPaused = true;
                    }

                }

            }
        }
        //Process1(), Process2()
        //이진화에 사용할 프레임 이미지 2개를 흑백화, 차연산한다.
        //차연산을 통해, 프레임 이미지 2개가 얼마나 차이나느냐, 즉 화면 내의 것이 얼마나 많이 움직였는가를 알 수 있다.
        private void Process1()
        {

            //흑백화된 이미지를 담을 변수 생성
            Mat image1 = new Mat();
            Mat image2 = new Mat();
            //세 프레임을 각각 흑백화하여 image1, 2, 3에 담는다
            Imgproc.cvtColor(cameraTexture1, image1, Imgproc.COLOR_BGR2GRAY);
            Imgproc.cvtColor(cameraTexture2, image2, Imgproc.COLOR_BGR2GRAY);

            Core.absdiff(image1, image2, test1);

            image1.Dispose();
            image2.Dispose();
        }

        private void Process2()
        {
            //흑백화된 이미지를 담을 변수 생성
            Mat image1 = new Mat();
            Mat image2 = new Mat();
            //세 프레임을 각각 흑백화하여 image1, 2, 3에 담는다
            Imgproc.cvtColor(cameraTexture2, image1, Imgproc.COLOR_BGR2GRAY);
            Imgproc.cvtColor(cameraTexture3, image2, Imgproc.COLOR_BGR2GRAY);

            Core.absdiff(image1, image2, test2);

            image1.Dispose();
            image2.Dispose();

        }

        void OnDestroy()
        {
            webCamTextureToMatHelper.Dispose();
        }
    }
}





