# ForestalCasablancaApp
As a freelance developer for Forestal Casablanca, I undertook the task of enhancing and streamlining their operational processes. The company's workflow involved designated employees measuring trucks loaded with various wood species. Subsequently, this measurement data was manually transferred to another employee at the central camp who would re-enter the information into an Excel spreadsheet for further calculations and report generation.

To address this inefficiency, I conceptualized, designed, and implemented a mobile application using .NET MAUI. The app enables designated users to: 
* Input data related to the products in the trucks, automatically calculating volumes based on the selected product.
* Once the calculations are completed, the app seamlessly generates a PDF file, serving as both a summary and an invoice for the product.
This solution not only eliminated manual data entry errors but also significantly expedited the entire process, contributing to improved efficiency and accuracy in their forestry operations.

![26shots_so](https://github.com/DamianQS7/ForestalCasablancaApp/assets/102097286/be12c2b9-ba61-456e-9ab5-00f3f1ca91d2)

## Implementation Details
* UI Designed with XAML
* MVVM Architecture (Views are called Pages in this Project).
  - Change Notification Events where mostly implemented using the MVVM Community Toolkit.
* Services for PDF Generation, HTTP Requests, Info Messages and Calculations.
* Navigation was implemented using Shell.
* Custom Controls using ContentViews
* Light and Dark Theme using App Preferences API.

## Licenses

This project uses some third-party assets with a license that requires attribution:

- [.NET MAUI Community Toolkit](https://github.com/CommunityToolkit/Maui)
- [MVVM Community Toolkit](https://github.com/CommunityToolkit/WindowsCommunityToolkit)
