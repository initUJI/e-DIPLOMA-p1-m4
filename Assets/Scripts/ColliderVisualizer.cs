using UnityEngine;

[DisallowMultipleComponent]
public class ColliderVisualizer : MonoBehaviour
{
    public Color colliderColor = Color.green;
    public float lineWidth = 0.1f;
    public LayerMask ignoreLayers; // LayerMask para especificar las capas a ignorar (incluye UI)

    private Material lineMaterial;

    void Start()
    {
        // Create a simple shader to render the colliders
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader)
        {
            hideFlags = HideFlags.HideAndDontSave
        };
        // Enable depth test
        lineMaterial.SetInt("_ZWrite", 1);
        lineMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
        lineMaterial.SetFloat("_LineWidth", lineWidth);
    }

    void OnRenderObject()
    {
        // Verificar si el nombre del objeto contiene "Wire"
        if (gameObject.name.Contains("Wire"))
        {
            return;
        }

        // Verificar si el objeto principal está activo
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        lineMaterial.SetColor("_Color", colliderColor);
        lineMaterial.SetPass(0);

        foreach (var collider in GetComponentsInChildren<Collider>(true))
        {
            // Ignorar colisionadores en las capas especificadas o si el objeto está desactivado
            if (((1 << collider.gameObject.layer) & ignoreLayers) != 0 || !collider.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (collider is BoxCollider)
            {
                DrawBoxCollider((BoxCollider)collider);
            }
            else if (collider is SphereCollider)
            {
                DrawSphereCollider((SphereCollider)collider);
            }
            else if (collider is CapsuleCollider)
            {
                DrawCapsuleCollider((CapsuleCollider)collider);
            }
            else if (collider is MeshCollider)
            {
                DrawMeshCollider((MeshCollider)collider);
            }
        }
    }

    private void DrawBoxCollider(BoxCollider box)
    {
        Vector3 center = box.center;
        Vector3 size = box.size;

        Vector3[] vertices = new Vector3[8];
        vertices[0] = box.transform.TransformPoint(center + new Vector3(-size.x, -size.y, -size.z) * 0.5f);
        vertices[1] = box.transform.TransformPoint(center + new Vector3(size.x, -size.y, -size.z) * 0.5f);
        vertices[2] = box.transform.TransformPoint(center + new Vector3(size.x, -size.y, size.z) * 0.5f);
        vertices[3] = box.transform.TransformPoint(center + new Vector3(-size.x, -size.y, size.z) * 0.5f);
        vertices[4] = box.transform.TransformPoint(center + new Vector3(-size.x, size.y, -size.z) * 0.5f);
        vertices[5] = box.transform.TransformPoint(center + new Vector3(size.x, size.y, -size.z) * 0.5f);
        vertices[6] = box.transform.TransformPoint(center + new Vector3(size.x, size.y, size.z) * 0.5f);
        vertices[7] = box.transform.TransformPoint(center + new Vector3(-size.x, size.y, size.z) * 0.5f);

        GL.Begin(GL.LINES);
        for (int i = 0; i < 4; i++)
        {
            GL.Vertex(vertices[i]);
            GL.Vertex(vertices[(i + 1) % 4]);
            GL.Vertex(vertices[i + 4]);
            GL.Vertex(vertices[((i + 1) % 4) + 4]);
            GL.Vertex(vertices[i]);
            GL.Vertex(vertices[i + 4]);
        }
        GL.Vertex(vertices[0]);
        GL.Vertex(vertices[3]);
        GL.Vertex(vertices[1]);
        GL.Vertex(vertices[2]);
        GL.Vertex(vertices[4]);
        GL.Vertex(vertices[7]);
        GL.Vertex(vertices[5]);
        GL.Vertex(vertices[6]);
        GL.End();
    }

    private void DrawSphereCollider(SphereCollider sphere)
    {
        Vector3 center = sphere.transform.TransformPoint(sphere.center);
        float radius = sphere.radius * Mathf.Max(sphere.transform.lossyScale.x, Mathf.Max(sphere.transform.lossyScale.y, sphere.transform.lossyScale.z));

        GL.PushMatrix();
        GL.MultMatrix(Matrix4x4.TRS(center, Quaternion.identity, Vector3.one * radius));
        GL.Begin(GL.LINES);
        for (int i = 0; i < 360; i += 10)
        {
            float rad = Mathf.Deg2Rad * i;
            float radNext = Mathf.Deg2Rad * (i + 10);
            GL.Vertex3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
            GL.Vertex3(Mathf.Cos(radNext), Mathf.Sin(radNext), 0);
            GL.Vertex3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
            GL.Vertex3(Mathf.Cos(radNext), 0, Mathf.Sin(rad));
            GL.Vertex3(0, Mathf.Cos(rad), Mathf.Sin(rad));
            GL.Vertex3(0, Mathf.Cos(radNext), Mathf.Sin(radNext));
        }
        GL.End();
        GL.PopMatrix();
    }

    private void DrawCapsuleCollider(CapsuleCollider capsule)
    {
        Vector3 center = capsule.transform.TransformPoint(capsule.center);
        float radius = capsule.radius * Mathf.Max(capsule.transform.lossyScale.x, Mathf.Max(capsule.transform.lossyScale.y, capsule.transform.lossyScale.z));
        float height = capsule.height * capsule.transform.lossyScale.y - 2 * radius;
        if (height < 0) height = 0;

        GL.PushMatrix();
        GL.MultMatrix(Matrix4x4.TRS(center, Quaternion.identity, Vector3.one));

        GL.Begin(GL.LINES);
        for (int i = 0; i < 360; i += 10)
        {
            float rad = Mathf.Deg2Rad * i;
            float radNext = Mathf.Deg2Rad * (i + 10);

            Vector3 p1 = new Vector3(Mathf.Cos(rad) * radius, height / 2, Mathf.Sin(rad) * radius);
            Vector3 p2 = new Vector3(Mathf.Cos(radNext) * radius, height / 2, Mathf.Sin(radNext) * radius);
            Vector3 p3 = new Vector3(Mathf.Cos(rad) * radius, -height / 2, Mathf.Sin(rad) * radius);
            Vector3 p4 = new Vector3(Mathf.Cos(radNext) * radius, -height / 2, Mathf.Sin(radNext) * radius);

            GL.Vertex(p1);
            GL.Vertex(p2);
            GL.Vertex(p3);
            GL.Vertex(p4);
            GL.Vertex(p1);
            GL.Vertex(p3);
            GL.Vertex(p2);
            GL.Vertex(p4);

            GL.Vertex(new Vector3(Mathf.Cos(rad) * radius, height / 2, Mathf.Sin(rad) * radius));
            GL.Vertex(new Vector3(Mathf.Cos(rad) * radius, -height / 2, Mathf.Sin(rad) * radius));
            GL.Vertex(new Vector3(Mathf.Cos(radNext) * radius, height / 2, Mathf.Sin(radNext) * radius));
            GL.Vertex(new Vector3(Mathf.Cos(radNext) * radius, -height / 2, Mathf.Sin(radNext) * radius));
        }
        GL.End();

        GL.PopMatrix();
    }

    private void DrawMeshCollider(MeshCollider meshCollider)
    {
        if (meshCollider.sharedMesh == null) return;

        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        GL.PushMatrix();
        GL.MultMatrix(meshCollider.transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        for (int i = 0; i < triangles.Length; i += 3)
        {
            GL.Vertex(vertices[triangles[i]]);
            GL.Vertex(vertices[triangles[i + 1]]);
            GL.Vertex(vertices[triangles[i + 1]]);
            GL.Vertex(vertices[triangles[i + 2]]);
            GL.Vertex(vertices[triangles[i + 2]]);
            GL.Vertex(vertices[triangles[i]]);
        }
        GL.End();
        GL.PopMatrix();
    }
}
