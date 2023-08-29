
/// <summary>
/// 用于描述对象池物体的基本接口,可根据需要进行拓展
/// </summary>

public interface IPoolable
{
    //初始化时使用
    void Init();
    
    //显示时使用
    void Show();
    
    //隐藏时使用
    void Hide();
    
    //销毁时使用
    void Destroy();
}