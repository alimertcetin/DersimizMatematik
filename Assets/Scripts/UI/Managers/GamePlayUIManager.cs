﻿using LessonIsMath.DoorSystems;
using LessonIsMath.Input;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using UnityEngine;
using XIV.UI;

namespace LessonIsMath.UI
{
    public class GamePlayUIManager : MonoBehaviour
    {
        [Header("Listening To")]
        [SerializeField] BoolEventChannelSO pauseMenuUIChannel;
        [SerializeField] BoolEventChannelSO blackBoardUIChannel;
        [SerializeField] ArithmeticDoorEventChannelSO arithmeticDoorUIChannel;
        [SerializeField] BoolEventChannelSO keycardUIChannel;
        [SerializeField] BoolEventChannelSO hudUIChannel;

        void OnEnable()
        {
            pauseMenuUIChannel.OnEventRaised += ShowPauseMenuUI;
            blackBoardUIChannel.OnEventRaised += ShowBlackBoardUI;
            arithmeticDoorUIChannel.OnEventRaised += ShowLockedDoorUI;
            keycardUIChannel.OnEventRaised += ShowKeycardUI;
            hudUIChannel.OnEventRaised += ShowHud;
        }

        void OnDisable()
        {
            pauseMenuUIChannel.OnEventRaised -= ShowPauseMenuUI;
            blackBoardUIChannel.OnEventRaised -= ShowBlackBoardUI;
            arithmeticDoorUIChannel.OnEventRaised -= ShowLockedDoorUI;
            keycardUIChannel.OnEventRaised -= ShowKeycardUI;
            hudUIChannel.OnEventRaised -= ShowHud;
        }

        void ShowPauseMenuUI(bool value)
        {
            if (value) UISystem.Show<PauseUI>();
            else UISystem.Hide<PauseUI>();
            hudUIChannel.RaiseEvent(!value);
        }

        void ShowBlackBoardUI(bool value)
        {
            if (value) UISystem.Show<BlackboardUI>();
            else UISystem.Hide<BlackboardUI>();
        }

        void ShowLockedDoorUI(ArithmeticOperationDoor arithmeticOperationDoor, bool value)
        {
            UISystem.GetUI<LockedDoor_UI>()?.SetDoor(arithmeticOperationDoor);
            if (value) UISystem.Show<LockedDoor_UI>();
            else UISystem.Hide<LockedDoor_UI>();
        }

        void ShowKeycardUI(bool value)
        {
            if (value) UISystem.Show<KeycardUI>();
            else UISystem.Hide<KeycardUI>();
            hudUIChannel.RaiseEvent(!value);
        }

        void ShowHud(bool value)
        {
            if (value) UISystem.Show<HUD_UI>();
            else UISystem.Hide<HUD_UI>();
        }
    }
}