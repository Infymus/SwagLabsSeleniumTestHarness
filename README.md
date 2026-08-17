# Playwright UI Test Harness - SauceDemo

A modern, scalable web UI automation framework built with **C#**, **.NET 8**, **NUnit**, and **Microsoft Playwright**. 

This repository demonstrates browser automation best practices—including the Page Object Model (POM) design pattern, async/await execution, dynamic test configuration, and cross-browser web testing.

---

## 🎯 Application Under Test

* **Frontend Web Application:** [SauceDemo (Swag Labs)](https://www.saucedemo.com/)
  * *A target e-commerce site used for validating web interactions, authentication states, cart operations, checkout workflows, and user-specific visual defects.*

---

## 🛠 Framework Architecture & Design Patterns

* **Page Object Model (POM):** Encapsulates page locators and user interactions into dedicated page classes (`LoginPage`, `InventoryPage`, `CartPage`, `CheckoutStepOnePage`, `CheckoutStepTwoPage`, `CheckoutCompletePage`) to promote code reusability and clean test maintainability.
* **BaseTest Framework Harness:** Inherits from Playwright's `PageTest` to manage browser lifecycle, context instantiation, and automated test setup/teardown execution.
* **Flexible Configuration Management:** Uses `Microsoft.Extensions.Configuration` to dynamically pull URLs, target browsers, and test user credentials from `appsettings.json`, environment variables, or User Secrets.
* **Rich Console Diagnostics:** Features structured test logging with timestamps and execution dividers to streamline debugging and pipeline artifact integration (e.g., Azure DevOps, GitHub Actions).
