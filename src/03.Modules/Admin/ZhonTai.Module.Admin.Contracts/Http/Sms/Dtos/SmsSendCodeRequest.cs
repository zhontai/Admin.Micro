using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 发送短信验证码
/// </summary>
public class SmsSendCodeRequest
{
    public static class Models
    {
        public class SlideTrack
        {
            public static class Models
            {
                public class Track
                {
                    public int X { get; set; }

                    public int Y { get; set; }

                    public int T { get; set; }
                }
            }

            public int BackgroundImageWidth { get; set; }

            public int BackgroundImageHeight { get; set; }

            public int SliderImageWidth { get; set; }

            public int SliderImageHeight { get; set; }

            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }

            public List<Models.Track> Tracks { get; set; }
        }
    }

    /// <summary>
    /// 手机号
    /// </summary>
    [Required(ErrorMessage = "请输入手机号")]
    public string Mobile { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    public string? CodeId { get; set; }

    /// <summary>
    /// 验证码Id
    /// </summary>
    [Required(ErrorMessage = "请完成安全验证")]
    public string CaptchaId { get; set; }

    /// <summary>
    /// 滑动轨迹
    /// </summary>
    [Required(ErrorMessage = "请完成安全验证")]
    public Models.SlideTrack Track { get; set; }
}