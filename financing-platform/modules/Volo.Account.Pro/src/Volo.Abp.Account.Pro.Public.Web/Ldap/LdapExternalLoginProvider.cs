using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Feature;
using Volo.Abp.Account.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Ldap;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace Volo.Abp.Account.Public.Web.Ldap;

public class LdapExternalLoginProvider : ExternalLoginProviderBase, ITransientDependency
{
    public const string Name = "Ldap";

    public ILogger<LdapExternalLoginProvider> Logger { get; set; }

    protected OpenLdapManager LdapManager { get; }

    protected ILdapSettingProvider LdapSettingProvider { get; }

    protected IFeatureChecker FeatureChecker { get; }

    protected ISettingProvider SettingProvider { get; }

    public LdapExternalLoginProvider(IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IdentityUserManager userManager,
        IIdentityUserRepository identityUserRepository,
        OpenLdapManager ldapManager,
        ILdapSettingProvider ldapSettingProvider,
        IFeatureChecker featureChecker,
        ISettingProvider settingProvider,
        IOptions<IdentityOptions> identityOptions)
            : base(guidGenerator,
                currentTenant,
                userManager,
                identityUserRepository,
                identityOptions)
    {
        LdapManager = ldapManager;
        LdapSettingProvider = ldapSettingProvider;
        FeatureChecker = featureChecker;
        SettingProvider = settingProvider;

        Logger = NullLogger<LdapExternalLoginProvider>.Instance;
    }

    public async override Task<bool> TryAuthenticateAsync(string userName, string plainPassword)
    {
        Logger.LogInformation("Try to use LDAP for external authentication");

        if (!await FeatureChecker.IsEnabledAsync(AccountFeature.EnableLdapLogin))
        {
            Logger.LogWarning("Ldap login feature is not enabled!");
            return false;
        }

        if (!await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLdapLogin))
        {
            Logger.LogWarning("Ldap login setting is not enabled!");
            return false;
        }

        return await LdapManager.AuthenticateAsync(await NormalizeUserNameAsync(userName), plainPassword);
    }

    protected async override Task<ExternalLoginUserInfo> GetUserInfoAsync(string userName)
    {
        var email = await LdapManager.GetUserEmailAsync(userName);
        if (email.IsNullOrWhiteSpace())
        {
            throw new Exception("Unable to get the email of ldap user!");
        }
        return new ExternalLoginUserInfo(email);
    }

    protected virtual async Task<string> NormalizeUserNameAsync(string userName)
    {
        return $"uid={userName}, {await LdapSettingProvider.GetBaseDcAsync()}";
    }
}
