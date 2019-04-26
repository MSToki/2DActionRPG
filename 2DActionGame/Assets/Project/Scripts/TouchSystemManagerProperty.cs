using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TouchSystemManager
{
    public class TouchSystemManagerProperty : MonoBehaviour
    {
        // 画面がタッチされているか判定
        public bool touchFlag { set; get; }

        // タッチフェーズの状態
        public TouchPhase nowTouchPhase { set; get; }

        // タッチ開始時のポジション
        public Vector2 startTouchPosition { set; get; }

        // タッチ終了時のポジション
        public Vector2 endTouchPosition { set; get; }

        // タッチ開始時からの経過時間
        public float beganTime { set; get; }
    }
}


