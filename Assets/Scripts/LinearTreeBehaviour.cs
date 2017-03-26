using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearTreeBehaviour : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private int _level = 3;

    [SerializeField]
    private float _left = 0f;

    [SerializeField]
    private float _top = 0f;

    [SerializeField]
    private float _right = 10f;

    [SerializeField]
    private float _bottom = 10f;

    [SerializeField]
    private float _front = 0f;

    [SerializeField]
    private float _back = 10f;
    #endregion SerializeField

    #region Variables
    private LinearTreeManager<GameObject> _manager;
    private MortonCellViewer _cellViewer;
    private MortonCellViewer CellViewer
    {
        get
        {
            if (_cellViewer == null)
            {
                MortonCellViewer viewer = GetComponent<MortonCellViewer>();
                if (viewer == null)
                {
                    viewer = gameObject.AddComponent<MortonCellViewer>();
                }
                _cellViewer = viewer;
            }
            return _cellViewer;
        }
    }

    [SerializeField]
    private GameObject _object1;

    [SerializeField]
    private GameObject _object2;

    [SerializeField]
    private GameObject _object3;

    [SerializeField]
    private GameObject _object4;
    #endregion Variables


    #region MonoBehaviour
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        CellViewer.Left = _left;
        CellViewer.Right = _right;
        CellViewer.Top = _top;
        CellViewer.Bottom = _bottom;
        CellViewer.Front = _front;
        CellViewer.Back = _back;
        CellViewer.Division = 1 << _level;
    }

    void Awake()
    {
        CellViewer.Left = _left;
        CellViewer.Right = _right;
        CellViewer.Top = _top;
        CellViewer.Bottom = _bottom;
        CellViewer.Front = _front;
        CellViewer.Back = _back;
        CellViewer.Division = 1 << _level;
    }

	void Start()
    {
        _manager = new LinearTreeManager<GameObject>(_level, _left, _top, _right, _bottom, _front, _back);

        // オブジェクトを仮登録してみる
        RegisterObjects();

        // Check collisions
        List<GameObject> collisionList = new List<GameObject>();
       _manager.GetAllCollisionList(collisionList);
	}
    #endregion MonoBehaviour

    /// <summary>
    /// ゲームオブジェクトを登録する
    /// </summary>
    /// <param name="target">ターゲットのゲームオブジェクト</param>
    void RegisterObject(GameObject target)
    {
        TreeData<GameObject> data = new TreeData<GameObject>(target);
        MeshFilter filter = target.GetComponent<MeshFilter>();
        Bounds bounds = filter.mesh.bounds;

        _manager.Register(bounds, data);
    }

    void UnregisterObject(GameObject target)
    {

    }

    /// <summary>
    /// オブジェクトを仮に登録してみる
    /// </summary>
    void RegisterObjects()
    {
        TreeData<GameObject> data1 = new TreeData<GameObject>(_object1);
        TreeData<GameObject> data2 = new TreeData<GameObject>(_object2);
        TreeData<GameObject> data3 = new TreeData<GameObject>(_object3);
        TreeData<GameObject> data4 = new TreeData<GameObject>(_object4);

        //RegisterObject(_object1);

        // オブジェクトを登録のテスト
        //_manager.Register(10f, 15f, 40f, 45f, 0f, 30f, data1);
        //_manager.Register(45f, 30f, 55f, 40f, 0f, 10f, data2);
        //_manager.Register(76f, 26f, 86f, 36f, 13f, 23f, data3);
        //_manager.Register(55f, 55f, 70f, 70f, 0f, 15f, data4);
        _manager.Register(-40f, 15f, -10f, 45f, 0f, 30f, data1);
        _manager.Register(-5f, 30f, 5f, 40f, 0f, 10f, data2);
        _manager.Register(26f, 26f, 36f, 36f, 13f, 23f, data3);
        _manager.Register(5f, 55f, 20f, 70f, 0f, 15f, data4);
    }
}
