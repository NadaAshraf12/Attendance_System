namespace CleanArch.Domain.Base.BaseInterface
{
    public interface IHasTimestamps
    {
        string? CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        string? UpdatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
    }
}
