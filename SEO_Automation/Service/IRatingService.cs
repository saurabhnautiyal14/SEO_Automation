using System.Threading.Tasks;

namespace SEO_Automation.Service
{
    public interface IRatingService
    {
        Task<string> GetRanking(string keyword, string url);
    }
}