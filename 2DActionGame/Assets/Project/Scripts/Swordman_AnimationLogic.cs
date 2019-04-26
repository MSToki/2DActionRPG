using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman_AnimationLogic : PlayerController
{
    protected void AnimUpdate()
    {// 親クラスのメンバや現在のアニメーション動作情報を参照し、
     // 次のアニメーション動作を決定する
        if (!anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {// アニメーションステートが"Attack"でない

            if (isAttack)
            {// 攻撃ボタンを押した

                anime_Swordman.Play("Attack");
            }
            else
            {// 攻撃ボタンを押してない

                if (isMove)
                {// 移動してる

                    anime_Swordman.Play("Run");
                }
                else
                {// 移動してない

                    if (!isJump) // ジャンプしてない
                        anime_Swordman.Play("Idle");

                }
            }
        }
    }

    private void Update()
    {

        checkInput();

        if (this.transform.GetComponent<Rigidbody2D>().velocity.magnitude > 30)
        {
            this.transform.GetComponent<Rigidbody2D>().velocity =
                new Vector2(this.transform.GetComponent<Rigidbody2D>().velocity.x - 0.1f, this.transform.GetComponent<Rigidbody2D>().velocity.y - 0.1f);

        }
    }

    public void checkInput()
    {// 各入力チェックから動作を決定する

        if (isSit)
        {// 下入力あり
            anime_Swordman.Play("Sit");
        }
        else
        {// 下入力なし

            anime_Swordman.Play("Idle");
            isSit = false;
        }

        if (anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Sit") || anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {// 良くない記述な気がする
            if (isSit)
            {
                if (currentJumpCount < JumpCount)  // 0 , 1
                {
                    DownJump();
                }
            }

            return;
        }

        GroundCheckUpdate();

        if (!anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (isAttack)
            {

                anime_Swordman.Play("Attack");
            }
            else
            {

                if (isMove)
                {
                    if (!isOnceJump)
                        anime_Swordman.Play("Idle");

                }
                else
                {

                    anime_Swordman.Play("Run");
                }
            }
        }

        if (isDead)
        {
            anime_Swordman.Play("Die");
        }

        // 기타 이동 인풋.


        if (!anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {// 攻撃中でない時、移動の向きを補正
            if (isMoveLeft)
                if (!isMoveRight)
                    Filp(false);
                else return;
            else if (isMoveRight)
                if (!isMoveLeft)
                    Filp(true);
        }


        if (isJump)
        {// 攻撃中でなければジャンプモーション
            if (anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;
            if (currentJumpCount < JumpCount)  // 0 , 1
            {
                if (!isSit)
                {
                    prefromJump();
                }
                else
                {
                    DownJump();
                }
            }
        }
    }

    protected void Filp(bool bLeft)
    {// キャラ向き反転

        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }

    protected override void LandingEvent()
    {// わからん

        if (!anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Run") && !anime_Swordman.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            anime_Swordman.Play("Idle");
    }
    public override void StartPlus()
    {// start処理に追加したい場合はここに記述
        // そもそもこの記述をしなくてもオーバーライドしないので不要
    }
    public override void UpdatePlus()
    {// update処理に追加したい場合はここに記述
        // そもそもこの記述をしなくてもオーバーライドしないので不要
    }
}
