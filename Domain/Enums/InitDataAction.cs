using System.ComponentModel;

namespace Domain.Enums;

public enum InitDataAction
{
    [Description("AppRole")]
    AppRole,
    [Description("AppUser")]
    AppUser,
    [Description("AppCategory")]
    AppCategory,
    [Description("AppPaymentMethod")]
    AppPaymentMethod,
    [Description("AppExternalEcommerce")]
    AppExternalEcommerce,
    [Description("AppCity")]
    AppCity,
    [Description("AppUnit")]
    AppUnit,
    [Description("AppAll")]
    AppAll,
}