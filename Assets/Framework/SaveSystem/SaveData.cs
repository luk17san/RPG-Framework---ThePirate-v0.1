using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public PlayerSaveData Player;
}

[Serializable]
public class PlayerSaveData
{
    public string ShipID;

    public float PositionX;
    public float PositionY;
    public float PositionZ;

    public float Health;
}