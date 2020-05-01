using libAPICache.Entities;
using libVSTS.models;


namespace libAPICache.tests.Entities
{
    public class BaseMock : EFBase<Models.VSTS.WorkItem, libVSTS.models.WorkItem>
    {
        public int EnumerablesTimesCalled { get; set; } = 0;
        public int UpdateEntityDataTimesCalled { get; set; } = 0;
        public EFDbContext InsertedContext { get; set; }

        public BaseMock(EFDbContext context) : base(context, null)
        {
            InsertedContext = context;
            Entries = DbSet = context.VSTSWorkItems;
        }

        public override Models.VSTS.WorkItem UpdateEnumerables(WorkItem activity, Models.VSTS.WorkItem timeEntry)
        {
            EnumerablesTimesCalled += 1;
            return timeEntry;
        }

        public override void UpdateEntityData(Models.VSTS.WorkItem destination, Models.VSTS.WorkItem workItem)
        {
            UpdateEntityDataTimesCalled += 1;
        }
    }
}