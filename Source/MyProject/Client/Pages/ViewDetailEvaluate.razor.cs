using Microsoft.AspNetCore.Components;
using MyProject.Shared.Entities;

namespace MyProject.Client.Pages
{
    public partial class ViewDetailEvaluate
    {
        #region properties
        [Parameter]
        public Candidate GradeTestCandidate { get; set; } = new Candidate();

        [Parameter]
        public List<Candidate>  EvaluateCandidateViewDetail { get; set; } = new List<Candidate>();
        [Parameter]
        public EventCallback CloseThis { get; set; }

        #endregion properties


        #region functions
        public async Task closeThis() 
        {
            await CloseThis.InvokeAsync();
        }
        #endregion functions
    }
}
