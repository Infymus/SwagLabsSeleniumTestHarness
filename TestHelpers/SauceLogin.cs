using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using SwagLabs.TestMethods;

namespace SwagLabs.TestHelpers
{
   public class SauceLogin : BaseTest
   {
      public class LoginPage
      {
         private readonly IWebDriver _driver;
         private readonly WebDriverWait _wait;

         // Locators using resilient data-test attributes
         private By UsernameInputLocator => By.CssSelector("[data-test='username']");
         private By PasswordInputLocator => By.CssSelector("[data-test='password']");
         private By LoginButtonLocator => By.CssSelector("[data-test='login-button']");
         private By ErrorMessageLocator => By.CssSelector("[data-test='error']");

         public LoginPage(IWebDriver driver)
         {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
         }

         public void NavigateTo(string url)
         {
            _driver.Navigate().GoToUrl(url);
         }

         public void Login(string username, string password)
         {
            var usernameInput = _wait.Until(driver =>
            {
               var el = driver.FindElement(UsernameInputLocator);
               return el.Displayed ? el : null;
            });
            usernameInput.Clear();
            usernameInput.SendKeys(username);

            var passwordInput = _driver.FindElement(PasswordInputLocator);
            passwordInput.Clear();
            passwordInput.SendKeys(password);

            var loginButton = _driver.FindElement(LoginButtonLocator);
            loginButton.Click();
         }

         public string GetErrorMessageText()
         {
            try
            {
               var errorMessage = _wait.Until(driver =>
               {
                  var el = driver.FindElement(ErrorMessageLocator);
                  return el.Displayed ? el : null;
               });
               return errorMessage.Text ?? string.Empty;
            }
            catch (WebDriverTimeoutException)
            {
               return string.Empty;
            }
         }

         public bool IsLoginFormDisplayed()
         {
            try
            {
               var loginButton = _wait.Until(driver =>
               {
                  var el = driver.FindElement(LoginButtonLocator);
                  return el.Displayed ? el : null;
               });
               return loginButton != null;
            }
            catch (WebDriverTimeoutException)
            {
               return false;
            }
         }
      }
   }
}