﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.FancyZonesEditor.UITests
{
    [TestClass]
    public class Init
    {
        private static Process? appDriver;

        [AssemblyInitialize]
        public static void SetupAll(TestContext context)
        {
            string winAppDriver = Path.Combine($"{Environment.CurrentDirectory}", @".\..\..\..\..\..\deps\WinAppDriver", "WinAppDriver.exe");

            context.WriteLine($"Attempting to launch WinAppDriver at: {winAppDriver}");
            context.WriteLine($"Working directory: {Environment.CurrentDirectory}");

            appDriver = Process.Start(winAppDriver);
        }

        [AssemblyCleanup]
        public static void CleanupAll()
        {
            try
            {
                appDriver?.Kill();
            }
            catch
            {
            }
        }
    }
}