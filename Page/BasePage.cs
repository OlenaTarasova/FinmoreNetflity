using Microsoft.Playwright;

namespace FinmoreNetflity.Pages;

public class BasePage
{
    public  IPage Page;
    protected const string BaseUrl = "https://finmore.netlify.app/";

    public BasePage(IPage page)
    {
        Page = page ?? throw new ArgumentNullException(nameof(page));
    }

    public async Task NavigateAsync(string relativeUrl = "")
    {
        await Page.GotoAsync($"{BaseUrl}{relativeUrl}", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });
    }

    public async Task WaitForNetworkIdleAsync()
    {
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}