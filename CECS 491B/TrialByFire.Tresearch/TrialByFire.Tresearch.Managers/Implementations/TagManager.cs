using TrialByFire.Tresearch.DAL.Contracts;
using TrialByFire.Tresearch.Managers.Contracts;
using TrialByFire.Tresearch.Services.Contracts;

namespace TrialByFire.Tresearch.Managers.Implementations
{
    public class TagManager : ITagManager
    {
        private ISqlDAO _sqlDAO;
        private ILogService _logService;
        private ITagService _tagService;
        public TagManager(ISqlDAO sqlDAO, ILogService logService, ITagService tagService)
        {
            _sqlDAO = sqlDAO;
            _logService = logService;
            _tagService = tagService;
        }


    
    }
}
