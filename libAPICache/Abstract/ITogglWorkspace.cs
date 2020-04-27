namespace libAPICache.Abstract
{
    public interface ITogglWorkspace : IBase<Models.Toggl.Workspace, libToggl.models.Workspace>
    {
        void CacheEntries();
    }
}