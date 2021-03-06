using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Volo.Abp.Account.Public.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;

public class ConfirmPhoneNumberModalModel : AccountPageModel
{
    private readonly IAccountAppService _accountAppService;

    [BindProperty]
    [Required]
    public string PhoneConfirmationToken { get; set; }

    public ConfirmPhoneNumberModalModel(IAccountAppService accountAppService)
    {
        _accountAppService = accountAppService;
    }

    public virtual async Task OnGetAsync()
    {
        if (!await SettingProvider.GetAsync(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation, true))
        {
            throw new BusinessException("Volo.Account:PhoneNumberConfirmationDisabled"); //TODO: Localize!
        }

        await _accountAppService.SendPhoneNumberConfirmationTokenAsync(new SendPhoneNumberConfirmationTokenDto
        {
            UserId = CurrentUser.GetId()
        });
    }

    public virtual async Task OnPostAsync()
    {
        await _accountAppService.ConfirmPhoneNumberAsync(new ConfirmPhoneNumberInput
        {
            UserId = CurrentUser.GetId(),
            Token = PhoneConfirmationToken
        });
    }
}
