using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using UnityEditor;


public class PipeMeshCreator : PathSceneTool
{
    [Header("Debug settings")]
    public bool debuggingOn = false;
    public bool debugPath = false;
    public bool debugBasePath = false;
    public bool debugVerts = false;
    public bool debugMesh = false;
    public bool extendSegments = false;
    public Color debugMonocolor = Color.red;
    public bool multicolorDebugging = false;
    public Vector2Int debugIndeciesRange;


    [Header("Mesh settings")]
    [Range(0, 32)]
    public float radius = 0.1f;
    [Range(3, 32)]
    public int numOfVerts = 8;
    [Min(0.01f)]
    public float maxSegmentDistance = 0.1f;
    public bool addCollision = false;

    [Header("Material settings")]
    public Material pipeMaterial;
    public Vector2 tiling = new Vector2(1, 1);

    [SerializeField, HideInInspector]
    GameObject meshHolder;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;

    //Used for calculating the verts and normals normally and when extending the mesh (Hence a list for easier extension).
    public List<Vector3> pipePath;
    public List<Vector3> pipePathNorms;

    Vector3[] debugVertsArr;
    public int[] debugIndices;

    protected override void PathUpdated()
    {
        if (pathCreator != null)
        {
            AssignMeshComponents();
            AssignMaterials();
            CreateRoadMesh();
        }
    }

    //Checks if the base path needs any extension and if so, inserts vertices between the needing vertices.
    //Might improve UVs?
    void CheckForExtension(int index)
    {
        float dist = Vector3.Distance(path.GetPoint(index), path.GetPoint(index + 1));

        if (dist > maxSegmentDistance)
        {
            int numOfExtraSegments = Mathf.FloorToInt(dist / maxSegmentDistance);
            Vector3 dir = (path.GetPoint(index + 1) - path.GetPoint(index)).normalized * maxSegmentDistance;

            for (int k = 1; k < numOfExtraSegments; k++)
            {
                pipePath.Add(path.GetPoint(index) + k * dir);
                pipePathNorms.Add(path.GetNormal(index));
            }
        }
    }

    //Using the given path, rotates about the point in a circle and creates vertices.
    void GeneratePipeVertsAndNormals(Vector3[] verts, Vector3[] normals)
    {

        float rotAngle = 360f / numOfVerts;
        int index;
        Vector3 shim = Vector3.zero;

        for (int j = 1; j < pipePath.Count; j++)
        {
            //print(internalPath.Count.ToString() + "," + internalPathNormals.Count.ToString() + "," + internalNumPoints.ToString() + "," + j.ToString());

            Vector3 dir = pipePath[j] - pipePath[j - 1];
            Vector3 cross = (Vector3.Cross(pipePathNorms[j], dir)).normalized * radius;


            for (int i = 0; i <= numOfVerts; i++)
            {
                index = (j - 1) * (numOfVerts) + i;

                Quaternion q = Quaternion.AngleAxis((i == 0 ? 0 : rotAngle), dir);
                cross = q * cross;

                normals[index] = cross.normalized;

                verts[index] = pipePath[j] + cross;

                if (debuggingOn)
                    debugIndices[index] = index;

                if (index == 0)
                    shim = verts[index];
                else if (index == numOfVerts)
                    verts[index] = shim;


            }
        }

    }

    //Calculates the indices that draw each triangle.
    void CalculateTriArray(int[] Tris)
    {
        int vertIndex = 0;
        int triIndex = 0;

        // Set triangle indices
        for (int i = 0; i < (pipePath.Count - 2) * numOfVerts; i++)
        {
            Tris[triIndex + 5] = (vertIndex + numOfVerts + 1);
            Tris[triIndex + 4] = (vertIndex + 1);
            Tris[triIndex + 3] = (vertIndex);
            Tris[triIndex + 2] = (vertIndex + numOfVerts);
            Tris[triIndex + 1] = (vertIndex + numOfVerts + 1);
            Tris[triIndex + 0] = (vertIndex);

            vertIndex += 1;
            triIndex += 6;
        }
    }

    //Calculates the UVs for each vertex on the mesh.
    void CalculateUVs(Vector2[] uvs)
    {
        for (int i = 0; i < pipePath.Count * numOfVerts; i++)
        {
            float yVal = i / ((float)pipePath.Count * numOfVerts / 2f);

            if (i == 0)
                uvs[i] = new Vector2(0, yVal) * tiling;
            else if (i % numOfVerts == 0)
                uvs[i] = new Vector2(1f, (i - 1) / ((float)pipePath.Count * numOfVerts / 2f)) * tiling;
            else
                uvs[i] = new Vector2((i % numOfVerts) / (float)(numOfVerts), yVal) * tiling;
        }
    }

    void CreateRoadMesh()
    {
        pipePath = new List<Vector3>();
        pipePathNorms = new List<Vector3>();

        for (int i = 0; i + 1 < path.NumPoints; i++)
        {
            pipePath.Add(path.GetPoint(i));
            pipePathNorms.Add(path.GetNormal(i));

            if (extendSegments)
                CheckForExtension(i);
        }

        int totalNumOfVerts = numOfVerts * pipePath.Count;
        Vector3[] verts = new Vector3[totalNumOfVerts];
        Vector3[] normals = new Vector3[totalNumOfVerts];
        Vector2[] uvs = new Vector2[totalNumOfVerts];

        if (debuggingOn)
            debugIndices = new int[totalNumOfVerts];

        GeneratePipeVertsAndNormals(verts, normals);

        int numTris = 2 * (pipePath.Count) * numOfVerts;
        int[] Triangles = new int[numTris * 3];

        CalculateTriArray(Triangles);

        CalculateUVs(uvs);

        if (debuggingOn)
        {
            debugVertsArr = new Vector3[verts.Length];
            debugVertsArr = verts;

            DebugMesh();
            OnDrawGizmos();
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.subMeshCount = 1;
        mesh.SetTriangles(Triangles, 0);
        mesh.RecalculateBounds();
    }

    //Add MeshRenderer and MeshFilter components to this gameobject if not already attached
    void AssignMeshComponents()
    {
        if (transform.childCount == 0)
        {
            meshHolder = new GameObject();
            meshHolder.transform.SetParent(transform);
        }

        meshHolder.transform.rotation = Quaternion.identity;
        meshHolder.transform.position = Vector3.zero;
        meshHolder.transform.localScale = Vector3.one;

        // Ensure mesh renderer and filter components are assigned
        if (!meshHolder.gameObject.GetComponent<MeshFilter>())
        {
            meshHolder.gameObject.AddComponent<MeshFilter>();
        }
        if (!meshHolder.GetComponent<MeshRenderer>())
        {
            meshHolder.gameObject.AddComponent<MeshRenderer>();
        }

        meshRenderer = meshHolder.GetComponent<MeshRenderer>();
        meshFilter = meshHolder.GetComponent<MeshFilter>();
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        meshFilter.mesh = mesh;
    }

    void AssignMaterials()
    {
        if (pipeMaterial != null)
        {
            meshRenderer.sharedMaterials = new Material[] { pipeMaterial };
        }
    }

    //Uses the method used to record triangle indices, draws a wire mesh, easier to see if the mesh is corrupt.
    void DebugMesh()
    {
        if (!debugMesh)
            return;

        Color[] colors = { Color.black, Color.red, Color.green, Color.blue, Color.yellow, Color.cyan };
        int vertIndex = 0;
        Color debugColor = debugMonocolor;


        for (int i = 0; i < (pipePath.Count - 2) * (numOfVerts) - 1; i++)
        {
            if (multicolorDebugging)
                debugColor = colors[i % colors.Length];

            Debug.DrawLine(debugVertsArr[vertIndex], debugVertsArr[(vertIndex + numOfVerts + 1)], debugColor, Time.deltaTime / 2);
            Debug.DrawLine(debugVertsArr[(vertIndex + numOfVerts + 1)], debugVertsArr[(vertIndex + 1)], debugColor, Time.deltaTime / 2);
            Debug.DrawLine(debugVertsArr[(vertIndex + 1)], debugVertsArr[(vertIndex + numOfVerts + 1)], debugColor, Time.deltaTime / 2);
            Debug.DrawLine(debugVertsArr[(vertIndex + numOfVerts + 1)], debugVertsArr[(vertIndex + numOfVerts + 2)], debugColor, Time.deltaTime / 2);

            //(vertIndex);
            //(vertIndex + numOfVerts + 1);
            //(vertIndex + 1);
            //(vertIndex + 1);
            //(vertIndex + numOfVerts + 1);
            //(vertIndex + numOfVerts + 2);

            vertIndex += 1;
        }


    }

    //Draws the index of each vertex at it's position.
    void DebugVerts()
    {
        if (debugIndeciesRange.x > debugIndeciesRange.y)
        {
            int temp = debugIndeciesRange.y;
            debugIndeciesRange.y = debugIndeciesRange.x;
            debugIndeciesRange.x = temp;
        }
        if (debugIndeciesRange.x < 0)
            debugIndeciesRange.x = 0;
        if (debugIndeciesRange.y > debugVertsArr.Length)
            debugIndeciesRange.y = debugVertsArr.Length - 1;

        for (int i = debugIndeciesRange.x; i < debugIndeciesRange.y; i++)
        {
            #if UNITY_EDITOR
                 Handles.Label(debugVertsArr[i], debugIndices[i].ToString());
            #endif
        }
    }

    //Goes through each vertex of the extended path and draws a sphere.
    void DebugExtendedPath()
    {
        for (int i = 0; i < pipePath.Count; i++)
        {
            Gizmos.color = i % 2 == 0 ? Color.white : Color.black;
            Gizmos.DrawSphere(pipePath[i], i % 2 == 0 ? 0.01f : 0.0125f);
        }
    }

    //Goes through each vertex of the base path and draws a sphere.
    void DebugBasePath()
    {
        for (int i = 0; i < path.NumPoints; i++)
        {
            Gizmos.color = i % 2 == 0 ? Color.red : Color.blue;
            Gizmos.DrawSphere(path.GetPoint(i), 0.01f);
        }
    }

    private void OnDrawGizmos()
    {
        if (!debuggingOn)
            return;

        if (debugVerts)
            DebugVerts();

        if (debugPath)
            DebugExtendedPath();

        if (debugBasePath)
            DebugBasePath();
    }

    public override void UpdateCollision()
    {
        MeshCollider meshColl = meshHolder.GetComponent<MeshCollider>();
        if (meshColl)
            meshColl.sharedMesh = mesh;
        else
        {
            meshColl = meshHolder.AddComponent<MeshCollider>();
            meshColl.sharedMesh = mesh;
        }
    }
}
