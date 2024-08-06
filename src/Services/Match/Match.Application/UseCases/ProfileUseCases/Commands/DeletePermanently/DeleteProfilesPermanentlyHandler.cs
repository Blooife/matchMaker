using Match.Domain.Interfaces;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.DeletePermanently;

public class DeleteProfilesPermanentlyHandler(IDbCleanupService _cleanupService) : IRequestHandler<DeleteProfilesPermanentlyCommand>
{
    public async Task Handle(DeleteProfilesPermanentlyCommand request, CancellationToken cancellationToken)
    {
        await _cleanupService.DeleteOldRecordsAsync(request.Ids, cancellationToken);
    }
}