using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;       //DateTime 를 사용하기 위한


public class SceneManager : MonoBehaviour {

    private static SceneManager sInstance;
    public static SceneManager Instance
    {
        get
        {
            if (sInstance == null)
            {
                GameObject newGameObject = new GameObject("_SceneManager");
                sInstance = newGameObject.AddComponent<SceneManager>();
            }

            return sInstance;
        }
    }

    internal static AsyncOperation LoadSceneAsync(int sceneIndex)
    {
        throw new NotImplementedException();
    }

    private float factor;        //연출 진행도

    private Image fadeImage;            //Fade 에 사용될 이미지
    private int nextSceneIdx;       //체인지 될 씬 인덱스
    private AsyncOperation oper;
    private float fadeTime;      //Fade 의 연출의 총시간
    private float fadeDeltaTime; //Fade 의 연출시 지난 시간
    public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);      //1 초의 간격을 가지는 선형 Curve 그래프

    private delegate void ChageProcess();       //변경씬 연출 델리게이트
    private ChageProcess changeProcess;
    private RawImage screenRawImage;
    private Texture2D screenImage;


    private string screenAnimName = "";
    private Animator screenAnimator;

    private bool isNowFading;      //지금 씬 변화(연출)중이니...


    void Awake()
    {
        if (sInstance == null)
            sInstance = this;

        //비활성화...
        this.fadeImage = this.transform.Find("FadeImage").GetComponent<Image>();
        this.fadeImage.enabled = false;

        this.screenRawImage = this.transform.Find("RawImage").GetComponent<RawImage>();
        this.screenRawImage.enabled = false;

        this.screenAnimator = this.screenRawImage.gameObject.GetComponent<Animator>();
        this.screenAnimator.enabled = false;




        //스크린의 내용을 읽을 Texture 준비
        this.screenImage = new Texture2D(
            Screen.width,
            Screen.height, TextureFormat.RGB24, false)
        {
            filterMode = FilterMode.Point
        };


        //씬이 변경되어도 파괴되지 않는 오브젝트로 등록
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        //chageProcess 에 함수가 물려있다면... 실행
        if (changeProcess != null)
            changeProcess();
    }




    public void SceneChange(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);

        isNowFading = false;
    }

    //string 을 가변인자로....
    public void SceneChangeAdd(params string[] sceneNames)
    {
        //첫번째 이름의 씬은 그냥 로딩
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNames[0]);

        //씬추가
        for (int i = 1; i < sceneNames.Length; i++)
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNames[i]);
    }


    public void SceneChangeColorFade(int sceneIndex, float fadeTime, Color fadeColor )
    {
        //다음씬 기억
        this.nextSceneIdx = sceneIndex;
        this.oper = null;

        //총연출시간 기억
        this.fadeTime = fadeTime;
        this.fadeDeltaTime = 0.0f;

        //컬러셋팅
        this.fadeImage.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0.0f);
        this.fadeImage.enabled = true;

        //프로세싱 함수에 Fade 연출 물린다.
        this.changeProcess = new ChageProcess(UpdateColorFadeImageUpdate);
        this.changeProcess += new ChageProcess(UpdateColorFadeIn);

        isNowFading = true;
    }

    public void SceneChangeColorFade( AsyncOperation oper, float fadeTime, Color fadeColor)
    {
        //다음씬 기억
        this.nextSceneIdx = -1;
        this.oper = oper;


        //총연출시간 기억
        this.fadeTime = fadeTime;
        this.fadeDeltaTime = 0.0f;

        //컬러셋팅
        this.fadeImage.color =
            new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0.0f);
        this.fadeImage.enabled = true;

        //프로세싱 함수에 Fade 연출 물린다.
        this.changeProcess = new ChageProcess(this.UpdateColorFadeImageUpdate);
        this.changeProcess += new ChageProcess(UpdateColorFadeIn);

        isNowFading = true;
    }



    public void SceneChangeCrossFade(int sceneIndex, float fadeTime, string AnimName = "" )
    {
        //다음씬 기억
        this.nextSceneIdx = sceneIndex;
        this.oper = null;

        //총연출시간 기억
        this.fadeTime = fadeTime;
        this.fadeDeltaTime = 0.0f;

        //Anmation 이름 대입
        this.screenAnimName = AnimName;

        isNowFading = true;

        //현제 씬의 이미지를 준비한다.
        StartCoroutine(ReadyNowSceneImage());
    }




    public void SceneChangeCrossFade(AsyncOperation oper, float fadeTime, string AnimName = "")
    {
        //다음씬 기억
        this.nextSceneIdx = -1;
        this.oper = oper;

        //총연출시간 기억
        this.fadeTime = fadeTime;
        this.fadeDeltaTime = 0.0f;


        //Anmation 이름 대입
        this.screenAnimName = AnimName;

        isNowFading = true;

        //현제 씬의 이미지를 준비한다.
        StartCoroutine(ReadyNowSceneImage());
    }


    IEnumerator ReadyNowSceneImage()
    {
        yield return new WaitForEndOfFrame();

        //화면 크기에 맞춰.
        this.screenImage.Reinitialize(Screen.width, Screen.height);
        this.screenImage.Apply();

        //화면내용읽는다.
        this.screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        this.screenImage.Apply();


        //Raw Image 에 물린다.
        this.screenRawImage.texture = this.screenImage;
        this.screenRawImage.enabled = true;

        //한 Pixel 의 UV 량
        float onePixelU = 1.0f / Screen.width;
        float onePixelV = 1.0f / Screen.height;

        //Rect uvRect = this.screenRawImage.uvRect;
        //uvRect.x = -onePixelU * 0.5f;
        //uvRect.y = -onePixelV * 0.5f;
        //this.screenRawImage.uvRect = uvRect;
        //this.screenRawImage.transform.localPosition =
        //    new Vector3(onePixelU * 0.5f, onePixelV * 0.5f, 0.0f);


        this.changeProcess = new ChageProcess(this.UpdateFactorDown);
        this.changeProcess += new ChageProcess(this.UpdateImageUpdate);


        this.screenAnimator.enabled = true;
        this.screenAnimator.speed = 0.0f;



        //씬바로 돌린다.
        if (this.oper != null)
        {
            this.oper.allowSceneActivation = true;
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(this.nextSceneIdx);
        }
    }





    ///////////////////////////////////////////////////////////////////////////////////
    void UpdateColorFadeIn()
    {
        this.fadeDeltaTime += Time.deltaTime;
        float t = this.fadeDeltaTime / (this.fadeTime * 0.5f);
        this.factor = this.fadeCurve.Evaluate( t );

        if (this.factor >= 1.0f)
        {
            //씬바로 돌린다.
            if (this.oper != null)
                this.oper.allowSceneActivation = true;
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene(this.nextSceneIdx);


            //ChageProcess 갱신
            this.changeProcess -= UpdateColorFadeIn;
            this.changeProcess += new ChageProcess(UpdateColorFadeOut);
        }

    }

    void UpdateColorFadeOut()
    {
        this.fadeDeltaTime -= Time.deltaTime;
        float t = this.fadeDeltaTime / (this.fadeTime * 0.5f);
        this.factor = this.fadeCurve.Evaluate(t);

        if (this.factor <= 0.0f)
        {
            //연출 끝
            this.fadeImage.enabled = false;
            this.changeProcess = null;

            isNowFading = false;
            this.oper = null;
        }

    }

    void UpdateColorFadeImageUpdate()
    {
        Color color = this.fadeImage.color;
        color.a = factor;
        this.fadeImage.color = color;
    }

    //////////////////////////////////////////////////////////////////////////

    void UpdateFactorDown()
    {
        this.fadeDeltaTime += Time.deltaTime;
        float t = this.fadeDeltaTime / (this.fadeTime);
        this.factor = t;

        if (this.factor >= 1.0f)
        {
            this.factor = 0.0f;

            //연출 끝
            if (this.screenAnimName.Length > 0)
            {
                this.screenAnimator.speed = 0.0f;
                this.screenAnimator.Play(this.screenAnimName, 0, 0);
            }


            this.screenAnimator.enabled = false;
            this.screenRawImage.enabled = false;
            this.screenRawImage.texture = null;
            this.changeProcess = null;
            this.oper = null;

            isNowFading = false;
        }

    }

    void UpdateImageUpdate()
    {
        if (this.screenAnimName.Length == 0)
        {
            Color color = this.screenRawImage.color;
            color.a = 1.0f - factor;
            this.screenRawImage.color = color;
        }
        else
        {

            this.screenAnimator.Play(this.screenAnimName, 0, this.factor);
        }

    }









    //////////////////////////////////////////////////////////////////////////

    public void SaveSceneShot()
    {
        StartCoroutine("SaveScreenShotCo");
    }

    IEnumerator SaveScreenShotCo()
    {
        //프레임의 마지막 까지 기다린다.
        yield return new WaitForEndOfFrame();

        //여기까지오면 랜더링이 끝나고 한프레임의 이미지가 완성되어있다.

        //화면을 저장할 Texture 가 필요
        Texture2D screenTex = new Texture2D(
            Screen.width,
            Screen.height,
            TextureFormat.RGB24,
            false);

        //Texture 가 화면 퍼퍼의 내용을 읽는다.
        screenTex.ReadPixels(
            new Rect(0, 0, Screen.width, Screen.height),        //화면읽을 영역
            0, 0);                                              //Texture 어디에 찍을꺼니...

        //읽근거 적용
        screenTex.Apply();

        //PNG 포맷의 바이트로 바꿔재낀다.
        //byte[] imageBytes = screenTex.EncodeToPNG();
        byte[] imageBytes = screenTex.EncodeToJPG(100);       //0 ~ 100 사이의 퀄리티.

        //Destroy(screenTex);

        //너는 필요가 없으니 즉결처형
        DestroyImmediate(screenTex);

        //screenTex 의 내용을 파일에 저장
        //DateTime.Now 현제 실제 시간을 얻어온다...
        string fileName = DateTime.Now.ToString("yyyy-mm-dd-hh-MM-ss") + ".jpg";

        //저장 경로...
        string savePath = Application.persistentDataPath + "/" + fileName;

        //파일로 저정
        using (FileStream fs = new FileStream(savePath, FileMode.Create))
        {

            //바이트 배열 파일로 쓴다.
            fs.Write(imageBytes,
                0,
                imageBytes.Length);

            fs.Close();
        }

    }



    public bool IsFading
    {
        get{

            return this.isNowFading;

        }
    }


}
