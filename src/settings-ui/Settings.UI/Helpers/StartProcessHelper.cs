﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace Microsoft.PowerToys.Settings.UI.Helpers
{
    public static class StartProcessHelper
    {
        public const string ColorsSettings = "ms-settings:colors";
        public const string DiagnosticsAndFeedback = "ms-settings:privacy-feedback";

        public static void Start(string process)
        {
            Process.Start(new ProcessStartInfo(process) { UseShellExecute = true });
        }
    }
}
