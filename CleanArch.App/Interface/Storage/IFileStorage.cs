using CleanArch.Common.Dtos;


namespace CleanArch.App.Interface.Storage;


public interface IFileStorage
{
    Task<FileResourceDto?> SaveAsync(
    Stream content,
    string originalFileName,
    string folder,
    CancellationToken ct = default);


    Task<IReadOnlyList<FileResourceDto>> SaveManyAsync(
    IEnumerable<(Stream Content, string OriginalFileName)> files,
    string folder,
    CancellationToken ct = default);


    Task<bool> DeleteByUrlAsync(string fileUrl, CancellationToken ct = default);
}