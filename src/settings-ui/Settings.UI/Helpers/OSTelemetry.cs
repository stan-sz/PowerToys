// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Win32;

namespace Microsoft.PowerToys.Settings.UI.Helpers
{
    internal enum OSTelemetryLevel
    {
        DataDiagnosticsOff = 0,
        RequiredDiagnosticData = 1,
        OptionalDiagnosticData = 3,
    }

    public static class OSTelemetry
    {
        private static readonly string OSTelemetryLevelRegKey = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection";
        private static readonly string OSTelemetryLevelGPORegKey = @"SOFTWARE\Policies\Microsoft\Windows\DataCollection";
        private static readonly string TelemetryLevelRegValue = "AllowTelemetry";

        private static OSTelemetryLevel GetOSTelemetryLevel()
        {
            OSTelemetryLevel telemetryLevel = OSTelemetryLevel.DataDiagnosticsOff;

            try
            {
                RegistryKey telemetryLevelRegKey = Registry.LocalMachine.OpenSubKey(OSTelemetryLevelRegKey, false);
                telemetryLevel = (OSTelemetryLevel)telemetryLevelRegKey?.GetValue(TelemetryLevelRegValue);
                telemetryLevelRegKey?.Close();
            }
            catch
            {
                return telemetryLevel;
            }

            return telemetryLevel;
        }

        public static bool IsOSOptionalDataDiagnosticsAllowed()
        {
            return GetOSTelemetryLevel() == OSTelemetryLevel.OptionalDiagnosticData;
        }

        private static OSTelemetryLevel GetOSGPOTelemetryLevel()
        {
            // We assume it is not GPO enforced by default
            OSTelemetryLevel telemetryLevel = OSTelemetryLevel.OptionalDiagnosticData;

            try
            {
                RegistryKey telemetryLevelRegKey = Registry.LocalMachine.OpenSubKey(OSTelemetryLevelGPORegKey, false);
                if (telemetryLevelRegKey is not null)
                {
                    if (telemetryLevelRegKey.GetValueNames().Contains(TelemetryLevelRegValue))
                    {
                        telemetryLevel = (OSTelemetryLevel)telemetryLevelRegKey.GetValue(TelemetryLevelRegValue);
                        telemetryLevelRegKey.Close();
                    }
                }
            }
            catch
            {
                return telemetryLevel;
            }

            return telemetryLevel;
        }

        public static bool IsOSOptionalDataDiagnosticsGPOEnforced()
        {
            return GetOSGPOTelemetryLevel() == OSTelemetryLevel.OptionalDiagnosticData;
        }
    }
}
