namespace NbgDev.Pst.App.Layout;

public partial class MainLayout
{
    private bool _drawerOpen = true;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}
