using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearTreeController : MonoBehaviour
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
        MortonAgent agent = target.GetComponent<MortonAgent>();
        if (agent == null)
        {
            Debug.LogWarningFormat("Augument must have a `MortonAgent` component. {0}", target);
            return;
        }

        agent.Manager = _manager;

        //_manager.Register(agent.Bounds, agent.TreeData);
    }

    void UnregisterObject(GameObject target)
    {

    }

    /// <summary>
    /// オブジェクトを仮に登録してみる
    /// </summary>
    void RegisterObjects()
    {
        RegisterObject(_object1);
        RegisterObject(_object2);
        RegisterObject(_object3);
        RegisterObject(_object4);

        #region For Mock
        //TreeData<GameObject> data1 = new TreeData<GameObject>(_object1);
        //TreeData<GameObject> data2 = new TreeData<GameObject>(_object2);
        //TreeData<GameObject> data3 = new TreeData<GameObject>(_object3);
        //TreeData<GameObject> data4 = new TreeData<GameObject>(_object4);

        //// size: 3
        //_manager.Register(-4f, 4.5f, -1f, 1.5f, -5f, -2f, data1);

        //// size: 1
        //_manager.Register(-0.5f, 4f, 0.5f, 3f, -5f, -4f, data2);

        //// size: 1
        //_manager.Register(2.6f, 3.6f, 3.6f, 2.6f, -3.7f, -2.7f, data3);

        //// size: 1.5
        //_manager.Register(0.5f, 7f, 2f, 5.5f, -5f, -3.5f, data4);
        #endregion For Mock
    }
}
