using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An agent will be in octree.
/// </summary>
public class MortonAgent : MonoBehaviour
{
    private LinearTreeManager<GameObject> _manager;
    public LinearTreeManager<GameObject> Manager
    {
        get
        {
            return _manager;
        }
        set
        {
            if (_manager == value)
            {
                return;
            }

            // Remove from current manager.
            TreeData.Remove();

            // Change to new manager and register myself.
            _manager = value;
            RegisterUpdate();
        }
    }

    public TreeData<GameObject> TreeData { get; private set; }

    private Collider _collider;
    // Bounds like AABB of this game object.
    public Bounds Bounds
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
            }
            return _collider.bounds;
        }
    }

    #region MonoBehaviour
    void Awake()
    {
        TreeData = new TreeData<GameObject>(gameObject);
    }

    void OnDestroy()
    {
        TreeData.Remove();
    }

    void Update()
    {
        if (_manager == null)
        {
            return;
        }

        RegisterUpdate();
    }
    #endregion MonoBehaviour

    void RegisterUpdate()
    {
        _manager.Register(Bounds, TreeData);
    }
}
