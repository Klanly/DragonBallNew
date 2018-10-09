引导观察者使用说明
1.创建一个类(例: JCReceiver),继承EventSender,并override OnEvent函数
2.打开GuideListener.cs 在registered中实例观察者对象, 并在该类中添加对象返回函数
3.各观察者OnEvent会自动收到消息,请实现具体功能
4.所有的消息在 EventTypeDefine 类中定义,请保持关注

