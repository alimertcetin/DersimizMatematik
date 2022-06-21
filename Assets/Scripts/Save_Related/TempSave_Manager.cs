//using System.Collections.Generic;
//using UnityEngine;

//public class TempSave_Manager : MonoBehaviour
//{
//    instance_Player_Inventory inventory;
//    instance_LittlePeopleController littlePeopleController;
//    public List<Door_Is_Locked> door_Is_LockedList = new List<Door_Is_Locked>();

//    private void Awake()
//    {
//        inventory = FindObjectOfType<instance_Player_Inventory>();
//        littlePeopleController = FindObjectOfType<instance_LittlePeopleController>();
//    }

//    public void SavePlayer() => SaveSystem.SavePlayer(inventory, littlePeopleController);

//    public void LoadPlayer()
//    {
//        PlayerData data = SaveSystem.LoadPlayer(inventory, littlePeopleController);

//        inventory.Sayi_LoadFromSave(data.rakam_0, data.rakam_1, data.rakam_2, data.rakam_3,
//            data.rakam_4, data.rakam_5, data.rakam_6, data.rakam_7, data.rakam_8, data.rakam_9,
//            data.inventoryCapacity);

//        inventory.Keycard_LoadFromSave(data.yesilKeycard, data.sariKeycard, data.kirmiziKeycard);

//        Vector3 position;
//        position.x = data.position[0];
//        position.y = data.position[1];
//        position.z = data.position[2];
//        littlePeopleController.gameObject.transform.position = position;
//    }

//    public void SaveDoors()
//    {
//        int i = 0;
//        foreach (var item in door_Is_LockedList)
//        {
//            SaveSystem.SaveDoorIsLocked(item);
//        }
//    }

//    public void LoadDoors()
//    {
//        int i = 0;
//        foreach (var item in door_Is_LockedList)
//        {
//            var doors = SaveSystem.LoadDoorIsLocked(item);
//            doors[i].Door_Is_Locked_DoorLocked = item.DoorLocked;
//            doors[i].Door_Is_Locked_DoorLockedAnswer = item.DoorLockedAnswer;
//            doors[i].Door_Is_Locked_DoorLockedQuestion = item.DoorLockedQuestion;
//            i++;
//        }
//    }
//}
