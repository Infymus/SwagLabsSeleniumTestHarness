using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwagLabs.TestHelpers
{
   public class InventoryPage
   {
      private readonly IWebDriver _driver;
      private readonly WebDriverWait _wait;

      // Locators
      private By PageTitleLocator => By.ClassName("title");
      private By InventoryContainerLocator => By.Id("inventory_container");
      private By AddBackpackButtonLocator => By.CssSelector("[data-test='add-to-cart-sauce-labs-backpack']");
      private By RemoveBackpackButtonLocator => By.CssSelector("[data-test='remove-sauce-labs-backpack']");
      private By BackpackToggleCountLocator => By.CssSelector("[data-test='add-to-cart-sauce-labs-backpack'], [data-test='remove-sauce-labs-backpack']");
      private By CartBadgeLocator => By.CssSelector("[data-test='shopping-cart-badge']");
      private By CartLinkLocator => By.CssSelector("[data-test='shopping-cart-link']");
      private By ItemImagesLocator => By.CssSelector(".inventory_item_img img");

      public InventoryPage(IWebDriver driver)
      {
         _driver = driver;
         _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
      }

      public bool IsInventoryContainerDisplayed()
      {
         try
         {
            var element = _wait.Until(driver =>
            {
               var el = driver.FindElement(InventoryContainerLocator);
               return el.Displayed ? el : null;
            });
            return element != null;
         }
         catch (WebDriverTimeoutException)
         {
            return false;
         }
      }

      public string GetPageTitleText()
      {
         var element = _wait.Until(driver =>
         {
            var el = driver.FindElement(PageTitleLocator);
            return el.Displayed ? el : null;
         });
         return element.Text ?? string.Empty;
      }

      public void AddBackpackToCart()
      {
         var button = _wait.Until(driver =>
         {
            var el = driver.FindElement(AddBackpackButtonLocator);
            return (el.Displayed && el.Enabled) ? el : null;
         });
         button.Click();
      }

      public void RemoveBackpackFromCart()
      {
         var button = _wait.Until(driver =>
         {
            var el = driver.FindElement(RemoveBackpackButtonLocator);
            return (el.Displayed && el.Enabled) ? el : null;
         });
         button.Click();
      }

      public string GetBackpackButtonText()
      {
         var button = _wait.Until(driver =>
         {
            var el = driver.FindElement(BackpackToggleCountLocator);
            return el.Displayed ? el : null;
         });
         return button.Text ?? string.Empty;
      }

      public bool IsCartBadgeDisplayed()
      {
         try
         {
            var elements = _driver.FindElements(CartBadgeLocator);
            return elements.Count > 0 && elements[0].Displayed;
         }
         catch (Exception)
         {
            return false;
         }
      }

      public string GetCartBadgeCount()
      {
         var element = _wait.Until(driver =>
         {
            var el = driver.FindElement(CartBadgeLocator);
            return el.Displayed ? el : null;
         });
         return element.Text ?? string.Empty;
      }

      public void ClickCart()
      {
         var link = _wait.Until(driver =>
         {
            var el = driver.FindElement(CartLinkLocator);
            return (el.Displayed && el.Enabled) ? el : null;
         });
         link.Click();
      }

      /// <summary>
      /// Retrieves all image src attributes for items on the inventory page.
      /// </summary>
      public List<string> GetInventoryImageSources()
      {
         _wait.Until(driver =>
         {
            var elements = driver.FindElements(ItemImagesLocator);
            return elements.Count > 0 ? elements : null;
         });

         var imageElements = _driver.FindElements(ItemImagesLocator);

         return imageElements
             .Select(img => img.GetAttribute("src"))
             .Where(src => !string.IsNullOrEmpty(src))
             .ToList()!;
      }
   }
}