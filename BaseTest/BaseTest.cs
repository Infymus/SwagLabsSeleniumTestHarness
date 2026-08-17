using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using SwagLabs.DataBase;

namespace SwagLabs.TestMethods
{
   /// <summary>
   /// Base test class for Selenium WebDriver test execution.
   /// Handles ChromeDriver initialization, appsettings configuration loading, 
   /// explicit wait setup, and driver teardown.
   /// </summary>
   public class BaseTest
   {
      // Selenium Driver & Wait
      public IWebDriver? Driver;
      public WebDriverWait? Wait;
      public Process? _chromeProcess;

      // File Downloads
      public string DownloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");

      // URL and Connection Strings
      public string? InitialUrl;
      public static string? ConnectionString;

      // Target Base URLs
      public static string? SauceDemoBaseUrl;

      // SauceDemo Shared Password
      public static string? LoginPassword;

      // SauceDemo User Accounts
      public static string? StandardUser;
      public static string? LockedOutUser;
      public static string? ProblemUser;
      public static string? PerformanceGlitchUser;
      public static string? ErrorUser;
      public static string? VisualUser;

      // Database
      public static dataBaseQuery? DBQuery;

      // ######### Setup and TearDown #####################################################################

      [OneTimeSetUp]
      public void GlobalSetup()
      {
         // Create local Downloads directory if it does not exist
         if (!Directory.Exists(DownloadDirectory))
         {
            Directory.CreateDirectory(DownloadDirectory);
         }
      }

      /// <summary>
      /// Sets up each test, grabs configuration data from appsettings.json,
      /// and initializes the Headless ChromeDriver instance.
      /// </summary>
      [SetUp]
      public void SetupEachTest()
      {
         // SetupEachTest()
         DebugOutput("SetupEachTest()");

         // Configuration Setup
         DebugOutput("ConfigurationBuilder()");
         var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddEnvironmentVariables()
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddUserSecrets<BaseTest>()
             .Build();

         // Connection Strings
         ConnectionString = configuration["ConnectionStrings:DBConnectionString"];

         // Target URLs
         SauceDemoBaseUrl = configuration["TargetUrls:SauceDemoBaseUrl"];

         // Set default test URL
         InitialUrl = SauceDemoBaseUrl;

         // Shared Credentials
         LoginPassword = configuration["SauceDemo:Password"];

         // Specific User Accounts
         StandardUser = configuration["SauceDemo:Users:StandardUser"];
         LockedOutUser = configuration["SauceDemo:Users:LockedOutUser"];
         ProblemUser = configuration["SauceDemo:Users:ProblemUser"];
         PerformanceGlitchUser = configuration["SauceDemo:Users:PerformanceGlitchUser"];
         ErrorUser = configuration["SauceDemo:Users:ErrorUser"];
         VisualUser = configuration["SauceDemo:Users:VisualUser"];

         // Debug Outputs for Verification
         DebugOutput($"ConnectionString = {ConnectionString}");
         DebugOutput($"SauceDemoBaseUrl = {SauceDemoBaseUrl}");
         DebugOutput($"StandardUser = {StandardUser}");

         // Chrome Driver Initialization
         var options = new ChromeOptions();
         // options.AddArgument("--headless=new");
         options.AddArguments("--no-sandbox");
         options.AddArguments("--disable-gpu");
         options.AddUserProfilePreference("download.prompt_for_download", false);
         options.AddUserProfilePreference("download.directory_upgrade", true);
         options.AddUserProfilePreference("download.default_directory", DownloadDirectory);
         options.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
         options.AddArgument("--allow-running-insecure-content");
         options.AddArgument("--disable-features=InsecureDownloadWarnings");
         options.AddUserProfilePreference("safebrowsing.enabled", true);

         DebugOutput("Start Chrome Driver");
         var chromeDriverService = ChromeDriverService.CreateDefaultService();
         Driver = new ChromeDriver(chromeDriverService, options);
         Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

         try
         {
            _chromeProcess = Process.GetProcessById(chromeDriverService.ProcessId);
         }
         catch (Exception ex)
         {
            DebugOutput($"Warning: Could not capture ChromeDriver process ID: {ex.Message}");
         }
      }

      [TearDown]
      public void TearDown()
      {
         DebugOutput("TearDown()");

         if (Driver != null)
         {
            Driver.Quit();
            Driver.Dispose();
            Driver = null;
            DebugOutput("Chrome Driver closed successfully.");
         }
      }

      /// <summary>
      /// Adds to the Console for easy Debugging, Logging & Test Results to Azure DevOps
      /// </summary>
      /// <param name="inDebugData"></param>
      public static void DebugOutput(string inDebugData)
      {
         DateTime dateTime = DateTime.Now;
         string formattedDate = dateTime.ToString("MM-dd-yyyy @ hh:mm:ss tt");
         Debug.WriteLine($"{formattedDate} : {inDebugData}");
         TestContext.WriteLine($"{formattedDate} : {inDebugData}");
      }

      /// <summary>
      /// Writes out a line separator to make it easier to read the debug output
      /// </summary>
      public static void DebugOutputSep()
      {
         DebugOutput($"{new string('=', 60)}");
      }
   }
}