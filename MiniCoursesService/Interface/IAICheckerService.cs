namespace MiniCoursesService.Interface
{
    public interface IAICheckerService
    {
        Task<double> IsAIWrittenAsync(string text);

    }
}