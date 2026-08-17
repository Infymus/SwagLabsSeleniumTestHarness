using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace SwagLabs.TestHelpers
{
   public class CartPage
   {
      private readonly IWebDriver _driver;
      private readonly WebDriverWait _wait;

      public CartPage(IWebDriver driver)
      {
         _driver = driver;
         _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
      }

      public bool IsItemInCart(string itemName)
      {
         try
         {
            string xpath = $"//*[@data-test='inventory-item-name' and text()='{itemName}']";
            var element = _wait.Until(driver => 
            {
               var el = driver.FindElement(By.XPath(xpath));
               return el.Displayed ? el : null;
            });

            return element != null;
         }
         catch (WebDriverTimeoutException)
         {
            return false;
         }
      }

      public void ClickCheckout()
      {
         var checkoutButton = _wait.Until(driver =>
         {
            var el = driver.FindElement(By.CssSelector("[data-test='checkout']"));
            return (el.Displayed && el.Enabled) ? el : null;
         });

         checkoutButton.Click();
      }
   }

   public class CheckoutStepOnePage
   {
      private readonly IWebDriver _driver;
      private readonly WebDriverWait _wait;

      public CheckoutStepOnePage(IWebDriver driver)
      {
         _driver = driver;
         _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
      }

      public void FillCustomerInfoAndContinue(string firstName, string lastName, string zipCode)
      {
         var firstNameInput = _wait.Until(driver =>
         {
            var el = driver.FindElement(By.CssSelector("[data-test='firstName']"));
            return el.Displayed ? el : null;
         });

         firstNameInput.Clear();
         firstNameInput.SendKeys(firstName);

         var lastNameInput = _driver.FindElement(By.CssSelector("[data-test='lastName']"));
         lastNameInput.Clear();
         lastNameInput.SendKeys(lastName);

         var zipCodeInput = _driver.FindElement(By.CssSelector("[data-test='postalCode']"));
         zipCodeInput.Clear();
         zipCodeInput.SendKeys(zipCode);

         var continueButton = _driver.FindElement(By.CssSelector("[data-test='continue']"));
         continueButton.Click();
      }
   }

   public class CheckoutStepTwoPage
   {
      private readonly IWebDriver _driver;
      private readonly WebDriverWait _wait;

      public CheckoutStepTwoPage(IWebDriver driver)
      {
         _driver = driver;
         _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
      }

      public bool IsItemTotalDisplayed()
      {
         try
         {
            var subtotalLabel = _wait.Until(driver =>
            {
               var el = driver.FindElement(By.CssSelector("[data-test='subtotal-label']"));
               return el.Displayed ? el : null;
            });

            return subtotalLabel != null;
         }
         catch (WebDriverTimeoutException)
         {
            return false;
         }
      }

      public void ClickFinish()
      {
         var finishButton = _wait.Until(driver =>
         {
            var el = driver.FindElement(By.CssSelector("[data-test='finish']"));
            return (el.Displayed && el.Enabled) ? el : null;
         });

         finishButton.Click();
      }
   }

   public class CheckoutCompletePage
   {
      private readonly IWebDriver _driver;
      private readonly WebDriverWait _wait;

      public CheckoutCompletePage(IWebDriver driver)
      {
         _driver = driver;
         _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
      }

      public string GetCompleteHeader()
      {
         var headerElement = _wait.Until(driver =>
         {
            var el = driver.FindElement(By.CssSelector("[data-test='complete-header']"));
            return el.Displayed ? el : null;
         });

         return headerElement.Text ?? string.Empty;
      }
   }
}