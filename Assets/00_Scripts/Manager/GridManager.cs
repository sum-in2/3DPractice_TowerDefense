using System;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public TowerSpot towerSpotPrefab;
    public Transform parentTransform;
    public int width = 5;
    public int height = 5;
    public float cellSpacing = 1.2f; // 칸 사이 간격
    public GameObject[] WayPoints;

    private TowerSpot[,] spots;

    void Start()
    {
        spots = new TowerSpot[width, height];

        float xOffset = width / 2f;
        float yOffset = height / 2f;
        TowerSpotInit(xOffset, yOffset);
        WayPointsInit(xOffset, yOffset);
    }

    void TowerSpotInit(float xOffset, float yOffset)
    {
        int i = 1;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float posX = (x - xOffset) * cellSpacing;
                float posZ = (y - yOffset) * cellSpacing;

                Vector3 pos = new Vector3(posX, 0, posZ);
                TowerSpot spot = Instantiate(towerSpotPrefab, pos, Quaternion.identity, parentTransform);
                spot.SetIndex(x, y);
                spot.SetParent(parentTransform);
                spot.name = "TowerSpot" + i++;
                spots[x, y] = spot;
            }
        }
    }

    void WayPointsInit(float xOffset, float yOffset)
    {
        Vector3[] corners = new Vector3[4];

        corners[0] = new Vector3((0 - xOffset - 1) * cellSpacing, 0, (0 - yOffset - 1) * cellSpacing); // 좌상단
        corners[1] = new Vector3((0 - xOffset - 1) * cellSpacing, 0, (height - yOffset) * cellSpacing); // 좌하단
        corners[2] = new Vector3((width - xOffset) * cellSpacing, 0, (height - yOffset) * cellSpacing); // 우하단
        corners[3] = new Vector3((width - xOffset) * cellSpacing, 0, (0 - yOffset - 1) * cellSpacing); // 우상단

        for (int i = 0; i < 4; i++)
        {
            WayPoints[i].transform.position = corners[i];
        }
    }
}