using GlobalPayments.Api.Entities;
using GlobalPayments.Api.PaymentMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GlobalPayments.Api.Tests {
    [TestClass]
    public class RealexCreditTests {
        CreditCardData card;

        [TestInitialize]
        public void Init() {

            ServicesContainer.ConfigureService(new GatewayConfig {
                MerchantId = "heartlandgpsandbox",
                AccountId = "api",
                SharedSecret = "secret",
                RebatePassword = "rebate",
                RefundPassword = "refund",
                ServiceUrl = "https://api.sandbox.realexpayments.com/epage-remote.cgi",
                OpenPathApiKey = "fteaWC5MYeVKdZ2EaQASDDgCtAS4Fh2zrzW4Yyds",
                OpenPathApiUrl = "https://localhost:44376/v1/globalpayments"
                // OpenPathApiKey = "VgSsh3Vh24DuwjuU3fsvccZ7CSWPZQ9EaV9K6xuE",
                // OpenPathApiUrl = "https://staging-api.openpath.io/v1/globalpayments"
            });

            card = new CreditCardData {
                Number = "4111111111111111",
                ExpMonth = 2,
                ExpYear = 2022,
                Cvn = "222",
                CardHolderName = "John Doe"
            };
        }

        [TestMethod]
        public void CreditAuthorization() {
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

        [TestMethod]
        public void CreditAuthorizationForMultiCapture() {
            var authorization = card.Authorize(14m)
                .WithCurrency("USD")
                .WithMultiCapture(true)
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(authorization);
            Assert.AreEqual("00", authorization.ResponseCode, authorization.ResponseMessage);

            var capture = authorization.Capture(3m)
                .Execute();
            Assert.IsNotNull(capture);
            Assert.AreEqual("00", capture.ResponseCode, capture.ResponseMessage);

            var capture2 = authorization.Capture(5m)
                .Execute();
            Assert.IsNotNull(capture);
            Assert.AreEqual("00", capture.ResponseCode, capture.ResponseMessage);

            var capture3 = authorization.Capture(7m)
                .Execute();
            Assert.IsNotNull(capture);
            Assert.AreEqual("00", capture.ResponseCode, capture.ResponseMessage);
        }

        [TestMethod]
        public void CreditSale() {
            var response = card.Charge(15m)
                .WithCurrency("USD")
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);
        }

        [TestMethod]
        public void CreditSaleWithRecurring() {
            var response = card.Charge(15m)
                .WithCurrency("USD")
                .WithRecurringInfo(RecurringType.Fixed, RecurringSequence.First)
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);
        }

        [TestMethod]
        public void CreditRefund() {
            var response = card.Refund(16m)
                .WithCurrency("USD")
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);
        }

        [TestMethod]
        public void CreditRebate() {
            var response = card.Charge(17m)
                .WithCurrency("USD")
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);

            var rebate = response.Refund(17m)
                .WithCurrency("USD")
                .Execute();
            Assert.IsNotNull(rebate);
            Assert.AreEqual("00", rebate.ResponseCode, rebate.ResponseMessage);
        }

        [TestMethod]
        public void CreditVoid() {
            var response = card.Charge(15m)
                .WithCurrency("USD")
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);

            var voidResponse = response.Void().Execute();
            Assert.IsNotNull(voidResponse);
            Assert.AreEqual("00", voidResponse.ResponseCode, voidResponse.ResponseMessage);
        }

        [TestMethod]
        public void CreditVerify() {
            var response = card.Verify()
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);
        }

        [TestMethod]
        public void CreditFraudResponse() {
            var billingAddress = new Address();
            billingAddress.StreetAddress1 = "Flat 123";
            billingAddress.StreetAddress2 = "House 456";
            billingAddress.StreetAddress3 = "Cul-De-Sac";
            billingAddress.City = "Halifax";
            billingAddress.Province = "West Yorkshire";
            billingAddress.State = "Yorkshire and the Humber";
            billingAddress.Country = "GB";
            billingAddress.PostalCode = "E77 4QJ";

            var shippingAddress = new Address();
            shippingAddress.StreetAddress1 = "House 456";
            shippingAddress.StreetAddress2 = "987 The Street";
            shippingAddress.StreetAddress3 = "Basement Flat";
            shippingAddress.City = "Chicago";
            shippingAddress.State = "Illinois";
            shippingAddress.Province = "Mid West";
            shippingAddress.Country = "US";
            shippingAddress.PostalCode = "50001";

            var fraudResponse = card.Charge(199.99m)
                .WithCurrency("EUR")
                .WithAddress(billingAddress, AddressType.Billing)
                .WithAddress(shippingAddress, AddressType.Shipping)
                .WithProductId("SID9838383")
                .WithClientTransactionId("Car Part HV")
                .WithCustomerId("E8953893489")
                .WithCustomerIpAddress("123.123.123.123")
                .Execute();
            Assert.IsNotNull(fraudResponse);
            Assert.AreEqual("00", fraudResponse.ResponseCode, fraudResponse.ResponseMessage);
        }

        [TestMethod, Ignore]
        public void TestAuthMobileGooglePay()
        {
            var token = new CreditCardData
            {
                Token = "{\"version\":\"EC_v1\",\"data\":\"dvMNzlcy6WNB\",\"header\":{\"ephemeralPublicKey\":\"MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEWdNhNAHy9kO2Kol33kIh7k6wh6E\",\"transactionId\":\"fd88874954acdb299c285f95a3202ad1f330d3fd4ebc22a864398684198644c3\",\"publicKeyHash\":\"h7WnNVz2gmpTSkHqETOWsskFPLSj31e3sPTS2cBxgrk\"}}",
                MobileType = MobilePaymentMethodType.GOOGLEPAY
            };

            var response = token.Charge(15m)
                .WithCurrency("EUR")
                .Execute();

            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode);
        }

        [TestMethod]
        public void TestOpenPath_Declined_Because_Of_Country()
        {
            var response = card.Charge(15m)
                .WithCurrency("USD")
                .WithRecurringInfo(RecurringType.Fixed, RecurringSequence.First)
                .WithAccountType(AccountType.CHECKING)
                .WithAddress(new Address
                {
                    City = "Singapore",
                    Country = "Singapore",
                    CountryCode = "SG",
                    PostalCode = "1772",
                    Province = "NCR"
                })
                .WithClientTransactionId("TRANSACTION001")
                .WithCustomerId("1")
                .WithDescription("Test description")
                .WithEcommerceInfo(new EcommerceInfo
                {
                    Channel = EcommerceChannel.ECOM,
                    ShipDay = 1,
                    ShipMonth = 10
                })
                .WithInvoiceNumber("INVOICE001")
                .WithProductId("PRODUCT001")
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);
        }

        [TestMethod]
        public void TestOpenPath_Approved()
        {
            var transaction = card.Charge(15m)
                .WithCurrency("USD")
                .WithRecurringInfo(RecurringType.Fixed, RecurringSequence.First)
                .WithAccountType(AccountType.CHECKING)
                .WithAddress(new Address
                {
                    City = "Lake Forest",
                    Country = "United States",
                    CountryCode = "US",
                    PostalCode = "92630",
                    Province = "CA"
                })
                .WithClientTransactionId("TRANSACTION001")
                .WithCustomerId("1")
                .WithDescription("Test description")
                .WithEcommerceInfo(new EcommerceInfo
                {
                    Channel = EcommerceChannel.ECOM,
                    ShipDay = 1,
                    ShipMonth = 10
                })
                .WithInvoiceNumber("INVOICE001")
                .WithProductId("PRODUCT001")
                .WithAllowDuplicates(true);
            var response = transaction.Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);
        }

        [TestMethod]
        public void TestOpenPath_Approved_Processed_In_Stripe()
        {
            var transaction = card.Charge(15m)
                .WithCurrency("USD")
                .WithRecurringInfo(RecurringType.Fixed, RecurringSequence.First)
                .WithAccountType(AccountType.CHECKING)
                .WithAddress(new Address
                {
                    City = "Manila",
                    Country = "Philippines",
                    CountryCode = "PH",
                    PostalCode = "1772",
                    Province = "00"
                })
                .WithClientTransactionId("TRANSACTION001")
                .WithCustomerId("1")
                .WithDescription("Test description")
                .WithEcommerceInfo(new EcommerceInfo
                {
                    Channel = EcommerceChannel.ECOM,
                    ShipDay = 1,
                    ShipMonth = 10
                })
                .WithInvoiceNumber("INVOICE001")
                .WithProductId("PRODUCT001")
                .WithAllowDuplicates(true);
            var response = transaction.Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);
        }

        [TestMethod]
        public void TestOpenPath_BounceBack()
        {
            var transaction = card.Charge(15m)
                .WithCurrency("USD")
                .WithRecurringInfo(RecurringType.Fixed, RecurringSequence.First)
                .WithAccountType(AccountType.CHECKING)
                .WithAddress(new Address
                {
                    City = "Lake Forest",
                    Country = "United States",
                    CountryCode = "US",
                    PostalCode = "92630",
                    Province = "CA"
                })
                .WithClientTransactionId("TRANSACTION001")
                .WithCustomerId("1")
                .WithDescription("Test description")
                .WithEcommerceInfo(new EcommerceInfo
                {
                    Channel = EcommerceChannel.ECOM,
                    ShipDay = 1,
                    ShipMonth = 10
                })
                .WithInvoiceNumber("INVOICE001")
                .WithProductId("PRODUCT001")
                .WithAllowDuplicates(true);
            var response = transaction.Execute();

            if (response.ResponseMessage == "OpenPathBouncedBack")
            {
                response = transaction.Execute();
            }


            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode, response.ResponseMessage);
        }
    }
}
