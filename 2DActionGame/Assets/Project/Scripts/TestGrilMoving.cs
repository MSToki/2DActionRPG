using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchSystemManager;

public class TestGrilMoving : MonoBehaviour
{
    TouchManager touchManager;
    TouchSystemManagerProperty[] touchPropertys;

    private bool playerMoving;

    // Start is called before the first frame update
    void Start()
    {
        touchManager = new TouchManager();
        touchPropertys = touchManager.getTouchManager();
        playerMoving = false;
    }

    // Update is called once per frame
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
                    playerMoving = true;
                }
                else if (touchPropertys[i].nowTouchPhase == TouchPhase.Ended)
                {
                    
                }
            }
        }
    }

    private void FixedUpdate()
    {
        for (var i = 0; i < touchManager.getMaxTouches(); i++){
            if (touchPropertys[i].nowTouchPhase == TouchPhase.Began && playerMoving)
            {
            }
            else if (touchPropertys[i].nowTouchPhase == TouchPhase.Moved && playerMoving)
            {
                // 移動ベクトル方向取得
                Vector2 vct = (touchPropertys[i].endTouchPosition - touchPropertys[i].startTouchPosition).normalized;
                this.gameObject.GetComponent<Rigidbody2D>().velocity = vct * 2.0f;
                    
                Debug.Log("マウス長押し　vecter：" + vct);
            }
            else if (touchPropertys[i].nowTouchPhase == TouchPhase.Ended && playerMoving)
            {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                playerMoving = false;
            }
        }
    }
}
