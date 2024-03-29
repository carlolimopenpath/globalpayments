﻿using System;
using System.Collections.Generic;
using GlobalPayments.Api.Entities;
using GlobalPayments.Api.Gateways;
using GlobalPayments.Api.Terminals;

namespace GlobalPayments.Api {
    public class ConfiguredServices : IDisposable {
        internal IPaymentGateway GatewayConnector { get; set; }

        internal IRecurringService RecurringConnector { get; set; }

        internal IReportingService ReportingService { get; set; }

        internal IDeviceInterface DeviceInterface { get; private set; }

        private DeviceController _deviceController;
        internal DeviceController DeviceController {
            get {
                return _deviceController;
            }
            set {
                _deviceController = value;
                DeviceInterface = value.ConfigureInterface();
            }
        }

        internal OnlineBoardingConnector BoardingConnector { get; set; }

        internal TableServiceConnector TableServiceConnector { get; set; }

        internal PayrollConnector PayrollConnector { get; set; }

        public void Dispose() {
            DeviceController.Dispose();
        }
    }

    /// <summary>
    /// Maintains references to the currently configured gateway/device objects
    /// </summary>
    /// <remarks>
    /// The public `ServicesContainer.Configure` method is the only call
    /// required of the integrator to configure the SDK's various gateway/device
    /// interactions. The configured gateway/device objects are handled
    /// internally by exposed APIs throughout the SDK.
    /// </remarks>
    public class ServicesContainer : IDisposable {
        private Dictionary<string, ConfiguredServices> _configurations;
        private static ServicesContainer _instance;

        internal static ServicesContainer Instance {
            get {
                if (_instance == null)
                    _instance = new ServicesContainer();
                return _instance;
            }
        }

        /// <summary>
        /// Configure the SDK's various gateway/device interactions
        /// </summary>
        public static void Configure(ServicesConfig config, string configName = "default") {
            config.Validate();

            // configure devices
            ConfigureService(config.DeviceConnectionConfig, configName);

            // configure table service
            ConfigureService(config.TableServiceConfig, configName);

            // configure payroll
            ConfigureService(config.PayrollConfig, configName);

            // configure gateways
            ConfigureService(config.GatewayConfig, configName);
        }

        public static void ConfigureService<T>(T config, string configName = "default") where T : Configuration {
            if (config != null) {
                if (!config.Validated)
                    config.Validate();

                var cs = Instance.GetConfiguration(configName);
                config.ConfigureContainer(cs);

                Instance.AddConfiguration(configName, cs);
            }
        }

        private ServicesContainer() {
            _configurations = new Dictionary<string, ConfiguredServices>();
        }

        private ConfiguredServices GetConfiguration(string configName) {
            if (_configurations.ContainsKey(configName))
                return _configurations[configName];
            return new ConfiguredServices();
        }

        private void AddConfiguration(string configName, ConfiguredServices config) {
            if (_configurations.ContainsKey(configName))
                _configurations[configName] = config;
            else _configurations.Add(configName, config);
        }

        internal IPaymentGateway GetClient(string configName) {
            if (_configurations.ContainsKey(configName))
                return _configurations[configName].GatewayConnector;
            throw new ApiException("The specified configuration has not been configured for gateway processing.");
        }

        internal IDeviceInterface GetDeviceInterface(string configName) {
            if (_configurations.ContainsKey(configName))
                return _configurations[configName].DeviceInterface;
            throw new ApiException("The specified configuration has not been configured for terminal interaction.");
        }

        internal DeviceController GetDeviceController(string configName) {
            if (_configurations.ContainsKey(configName))
                return _configurations[configName].DeviceController;
            throw new ApiException("The specified configuration has not been configured for terminal interaction.");
        }

        internal IRecurringService GetRecurringClient(string configName) {
            if (_configurations.ContainsKey(configName))
                return _configurations[configName].RecurringConnector;
            throw new ApiException("The specified configuration has not been configured for recurring processing.");
        }

        internal TableServiceConnector GetTableServiceClient(string configName) {
            if (_configurations.ContainsKey(configName))
                return _configurations[configName].TableServiceConnector;
            throw new ApiException("The specified configuration has not been configured for table service.");
        }

        internal OnlineBoardingConnector GetBoardingConnector(string configName) {
            if (_configurations.ContainsKey(configName))
                return _configurations[configName].BoardingConnector;
            return null;
        }

        internal PayrollConnector GetPayrollClient(string configName) {
            if (_configurations.ContainsKey(configName))
                return _configurations[configName].PayrollConnector;
            throw new ApiException("The specified configuration has not been configured for payroll.");
        }

        internal IReportingService GetReportingClient(string configName) {
            if (_configurations.ContainsKey(configName))
                return _configurations[configName].ReportingService;
            throw new ApiException("The specified configuration has not been configured for reporting.");
        }

        /// <summary>
        /// Implementation for `IDisposable`
        /// </summary>
        public void Dispose() {
            foreach (var config in _configurations.Values)
                config.Dispose();
        }
    }
}
