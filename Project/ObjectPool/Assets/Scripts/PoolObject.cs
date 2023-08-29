using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该类为对象池物体的基类,实现了IPoolable的功能接口
/// 用户可根据自己需求进行接口以及该类的拓展
/// </summary>

public class PoolObject : MonoBehaviour, IPoolable
{
    public enum State
    {
        Idle = 1,
        InUse = 2,
        Destroyed = 3,
    }

    private State m_State;
    public bool IsUsing()
    {
        return m_State == State.InUse;
    }

    public bool IsDestroyed()
    {
        return m_State == State.Destroyed;
    }
    
    //刚创建时候的初始化操作
    public void Init()
    {
        m_State = State.Idle;
    }

    //取出时使用时的操作
    public void Show()
    {
        m_State = State.InUse;
    }

    //放回时的操作
    public void Hide()
    {
        m_State = State.Idle;
    }

    public void Destroy()
    {
        m_State = State.Destroyed;
        GameObject.Destroy(gameObject);
    }
}
