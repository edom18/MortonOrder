using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearTreeBehaviour : MonoBehaviour
{
    private LinearTreeManager<GameObject> _manager;

    [SerializeField]
    private GameObject _object1;

    [SerializeField]
    private GameObject _object2;

    [SerializeField]
    private GameObject _object3;

    [SerializeField]
    private GameObject _object4;

	void Start()
    {
        // Max Level
        int level = 3;

        float left = 0f;
        float top = 0f;
        float right = 100f;
        float bottom = 100f;
        _manager = new LinearTreeManager<GameObject>(level, left, top, right, bottom);

        // オブジェクトを仮登録してみる
        RegisterObjects();
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

        // オブジェクトを登録のテスト
        _manager.Register(10f, 15f, 40f, 45f, data1);
        _manager.Register(45f, 30f, 55f, 40f, data2);
        _manager.Register(76f, 26f, 86f, 36f, data3);
        _manager.Register(55f, 55f, 70f, 70f, data4);
    }
}
