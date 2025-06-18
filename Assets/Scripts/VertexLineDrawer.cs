using UnityEngine;

public class CubeEdgeLineDrawer : MonoBehaviour
{
    public Material lineMaterial;
    public float lineWidth = 0.01f;

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
            return;

        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        Transform tf = transform;

        int[,] edges = new int[,]
        {
            {0,1}, {1,7}, {7,6}, {6,0}, // 아래면
            {2,3}, {3,5}, {5,4}, {4,2}, // 윗면
            {0,2}, {1,3}, {7,5}, {6,4}  // 옆면 연결
        };



        for (int i = 0; i < edges.GetLength(0); i++)
        {
            GameObject lineObj = new GameObject("EdgeLine_" + i);
            lineObj.transform.parent = this.transform;
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.positionCount = 2;
            lr.useWorldSpace = true;
            lr.SetPosition(0, tf.TransformPoint(vertices[edges[i, 0]]));
            lr.SetPosition(1, tf.TransformPoint(vertices[edges[i, 1]]));
        }
    }
}
