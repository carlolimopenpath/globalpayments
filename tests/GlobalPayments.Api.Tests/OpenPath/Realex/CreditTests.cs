using GlobalPayments.Api.PaymentMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalPayments.Api.Tests.OpenPath.Realex
{

    [TestClass]
    public class CreditTests
    {
        CreditCardData card;

        [TestInitialize]
        public void Init()
        {

            ServicesContainer.ConfigureService(new GatewayConfig
            {
                MerchantId = "heartlandgpsandbox",
                AccountId = "api",
                SharedSecret = "secret",
                RebatePassword = "rebate",
                RefundPassword = "refund",
                ServiceUrl = "https://api.sandbox.realexpayments.com/epage-remote.cgi",
                OpenPathApiKey = "fteaWC5MYeVKdZ2EaQASDDgCtAS4Fh2zrzW4Yyds",
                OpenPathApiUrl = "https://localhost:44376/v1/globalpayments"
            });

            card = new CreditCardData
            {
                Number = "4111111111111111",
                ExpMonth = 12,
                ExpYear = 2025,
                Cvn = "123",
                CardHolderName = "Carlo Lim"
            };
        }


        [TestMethod]
        public void CreditAuthorization()
        {
            var authorization = card.Authorize(14m)
                .WithCurrency("USD")
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(authorization);
            Assert.AreEqual("00", authorization.ResponseCode, authorization.ResponseMessage);

            var capture = authorization.Capture(14m)
                .Execute();
            Assert.IsNotNull(capture);
            Assert.AreEqual("00", capture.ResponseCode, capture.ResponseMessage);
        }


    }
}
