using Microsoft.AspNetCore.Authorization;
using System;

namespace NotificationService.Api.Authentication;

/// <summary>
/// This attribute applies the API Key authorization policy.
/// It MUST inherit from a class that derives from System.Attribute.
/// AuthorizeAttribute already does this, so inheriting from it makes this a valid attribute class.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : AuthorizeAttribute
{
    public ApiKeyAttribute()
    {
        Policy = ApiKeyAuthenticationOptions.PolicyName;
    }
}
