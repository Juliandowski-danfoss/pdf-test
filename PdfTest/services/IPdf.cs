namespace PdfTest.services
{
    public interface IPdf
    {
        Stream Pdf(string? url = null);
    }
}
