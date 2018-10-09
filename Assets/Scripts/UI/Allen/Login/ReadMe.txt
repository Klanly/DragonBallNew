如何扩展登陆-主要是各种第三方的平台的登陆

1. 在loginmethod里面，继承IGetUniqueID，实现自己的登陆方式
2. 在GetUniqueIDFactory里面添加相应的实例化代码，生成IGetUniqueID的子类
3. 在LoginView里面的onButtonClick里去调用GetUniqueIDFactory.createInstance()，这里可决定实例化的参数

使用的是策略模式 和 简单工厂

   

