using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class CreateVerificationCodeCommandPostHandler
        : ICommandPostHandler<CreateVerificationCodeCommand, Result<string>>
    {
        public CreateVerificationCodeCommandPostHandler()
        {
            // TODO: Inject email service.
        }

        public Task PostHandleAsync(CreateVerificationCodeCommand message, Result<string>? messageResult, CancellationToken cancellationToken = default)
        {
            if (messageResult?.TryGetValue(out var code) == true)
            {
                // TODO: Send the code to the user via email.
                Console.WriteLine($"CODE: {code}");
            }

            return Task.CompletedTask;
        }
    }
}
