namespace codecov.Coverage
{
    public interface IUpload
    {
        void Uploader(string report, string url);
    }
}