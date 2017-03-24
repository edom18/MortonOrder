using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CellMesh : MonoBehaviour
{
    [SerializeField]
    private float _width;

    [SerializeField]
    private float _height;

    [SerializeField]
    private float _depth;

    [SerializeField]
    private int _division;

    private Mesh _mesh;

	void Start()
    {
        int x = 2;
        int y = 2;
        int z = 2;

        // セルの単位
        float unit = 0.1f;

        // Mesh頂点
        Vector3[] vert = new Vector3[x * y * z];

        Vector3[] normals = new Vector3[x * y * z];

        // 頂点インデックス
        int[] indcies = new int[(x * y * z) * 3];

        for (int i = 0; i < z; i++)
        {
            for (int j = 0; j < y; j++)
            {
                for (int k = 0; k < x; k++)
                {
                    int index = k + (j * x) + (i * x * y);
                    vert[index] = new Vector3(k * unit, j * unit, i * unit);
                }
            }
        }

        //Vector3[] vert = new[]
        //{
        //    new Vector3(0, 0, 0),
        //    new Vector3(1f, 0, 0),
        //    new Vector3(0, 1f, 0),
        //    new Vector3(1f, 1f, 0),
        //};

        //int[] indcies = new[]
        //{
        //    //0, 1, 2,
        //    //2, 3, 1,
        //    0, 2, 2,
        //    2, 3, 3,
        //    3, 1, 1,
        //    1, 0, 0,
        //};

        //Vector3[] normals = new[]
        //{
        //    new Vector3(0, 1f, 0),
        //    new Vector3(0, 1f, 0),
        //    new Vector3(0, 1f, 0),
        //    new Vector3(0, 1f, 0),
        //};

        _mesh = new Mesh();
        _mesh.vertices = vert;
        _mesh.triangles = indcies;
        _mesh.normals = normals;
	}

    void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1f, 0, 0, 0.5f);
        //Gizmos.DrawWireMesh(_mesh, transform.position);

        Color normalColor = new Color(1f, 0, 0, 0.5f);
        Color centerColor = new Color(0, 0, 1f, 1f);

        // ひとつの区間の単位
        float uw = _width / _division;
        float uh = _height / _division;
        float ud = _depth / _division;

        Vector3 tow = transform.right * _width;
        Vector3 toh = transform.up * _height;
        Vector3 tod = transform.forward * _depth;

        int halfDivision = _division / 2;

        for (int i = 0; i <= _division; i++)
        {
            for (int j = 0; j <= _division; j++)
            {
                if (i == halfDivision || j == halfDivision)
                {
                    Gizmos.color = centerColor;
                }
                else
                {
                    Gizmos.color = normalColor;
                }
                Vector3 offset = (transform.right * uw * i) + (transform.up * uh * j);
                Vector3 from = transform.position + offset;
                Vector3 to = from + tod;
                Gizmos.DrawLine(from, to);
            }
        }

        for (int i = 0; i <= _division; i++)
        {
            for (int j = 0; j <= _division; j++)
            {
                if (i == halfDivision || j == halfDivision)
                {
                    Gizmos.color = centerColor;
                }
                else
                {
                    Gizmos.color = normalColor;
                }
                Vector3 offset = (transform.forward * ud * i) + (transform.up * uh * j);
                Vector3 from = transform.position + offset;
                Vector3 to = from + tow;
                Gizmos.DrawLine(from, to);
            }
        }

        for (int i = 0; i <= _division; i++)
        {
            for (int j = 0; j <= _division; j++)
            {
                if (i == halfDivision || j == halfDivision)
                {
                    Gizmos.color = centerColor;
                }
                else
                {
                    Gizmos.color = normalColor;
                }
                Vector3 offset = (transform.forward * ud * i) + (transform.right * uw * j);
                Vector3 from = transform.position + offset;
                Vector3 to = from + toh;
                Gizmos.DrawLine(from, to);
            }
        }
    }
}
