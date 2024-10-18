using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace YssWebstoreApi.Formatters
{
    public class PlainTextFormatter : TextInputFormatter
    {
        public PlainTextFormatter()
        {
            SupportedMediaTypes.Add("text/plain");
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            using (var reader = new StreamReader(context.HttpContext.Request.Body, encoding))
            {
                return await InputFormatterResult.SuccessAsync(await reader.ReadToEndAsync());
            }
        }
    }
}
