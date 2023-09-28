namespace ZhonTai.Module.Admin.Contracts.Http;

public class ViewSyncRequest
{
    public static class Models
    {
        public class ViewSync
        {
            /// <summary>
            /// 视图命名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 地址
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// 视图名称
            /// </summary>
            public string Label { get; set; }

            /// <summary>
            /// 说明
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// 缓存
            /// </summary>
            public bool Cache { get; set; } = true;
        }
    }

    public List<Models.ViewSync> Views { get; set; }
}