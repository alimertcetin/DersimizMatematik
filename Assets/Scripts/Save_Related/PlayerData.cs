
[System.Serializable]
public class PlayerData
{
    public int rakam_0, rakam_1, rakam_2, rakam_3, rakam_4,
                         rakam_5, rakam_6, rakam_7, rakam_8, rakam_9;
    public int yesilKeycard, sariKeycard, kirmiziKeycard;

    public int inventoryCapacity;

    public float[] position = new float[3];

    public PlayerData(PlayerInventorySO inventorySO, PlayerController littlePeopleController)
    {
        rakam_0 = inventorySO.Rakam_0; rakam_1 = inventorySO.Rakam_1; rakam_2 = inventorySO.Rakam_2;
        rakam_3 = inventorySO.Rakam_3; rakam_4 = inventorySO.Rakam_4; rakam_5 = inventorySO.Rakam_5;
        rakam_6 = inventorySO.Rakam_6; rakam_7 = inventorySO.Rakam_7; rakam_8 = inventorySO.Rakam_8;
        rakam_9 = inventorySO.Rakam_9;
        yesilKeycard = inventorySO.yesilKeycard;
        sariKeycard = inventorySO.sariKeycard;
        kirmiziKeycard = inventorySO.kirmiziKeycard;

        inventoryCapacity = inventorySO.Capacity;

        position[0] = littlePeopleController.transform.position.x;
        position[1] = littlePeopleController.transform.position.y;
        position[2] = littlePeopleController.transform.position.z;
    }
}
