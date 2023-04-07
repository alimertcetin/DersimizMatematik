using System;
using LessonIsMath.ScriptableObjects.ChannelSOs;
using LessonIsMath.Tween;
using LessonIsMath.UI;
using UnityEngine;
using XIV.Extensions;
using XIV.InventorySystem.Items;
using XIV.InventorySystem.ScriptableObjects.ItemSOs;
using XIV.SaveSystems;

namespace LessonIsMath.DoorSystems
{
    [RequireComponent(typeof(DoorManager))]
    public class KeycardRequiredDoor : MonoBehaviour, IUIEventListener, ISaveable
    {
        [SerializeField] BoolEventChannelSO keycardUIChannel;
        [SerializeField] KeycardItemSO[] requiredKeycards;
        [SerializeField] CardReader[] cardReaders;
        Transform cardReaderParent;
        Vector3 cardReaderInitialPosition;
        Quaternion cardReaderInitialRotation;
        bool[] removedKeycards;
        DoorManager doorManager;
        CardReader activeCardReader;

        public CardReader GetCardReader() => activeCardReader;

        void Awake()
        {
            removedKeycards = new bool[requiredKeycards.Length];
            doorManager = GetComponent<DoorManager>();
        }

        void Start()
        {
            UpdateCardReader();
        }

        /// <summary>
        /// If <paramref name="item"/> is not removed returns index of item, -1 otherwise
        /// </summary>
        public int GetIndexOfRequiredItem(KeycardItem item)
        {
            for (var i = 0; i < requiredKeycards.Length; i++)
            {
                KeycardItemSO requiredKeycard = requiredKeycards[i];
                if (removedKeycards[i] || item.Equals(requiredKeycard.GetItem()) == false) continue;
                return i;
            }
            return -1;
        }

        public void RemoveAt(int index)
        {
            removedKeycards[index] = true;
            UpdateCardReader();
        }

        public void UpdateCardReader()
        {
            int length = requiredKeycards.Length;
            CountKeycards(out var greenCount, out var yellowCount, out var redCount);
            var current = greenCount + yellowCount + redCount;
            for (int i = 0; i < cardReaders.Length; i++)
            {
                cardReaders[i].UpdateVisual(1 - (current / (float)length));
            }
        }

        public void OnInteract(Vector3 interactorPos)
        {
            if (IsKeycardRequired() == false)
            {
                doorManager.OnInteractionEnd();
                return;
            }

            activeCardReader = cardReaders.GetClosestOnXZPlane(interactorPos);

            var cardReaderTransform = activeCardReader.transform;
            cardReaderInitialPosition = cardReaderTransform.position;
            cardReaderInitialRotation = cardReaderTransform.rotation;
            cardReaderParent = cardReaderTransform.parent;
            var camTransform = Camera.main.transform;
            activeCardReader.MoveTowardsTween(camTransform, () => camTransform.forward, 5f, () =>
            {
                UISystem.GetUI<KeycardUI>().SetKeycardRequiredDoor(this);
                cardReaderTransform.SetParent(camTransform);
                keycardUIChannel.RaiseEvent(true);
                UIEventSystem.Register<KeycardUI>(this);
            });
            activeCardReader.LookTween(camTransform, 80f, () =>
            {
                bool isDone = cardReaderTransform.parent == camTransform;
                var angle = Quaternion.Angle(Quaternion.LookRotation(-camTransform.forward), cardReaderTransform.rotation);
                if (angle > 0)
                {
                    activeCardReader.LookTween(camTransform, 80f);
                }
                return isDone;
            });
        }

        public bool IsKeycardRequired()
        {
            int length = requiredKeycards.Length;
            for (int i = 0; i < length; i++)
            {
                if (removedKeycards[i] == false) return true;
            }
            return false;
        }
        
        public void CountKeycards(out int greenCount, out int yellowCount, out int redCount)
        {
            greenCount = 0;
            yellowCount = 0;
            redCount = 0;
            for (int i = 0; i < removedKeycards.Length; i++)
            {
                if (removedKeycards[i]) continue;
                
                switch (requiredKeycards[i].GetItem().KeycardType)
                {
                    case KeycardType.Green:
                        greenCount++;
                        break;
                    case KeycardType.Yellow:
                        yellowCount++;
                        break;
                    case KeycardType.Red:
                        redCount++;
                        break;
                    default: throw new NotImplementedException(requiredKeycards[i].GetItem().KeycardType + " is not implemented.");
                }
            }
        }

        public string GetKeycardString()
        {
            if (IsKeycardRequired() == false) return "";
            CountKeycards(out int greenCount, out int yellowCount, out int redCount);

            string str = "You need ";
            if (greenCount > 0) str += $"{greenCount} green".Green();
            if (yellowCount > 0) str += $" {yellowCount} yellow".Yellow();
            if (redCount > 0) str += $" {redCount} red".Red();

            var total = greenCount + yellowCount + redCount;
            if (total < 2) str += " keycard";
            else str += " keycards";

            return total > 0 ? str : "";
        }

        void IUIEventListener.OnShowUI(GameUI ui) { }

        void IUIEventListener.OnHideUI(GameUI ui)
        {
            activeCardReader.transform.SetParent(cardReaderParent);
            activeCardReader.RotateTowardsTween(cardReaderInitialRotation, 80f);
            activeCardReader.MoveTowardsTween(cardReaderInitialPosition, 5f, () =>
            {
                doorManager.OnInteractionEnd();
                UIEventSystem.Unregister<KeycardUI>(this);
            });
        }
        
        #region --- Save ---

        [Serializable]
        struct SaveData
        {
            public bool[] removedKeycards;
        }

        object ISaveable.CaptureState()
        {
            return new SaveData
            {
                removedKeycards = removedKeycards,
            };
        }

        void ISaveable.RestoreState(object state)
        {
            SaveData saveData = (SaveData)state;
            removedKeycards = saveData.removedKeycards;
            UpdateCardReader();
            doorManager.RefreshDoorState();
        }

        #endregion
    }
}