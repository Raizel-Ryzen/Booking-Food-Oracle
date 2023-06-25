using System.ComponentModel;

namespace Domain.Enums;

public enum ProductOrderBy
{
    [Description("BoughtDesc")]
    BoughtDesc,
    [Description("BoughtAsc")]
    BoughtAsc,
    [Description("AmountDesc")]
    AmountDesc,
    [Description("AmountAsc")]
    AmountAsc,
    [Description("CreatedAtDesc")]
    CreatedAtDesc,
}