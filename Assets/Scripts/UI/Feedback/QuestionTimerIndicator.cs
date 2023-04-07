using System;
using ElRaccoone.Tweens;
using LessonIsMath.DoorSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XIV.Utils;
using XIV.XIVMath;

namespace LessonIsMath.UI
{
    public class QuestionTimerIndicator : MonoBehaviour, IUIEventListener
    {
        [HideInInspector] public DoorQuestionTimerUI doorQuestionTimerUI;
        [HideInInspector] public ArithmeticOperationDoor arithmeticOperationDoor;
        [HideInInspector] public Timer timer;
        
        [SerializeField] Sprite[] sprites;
        [SerializeField] Image image;
        [SerializeField] TextMeshProUGUI tmpText;
        
        Camera mainCam;
        const float MAX_DISTANCE = 15f;
        const float Z_DISTANCE = 5f;
        bool hasTween;
        bool isPaused;
        
        void Awake()
        {
            mainCam = Camera.main;
        }

        void OnEnable() => UIEventSystem.Register<PauseUI>(this);
        void OnDisable() => UIEventSystem.Unregister<PauseUI>(this);

        // Update is called once per frame
        void Update()
        {
            if (isPaused) return;
            
            timer.Update(Time.deltaTime);
            tmpText.text = (timer.Duration - timer.PassedTime).ToString("F1");

            var currentSprite = (int)XIVMathf.RemapClamped(timer.PassedTime, 0f, timer.Duration, 0, sprites.Length - 1);
            image.sprite = sprites[currentSprite];
            if (hasTween == false && currentSprite % sprites.Length > sprites.Length / 4)
            {
                hasTween = true;
                image.transform.localScale = Vector3.one;
                image.TweenLocalScale(Vector3.one * 0.75f, 0.5f)
                    .SetPingPong()
                    .SetLoopCount(int.MaxValue);
            }
            
            var position = arithmeticOperationDoor.transform.position;
            var normalizedDistance = Mathf.Clamp(Vector3.Distance(mainCam.transform.position, position) / MAX_DISTANCE, 0.65f, 1f);
            transform.localScale = Vector3.one * normalizedDistance;

            Vector3 screenPosition = mainCam.WorldToScreenPoint(position);
            screenPosition.z = Z_DISTANCE;
            transform.position = screenPosition;

            if (timer.IsDone == false) return;
            
            arithmeticOperationDoor.GenerateNewQuestion();
            doorQuestionTimerUI.RemoveKey(arithmeticOperationDoor);
            var lockedDoorUI = UISystem.GetUI<LockedDoor_UI>();
            if (lockedDoorUI.isActive)
            {
                lockedDoorUI.UpdateQuestion();
            }
        }

        void IUIEventListener.OnShowUI(GameUI ui)
        {
            isPaused = true;
            if (hasTween) image.TweenCancelAll();
            hasTween = false;
            this.image.enabled = false;
            this.tmpText.enabled = false;
        }

        void IUIEventListener.OnHideUI(GameUI ui)
        {
            isPaused = false;
            this.image.enabled = true;
            this.tmpText.enabled = true;
        }
    }
}