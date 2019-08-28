using GlobalPayments.Api.Entities;
using GlobalPayments.Api.PaymentMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalPayments.Api.Tests.Realex
{
    [TestClass]
    public class OpenPathTest
    {
        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void CreditCard_Payment()
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
                OpenPathApiUrl = "http://localhost:35808/v1/globalpayments"
                // OpenPathApiKey = "VgSsh3Vh24DuwjuU3fsvccZ7CSWPZQ9EaV9K6xuE",
                // OpenPathApiUrl = "https://staging-api.openpath.io/v1/globalpayments"
            });

            var creditCard = new CreditCardData
            {
                CardHolderName = "Jason Martin",
                CardPresent = false,
                Cvn = "2222",
                ExpMonth = 3,
                ExpYear = 2022,
                Number = "4111111111111111"
            };

            // build the address
            var address = new Address()
            {
                City = "Lake Forest",
                Country = "United States",
                CountryCode = "US",
                PostalCode = "92618",
                State = "CA",
                StreetAddress1 = "1707 Enterprice Way"
            };

            // build transaction
            var transaction = creditCard
                .Charge(19.95m)
                .WithAddress(address)
                .WithCurrency("USD")
            .OpenPathValidation();
            var result = transaction.Execute();
        }
    }
}
