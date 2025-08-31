# MVVMUtilityModules

Welcome to the **MVVM Utility Modules** repository! This project provides reusable, modular building blocks to simplify and accelerate the implementation of the MVVM (Model-View-ViewModel) architectural pattern in .NET applications. The focal modules in this repository are:

- **MVVMDialogsModule**: For managing dialogs in a decoupled, testable, MVVM-friendly way.
- **MVVMNavigationModule**: For handling navigation between views/pages, supporting both simple and complex navigation scenarios.

This README provides comprehensive documentation on both modules, including their setup, usage, extensibility, and best practices.

---

## Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [MVVMDialogsModule](#mvvmdialogsmodule)
  - [Purpose](#purpose)
  - [Key Features](#key-features)
  - [Setup & Registration](#setup--registration)
  - [Usage Patterns](#usage-patterns)
  - [Custom Dialogs](#custom-dialogs)
  - [Advanced Usage](#advanced-usage)
- [MVVMNavigationModule](#mvvmnavigationmodule)
  - [Purpose](#purpose-1)
  - [Key Features](#key-features-1)
  - [Setup & Registration](#setup--registration-1)
  - [Usage Patterns](#usage-patterns-1)
  - [Parameter Passing](#parameter-passing)
  - [Navigation Stack](#navigation-stack)
  - [Advanced Usage](#advanced-usage-1)
- [Best Practices](#best-practices)
- [FAQ](#faq)
- [Contributing](#contributing)
- [License](#license)

---

## Overview

These modules are designed to work seamlessly with any .NET MVVM framework (such as MVVM Light, Prism, CommunityToolkit.Mvvm, ReactiveUI, etc.), but they are framework-agnostic and rely only on standard C# and .NET features.

---

## Installation

You can add these modules to your project in several ways:

1. **Via NuGet** (if available):
   ```sh
   dotnet add package MVVMUtilityModules
   ```
2. **Manual Reference**:
   - Clone this repository.
   - Add the relevant project/files to your solution.

---

## MVVMDialogsModule

### Purpose

**MVVMDialogsModule** allows you to display dialogs (message boxes, custom input dialogs, confirmations, etc.) from your ViewModels without violating the MVVM principle of separation of concerns.

### Key Features

- Decouples dialog logic from UI layer
- Supports custom dialog types and view-model-driven dialogs
- Returns results asynchronously
- Testable: Can be mocked or stubbed in unit tests

### Setup & Registration

1. **Register the Dialog Service**

   In your application's startup (e.g., App.xaml.cs or Program.cs):

   ```csharp
   var dialogService = new DialogService();
   // Register with your DI container or Service Locator
   ```

2. **Connect Dialogs to Views**

   In your view layer (e.g., MainWindow), set up a mapping between dialog ViewModels and Views:

   ```csharp
   dialogService.Register<ConfirmationDialogViewModel, ConfirmationDialogView>();
   dialogService.Register<InputDialogViewModel, InputDialogView>();
   ```

### Usage Patterns

#### 1. Showing a Standard Dialog

In your ViewModel:

```csharp
// Inject IDialogService
private readonly IDialogService _dialogService;

// Show a confirmation dialog
var result = await _dialogService.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResult>(
    vm => {
        vm.Title = "Delete Item";
        vm.Message = "Are you sure you want to delete this item?";
    }
);

if (result == ConfirmationDialogResult.Yes) {
    // Perform delete
}
```

#### 2. Showing a Custom Dialog

```csharp
var input = await _dialogService.ShowDialogAsync<InputDialogViewModel, string>(
    vm => {
        vm.Title = "Enter Name";
        vm.Prompt = "Please enter your name:";
    }
);
if (!string.IsNullOrEmpty(input)) {
    // Use input
}
```

### Custom Dialogs

To add your own dialog:

1. **Create a Dialog ViewModel** (e.g., `MyCustomDialogViewModel`)
2. **Create a corresponding View/UserControl** (e.g., `MyCustomDialogView`)
3. **Register the mapping** with the DialogService (see above)

### Advanced Usage

- **Can be used for modal or non-modal dialogs**
- **Supports passing parameters and getting strongly-typed results**
- **DialogService can be mocked for unit testing ViewModels**

---

## MVVMNavigationModule

### Purpose

**MVVMNavigationModule** provides a flexible, MVVM-friendly way to perform navigation between Views/Pages in WPF, UWP, .NET MAUI, or Xamarin.Forms applications.

### Key Features

- ViewModel-driven navigation (no code-behind)
- Parameterized navigation
- Navigation history & stack management
- Supports modal and non-modal navigation
- Extensible for custom navigation logic

### Setup & Registration

1. **Register the Navigation Service**

   In your application's startup:

   ```csharp
   var navigationService = new NavigationService();
   // Register with DI or Service Locator
   ```

2. **Map ViewModels to Views**

   ```csharp
   navigationService.Register<HomeViewModel, HomeView>();
   navigationService.Register<DetailsViewModel, DetailsView>();
   ```

### Usage Patterns

#### 1. Simple Navigation

```csharp
// Inject INavigationService
private readonly INavigationService _navigationService;

// Navigate to another ViewModel
await _navigationService.NavigateToAsync<DetailsViewModel>();
```

#### 2. Navigation with Parameters

```csharp
await _navigationService.NavigateToAsync<DetailsViewModel>(new DetailsParameters { Id = 42 });
```

#### 3. Going Back

```csharp
await _navigationService.GoBackAsync();
```

### Parameter Passing

Parameters are typically passed as simple objects or classes. The NavigationService passes them to the target ViewModel, which can implement an interface such as `INavigationAware`:

```csharp
public interface INavigationAware
{
    Task OnNavigatedToAsync(object parameter);
}
```

### Navigation Stack

- The NavigationService maintains a stack of visited ViewModels/Views.
- Supports back navigation, clearing stack, modal navigation, etc.

### Advanced Usage

- **Modal Navigation**: Present a ViewModel modally and await its result.
- **Navigation Guards**: Implement logic to prevent or allow navigation.
- **Deep Linking**: Navigate directly to a ViewModel with a specific state.

---

## Best Practices

- **Always inject services (IDialogService, INavigationService) into your ViewModels for testability.**
- **Keep dialog and navigation logic in ViewModels, not Views or code-behind.**
- **Register all ViewModel-View mappings at startup.**
- **Leverage asynchronous APIs for dialogs and navigation to keep UI responsive.**

---

## FAQ

### Can I use these modules with any MVVM framework?

Yes! These modules are designed to be framework-agnostic and work with any .NET MVVM platform.

### How do I test ViewModels that use these modules?

Mock or stub the IDialogService and INavigationService interfaces, or use your testing framework's mocking capabilities.

### Can I display non-modal dialogs?

Yes, the DialogService supports both modal and non-modal scenarios.

---

## Contributing

Contributions are welcome! Please open an issue or pull request with your suggestions, bug fixes, or improvements.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Contact

For questions, ideas, or support, open an issue or reach out to the maintainer.

---

**Happy MVVM-ing!**
