using FluentAssertions;
using Moq;
using Profile.Application.Kafka.Producers;
using Profile.Application.UseCases.ProfileUseCases.Commands.DeletePermanently;
using Profile.Domain.Interfaces.Services;
using Shared.Messages.Profile;

namespace Profile.Tests.UnitTests.CommandsTests.ProfilesTests;

public class DeleteProfilesPermanentlyHandlerTests
{
    private readonly Mock<IDbCleanupService> _cleanupServiceMock;
    private readonly Mock<IProducerService> _producerServiceMock;
    private readonly DeleteProfilesPermanentlyHandler _handler;

    public DeleteProfilesPermanentlyHandlerTests()
    {
        _cleanupServiceMock = new Mock<IDbCleanupService>();
        _producerServiceMock = new Mock<IProducerService>();
        _handler = new DeleteProfilesPermanentlyHandler(_cleanupServiceMock.Object, _producerServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteOldRecordsAndProduceManyProfilesDeletedMessage()
    {
        var profileIds = new List<string> { "id1", "id2", "id3" };
        var command = new DeleteProfilesPermanentlyCommand(profileIds);

        await _handler.Handle(command, CancellationToken.None);

        _cleanupServiceMock.Verify(cs => cs.DeleteOldRecords(profileIds), Times.Once);
        _producerServiceMock.Verify(ps => ps.ProduceAsync(It.Is<ManyProfilesDeletedMessage>(m => m.ProfilesIds.SequenceEqual(profileIds))), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotThrowException_WhenNoProfilesAreProvided()
    {
        var profileIds = new List<string>();
        var command = new DeleteProfilesPermanentlyCommand(profileIds);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().NotThrowAsync();
        _cleanupServiceMock.Verify(cs => cs.DeleteOldRecords(profileIds), Times.Once);
        _producerServiceMock.Verify(ps => ps.ProduceAsync(It.Is<ManyProfilesDeletedMessage>(m => m.ProfilesIds.SequenceEqual(profileIds))), Times.Once);
    }
}