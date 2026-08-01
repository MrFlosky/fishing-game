using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private Image roomImage;
    [SerializeField] private RoomData roomData;
    
    public int layer;
    public int index;
    
    public List<MapButton> NextNodes = new();
    
    public void Initialize(RoomData data, int layer, int index)
    {
        roomData = data;

        this.layer = layer;
        this.index = index;

        roomName.text = data.name;
        roomImage.sprite = data.roomImage;
    }
    
    public void SelectOption()
    {
        SceneManager.LoadScene(roomData.sceneName);
    }
}
