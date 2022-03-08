using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Sms;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using Microsoft.Extensions.Options;
using Volo.Abp.Account;

namespace Tank.Financing;

public class AlibabaSmsSender : ISmsSender, ITransientDependency
{
    public SmsOptions Options { get; }

    public AlibabaSmsSender(IOptionsSnapshot<SmsOptions> options)
    {
        Options = options.Value;
    }

    public async Task SendAsync(SmsMessage smsMessage)
    {
        var client = CreateClient(Options.AccessKeyId, Options.AccessKeySecret, Options.EndPoint);
        var response = await client.SendSmsAsync(new SendSmsRequest
        {
            PhoneNumbers = smsMessage.PhoneNumber,
            SignName = Options.SignName,
            TemplateCode = Options.TemplateCode,
            TemplateParam = "{\"code\":\"1234\"}"
        });
    }

    private static Client CreateClient(string accessKeyId, string accessKeySecret, string endpoint)
    {
        return new Client(new AlibabaCloud.OpenApiClient.Models.Config
        {
            AccessKeyId = accessKeyId,
            AccessKeySecret = accessKeySecret,
            Endpoint = endpoint
        });
    }
}