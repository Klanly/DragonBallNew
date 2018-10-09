using System;

[Serializable]
public class PayInfo 
{
	// 必需参数，用户access token，要使用注意过期和刷新问题，最大64字符。
	public string accessToken;

	// 必需参数，360账号id，整数。
	public string qihooUserId;

	// 必需参数，所购买商品金额，以分为单位。金额大于等于100分，360SDK运行定额支付流程； 金额数为0，360SDK运行不定额支付流程。
	public string moneyAmount;

	// 必需参数，人民币与游戏充值币的默认比例，例如2，代表1元人民币可以兑换2个游戏币，整数。
	public string exchangeRate;

	// 必需参数，所购买商品名称，应用指定，建议中文，最大10个中文字。
	public string productName;

	// 必需参数，购买商品的商品id，应用指定，最大16字符。
	public string productId;

	// 必需参数，应用方提供的支付结果通知uri，最大255字符。360服务器将把支付接口回调给该uri，具体协议请查看文档中，支付结果通知接口–应用服务器提供接口。
	public string notifyUri;

	// 必需参数，游戏或应用名称，最大16中文字。
	public string appName;

	// 必需参数，应用内的用户名，如游戏角色名。 若应用内绑定360账号和应用账号，则可用360用户名，最大16中文字。（充值不分区服，
	// 充到统一的用户账户，各区服角色均可使用）。
	public string appUserName;

	// 必需参数，应用内的用户id。 若应用内绑定360账号和应用账号, 充值不分区服, 充到统一的用户账户, 各区服角色均可使用,
	// 则可用360用户ID。最大32字符。
	public string appUserId;

	// 可选参数，应用扩展信息1，原样返回，最大255字符。
	public string appExt1;

	// 可选参数，应用扩展信息2，原样返回，最大255字符。
	public string appExt2;

	// 可选参数，应用订单号，应用内必须唯一，最大32字符。
	public string appOrderId;

	public PayInfo() {}
}
