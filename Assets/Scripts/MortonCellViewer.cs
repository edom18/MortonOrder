using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortonCellViewer : MonoBehaviour
{
    #region Variables
    private float _width;
    public float Width
    {
        get
        {
            return _width;
        }
        set
        {
            _width = value;
            UpdateCells();
        }
    }

    private float _height;
    public float Height
    {
        get
        {
            return _height;
        }
        set
        {
            _height = value;
            UpdateCells();
        }
    }


    private float _depth;
    public float Depth
    {
        get
        {
            return _depth;
        }
        set
        {
            _depth = value;
            UpdateCells();
        }
    }

    private int _division;
    public int Division
    {
        get
        {
            return _division;
        }
        set
        {
            _division = value;
            UpdateCells();
        }
    }

    private float _unitWidth;
    private float _unitHeight;
    private float _unitDepth;
    private int _halfDivision;

    private Color _normalColor = new Color(1f, 0, 0, 0.5f);
    private Color _centerColor = new Color(0, 0, 1f, 1f);
    #endregion Variables

    void Start()
    {
        UpdateCells();
    }

    void UpdateCells()
    {
        // ひとつの区間の単位
        _unitWidth = Width / Division;
        _unitHeight = Height / Division;
        _unitDepth = Depth / Division;

        _halfDivision = Division / 2;
    }

    /// <summary>
    /// On draw gizomos.
    /// </summary>
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        Vector3 tow = transform.right * Width;
        Vector3 toh = transform.up * Height;
        Vector3 tod = transform.forward * Depth;

        for (int i = 0; i <= Division; i++)
        {
            for (int j = 0; j <= Division; j++)
            {
                if (i == _halfDivision || j == _halfDivision)
                {
                    Gizmos.color = _centerColor;
                }
                else
                {
                    Gizmos.color = _normalColor;
                }
                Vector3 offset = (transform.right * _unitWidth * i) + (transform.up * _unitHeight * j);
                Vector3 from = transform.position + offset;
                Vector3 to = from + tod;
                Gizmos.DrawLine(from, to);
            }
        }

        for (int i = 0; i <= Division; i++)
        {
            for (int j = 0; j <= Division; j++)
            {
                if (i == _halfDivision || j == _halfDivision)
                {
                    Gizmos.color = _centerColor;
                }
                else
                {
                    Gizmos.color = _normalColor;
                }
                Vector3 offset = (transform.forward * _unitDepth * i) + (transform.up * _unitHeight * j);
                Vector3 from = transform.position + offset;
                Vector3 to = from + tow;
                Gizmos.DrawLine(from, to);
            }
        }

        for (int i = 0; i <= Division; i++)
        {
            for (int j = 0; j <= Division; j++)
            {
                if (i == _halfDivision || j == _halfDivision)
                {
                    Gizmos.color = _centerColor;
                }
                else
                {
                    Gizmos.color = _normalColor;
                }
                Vector3 offset = (transform.forward * _unitDepth * i) + (transform.right * _unitWidth * j);
                Vector3 from = transform.position + offset;
                Vector3 to = from + toh;
                Gizmos.DrawLine(from, to);
            }
        }
    }
}
