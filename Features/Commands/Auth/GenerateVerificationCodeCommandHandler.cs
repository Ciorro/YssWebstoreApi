using Dapper;
using MediatR;
using System.Data;
using System.Security.Cryptography;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class GenerateVerificationCodeCommandHandler : IRequestHandler<GenerateVerificationCodeCommand, bool>
    {
        private readonly IDbConnection _cn;
        private readonly TimeProvider _timeProvider;

        public GenerateVerificationCodeCommandHandler(IDbConnection dbConnection, TimeProvider timeProvider)
        {
            _cn = dbConnection;
            _timeProvider = timeProvider;
        }

        public async Task<bool> Handle(GenerateVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            var parameters = new
            {
                AccountId = request.AccountId,
                VerificationCode = GenerateCode(),
                ExpiresAt = _timeProvider.GetUtcNow().Add(TimeSpan.FromMinutes(20))
            };

            string sql = @"UPDATE credentials SET
                               VerificationCode = @VerificationCode,
                               VerificationCodeExpiresAt = @ExpiresAt
                           WHERE AccountId = @AccountId";

            return await _cn.ExecuteAsync(sql, parameters) == 1;
        }

        private string GenerateCode()
        {
            return RandomNumberGenerator.GetInt32(999999)
                .ToString()
                .PadLeft(6, '0');
        }
    }
}
