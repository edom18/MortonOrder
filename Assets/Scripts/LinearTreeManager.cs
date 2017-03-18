using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 線形4分木管理をする
/// </summary>
public class LinearTreeManager<T>
{
    #region Variables
    // 分割最大数
    private readonly int _MaxLevel = 4;

    private int[] _pow;

    private Cell<T>[] _cellList;

    // 分割されたセル数の最大値
    private int _cellNum = 0;

    private int _level;

    private float _left;
    private float _top;
    private float _width;
    private float _height;
    private float _unitWidth;
    private float _unitHeight;
    #endregion Variables

    // コンストラクタ
    public LinearTreeManager(int level, float left, float top, float right, float bottom)
    {
        // 初期化
        Initialize(level, left, top, right, bottom);
    }


    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="level">分割レベル</param>
    /// <param name="left">左の座標</param>
    /// <param name="top">上の座標</param>
    /// <param name="right">右の座標</param>
    /// <param name="bottom">下の座標</param>
    /// <returns>初期化できたらtrue</returns>
    bool Initialize(int level, float left, float top, float right, float bottom)
    {
        // TODO: このあたりの配列数確保は要確認

        // MaxLevelを超えて初期化しようとした場合はエラー
        if (level > _MaxLevel + 1)
        {
            Debug.LogErrorFormat("Must be a level under the max level. [MAX: {0}]", _MaxLevel);
            return false;
        }

        // MaxLevelに応じた配列を作成
        _pow = new int[_MaxLevel + 1];

        // 各レベルでの空間数を算出
        // ルートは1、その子は4、さらにその子（孫）は16、と4^nで増えていく
        _pow[0] = 1;
        for (int i = 1; i < _MaxLevel + 1; i++)
        {
            _pow[i] = _pow[i - 1] * 4;
        }

        // level（0基点）の配列作成
        // e.g.)
        // 0レベル（ルート）ならセル数は1
        // 1レベルならセル数は5（(16 - 1) / 3）
        // 2レベルならセル数は21（(64 - 1) / 3）
        _cellNum = (_pow[level] - 1) / 3;
        _cellList = new Cell<T>[_cellNum];

        // 有効領域を登録
        // 左上の座標と幅、高さを保持
        _left = left;
        _top = top;
        _width = right - left;
        _height = bottom - top;

        // 分割数に応じた単位幅と単位高を求める
        // 分割数はlevelを指数とした2の累乗分増えてくため、
        // シフトで求めた数で割ることで単位を求める
        // e.g.)
        // 0レベルなら分割は1、1レベルなら分割は2（2^1）、2レベルなら4（2^2）
        _unitWidth = _width / (1 << level);
        _unitHeight = _height / (1 << level);

        _level = level;

        return true;
    }

    /// <summary>
    /// 指定範囲にオブジェクトを登録
    /// </summary>
    /// <param name="left">オブジェクトの左の点</param>
    /// <param name="top">オブジェクトの上の点</param>
    /// <param name="right">オブジェクトの右の点</param>
    /// <param name="bottom">オブジェクトの下の点</param>
    /// <param name="data">登録データオブジェクト</param>
    /// <returns>登録に成功したらtrue</returns>
    public bool Register(float left, float top, float right, float bottom, TreeData<T> data)
    {
        // オブジェクトの境界範囲からモートン番号を算出
        int belongLevel;
        int elem = GetMortonNumber(left, top, right, bottom, out belongLevel);

        // 算出されたモートン番号が、生成した空間分割数より大きい場合はエラー
        if (elem < _cellNum)
        {
            Debug.LogErrorFormat("Calcurated moton number is over the splited number. [MotonNumber: {0}]", elem);

            // 登録失敗
            return false;
        }

        // 算出されたモートン番号の空間がない場合は作成
        if (_cellList[elem] == null)
        {
            CreateNewCell(elem);
        }

        return _cellList[elem].Push(data);
    }

    /// <summary>
    /// 指定された番号の空間オブジェクトを新規生成
    /// </summary>
    /// <param name="elem">空間番号</param>
    bool CreateNewCell(int elem)
    {
        while (_cellList[elem] == null)
        {
            _cellList[elem] = new Cell<T>();

            // 親空間を作成する（存在していなければ）
            //
            // 親空間の算出は「親番号 = (int)((子番号 - 1) / 4)」で算出できる。
            // e.g.)
            // 5番の親番号の子空間は21 - 24を持つ。
            // 仮に22から計算すると、(22 - 1) / 4 = 5（intにキャスト）
            //
            // 結果として、4で割るということは、2bitシフトしていることに等しいため、（4が1（単位）になる計算）
            // 計算では高速化のためビットシフトで計算する
            elem = (elem - 1) >> 2;

            // 空間分割数以上になったら終了
            if (elem >= _cellNum)
            {
                break;
            }
        }

        return true;
    }

    /// <summary>
    /// モートン番号を算出
    /// </summary>
    /// <param name="left">算出対象オブジェクトの左の点</param>
    /// <param name="top">算出対象オブジェクトの上の点</param>
    /// <param name="right">算出対象オブジェクトの右の点</param>
    /// <param name="bottom">算出対象オブジェクトの下の点</param>
    /// <returns>算出されたモートン番号</returns>
    int GetMortonNumber(float left, float top, float right, float bottom, out int belongLevel)
    {
        // 左上のモートン番号を算出（lt）
        int lt_x = (int)left;
        int lt_y = (int)top;
        int lt = BitSeparate(lt_x) | (BitSeparate(lt_y) << 1);

        // 右下のモートン番号を算出（rb）
        int rb_x = (int)right;
        int rb_y = (int)bottom;
        int rb = BitSeparate(rb_x) | (BitSeparate(rb_y) << 1);

        // 左上と右下のモートン番号のXORを取る
        int xor = lt ^ rb;
        int i = 0;
        int shift = 0;
        int spaceIndex = 0;

        while (xor != 0)
        {
            if ((xor & 0x3) != 0)
            {
                // 空間シフト数を採用
                spaceIndex = (i + 1);
                shift = spaceIndex * 2;
            }

            // 2bitシフトさせて再チェック
            xor >>= 2;
            i++;
        }

        // モートン番号
        int morton = rb >> shift;

        // 所属する空間のレベル
        belongLevel = _level - spaceIndex;

        return morton;
    }

    /// <summary>
    /// 登録されているオブジェクトの衝突リストを取得する
    /// </summary>
    /// <param name="collisionList">衝突リスト（結果が保持される）</param>
    /// <returns>衝突リストのサイズ</returns>
    public int GetAllCollisionList(List<T> collisionList)
    {
        // 結果リストをクリア
        collisionList.Clear();

        // ルート空間の存在をチェック
        if (_cellList[0] == null)
        {
            return 0;
        }

        // ルート空間から衝突チェック開始
        LinkedList<T> colStac = new LinkedList<T>();
        GetCollisionList(0, collisionList, colStac);

        return collisionList.Count;
    }

    /// <summary>
    /// 各セルから全衝突可能リストを作成する
    /// </summary>
    /// <param name="elem">検索を開始する要素のindex</param>
    /// <param name="collisionList">衝突可能性のあるリストを格納する</param>
    /// <param name="colStac">衝突検知用のスタック</param>
    /// <returns><c>true</c>, if collision list was gotten, <c>false</c> otherwise.</returns>
    bool GetCollisionList(int elem, List<T> collisionList, LinkedList<T> colStac)
    {
        // 空間内のオブジェクト同士の衝突リスト作成
        // ルート空間からはじめ、その子空間へと移動しながら、「衝突可能性のある」オブジェクト同士の
        // ペアとなるリストを作成する
        // 結果は「collisionList」に格納される。
        // なお、リストは「ペア」構造となっていて、
        // 完成したリストからはふたつずつ取り出して衝突の詳細判定を行う想定。

        // ルート空間に登録されているリンクリストの最初の要素を取り出す
        TreeData<T> data = _cellList[elem].FirstData;

        // データがなくなるまで繰り返す
        while (data != null)
        {
            // まず、リンクリストの次を取り出す
            TreeData<T> next = data.Next;
            while (next != null)
            {
                // 衝突リスト作成
                collisionList.Add(data.Object);
                collisionList.Add(next.Object);
                next = next.Next;
            }

            // 衝突スタックと衝突リスト作成
            foreach (var obj in colStac)
            {
                collisionList.Add(data.Object);
                collisionList.Add(obj);
            }

            data = data.Next;
        }

        bool child = false;

        // 子空間に移動
        int objNum = 0;
        int nextElem;

        // 小空間を巡る
        // 今回は4分木なので子空間は4分割される
        // つまり、4回ループすることで小空間を網羅する
        for (int i = 0; i < 4; i++)
        {
            nextElem = elem * 4 + 1 + i;

            // 空間分割数以上 or 対象空間がない場合はスキップ
            bool needsSkip = (nextElem >= _cellNum ||
                             _cellList[nextElem] == null);
            if (needsSkip)
            {
                continue;
            }

            // 子空間への処理がまだ済んでいない場合は処理を行う
            // 同空間内のオブジェクトをスタックに追加した上で小空間の衝突リストを作成する
            // ただし、一度セットアップが済んでいる場合は本処理をスキップし、小空間の検索のみを実行する
            // （同空間のオブジェクト追加は一度のみ行う必要があるため）
            if (!child)
            {
                // 同空間のオブジェクトをスタックに積む
                data = _cellList[elem].FirstData;
                while (data != null)
                {
                    colStac.AddLast(data.Object);
                    objNum++;
                    data = data.Next;
                }
            }

            child = true;

            // 子空間を検索
            GetCollisionList(nextElem, collisionList, colStac);
        }

        // スタックからオブジェクトを外す
        // 計測したobjNum個数分、スタックから取り除く（＝子空間検索用に追加した分）
        if (child)
        {
            for (int i = 0; i < objNum; i++)
            {
                colStac.RemoveLast();
            }
        }

        return true;
    }

    #region Static Methods
    /// <summary>
    /// 渡された引数をbitで飛び飛びのものに変換する
    /// </summary>
    /// <param name="n">変換したい値</param>
    /// <returns>変換後の値</returns>
    static int BitSeparate(int n)
    {
        n = (n | (n << 8)) & 0x00ff00ff;
        n = (n | (n << 4)) & 0x0f0f0f0f;
        n = (n | (n << 2)) & 0x33333333;
        return (n | (n << 1)) & 0x55555555;
    }
    #endregion Static Methods
}
