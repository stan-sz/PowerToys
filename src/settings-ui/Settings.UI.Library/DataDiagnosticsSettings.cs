// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.PowerToys.Settings.UI.Library.Interfaces;

namespace Microsoft.PowerToys.Settings.UI.Library
{
    public class DataDiagnosticsSettings : ISettingsConfig
    {
        [JsonPropertyName("enable_data_diagnostics")]
        public bool EnableDataDiagnostics { get; set; }

        public DataDiagnosticsSettings()
        {
            EnableDataDiagnostics = false;
        }

        // converts the current to a json string.
        public string ToJsonString()
        {
            return JsonSerializer.Serialize(this);
        }

        private static string DefaultPowertoysVersion()
        {
            return interop.CommonManaged.GetProductVersion();
        }

        // This function is to implement the ISettingsConfig interface.
        // This interface helps in getting the settings configurations.
        public string GetModuleName()
        {
            // The SettingsUtils functions access general settings when the module name is an empty string.
            return string.Empty;
        }

        public bool UpgradeSettingsConfiguration() => false;
    }
}
