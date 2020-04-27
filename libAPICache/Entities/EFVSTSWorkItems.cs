using libAPICache.Models.VSTS;
using Microsoft.EntityFrameworkCore;

namespace libAPICache.Entities
{
    public class EFVSTSWorkItems : EFBase<Models.VSTS.WorkItem, libVSTS.models.WorkItem>
    {
        public EFVSTSWorkItems() : this(new EFDbContext())
        {
        }

        public EFVSTSWorkItems(EFDbContext context) : base(context)
        {
            Entries = _dbSet = _context.VSTSWorkItems;
        }

        public void CacheEntries()
        {
            string api_key = GetAPIKey("APISources:VSTS:API_Key");
            string organization = GetAPIKey("APISources:VSTS:Organization");
            string project = GetAPIKey("APISources:VSTS:Project");
            
            
        }

        public override WorkItem UpdateEnumerables(Models.VSTS.WorkItem source, Models.VSTS.WorkItem destination)
        {
            return destination;
        }
}
}