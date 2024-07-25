﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using global::PowerToys.GPOWrapper;
using Microsoft.PowerToys.Settings.UI.Library;
using Microsoft.PowerToys.Settings.UI.OOBE.Enums;
using Microsoft.PowerToys.Settings.UI.OOBE.ViewModel;
using Microsoft.PowerToys.Settings.UI.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;

namespace Microsoft.PowerToys.Settings.UI.OOBE.Views
{
    public sealed partial class OobeOverview : Page
    {
        private SettingsUtils _settingsUtils;

        public OobePowerToysModule ViewModel { get; set; }

        private bool _enableDataDiagnostics;

        public bool EnableDataDiagnostics
        {
            get
            {
                return _enableDataDiagnostics;
            }

            set
            {
                if (_enableDataDiagnostics != value)
                {
                    _enableDataDiagnostics = value;

                    string registryKey = @"HKEY_CURRENT_USER\Software\Classes\PowerToys\";
                    try
                    {
                        Registry.SetValue(registryKey, "AllowDataDiagnostics", value ? 1 : 0);
                    }
                    catch (Exception)
                    {
                    }

                    this.DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                    {
                        ShellPage.ShellHandler?.SignalGeneralDataUpdate();
                    });
                }
            }
        }

        public bool ShowDataDiagnosticsSetting => GetIsDataDiagnosticsInfoBarEnabled();

        private bool GetIsDataDiagnosticsInfoBarEnabled()
        {
            var isDataDiagnosticsGpoDisallowed = GPOWrapper.GetAllowDataDiagnosticsValue() == GpoRuleConfigured.Disabled;

            return !isDataDiagnosticsGpoDisallowed;
        }

        public OobeOverview()
        {
            this.InitializeComponent();
            _settingsUtils = new SettingsUtils();

            string registryKey = @"HKEY_CURRENT_USER\Software\Classes\PowerToys";
            object registryValue = Registry.GetValue(registryKey, "AllowDataDiagnostics", false);

            if (registryValue is not null)
            {
                _enableDataDiagnostics = (int)registryValue == 1 ? true : false;
            }

            ViewModel = new OobePowerToysModule(OobeShellPage.OobeShellHandler.Modules[(int)PowerToysModules.Overview]);
            DataContext = ViewModel;
        }

        private void SettingsLaunchButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (OobeShellPage.OpenMainWindowCallback != null)
            {
                OobeShellPage.OpenMainWindowCallback(typeof(DashboardPage));
            }

            ViewModel.LogOpeningSettingsEvent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LogOpeningModuleEvent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.LogClosingModuleEvent();
        }
    }
}
