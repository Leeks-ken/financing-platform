﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Account;

public class VerifyLinkLoginTokenInput
{
    [Required]
    public Guid UserId { get; set; }

    public Guid? TenantId { get; set; }

    [Required]
    public string Token { get; set; }
}
