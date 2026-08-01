using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private int layers = 6;
    [SerializeField] private int minNodes = 2;
    [SerializeField] private int maxNodes = 4;

    [Header("References")]
    [SerializeField] private MapButton buttonPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private List<RoomData> roomPool;

    [Header("Spacing")]
    [SerializeField] private float xSpacing = 250f;
    [SerializeField] private float ySpacing = 175f;

    private List<List<MapButton>> map = new();

    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        map.Clear();

        // Start
        var startLayer = new List<MapButton>();

        startLayer.Add(CreateNode(roomPool[0],Vector2.zero,0,0));

        map.Add(startLayer);

        // Middle Layers
        for (int layer = 1; layer < layers - 1; layer++)
        {
            int nodeCount = Random.Range(minNodes, maxNodes + 1);

            List<MapButton> currentLayer = new();

            float width = (nodeCount - 1) * xSpacing;

            for (int i = 0; i < nodeCount; i++)
            {
                RoomData room = roomPool[Random.Range(0, roomPool.Count)];

                Vector2 pos = new Vector2(
                    i * xSpacing - width / 2f,
                    layer * ySpacing);

                currentLayer.Add(CreateNode(room, pos, layer, i));
            }

            map.Add(currentLayer);
        }

        // Boss
        List<MapButton> endLayer = new();

        endLayer.Add(CreateNode(
            roomPool[0],
            new Vector2(0, (layers - 1) * ySpacing),
            layers - 1,
            0));

        map.Add(endLayer);

        GenerateConnections();
    }
    

    MapButton CreateNode(RoomData room, Vector2 position, int layer, int index)
    {
        MapButton button = Instantiate(buttonPrefab, parent);

        button.GetComponent<RectTransform>().anchoredPosition = position;

        button.Initialize(room, layer, index);

        return button;
    }

    void GenerateConnections()
    {
        for (int layer = 0; layer < map.Count - 1; layer++)
        {
            var current = map[layer];
            var next = map[layer + 1];

            foreach (var node in current)
            {
                int first = Random.Range(0, next.Count);

                node.NextNodes.Add(next[first]);

                if (Random.value > .5f && next.Count > 1)
                {
                    int second = Random.Range(0, next.Count);

                    if (second != first)
                        node.NextNodes.Add(next[second]);
                }
            }
        }

        // Make sure every node (except Start) has at least one incoming connection.
        for (int layer = 1; layer < map.Count; layer++)
        {
            foreach (var node in map[layer])
            {
                bool connected = false;

                foreach (var previous in map[layer - 1])
                {
                    if (previous.NextNodes.Contains(node))
                    {
                        connected = true;
                        break;
                    }
                }

                if (!connected)
                {
                    map[layer - 1][Random.Range(0, map[layer - 1].Count)]
                        .NextNodes.Add(node);
                }
            }
        }
    }
}
