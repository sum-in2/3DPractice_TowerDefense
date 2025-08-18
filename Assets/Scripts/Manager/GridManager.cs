using System;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public TowerSpot towerSpotPrefab;
    public Transform parentTransform;
    public int width = 5;
    public int height = 5;
    public float cellSpacing = 1.2f; // 칸 사이 간격

    private TowerSpot[,] spots;

    void Start()
    {
        spots = new TowerSpot[width, height];

        float xOffset = width / 2f;
        float yOffset = height / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float posX = (x - xOffset) * cellSpacing;
                float posZ = (y - yOffset) * cellSpacing;

                Vector3 pos = new Vector3(posX, 0, posZ);
                TowerSpot spot = Instantiate(towerSpotPrefab, pos, Quaternion.identity, parentTransform);
                spot.SetIndex(x, y);
                spots[x, y] = spot;
            }
        }
    }
}