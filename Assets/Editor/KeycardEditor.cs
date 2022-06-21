using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Keycard))] //Hangi nesnenin inspector görüntüsü değiştirilecek
public class KeycardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Keycard _keycard = (Keycard)target;

        if (GUI.changed)
        {
            if (_keycard.KeycardType == Door_and_Keycard_Level.Yesil)
            {
                Debug.Log("Seviye 1 seçildi.");
            }
            if (_keycard.KeycardType == Door_and_Keycard_Level.Sari)
            {
                Debug.Log("Seviye 2 seçildi.");
            }
            if (_keycard.KeycardType == Door_and_Keycard_Level.Kirmizi)
            {
                Debug.Log("Seviye 3 seçildi.");
            }

        }
    }
}