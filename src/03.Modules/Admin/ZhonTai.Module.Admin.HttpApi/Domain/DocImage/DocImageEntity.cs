using ZhonTai.Api.Core.Entities;
using FreeSql.DataAnnotations;
using ZhonTai.Module.Admin.HttpApi.Domain.Doc;

namespace ZhonTai.Module.Admin.HttpApi.Domain.DocImage;

/// <summary>
/// 文档图片
/// </summary>
[Table(Name = "ad_doc_image", OldName = "ad_document_image")]
[Index("idx_{tablename}_01", nameof(DocId) + "," + nameof(Url), true)]
public class DocImageEntity : EntityAdd
{
    /// <summary>
    /// 文档Id
    /// </summary>
    public long DocId { get; set; }

    public DocEntity Doc { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string Url { get; set; }
}