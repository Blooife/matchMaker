using MediatR;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Interfaces;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.DeletePermanently;

public class DeleteProfilesPermanentlyHandler(IDbCleanupService _cleanupService, ProducerService _producerService) : IRequestHandler<DeleteProfilesPermanentlyCommand>
{
    public async Task Handle(DeleteProfilesPermanentlyCommand request, CancellationToken cancellationToken)
    {
        _cleanupService.DeleteOldRecords(request.Ids);
        await _producerService.ProduceAsync(new ManyProfilesDeletedMessage(request.Ids));
    }
}