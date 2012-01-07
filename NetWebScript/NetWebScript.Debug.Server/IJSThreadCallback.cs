
namespace NetWebScript.Debug.Server
{
    public interface IJSThreadCallback
    {
        void OnBreakpoint(JSModuleDebugPoint point, JSStack stack);

        void OnStepDone(JSModuleDebugPoint point, JSStack stack);

        void OnStopped();

        void OnContinueDone();
    }
}
