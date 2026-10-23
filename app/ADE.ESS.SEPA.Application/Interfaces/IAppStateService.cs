using ADE.ESS.SEPA.Application.Models;

namespace ADE.ESS.SEPA.Application.Interfaces
{
    public interface IAppStateService
    {
        // State properties
        AppStateStore CurrentState { get; }
        bool IsInitialized { get; }

        // Events for state changes
        event Action<AppStateStore>? StateChanged;
        event Action<UserState>? UserStateChanged;
        event Action<EdOrgState>? EdOrgStateChanged;

        // User state management
        Task InitializeUserAsync();
        Task<UserState> GetUserStateAsync();
        Task UpdateUserStateAsync(UserState userState);
        Task ClearUserState();

        // EdOrg state management
        Task InitializeEdOrgAsync(int entityId);
        Task<EdOrgState> GetEdOrgStateAsync();
        Task UpdateEdOrgStateAsync(EdOrgState edOrgState);
        Task ClearEdOrgState();

        // Combined state operations
        Task InitializeAppStateAsync(int entityId);
        Task RefreshStateAsync();
        void ClearAllState();

        // Protected storage management
        void SaveStateToSession(); // Legacy compatibility
        Task SaveStateToStorageAsync();
        Task<bool> LoadStateFromSessionAsync();
        Task ClearAllStateAsync();

        // Entity operations
        Task<bool> ValidateEntityAccessAsync(int entityId);
        Task<string> GetEntityNameAsync(int entityId);

        // Impersonation (admin PEA selection)
        Task SetImpersonatedPeaAsync(int peaEntityId);
        Task ClearImpersonationAsync();
    }
}