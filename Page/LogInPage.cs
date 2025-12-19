using Microsoft.Playwright;
using System.Threading.Tasks;
using NUnit.Framework;
using FinmoreNetflity.Pages;

namespace FinmoreNetflity.Pages

{

    public class LogInPage : BasePage

    {
        
        private readonly ILocator _emailInput;
        private readonly ILocator _passwordInput;
        private readonly ILocator _loginSubmitButton;
        private readonly ILocator _registerButton;
        private readonly ILocator _registerTitle;
        private readonly ILocator _registerNameInput;
        private readonly ILocator _registerEmailInput;
        private readonly ILocator _registerPasswordInput;
        private readonly ILocator _registerConfirmPasswordInput;
        private readonly ILocator _registerSubmitButton;
        private readonly ILocator _dashboardTitle;
         public LogInPage(IPage page) : base(page)
        {
            _emailInput = Page.GetByTestId("login-email-input");
            _passwordInput = Page.GetByTestId("login-password-input");
            _loginSubmitButton = Page.GetByTestId("login-submit-button");
            _registerButton = Page.GetByTestId("switch-to-register-button");
            _registerTitle = Page.GetByTestId("register-title");
            _registerNameInput = Page.GetByTestId("register-name-input");
            _registerEmailInput = Page.GetByTestId("register-email-input"); 
            _registerPasswordInput = Page.GetByTestId("register-password-input");
            _registerConfirmPasswordInput = Page.GetByTestId("register-confirm-password-input");
            _registerSubmitButton = Page.GetByTestId("register-submit-button");
            _dashboardTitle = Page.GetByTestId("dashboard-title");


        }

public async Task NavigateToRegisterPageAsync()
        {
          await _registerButton.WaitForAsync(new() { Timeout = 60000 });
await _registerButton.ClickAsync();
await _registerTitle.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        }
        public async Task RegisterAsync(string BasicName, string BasicEmail, string BasicPassword, string BasicConfirmPassword)
        {
            await _registerNameInput.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            await  Fill( _registerNameInput, BasicName);
            await Fill( _registerEmailInput, BasicEmail);
            await Fill( _registerPasswordInput, BasicPassword);
            await Fill( _registerConfirmPasswordInput, BasicConfirmPassword);
            await ClickAsync(_registerSubmitButton, "Register Submit Button");
            await WaitForNetworkIdleAsync();
            Assert.That(await _dashboardTitle.IsVisibleAsync(), Is.True);
        }
        
        // public async Task LogInAsync(string BasicEmail, string BasicPassword)
        // {
        //     await _emailInput.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        //     await _emailInput.FillAsync( BasicEmail);
        //     await _passwordInput.FillAsync(BasicPassword);
        //     await _loginSubmitButton.ClickAsync();
        // }
    }
    }
    
