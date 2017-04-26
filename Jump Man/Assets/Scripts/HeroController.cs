using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour {

    public bool isJumping = false;
    private Rigidbody rigidbody;

    private Vector2 touchMove = new Vector2();
    private float scaleVelX = 10;
    private float scaleVelY = 10;

    private float maxVelX = 6f;
    private float maxVelY = 6f;

    private bool mouseDown = false;
    private bool touchDown = false;


    public GameObject objs;
    public GameObject camera;

    private bool isPausing = false;
    private bool isAlive = false;

    private float velY = 5f;

    private float cosTheat = 0;
    private float sinTheat = 0;

    public int maxIdx = -1;

    public Vector3 pos;
    public Vector3 dir;
    public float vh;
    public float vv;
    public bool showGuide = false;

    public Vector3 deltaPos = new Vector3();

    public bool isDesktop = true;

    private GameObject lastHit = null;

    public GameObject scorePanel;

    private Text scoreText;
    private int score;

    private void Awake()
    {
        cosTheat = Mathf.Cos(-Mathf.PI / 4);
        sinTheat = Mathf.Sin(-Mathf.PI / 4);
        deltaPos = camera.transform.position;
        scoreText = scorePanel.GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        Input.multiTouchEnabled = true;
        Debug.Log("Start");
        transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
    }
	
	// Update is called once per frame
	void Update () {
        if (isPausing||isAlive == false) {
            mouseDown = false;
            touchDown = false;
            return;
        }

        if (isDesktop)
        {
            pos = transform.position;
            Vector2 v = new Vector2();
            v.x = Input.mousePosition.x - touchMove.x;
            v.y = touchMove.y - Input.mousePosition.y;

            if (v.x >= 0)
            {
                v.x = Mathf.Min(v.x / (float)Screen.width * this.scaleVelX, this.maxVelX);
            }
            else
            {
                v.x = Mathf.Max(v.x / (float)Screen.width * this.scaleVelX, -this.maxVelX);
            }

            if (v.y >= 0)
            {
                v.y = Mathf.Min(v.y / (float)Screen.width * this.scaleVelY, this.maxVelY);
            }
            else
            {
                v.y = Mathf.Max(v.y / (float)Screen.width * this.scaleVelY, -this.maxVelY);
            }

            Vector2 newVel = new Vector2();
            newVel.y = v.x * cosTheat - v.y * sinTheat;
            newVel.x = v.y * cosTheat + v.x * sinTheat;
            dir = new Vector3(newVel.x, 0, newVel.y);
            vh = Mathf.Sqrt(newVel.x * newVel.x + newVel.y * newVel.y);
            vv = velY;



            if (Input.GetMouseButtonDown(0) && mouseDown == false)
            {
                OnMouseDown();
            }
            else if (Input.GetMouseButtonUp(0) && mouseDown == true)
            {
                OnMouseUp();
            }

        }
        else {
            if (Input.touchCount == 1)
            {

                if (Input.touches[0].phase == TouchPhase.Began && touchDown == false)
                { // 开始触屏
                    Debug.Log("Began");
                    touchMove.x = Input.touches[0].position.x;
                    touchMove.y = Input.touches[0].position.y;
                    touchDown = true;
                    showGuide = true;
                }
                else if (Input.touches[0].phase == TouchPhase.Moved && touchDown == true)
                {
                    Debug.Log("Moved");

                    pos = transform.position;
                    Vector2 v = new Vector2();
                    v.x = Input.touches[0].position.x - touchMove.x;
                    v.y = touchMove.y - Input.touches[0].position.y;

                    if (v.x >= 0)
                    {
                        v.x = Mathf.Min(v.x / (float)Screen.width * this.scaleVelX, this.maxVelX);
                    }
                    else
                    {
                        v.x = Mathf.Max(v.x / (float)Screen.width * this.scaleVelX, -this.maxVelX);
                    }

                    if (v.y >= 0)
                    {
                        v.y = Mathf.Min(v.y / (float)Screen.width * this.scaleVelY, this.maxVelY);
                    }
                    else
                    {
                        v.y = Mathf.Max(v.y / (float)Screen.width * this.scaleVelY, -this.maxVelY);
                    }

                    Vector2 newVel = new Vector2();
                    newVel.y = v.x * cosTheat - v.y * sinTheat;
                    newVel.x = v.y * cosTheat + v.x * sinTheat;
                    dir = new Vector3(newVel.x, 0, newVel.y);
                    vh = Mathf.Sqrt(newVel.x * newVel.x + newVel.y * newVel.y);
                    vv = velY;
                }
                else if (Input.touches[0].phase == TouchPhase.Ended && touchDown == true)
                {
                    touchDown = false;
                    if (isAlive)
                    {
                        Jump();
                    }
                    showGuide = false;
                    touchDown = false;
                }

               
            }
            
        }




        if (transform.position.y < -10)
        {
            Lose();
        }
        //else {
        //    Vector3 dir = gameObject.transform.position - camera.transform.position;
        //    float dis = Vector3.Distance(gameObject.transform.position, camera.transform.position);
        //    RaycastHit hit = new RaycastHit();
        //    if (Physics.Raycast(camera.transform.position, dir, out hit, dis))
        //    {
        //        if (hit.transform.gameObject.tag == "cylinder")
        //        {
        //            lastHit = hit.transform.gameObject;
        //            SetMaterialRenderingMode(lastHit.GetComponent<MeshRenderer>().material, RenderingMode.Transparent);
        //            lastHit.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0.5f);

        //        }
        //        else if (hit.transform.gameObject.tag != "cylinder" && lastHit != null)
        //        {
        //            SetMaterialRenderingMode(lastHit.GetComponent<MeshRenderer>().material, RenderingMode.Opaque);
        //            lastHit.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1f);
        //            lastHit = null;
        //        }
        //    }

        //}

        this.camera.transform.position = this.transform.position + new Vector3(deltaPos.x, deltaPos.y, deltaPos.z);
        //    args.Add("x", transform.position.x  + deltaPos.x);
        //    args.Add("y", transform.position.y + deltaPos.y);
        //    args.Add("z", transform.position.z + deltaPos.z);
    }


    void OnMouseDown()
    {
        if (isAlive == true) {
            touchMove.x = Input.mousePosition.x;
            touchMove.y = Input.mousePosition.y;
            mouseDown = true;
            showGuide = true;
        }
      
    }


    void OnMouseUp()
    {
        if (isAlive) { 
            Jump();
        }
        showGuide = false;
        mouseDown = false;

    }


    public void OnCollisionEnter(Collision collision) {
        isJumping = false;

        Platform platfrom = collision.gameObject.GetComponent<Platform>();

        if (platfrom != null) {
            if (platfrom.type == PlatformType.NormalPlatform)
            {
                if (platfrom.idx == -1) {
                    this.scorePanel.SetActive(true);
                }
                if (platfrom.idx > maxIdx)
                {
                    this.score += platfrom.idx - maxIdx;
                    maxIdx = platfrom.idx;
                   
                    this.scoreText.text = "" + this.score;
                }
                objs.gameObject.GetComponent<PlatformManager>().genNormalPlatform(platfrom.idx + 1);
                objs.gameObject.GetComponent<PlatformManager>().genNormalPlatform(platfrom.idx + 2);
                objs.gameObject.GetComponent<PlatformManager>().genNormalPlatform(platfrom.idx + 3);
                this.moveCamera();
            }
            else if (platfrom.type == PlatformType.CylinderPlatform) {
                if (platfrom.idx> maxIdx)
                {
                    this.score += platfrom.idx - maxIdx + 2;
                    maxIdx = platfrom.idx;
                    this.scoreText.text = "" + this.score;
                }
                objs.gameObject.GetComponent<PlatformManager>().genNormalPlatform(platfrom.idx + 1);
                objs.gameObject.GetComponent<PlatformManager>().genNormalPlatform(platfrom.idx + 2);
                objs.gameObject.GetComponent<PlatformManager>().genNormalPlatform(platfrom.idx + 3);
                this.moveCamera();
            }
            else if (platfrom.type == PlatformType.PricklyPlatform)
            { 
                //gameObject.GetComponent<MeshCollider>().enabled = false;
                //this.isAlive = false;
            }

        }

       
    }

    public void OnCollisionExit(Collision collision)
    {
        Platform platfrom = collision.gameObject.GetComponent<Platform>();
        if (platfrom != null)
        {
            isJumping = true;           
        }
    }


    private void Jump() {
        if (isJumping == false&&isMovingCamera == false && isAlive == true) {

            //Vector3 newVel = new Vector3();
            //newVel.x = vel.x * cosTheat - vel.z * sinTheat;
            //newVel.z = vel.z * cosTheat + vel.x * sinTheat;

            rigidbody.velocity = new Vector3(dir.x, velY, dir.z);

            float degree = Mathf.Acos(dir.x / Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z));
            if (dir.z < 0) {
                degree = -degree;
            }

            Debug.Log("degree="+degree);

            if (dir.x > dir.z)
                transform.rotation = Quaternion.AngleAxis(-degree/Mathf.PI * 180, new Vector3(0, 1, 0));
            else {
                transform.rotation = Quaternion.AngleAxis(-degree / Mathf.PI * 180, new Vector3(0, 1, 0) );
            }

            transform.GetChild(0).GetComponent<Animator>().Play("flip");
        }
    }

    public void Reset(){
        gameObject.SetActive(true);
        if (rigidbody != null) {
            rigidbody.velocity = new Vector3(0, 0, 0);
            rigidbody.ResetInertiaTensor();
            rigidbody.ResetCenterOfMass();
            transform.position = new Vector3(0, 2f, 0);
            rigidbody.angularVelocity = new Vector3(0, 0, 0);         
        }
        transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
        this.isJumping = false;
        gameObject.transform.GetChild(0).GetComponent<MeshCollider>().enabled = true;
        camera.transform.position = deltaPos;
        objs.gameObject.GetComponent<PlatformManager>().Reset();
        Time.timeScale = 1;
        isPausing = false;
        isAlive = true;
        maxIdx = -1;
        score = 0;
        this.scoreText.text = "" + 0;
        this.scorePanel.SetActive(false);
    }

    private bool isMovingCamera = false;

    public void moveCamera() {
        //if (isMovingCamera == false) {
        //    isMovingCamera = true;
        //    Hashtable args = new Hashtable();
        //    args.Add("easeType", iTween.EaseType.easeInOutExpo);
        //    args.Add("time", 1f);
        //    args.Add("loopType", "none");
        //    args.Add("oncomplete", "MoveCameraEnd");
        //    args.Add("onCompleteTarget", gameObject);
        //    args.Add("x", transform.position.x  + deltaPos.x);
        //    args.Add("y", transform.position.y + deltaPos.y);
        //    args.Add("z", transform.position.z + deltaPos.z);
        //    iTween.MoveTo(camera, args);
        //}
        
    }

    public void MoveCameraEnd() {
        isMovingCamera = false;
    }


    public void Pause() {
        this.isPausing = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        this.isPausing = false;
    }

    public void Lose() {
        if (this.camera != null) {
            this.camera.GetComponent<GameCtl>().GameOver();
            this.scorePanel.SetActive(false);
            this.isAlive = false;
            gameObject.SetActive(false);
        }
    }


    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }
}
