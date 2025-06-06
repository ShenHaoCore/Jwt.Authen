﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Jwt.Authen.Api.Models;

/// <summary>
/// 
/// </summary>
public class UpdateOrderModel
{
    /// <summary>
    /// 产品名称
    /// </summary>
    [property: Required]
    [property: DefaultValue("手机")]
    [property: Description("产品名称")]
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 价格
    /// </summary>
    [property: Required]
    [property: DefaultValue(typeof(decimal), "100.00")]
    [property: Description("价格")]
    public decimal Price { get; set; }
}
