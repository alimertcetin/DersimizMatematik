using System;
using UnityEngine;
using XIV.Extensions;
using XIV.Utils;

namespace GameCore.ScriptableObjects.DoorData
{
    [Serializable]
    public struct ArithmeticOperationData
    {
        public ArithmeticOperation operationBetweenValues;
        public int Value1;
        public int Value2;
        public int Answer;

        public override string ToString()
        {
            var op = operationBetweenValues switch
            {
                ArithmeticOperation.Add => "+",
                ArithmeticOperation.Subtract => "-",
                ArithmeticOperation.Multiply => "x",
                ArithmeticOperation.Divide => "/",
                _ => "",
            };
            return Value1.ToString().Space() + op.Space() + Value2.ToString();
        }
    }

    [CreateAssetMenu(menuName = "Game/DoorQuestionSO")]
    public class DoorQuestionSO : ScriptableObject
    {
        public ArithmeticOperationData arithmeticOperation;

#if UNITY_EDITOR

        [ContextMenu("Calculate Answer")]
        void CalculateAnswer()
        {
            ref var operation = ref arithmeticOperation;
            switch (operation.operationBetweenValues)
            {
                case ArithmeticOperation.None:
                    break;
                case ArithmeticOperation.Add:
                    operation.Answer = operation.Value1 + operation.Value2;
                    break;
                case ArithmeticOperation.Subtract:
                    operation.Answer = operation.Value1 - operation.Value2;
                    break;
                case ArithmeticOperation.Multiply:
                    operation.Answer = operation.Value1 * operation.Value2;
                    break;
                case ArithmeticOperation.Divide:
                    operation.Answer = operation.Value1 / operation.Value2;
                    break;
                default:
                    break;
            }

        }

#endif

    }

}