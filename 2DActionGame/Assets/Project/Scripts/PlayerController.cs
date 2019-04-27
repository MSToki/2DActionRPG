using System;
using System.Collections;
using System.Collections.Generic;
using TouchSystemManager;
using UnityEngine;

public abstract class PlayerController :MonoBehaviour
{
    [Header("[Setting]")]
    public float MoveSpeed = 6;
    public int JumpCount = 2;
    public float jumpForce = 15f;
    public float speedLv;     // 移動速度
    public Vector2 jumpLv;      // ジャンプ力

    TouchManager touchManager;
    TouchSystemManagerProperty[] touchPropertys;
    public Animator anime_Swordman;

    public bool isSit;
    public bool isAttack;
    public bool isMove;
    public bool isMoveRight;
    public bool isMoveLeft;
    public bool isJump;
    public bool isGrounded;
    public bool isOnceJump;
    public bool isDownJump;   // 落下或いは着地の判定結果
    public bool isDead;


    public int currentJumpCount = 0;

    protected float m_MoveX;  // なんやろこいつ消したい


    // Start is called before the first frame update
    void Start()
    {
        speedLv = 3.0f;  // コンポーネントに代入 
        jumpLv = new Vector2(0.0f, 0.0f); // addforceで飛ぶ

        touchManager = new TouchManager();
        touchPropertys = touchManager.getTouchManager();

        isSit = false;              // 座っている
        isAttack = false;           // 攻撃している
        isMove = false;             // 移動している
        isJump = false;             // ジャンプしている
        isGrounded = false;         // 地に足がついてる
        isOnceJump = false;         // 2段ジャンプができる
        isDownJump = false;         // 落下或いは着地の判定結果
        StartPlus();
    }

    public abstract void StartPlus();

    //********************************************************************************************
    //********************************************************************************************

    void Update()
    {
        touchManager.TouchUpdate();
        touchPropertys = touchManager.getTouchManager();

        for (var i = 0; i < touchManager.getMaxTouches(); i++)
        {
            if (touchPropertys[i].touchFlag)
            {
                if (touchPropertys[i].nowTouchPhase == TouchPhase.Began)
                {

                }
                else if (touchPropertys[i].nowTouchPhase == TouchPhase.Moved)
                {
                    isMove = true;
                }
                else if (touchPropertys[i].nowTouchPhase == TouchPhase.Ended)
                {

                }
            }
        }
        UpdatePlus();
    }

    public abstract void UpdatePlus();

    private void FixedUpdate()
    {
        for (var i = 0; i < touchManager.getMaxTouches(); i++)
        {
            if (touchPropertys[i].nowTouchPhase == TouchPhase.Began && isMove)
            {
            }
            else if (touchPropertys[i].nowTouchPhase == TouchPhase.Moved && isMove)
            {
                // 移動ベクトル方向取得
                Vector2 vct = touchPropertys[i].endTouchPosition - touchPropertys[i].startTouchPosition;

                if (vct.x > 2)
                {
                    // X軸正移動
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * speedLv;
                }
                else if (vct.x < -2)
                {
                    // X軸負移動
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.left * speedLv;
                }

                // Y軸判定
                if (vct.y > 2)
                {// 上入力判定（ジャンプ）
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpLv);
                }
                else if (vct.y > -2)
                {// 下入力判定

                }//

                Debug.Log("マウス長押し　vecter：" + vct);
            }
            else if (touchPropertys[i].nowTouchPhase == TouchPhase.Ended && isMove)
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                isMove = false;
            }
        }
    }

    //********************************************************************************************
    //********************************************************************************************





    protected void prefromJump()
    {// わからん
        anime_Swordman.Play("Jump");

        this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        this.transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        isOnceJump = true;
        isGrounded = false;


        currentJumpCount++;

    }

    protected void DownJump()
    {// わからん
        if (!isGrounded)
            return;


        if (!isDownJump)
        {
            anime_Swordman.Play("Jump");

            this.transform.GetComponent<Rigidbody2D>().AddForce(-Vector2.up * 10);
            isGrounded = false;

            this.transform.GetComponent<CapsuleCollider2D>().enabled = false;

            StartCoroutine(GroundCapsulleColliderTimmerFuc());

        }
    }

    IEnumerator GroundCapsulleColliderTimmerFuc()
    {// わからん
        yield return new WaitForSeconds(0.3f);
        this.transform.GetComponent<CapsuleCollider2D>().enabled = true;
    }


    //////着地判定 
    // わからん
    Vector2 RayDir = Vector2.down;


    float PretmpY;
    float GroundCheckUpdateTic = 0;
    float GroundCheckUpdateTime = 0.01f;
    protected void GroundCheckUpdate()
    {
        if (!isOnceJump)
            return;

        GroundCheckUpdateTic += Time.deltaTime;

        if (GroundCheckUpdateTic > GroundCheckUpdateTime)
        {
            GroundCheckUpdateTic = 0;

            if (PretmpY == 0)
            {
                PretmpY = transform.position.y;
                return;
            }

            float reY = transform.position.y - PretmpY;  //    -1  - 0 = -1 ,  -2 -   -1 = -3

            if (reY <= 0)
            {
                if (isGrounded)
                {
                    LandingEvent();
                    isOnceJump = false;

                }
                else
                {

                }
            }
            PretmpY = transform.position.y;

        }
    }
    protected abstract void LandingEvent();

}
