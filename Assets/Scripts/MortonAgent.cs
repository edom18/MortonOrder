using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An agent will be in octree.
/// </summary>
public class MortonAgent : MonoBehaviour
{
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
    #endregion MonoBehaviour

    public void UpdateAgent()
    {

    }
}
