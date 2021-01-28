using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace NUnit_Appium_Examples
{
    public class WindowsTests
    {
        private AppiumLocalService appiumLocalService;
        private WindowsDriver<WindowsElement> driver;

        public void SetupLocalService()
        {
            // Start the Appium server as local Node.js app
            appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            appiumLocalService.Start();

            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App,
                @"C:\SoftUni\COURSES\QA-Automation\Jan-2021\06.Appium\WindowsFormsApp.exe");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Windows");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "WindowsPC");
            driver = new WindowsDriver<WindowsElement>(appiumLocalService, appiumOptions);
        }

        [SetUp]
        public void SetupRemoteServer()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App,
                @"C:\SoftUni\COURSES\QA-Automation\Jan-2021\06.Appium\WindowsFormsApp.exe");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Windows");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "WindowsPC");
            // Start Appium as IPv6 HTTP server before: `appium -a ::1`
            driver = new WindowsDriver<WindowsElement>(
                new Uri("http://[::1]:4723/wd/hub"), appiumOptions);
        }

        [Test]
        public void Test_WinFormsApp_ValidData()
        {
            //var textBoxFirstNum = driver.FindElementByAccessibilityId("textBoxFirstNum");
            var textBoxFirstNum = driver.FindElementByXPath("//Edit[@AutomationId='textBoxFirstNum']");
            textBoxFirstNum.SendKeys("5");
            var textBoxSecondNum = driver.FindElementByAccessibilityId("textBoxSecondNum");
            textBoxSecondNum.SendKeys("7");
            var buttonCalc = driver.FindElementByAccessibilityId("buttonCalc");
            buttonCalc.Click();
            var textBoxSum = driver.FindElementByAccessibilityId("textBoxSum");
            Assert.AreEqual("12", textBoxSum.Text);
        }

        [Test]
        public void Test_WinFormsApp_InvalidData()
        {
            var textBoxFirstNum = driver.FindElementByAccessibilityId("textBoxFirstNum");
            textBoxFirstNum.SendKeys("5");
            var textBoxSecondNum = driver.FindElementByAccessibilityId("textBoxSecondNum");
            textBoxSecondNum.SendKeys("");
            var buttonCalc = driver.FindElementByAccessibilityId("buttonCalc");
            buttonCalc.Click();
            var textBoxSum = driver.FindElementByAccessibilityId("textBoxSum");
            Assert.AreEqual("error", textBoxSum.Text);
        }

        [TearDown]
        public void ShutDown()
        {
            driver.CloseApp();
            driver.Quit();
            appiumLocalService?.Dispose();
        }
    }
}