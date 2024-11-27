using MediatR;
using System.Security.Cryptography;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class GenerateVerificationCodeCommandHandler : IRequestHandler<GenerateVerificationCodeCommand, bool>
    {
        private readonly ICredentialsRepository _credentials;
        private readonly TimeProvider _timeProvider;
        private readonly IConfiguration _configuration;

        public GenerateVerificationCodeCommandHandler(ICredentialsRepository credentials, TimeProvider timeProvider, IConfiguration configuration)
        {
            _credentials = credentials;
            _timeProvider = timeProvider;
            _configuration = configuration;
        }

        public async Task<bool> Handle(GenerateVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            var credentials = await _credentials.GetByAccountIdAsync(request.AccountId);
            if (credentials is not null)
            {
                var lifetime = _configuration.GetValue<TimeSpan?>("Security:VerificationCodeLifetime")
                    ?? TimeSpan.FromMinutes(20);

                credentials.VerificationCode = GenerateCode();
                credentials.VerificationCodeExpiresAt = _timeProvider.GetUtcNow().Add(lifetime);

                return await _credentials.UpdateAsync(credentials) == credentials.Id;
            }

            //TODO: return error
            return false;
        }

        private string GenerateCode()
        {
            return RandomNumberGenerator.GetInt32(999999)
                .ToString()
                .PadLeft(6, '0');
        }
    }
}
