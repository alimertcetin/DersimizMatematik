using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Door_Status))] //Hangi nesnenin inspector görüntüsü değiştirilecek
public class Door_Status_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        Door_Status status = (Door_Status)target;

        base.OnInspectorGUI();

        if (!GUI.changed)
            return;

        if (!status.Door_Status_List.Contains(DoorStatus.KeycardRequired) && status.TryGetComponent(out DoorKeycard_Management keycard_Management))
        {
            DestroyImmediate(keycard_Management);
        }

        if (!status.Door_Status_List.Contains(DoorStatus.Locked) && status.TryGetComponent(out Door_Is_Locked lockedDoorScript))
        {
            DestroyImmediate(lockedDoorScript);
        }

        foreach (var item in status.Door_Status_List)
        {
            if (item == DoorStatus.Locked)
            {
                if (status.gameObject.GetComponent<Door_Is_Locked>() == null)
                    status.gameObject.AddComponent<Door_Is_Locked>();
            }
            else if (item == DoorStatus.KeycardRequired)
            {
                if (status.gameObject.GetComponent<DoorKeycard_Management>() == null)
                    status.gameObject.AddComponent<DoorKeycard_Management>();
            }
        }

    }
}