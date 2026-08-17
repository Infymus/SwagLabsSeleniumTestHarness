# Selenium UI Test Harness - SauceDemo

A modern, scalable web UI automation framework built with **C#**, **.NET 8**, **NUnit**, and **Selenium WebDriver (v4)**. 

This repository demonstrates browser automation best practices—including the Page Object Model (POM) design pattern, explicit lambda waits, headless Chrome execution, dynamic test configuration, and cross-browser web testing.

---

## 🎯 Application Under Test

* **Frontend Web Application:** [SauceDemo (Swag Labs)](https://www.saucedemo.com/)
  * *A target e-commerce site used for validating web interactions, authentication states, cart operations, checkout workflows, and user-specific visual defects.*

---

## 🛠 Framework Architecture & Design Patterns

* **Page Object Model (POM):** Encapsulates page locators and user interactions into dedicated page classes (`LoginPage`, `InventoryPage`, `CartPage`, `CheckoutStepOnePage`, `CheckoutStepTwoPage`, `CheckoutCompletePage`) to promote code reusability and clean test maintainability.
* **BaseTest Framework Harness:** Manages the `ChromeDriver` lifecycle, headless browser configuration, explicit `WebDriverWait` instances, and automated test setup/teardown execution.
* **Native Selenium 4 Lambda Waits:** Replaces legacy third-party wait helpers with modern, thread-safe `WebDriverWait` lambdas to handle dynamic DOM elements cleanly without external dependencies.
* **Flexible Configuration Management:** Uses `Microsoft.Extensions.Configuration` to dynamically pull URLs, target options, and test user credentials from `appsettings.json`, environment variables, or User Secrets.
* **Rich Console Diagnostics:** Features structured test logging (`DebugOutput`) with timestamps and execution dividers to streamline debugging and pipeline artifact integration (e.g., Azure DevOps, GitHub Actions).