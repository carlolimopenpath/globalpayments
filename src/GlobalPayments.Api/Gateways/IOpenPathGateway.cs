using GlobalPayments.Api.Builders;

namespace GlobalPayments.Api.Gateways
{
    public interface IOpenPathGateway
    {
        string OpenPathApiKey { get; set; }
        string OpenPathApiUrl { get; set; }
    }
}
