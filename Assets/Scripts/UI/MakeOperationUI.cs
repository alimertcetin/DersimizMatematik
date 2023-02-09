using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XIV.InventorySystem.Items;
using XIV.Utils;

namespace GameCore.UI
{
    public class MakeOperationUI : OperationUI, PlayerControls.IMakeOperationUIActions
    {
        [Header("UI Navigation")]
        [SerializeField] BlackboardMainUI blackBoardMainUI = null;
        [SerializeField] GameObject makeOperationUI = null;

        [Header("UI Elements")]
        [SerializeField] TMP_Text txt_OperationInputField = null;
        [SerializeField] TMP_Text txt_ReviewInput = null;
        [SerializeField] Button btn_Subtract;
        [SerializeField] Button btn_Add;
        [SerializeField] Button btn_Answer;

        [Header("UI Settings")]
        [SerializeField] protected int InputFiedlMaxTextLenght = 7;

        OperationHelper operationHelper;
        List<NumberItem> inputNumberItems = new List<NumberItem>();

        protected override void Awake()
        {
            base.Awake();
            InputManager.PlayerControls.MakeOperationUI.SetCallbacks(this);
        }

        public override void ShowUI()
        {
            makeOperationUI.SetActive(true);
            btn_Subtract.onClick.AddListener(SelectSubtractOperation);
            btn_Add.onClick.AddListener(SelectAddOperation);
            btn_Answer.onClick.AddListener(Answer);
            InputManager.MakeOperationUI.Enable();
            InputManager.BlackBoardUIManagement.Disable();
        }

        public override void CloseUI()
        {
            makeOperationUI.SetActive(false);
            btn_Subtract.onClick.RemoveListener(SelectSubtractOperation);
            btn_Add.onClick.RemoveListener(SelectAddOperation);
            btn_Answer.onClick.RemoveListener(Answer);
            InputManager.MakeOperationUI.Disable();
            InputManager.BlackBoardUIManagement.Enable();
            blackBoardMainUI.ShowUI();

            while (inputNumberItems.Count > 0)
            {
                Delete();
            }

            operationHelper.operation = ArithmeticOperation.None;
            txt_OperationInputField.text = "";
            txt_ReviewInput.text = "";
        }

        private void SelectSubtractOperation()
        {
            var currentInput = txt_OperationInputField.text;
            if (OperationUIHelper.SelectSubtractOperation(ref operationHelper, ref currentInput, out var error) == false)
            {
                txt_OperationInputField.text = currentInput;
                warningUIChannel.RaiseEvent(error);
                return;
            }
            txt_OperationInputField.text = currentInput;
            txt_ReviewInput.text = OperationUIHelper.GetCurrentOperationReviewString(ref operationHelper);
        }

        private void SelectAddOperation()
        {
            var currentInput = txt_OperationInputField.text;
            if (OperationUIHelper.SelectAddOperation(ref operationHelper, ref currentInput, out var error) == false)
            {
                txt_OperationInputField.text = currentInput;
                warningUIChannel.RaiseEvent(error);
                return;
            }
            txt_OperationInputField.text = currentInput;
            txt_ReviewInput.text = OperationUIHelper.GetCurrentOperationReviewString(ref operationHelper);
        }

        protected override void Delete()
        {
            if (inputNumberItems.Count == 0) return;

            int index = inputNumberItems.Count - 1;
            int amount = 1;
            inventory.TryAdd(inputNumberItems[index], ref amount);
            inputNumberItems.RemoveAt(index);
            
            if (txt_OperationInputField.text.Length == 0) return;
            txt_OperationInputField.text = txt_OperationInputField.text.Remove(txt_OperationInputField.text.Length - 1);
        }

        private void Answer()
        {
            var currentInput = txt_OperationInputField.text;
            if (OperationUIHelper.CalculateAnswer(ref operationHelper, ref currentInput, out var error) == false)
            {
                txt_OperationInputField.text = currentInput;
                warningUIChannel.RaiseEvent(error);
                return;
            }
            txt_OperationInputField.text = currentInput;
            txt_ReviewInput.text = "";
            inputNumberItems.Clear();

            int length = currentInput.Length;
            for (int i = 0; i < length; i++)
            {
                int digit = int.Parse(currentInput[i].ToString());

                for (int j = 0; j < numberItems.Length; j++)
                {
                    if (numberItems[j].item.Value == digit)
                    {
                        inputNumberItems.Add(numberItems[j].GetItem());
                        break;
                    }
                }
            }
        }

        protected override void OnNumberButtonClicked(int value)
        {
            if (OperationUIHelper.IsInputExistInInventory(inventory, value, out var index, out var error) == false)
            {
                warningUIChannel.RaiseEvent(error);
                return;
            }
            var currentInput = txt_OperationInputField.text;
            if (OperationUIHelper.AddInput(InputFiedlMaxTextLenght, ref currentInput, value, out error) == false)
            {
                txt_OperationInputField.text = currentInput;
                warningUIChannel.RaiseEvent(error);
                return;
            }

            inputNumberItems.Add(inventory[index].Item as NumberItem);
            int amount = 1;
            inventory.RemoveAt(index, ref amount);
            txt_OperationInputField.text = currentInput;
        }

        #region INPUT HANDLING

        void PlayerControls.IMakeOperationUIActions.OnZero(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(0);
        }

        void PlayerControls.IMakeOperationUIActions.OnOne(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(1);
        }

        void PlayerControls.IMakeOperationUIActions.OnTwo(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(2);
        }

        void PlayerControls.IMakeOperationUIActions.OnThree(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(3);
        }

        void PlayerControls.IMakeOperationUIActions.OnFour(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(4);
        }

        void PlayerControls.IMakeOperationUIActions.OnFive(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(5);
        }

        void PlayerControls.IMakeOperationUIActions.OnSix(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(6);
        }

        void PlayerControls.IMakeOperationUIActions.OnSeven(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(7);
        }

        void PlayerControls.IMakeOperationUIActions.OnEight(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(8);
        }

        void PlayerControls.IMakeOperationUIActions.OnNine(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberButtonClicked(9);
        }

        void PlayerControls.IMakeOperationUIActions.OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed) Answer();
        }

        void PlayerControls.IMakeOperationUIActions.OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) CloseUI();
        }

        void PlayerControls.IMakeOperationUIActions.OnDelete(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Delete();
                deleteStarted = true;
            }

            if (context.canceled)
            {
                deleteTimer.Restart();
                deleteStarted = false;
            }
        }

        void PlayerControls.IMakeOperationUIActions.OnMinus(InputAction.CallbackContext context)
        {
            if (context.performed) SelectSubtractOperation();
        }
        
        void PlayerControls.IMakeOperationUIActions.OnPlus(InputAction.CallbackContext context)
        {
            if (context.performed) SelectAddOperation();
        }
    }

    #endregion
}
