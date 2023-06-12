using Microsoft.AspNetCore.Components;
using MyProject.Shared.Entities;
using System.Net.Http.Json;

namespace MyProject.Client.Pages
{   

    public partial class ViewCVCandidate
    {
        #region properties
        [Parameter]
        public string pathCV { get; set; }
        [Parameter]
        public int idCandidate { get; set; }

        [Parameter]
        public EventCallback closeViewCV { get; set; }
        #endregion properties
    }
}
