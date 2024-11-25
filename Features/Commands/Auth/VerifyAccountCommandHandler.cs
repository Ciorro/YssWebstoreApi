using Dapper;
using MediatR;
using System.Data;
using YssWebstoreApi.Repositories.Abstractions;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class VerifyAccountCommandHandler : IRequestHandler<VerifyAccountCommand, bool>
    {
        private readonly IDbConnection _cn;
        private readonly ICredentialsRepository _credentials;
        private readonly TimeProvider _timeProvider;

        public VerifyAccountCommandHandler(IDbConnection cn, ICredentialsRepository credentials, TimeProvider timeProvider)
        {
            _cn = cn;
            _credentials = credentials;
            _timeProvider = timeProvider;
        }

        public async Task<bool> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
        {
            var credentials = await _credentials.GetByAccountIdAsync(request.AccountId);
            var codeExpiresAt = credentials?.VerificationCodeExpiresAt?.DateTime ?? DateTime.MinValue;
            var currentTime = _timeProvider.GetUtcNow().DateTime;

            if (credentials is null ||
                credentials.VerificationCode != request.VerificationCode ||
                codeExpiresAt < currentTime)
            {
                throw new UnauthorizedAccessException();
            }

            var parameters = new
            {
                Id = credentials.Id
            };

            string sql = @"UPDATE credentials SET
                               IsVerified=1,
                               VerificationCode=NULL,
                               VerificationCodeExpiresAt=NULL
                           WHERE credentials.Id=@Id";

            return await _cn.ExecuteAsync(sql, parameters) == 1;
        }
    }
}
