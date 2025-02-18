namespace datntdev.Abp.Auditing
{
    public interface IAuditSerializer
    {
        string Serialize(object obj);
    }
}