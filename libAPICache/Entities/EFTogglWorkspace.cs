using libAPICache.Abstract;
using libAPICache.Models.Toggl;

namespace libAPICache.Entities
{
    public class EFTogglWorkspace : EFBase<Workspace, libToggl.models.Workspace>, ITogglWorkspace
    {
        
    }
}