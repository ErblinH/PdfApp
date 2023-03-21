namespace PdfApp.Domain.Entities
{
    public class ConvertJob : BaseEntity
    {
        public Guid Name { get; set; }
        public string HtmlInput { get; set; }
        public int ConverterOptionsId { get; set; }
        public ConverterOptions ConverterOptions { get; set; }
    }
}
