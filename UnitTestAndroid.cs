using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using System;

namespace NUnit_Appium_Examples
{
    public class AndroidTests
    {
        private AppiumLocalService appiumLocalService;
        private AndroidDriver<AndroidElement> driver;

        [OneTimeSetUp]
        public void SetupRemoteServer()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App,
                @"C:\SoftUni\COURSES\QA-Automation\Jan-2021\06.Appium\com.example.androidappsummator.apk");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "Pixel 2 Q 10.0 - API 29");

            // Start Appium as IPv6 HTTP server before: `appium -a ::1`
            driver = new AndroidDriver<AndroidElement>(
                new Uri("http://[::1]:4723/wd/hub"), appiumOptions);
        }

        public void SetupLocalService()
        {
            // Start the Appium server as local Node.js app
            appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            appiumLocalService.Start();

            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App,
                @"C:\SoftUni\COURSES\QA-Automation\Jan-2021\06.Appium\com.example.androidappsummator.apk");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "Pixel 2 Q 10.0 - API 29");
            driver = new AndroidDriver<AndroidElement>(appiumLocalService, appiumOptions);
        }

        [Test]
        public void Test_AndroidApp_Summator_ValidData()
        {
            var textBoxFirstNum = driver.FindElementById("com.example.androidappsummator:id/editText1");
            textBoxFirstNum.Clear();
            textBoxFirstNum.SendKeys("5");
            var textBoxSecondNum = driver.FindElementById("com.example.androidappsummator:id/editText2");
            textBoxSecondNum.Clear();
            textBoxSecondNum.SendKeys("7");
            var buttonCalc = driver.FindElementById("com.example.androidappsummator:id/buttonCalcSum");
            buttonCalc.Click();
            var textBoxSum = driver.FindElementById("com.example.androidappsummator:id/editTextSum");
            Assert.AreEqual("12", textBoxSum.Text);
        }

        [Test]
        public void Test_AndroidApp_Summator_InvalidData()
        {
            var textBoxFirstNum = driver.FindElementById("com.example.androidappsummator:id/editText1");
            textBoxFirstNum.Clear();
            var textBoxSecondNum = driver.FindElementById("com.example.androidappsummator:id/editText2");
            textBoxSecondNum.Clear();
            textBoxSecondNum.SendKeys("7");
            var buttonCalc = driver.FindElementById("com.example.androidappsummator:id/buttonCalcSum");
            buttonCalc.Click();
            var textBoxSum = driver.FindElementById("com.example.androidappsummator:id/editTextSum");
            Assert.AreEqual("error", textBoxSum.Text);
        }

        [OneTimeTearDown]
        public void ShutDown()
        {
            driver.CloseApp();
            driver.Quit();
            appiumLocalService?.Dispose();
        }
    }
}