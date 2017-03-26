using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortonCellViewer : MonoBehaviour
{
    #region Variables
    private float _left;
    public float Left
    {
        get
        {
            return _left;
        }
        set
        {
            _left = value;
            _width = _right - _left;
            UpdateCells();
        }
    }

    private float _right;
    public float Right
    {
        get
        {
            return _right;
        }
        set
        {
            _right = value;
            _width = _right - _left;
            UpdateCells();
        }
    }

    private float _top;
    public float Top
    {
        get
        {
            return _top;
        }
        set
        {
            _top = value;
            _height = _top - _bottom;
            UpdateCells();
        }
    }

    private float _bottom;
    public float Bottom
    {
        get
        {
            return _bottom;
        }
        set
        {
            _bottom = value;
            _height = _top - _bottom;
            UpdateCells();
        }
    }

    private float _front;
    public float Front
    {
        get
        {
            return _front;
        }
        set
        {
            _front = value;
            _depth = _back - _front;
            UpdateCells();
        }
    }

    private float _back;
    public float Back
    {
        get
        {
            return _back;
        }
        set
        {
            _back = value;
            _depth = _back - _front;
            UpdateCells();
        }
    }

    private float _width;
    private float _height;
    private float _depth;

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
        _unitWidth = _width / Division;
        _unitHeight = _height / Division;
        _unitDepth = _depth / Division;

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

        Vector3 tow = transform.right * _width;
        Vector3 toh = transform.up * _height;
        Vector3 tod = transform.forward * _depth;

        // XY平面
        for (int i = 0; i <= Division; i++)
        {
            for (int j = 0; j <= Division; j++)
            {
                bool isCenter = (i == _halfDivision || j == _halfDivision);
                Gizmos.color = isCenter ? _centerColor : _normalColor;

                Vector3 offset = (transform.right * (_unitWidth * i + _left)) + (transform.up * (_unitHeight * j + _bottom)) + (transform.forward * _front);
                Vector3 from = transform.position + offset;
                Vector3 to = from + tod;
                Gizmos.DrawLine(from, to);
            }
        }

        // YZ平面
        for (int i = 0; i <= Division; i++)
        {
            for (int j = 0; j <= Division; j++)
            {
                bool isCenter = (i == _halfDivision || j == _halfDivision);
                Gizmos.color = isCenter ? _centerColor : _normalColor;

                Vector3 offset = (transform.forward * (_unitDepth * i + _front)) + (transform.up * (_unitHeight * j + _bottom)) + (transform.right * _left);
                Vector3 from = transform.position + offset;
                Vector3 to = from + tow;
                Gizmos.DrawLine(from, to);
            }
        }

        // XZ平面
        for (int i = 0; i <= Division; i++)
        {
            for (int j = 0; j <= Division; j++)
            {
                bool isCenter = (i == _halfDivision || j == _halfDivision);
                Gizmos.color = isCenter ? _centerColor : _normalColor;

                Vector3 offset = (transform.forward * (_unitDepth * i + _front)) + (transform.right * (_unitWidth * j + _left)) + (transform.up * _bottom);
                Vector3 from = transform.position + offset;
                Vector3 to = from + toh;
                Gizmos.DrawLine(from, to);
            }
        }
    }
}
