namespace MiniCoursesDomain.DTO;

public class SaplingResponse
{
    public double Score { get; set; }
    public List<SentenceScore> SentenceScores { get; set; }
}