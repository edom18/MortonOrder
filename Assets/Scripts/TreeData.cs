using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 4分木に登録されるデータオブジェクト
///
/// 登録される先の空間（Cell）情報と、
/// 操作対象のオブジェクト、
/// そしてリンクリストのため前後の同関連データへの参照保持する
/// </summary>
/// <typeparam name="T">管理対象のオブジェクトの型</typeparam>
public class TreeData<T>
{
    public Cell<T> Cell { get; set; }
    public T Object { get; private set; }
    public TreeData<T> Previous { get; set; }
    public TreeData<T> Next { get; set; }

    // コンストラクタ
    public TreeData(T target)
    {
        Object = target;
    }

    /// <summary>
    /// 空間から逸脱する
    /// </summary>
    /// <returns>成功: true, 失敗: false</returns>
    public bool Remove()
    {
        // すでに逸脱している場合は処理しない
        if (Cell == null)
        {
            return false;
        }

        // 逸脱を空間に伝える
        if (!Cell.OnRemove(this))
        {
            return false;
        }

        // 逸脱処理
        // リンクリストの前後をつなぎ、自身のリンクを外す
        if (Previous != null)
        {
            Previous.Next = Next;
        }

        if (Next != null)
        {
            Next.Previous = Previous;
        }

        Previous = null;
        Next = null;
        Cell = null;

        return true;
    }
}
