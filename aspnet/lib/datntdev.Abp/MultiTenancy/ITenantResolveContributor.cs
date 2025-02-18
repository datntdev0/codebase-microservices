namespace datntdev.Abp.MultiTenancy
{
    public interface ITenantResolveContributor
    {
        int? ResolveTenantId();
    }
}