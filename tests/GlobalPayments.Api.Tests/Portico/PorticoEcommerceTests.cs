﻿using GlobalPayments.Api.Entities;
using GlobalPayments.Api.PaymentMethods;
using GlobalPayments.Api.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GlobalPayments.Api.Tests.Portico {
    [TestClass]
    public class PorticoEcommerceTests {
        CreditCardData card;

        public PorticoEcommerceTests() {
            ServicesContainer.ConfigureService(new GatewayConfig {
                SecretApiKey = "skapi_cert_MTyMAQBiHVEAewvIzXVFcmUd2UcyBge_eCpaASUp0A",
                ServiceUrl = "https://cert.api2.heartlandportico.com",
                OpenPathApiKey = "fteaWC5MYeVKdZ2EaQASDDgCtAS4Fh2zrzW4Yyds",
                OpenPathApiUrl = "http://localhost:35808/v1/globalpayments"
            });

            card = TestCards.VisaManual();
        }

        [TestMethod]
        public void EcomWithMoto() {
            Transaction response = card.Charge(9m)
                .WithCurrency("USD")
                .WithEcommerceInfo(new EcommerceInfo {
                    Channel = EcommerceChannel.MOTO
                })
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode);
        }

        [TestMethod]
        public void EcomWithDirectMarketShipDate() {
            Transaction response = card.Charge(10m)
                .WithCurrency("USD")
                .WithEcommerceInfo(new EcommerceInfo {
                    ShipDay = 25,
                    ShipMonth = 12
                })
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode);
        }

        [TestMethod]
        public void EcomWithDirectMarketInvoiceNoShipDate() {
            Transaction response = card.Charge(10m)
                .WithCurrency("USD")
                .WithEcommerceInfo(new EcommerceInfo {})
                .WithInvoiceNumber("1234567890")
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode);
        }

        [TestMethod]
        public void EcomWithDirectMarketDataInvoiceAndShipDate() {
            Transaction response = card.Charge(11m)
                .WithCurrency("USD")
                .WithEcommerceInfo(new EcommerceInfo {
                    ShipDay = 25,
                    ShipMonth = 12
                })
                .WithInvoiceNumber("1234567890")
                .WithAllowDuplicates(true)
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode);
        }

        [TestMethod]
        public void EcomWithSecureEcommerce() {
            card.ThreeDSecure = new ThreeDSecure {
                PaymentDataSource = "ApplePay",
                Cavv = "XXXXf98AAajXbDRg3HSUMAACAAA=",
                Eci = 5
            };

            Transaction response = card.Charge(10m)
                .WithCurrency("USD")
                .WithInvoiceNumber("1234567890")
                .WithAllowDuplicates(true)
                .WithAddress(new Address
                {
                    City = "Lake Forest",
                    Country = "United States",
                    CountryCode = "US",
                    PostalCode = "92630",
                    Province = "California"
                })
                .Execute();
            Assert.IsNotNull(response);
            Assert.AreEqual("00", response.ResponseCode);
        }
    }
}
