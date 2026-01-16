using Microsoft.AspNetCore.Components;
using NbgDev.Pst.App.Models;

namespace NbgDev.Pst.App.Components;

public partial class PermissionsPanel
{
    [Parameter]
    public Project? Project { get; set; }
}
