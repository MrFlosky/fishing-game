using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "RoomData", menuName = "RoomData/Default", order = 1)]
public class RoomData : ScriptableObject
{
    public string roomName = "Room";
    public Sprite roomImage;
    
    public string sceneName = "Unset";
}