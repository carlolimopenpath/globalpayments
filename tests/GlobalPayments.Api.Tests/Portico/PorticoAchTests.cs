using GlobalPayments.Api.Entities;
using GlobalPayments.Api.PaymentMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GlobalPayments.Api.Tests {
    [TestClass]
    public class PorticoAchTests
    {
        eCheck check;
        Address address;
        eCheck activateCard1;
        eCheck activateCard2;
        eCheck activateCard3;


        [TestInitialize]
        public void Init() {
            ServicesContainer.ConfigureService(new GatewayConfig {
                SecretApiKey = "skapi_cert_MTYcAgCxWWEAp8AlBkChNfvTSWWkjf-nkAw2cdoijg",
                ServiceUrl = "https://cert.api2.heartlandportico.com",
                // OpenPathApiKey = "fteaWC5MYeVKdZ2EaQASDDgCtAS4Fh2zrzW4Yyds",
                OpenPathApiUrl = "http://localhost:35808/v1/globalpayments"
            });

            activateCard1 = new eCheck
            {
                CheckHolderName = "First Last",
                RoutingNumber = "122000030",
                AccountNumber = "1357902468",
                EntryMode = EntryMethod.Manual,
                SecCode = "WEB",
                AccountType = AccountType.CHECKING,
                CheckType = CheckType.PERSONAL,
            };

            activateCard2 = new eCheck
            {
                CheckHolderName = "First Last",
                RoutingNumber = "122000030",
                AccountNumber = "1357902468",
                EntryMode = EntryMethod.Manual,
                SecCode = "WEB",
                AccountType = AccountType.SAVINGS,
                CheckType = CheckType.PERSONAL,
            };


            activateCard3 = new eCheck
            {
                CheckHolderName = "First Last",
                RoutingNumber = "122000030",
                AccountNumber = "1357902468",
                EntryMode = EntryMethod.Manual,
                SecCode = "WEB",
                AccountType = AccountType.SAVINGS,
                CheckType = CheckType.BUSINESS,
                CheckName = "First Last"
            };

            check = new eCheck {
                AccountNumber = "24413815",
                RoutingNumber = "490000018",
                CheckType = CheckType.PERSONAL,
                SecCode = SecCode.PPD,
                AccountType = AccountType.CHECKING,
                EntryMode = EntryMethod.Manual,
                CheckHolderName = "John Doe",
                DriversLicenseNumber = "09876543210",
                DriversLicenseState = "TX",
                PhoneNumber = "8003214567",
                BirthYear = 1997,
                SsnLast4 = "4321",
                CheckName = ""
            };

            address = new Address {
                StreetAddress1 = "123 Main St.",
                City = "Downtown",
                State = "NJ",
                PostalCode = "12345"
            };
        }

        [TestMethod]
        public void ActivateTest()
        {
            //var transaction1 = activateCard1
            //    .Charge(1.23m)
            //    .WithCurrency("USD")
            //    .WithAddress(address)
            //    .Execute();

            //var transaction2 = activateCard2
            //    .Charge(12.34m)
            //    .WithCurrency("USD")
            //    .WithAddress(address)
            //    .Execute();

            var transaction3 = activateCard3
                .Charge(123.45m)
                .WithCurrency("USD")
                .WithAddress(address)
                .Execute();
        }


        [TestMethod]
        public void CheckSale() {
            var response = check.Charge(11m)
                .WithCurrency("USD")
                .WithAddress(address)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);
        }

        [TestMethod]
        public void CheckVoidFromTransactionId() {
            var response = check.Charge(10.00m)
                .WithCurrency("USD")
                .WithAddress(address)
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode);

            var voidResponse = Transaction.FromId(response.TransactionId, PaymentMethodType.ACH)
                .Void()
                .Execute();
            Assert.IsNotNull(voidResponse);
            Assert.AreEqual("00", voidResponse.ResponseCode);
        }

        [TestMethod]
        public void checkNewCryptoUrl() {
            ServicesContainer.ConfigureService(new GatewayConfig {
                SecretApiKey = "skapi_cert_MTyMAQBiHVEAewvIzXVFcmUd2UcyBge_eCpaASUp0A",
                ServiceUrl = "https://cert.api2-c.heartlandportico.com"
            });
            var response = check.Charge(10.00m)
                .WithCurrency("USD")
                .WithAddress(address)
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode);

        }
    }
}
