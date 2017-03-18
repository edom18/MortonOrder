using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearTreeBehaviour : MonoBehaviour
{
    private LinearTreeManager<GameObject> _manager;

	void Start()
    {
        int level = 4;
        float left = 10f;
        float top = 10f;
        float right = 100f;
        float bottom = 100f;
        _manager = new LinearTreeManager<GameObject>(level, left, top, right, bottom);
	}
	
	void Update()
    {
		
	}
}
