# ObjectPool
ObjectPool implementation based on unity
基于Unity的对象池Demo.
该例子中尝试使用一个Mgr来进行全局管理,每个对象池都有自己的ID，这样出现问题的时候大多只会出现在对象池自己身上,用户不太需要关注自己是不是使用正确了
用户只需要用好Create,Clear以及释放掉pool的引用即可,这样就可以把锅甩给对象池功能本身,出现问题的时候就不需要两头一起看了
