namespace datntdev.Microservice.Models.Auth;

public enum TenantAvailabilityState
{
    Available = 1,
    InActive,
    NotFound
}

public class GetTenantStatusOutput
{
    public int? TenantId { get; set; }

    public TenantAvailabilityState State { get; set; }

    public GetTenantStatusOutput(TenantAvailabilityState state, int? tenantId = null)
    {
        State = state;
        TenantId = tenantId;
    }
}
