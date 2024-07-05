namespace FamilieLaissFrontend.Client.Layout;

public partial class MainLayout
{
    private bool DrawerOpen { get; set; } = true;

    //[Inject]
    //public IGeneralDataService GeneralDataService { get; set; } = default!;

    //[Inject]
    //public ISecretDataService SecretDataService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        //SecretDataService.GoogleMapsApiKey = await GeneralDataService.GetGoogleMapsApiKeyAsync();
    }

    public void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }
}
