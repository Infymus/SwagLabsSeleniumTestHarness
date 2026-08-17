using NUnit.Framework;
using SwagLabs.TestHelpers;
using SwagLabs.TestMethods;
using System.Collections.Generic;
using System.Linq;
using static SwagLabs.TestHelpers.SauceLogin;

namespace SwagLabs.TestCases
{
   /// <summary>
   /// Tests the UI web application using Selenium WebDriver against saucedemo.com
   /// </summary>
   [TestFixture]
   public class SauceDemo : BaseTest
   {
      [TestCase("P1_SuccessfulLogin_LoadsInventoryPage")]
      public void P1_SuccessfulLogin_LoadsInventoryPage(string testName)
      {
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         // 1. Navigate to SauceDemo Base URL using InitialUrl/SauceDemoBaseUrl
         string targetUrl = InitialUrl ?? "https://www.saucedemo.com/";
         var loginPage = new LoginPage(Driver!);
         loginPage.NavigateTo(targetUrl);

         // 2, 3 & 4. Enter StandardUser credentials and click Login button
         loginPage.Login(StandardUser ?? "standard_user", LoginPassword ?? "secret_sauce");

         // 5. Initialize InventoryPage Object Model
         var inventoryPage = new InventoryPage(Driver!);

         // 6. Assert that the "Products" header or inventory container is displayed
         bool isContainerDisplayed = inventoryPage.IsInventoryContainerDisplayed();
         Assert.That(isContainerDisplayed, Is.True, "Expected inventory container to be displayed after login.");

         string titleText = inventoryPage.GetPageTitleText();
         Assert.That(titleText, Is.EqualTo("Products"), "Expected header title to be 'Products'.");

         // 7. Assert that the browser URL contains "/inventory.html"
         Assert.That(Driver!.Url, Does.Contain("/inventory.html"), "Expected URL to contain '/inventory.html'.");

         // Test Finished, Write out Debug Log
         DebugOutput($"**** TEST PASSED");
      }

      [TestCase("P1_EndToEndCheckout_CompletesOrderSuccessfully")]
      public void P1_EndToEndCheckout_CompletesOrderSuccessfully(string testName)
      {
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         string targetUrl = SauceDemoBaseUrl ?? "https://www.saucedemo.com/";
         string selectedItemName = "Sauce Labs Backpack";

         // 1. Log in with StandardUser credentials.
         var loginPage = new LoginPage(Driver!);
         loginPage.NavigateTo(targetUrl);
         loginPage.Login(StandardUser ?? "standard_user", LoginPassword ?? "secret_sauce");

         // 2. From the Inventory Page, select and add an item ("Sauce Labs Backpack") to the cart.
         var inventoryPage = new InventoryPage(Driver!);
         inventoryPage.AddBackpackToCart();

         // 3. Assert that the shopping cart badge updates to "1".
         string cartBadgeCount = inventoryPage.GetCartBadgeCount();
         Assert.That(cartBadgeCount, Is.EqualTo("1"), "Expected shopping cart badge count to be '1'.");
         DebugOutput($"Cart Badge Count Verified: {cartBadgeCount}");

         // 4. Click the shopping cart link to navigate to CartPage.
         inventoryPage.ClickCart();
         var cartPage = new CartPage(Driver!);

         // 5. Assert that the selected item appears in the cart item list.
         bool isItemInCart = cartPage.IsItemInCart(selectedItemName);
         Assert.That(isItemInCart, Is.True, $"Expected '{selectedItemName}' to be visible in the cart.");
         DebugOutput($"Cart Item Verified: {selectedItemName}");

         // 6. Click the "Checkout" button to navigate to CheckoutStepOne.
         cartPage.ClickCheckout();
         var checkoutStepOne = new CheckoutStepOnePage(Driver!);

         // 7. Fill in First Name, Last Name, and Postal Code, then click "Continue".
         checkoutStepOne.FillCustomerInfoAndContinue("John", "Doe", "84095");
         var checkoutStepTwo = new CheckoutStepTwoPage(Driver!);

         // 8. On CheckoutStepTwo, verify item total and click "Finish".
         bool isTotalDisplayed = checkoutStepTwo.IsItemTotalDisplayed();
         Assert.That(isTotalDisplayed, Is.True, "Expected item subtotal to be displayed on Checkout Step Two.");
         checkoutStepTwo.ClickFinish();

         // 9. Assert that the Checkout Complete page displays "Thank you for your order!".
         var checkoutComplete = new CheckoutCompletePage(Driver!);
         string completeHeader = checkoutComplete.GetCompleteHeader();
         Assert.That(completeHeader, Is.EqualTo("Thank you for your order!"), "Expected confirmation message header.");

         DebugOutput($"Confirmation Header Verified: '{completeHeader}'");
         DebugOutput("**** TEST PASSED");
      }

      [TestCase("P2_LockedOutUser_DisplaysErrorMessage")]
      public void P2_LockedOutUser_DisplaysErrorMessage(string testName)
      {
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         // 1. Navigate to SauceDemo Base URL.
         string targetUrl = SauceDemoBaseUrl ?? "https://www.saucedemo.com/";
         var loginPage = new LoginPage(Driver!);
         loginPage.NavigateTo(targetUrl);

         // 2 & 3. Enter LockedOutUser credentials ("locked_out_user" and "secret_sauce") and click Login.
         string lockedUser = LockedOutUser ?? "locked_out_user";
         string password = LoginPassword ?? "secret_sauce";

         loginPage.Login(lockedUser, password);
         DebugOutput($"Attempted login with user: '{lockedUser}'");

         // 4. Assert that the login form remains visible (login did not proceed).
         bool isLoginFormVisible = loginPage.IsLoginFormDisplayed();
         Assert.That(isLoginFormVisible, Is.True, "Expected login form to remain visible after failed login attempt.");
         DebugOutput("Verified login form is still displayed.");

         // 5. Assert that the error message container displays text containing "Epic sadface: Sorry, this user has been locked out.".
         string expectedErrorMessage = "Epic sadface: Sorry, this user has been locked out.";
         string actualErrorMessage = loginPage.GetErrorMessageText();

         Assert.That(actualErrorMessage, Does.Contain(expectedErrorMessage), $"Expected error message to contain: '{expectedErrorMessage}'");
         DebugOutput($"Verified Error Message Displayed: '{actualErrorMessage}'");

         DebugOutput("**** TEST PASSED");
      }

      [TestCase("P2_RemoveItemFromCart_UpdatesCartBadge")]
      public void P2_RemoveItemFromCart_UpdatesCartBadge(string testName)
      {
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         // 1. Log in with StandardUser credentials.
         string targetUrl = SauceDemoBaseUrl ?? "https://www.saucedemo.com/";
         var loginPage = new LoginPage(Driver!);
         loginPage.NavigateTo(targetUrl);
         loginPage.Login(StandardUser ?? "standard_user", LoginPassword ?? "secret_sauce");

         var inventoryPage = new InventoryPage(Driver!);

         // 2. Add an item to the cart from the Inventory page.
         inventoryPage.AddBackpackToCart();
         string badgeCountAfterAdd = inventoryPage.GetCartBadgeCount();
         Assert.That(badgeCountAfterAdd, Is.EqualTo("1"), "Expected shopping cart badge to show '1' after adding item.");
         DebugOutput($"Item added. Cart badge count: {badgeCountAfterAdd}");

         // 3. Click "Remove" for that item.
         inventoryPage.RemoveBackpackFromCart();
         DebugOutput("Clicked 'Remove' button for Sauce Labs Backpack.");

         // 4. Assert that the item button text reverts back to "Add to cart".
         string buttonText = inventoryPage.GetBackpackButtonText();
         Assert.That(buttonText, Is.EqualTo("Add to cart"), "Expected item button text to revert back to 'Add to cart'.");
         DebugOutput($"Verified item button text: '{buttonText}'");

         // 5. Assert that the shopping cart badge is no longer displayed (count is 0).
         bool isBadgeVisible = inventoryPage.IsCartBadgeDisplayed();
         Assert.That(isBadgeVisible, Is.False, "Expected shopping cart badge to no longer be displayed after removing all items.");
         DebugOutput("Verified shopping cart badge is hidden.");

         DebugOutput("**** TEST PASSED");
      }

      [TestCase("P3_ProblemUser_DetectsBrokenImagesOnInventoryPage")]
      public void P3_ProblemUser_DetectsBrokenImagesOnInventoryPage(string testName)
      {
         DebugOutputSep();
         DebugOutput($"inTestCaseID = {testName}");
         DebugOutputSep();

         // 1. Navigate to SauceDemo Base URL.
         string targetUrl = SauceDemoBaseUrl ?? "https://www.saucedemo.com/";
         var loginPage = new LoginPage(Driver!);
         loginPage.NavigateTo(targetUrl);

         // 2 & 3. Enter ProblemUser credentials ("problem_user" and "secret_sauce") and click Login.
         string user = ProblemUser ?? "problem_user";
         string password = LoginPassword ?? "secret_sauce";

         loginPage.Login(user, password);
         DebugOutput($"Logged in with problem user: '{user}'");

         // 4. Inspect the item image source URLs on the Inventory page.
         var inventoryPage = new InventoryPage(Driver!);
         List<string> imageSources = inventoryPage.GetInventoryImageSources();

         Assert.That(imageSources, Is.Not.Empty, "Expected to find inventory item images on the page.");
         DebugOutput($"Found {imageSources.Count} item images on the Inventory Page.");

         // 5. Assert that image sources are broken or point to incorrect assets (e.g., "sl-404").
         int brokenImageCount = imageSources.Count(src => src.Contains("sl-404"));

         DebugOutput($"Detected {brokenImageCount} broken images containing 'sl-404'.");

         Assert.That(brokenImageCount, Is.GreaterThan(0), "Expected problem_user session to display broken 'sl-404' image sources.");

         DebugOutput("**** TEST PASSED");
      }
   }
}