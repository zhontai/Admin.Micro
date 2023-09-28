namespace ZhonTai.Module.Admin.Contracts.Events;

public class SmsSendCodeEvent
{
    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 文本
    /// </summary>
    public string Text { get; set; }

}