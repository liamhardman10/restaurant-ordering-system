# Restaurant Ordering System

## Project Overview
A Windows desktop application created in C# & using .NET 10.0. Useful for restaurant order management with price and receipt generation capabilities, along with Discount calculations.
Final Project for Software Engineering 2. (not original project idea)

## Features
- Create and manage restaurant orders
- Apply discounts
- Generate receipts with details
- Real-time calculation of prices

## File Structure
- `src/RestaurantOrderSystem/` - Main application folder
  - `Models/`
  - `Services/`
  - `Factory/`
  - `Views/`
- `tests/` - Unit tests

## Installation Instructions
- Check the latest release
- Download Restaurant Ordering Kiosk.zip file
- Extract zip contents to folder
- Run .exe file locally!

## API Usage Details
- `IOrderService`: Manage orders
- `IPricingService`: Calculate prices
- `IDiscountService`: Apply discounts
- `ReceiptFactory`: Create receipts

## How Data is Stored
- Data uses in-memory storage during runtime

## Known Issues/Limitations
- Lacks database persistence
- Limited to 10 menu items (demo)
- Discounts aren't able to be combined

## Debugging Summary
- Use unit tests for core logic
- Check discount application order
- Validate input ranges

## Experimental Branch
- Feature: `experimental/dynamic-pricing`
- Attempt to implement time-based pricing
