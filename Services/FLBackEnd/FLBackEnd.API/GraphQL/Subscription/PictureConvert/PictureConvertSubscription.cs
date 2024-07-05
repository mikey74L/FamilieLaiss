using System.Runtime.CompilerServices;
using FamilieLaissSharedObjects.Enums;
using FLBackEnd.API.Enums;
using FLBackEnd.Domain.Entities;
using FLBackEnd.Infrastructure.DatabaseContext;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace FLBackEnd.API.GraphQL.Subscription.PictureConvert;

[ExtendObjectType(typeof(Subscription))]
public class PictureConvertSubscription
{
    public async IAsyncEnumerable<PictureConvertStatus> OnPictureConvertStatusWaitingStream(
        [Service] IDbContextFactory<FamilieLaissDbContext> dbContextFactory,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        //Create source stream first so that no events are missed
        var sourceStream =
            await eventReceiver.SubscribeAsync<PictureConvertStatus>(
                nameof(SubscriptionType.PictureConvertStatusWaiting), cancellationToken).ConfigureAwait(false);

        //Create database context
        var databaseContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        //First send existing items from database
        var waitingItems =
            databaseContext.PictureConvertStatusEntries.Where(x =>
                    x.Status == EnumPictureConvertStatus.WaitingForConversion)
                .Include(nameof(PictureConvertStatus.UploadPicture));

        foreach (var waitingItem in waitingItems)
        {
            yield return waitingItem;
        }

        //Dispose database context
        await databaseContext.DisposeAsync().ConfigureAwait(false);

        //Then send new items from event stream
        await foreach (PictureConvertStatus statusItem in sourceStream.ReadEventsAsync()
                           .WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return statusItem;
        }
    }

    public async IAsyncEnumerable<PictureConvertStatus> OnPictureConvertStatusErrorStream(
        [Service] IDbContextFactory<FamilieLaissDbContext> dbContextFactory,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        //Create source stream first so that no events are missed
        var sourceStream =
            await eventReceiver.SubscribeAsync<PictureConvertStatus>(nameof(SubscriptionType.PictureConvertStatusError),
                cancellationToken).ConfigureAwait(false);

        //Create database context
        var databaseContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        //First send existing items from database
        var errorItems =
            databaseContext.PictureConvertStatusEntries.Where(x =>
                    x.Status == EnumPictureConvertStatus.ConvertedWithErrors)
                .Include(nameof(PictureConvertStatus.UploadPicture));

        foreach (var errorItem in errorItems)
        {
            yield return errorItem;
        }

        //Dispose database context
        await databaseContext.DisposeAsync().ConfigureAwait(false);

        //Then send new items from event stream
        await foreach (PictureConvertStatus statusItem in sourceStream.ReadEventsAsync()
                           .WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return statusItem;
        }
    }

    public async IAsyncEnumerable<PictureConvertStatus> OnPictureConvertStatusSuccessStream(
        [Service] IDbContextFactory<FamilieLaissDbContext> dbContextFactory,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        //Create source stream first so that no events are missed
        var sourceStream =
            await eventReceiver.SubscribeAsync<PictureConvertStatus>(
                nameof(SubscriptionType.PictureConvertStatusSuccess), cancellationToken).ConfigureAwait(false);

        //Create database context
        var databaseContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        //First send existing items from database
        var successItems =
            databaseContext.PictureConvertStatusEntries.Where(x =>
                    x.Status == EnumPictureConvertStatus.SucessfullyConverted)
                .Include(nameof(PictureConvertStatus.UploadPicture));

        foreach (var successItem in successItems)
        {
            yield return successItem;
        }

        //Dispose database context
        await databaseContext.DisposeAsync().ConfigureAwait(false);

        //Then send new items from event stream
        await foreach (PictureConvertStatus statusItem in sourceStream.ReadEventsAsync()
                           .WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return statusItem;
        }
    }

    public async IAsyncEnumerable<PictureConvertStatus> OnPictureConvertStatusCurrentStream(
        [Service] IDbContextFactory<FamilieLaissDbContext> dbContextFactory,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        //Create source stream first so that no events are missed
        var sourceStream =
            await eventReceiver.SubscribeAsync<PictureConvertStatus>(
                nameof(SubscriptionType.PictureConvertStatusCurrent), cancellationToken).ConfigureAwait(false);

        //Create database context
        var databaseContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        //First send existing items from database
        var currentItem =
            await databaseContext.PictureConvertStatusEntries.Where(x =>
                    x.Status != EnumPictureConvertStatus.SucessfullyConverted &&
                    x.Status != EnumPictureConvertStatus.ConvertedWithErrors &&
                    x.Status != EnumPictureConvertStatus.TransientError &&
                    x.Status != EnumPictureConvertStatus.WaitingForConversion)
                .Include(nameof(PictureConvertStatus.UploadPicture))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (currentItem != null)
        {
            yield return currentItem;
        }

        //Dispose database context
        await databaseContext.DisposeAsync().ConfigureAwait(false);

        //Then send new items from event stream
        await foreach (PictureConvertStatus statusItem in sourceStream.ReadEventsAsync()
                           .WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return statusItem;
        }
    }

    [GraphQLDescription("Subscription for new picture convert status items in waiting state")]
    [Subscribe(With = nameof(OnPictureConvertStatusWaitingStream))]
    public PictureConvertStatus OnPictureConvertStatusWaiting([EventMessage] PictureConvertStatus waitingStatusItem) =>
        waitingStatusItem;

    [GraphQLDescription("Subscription for new picture convert status items in error state")]
    [Subscribe(With = nameof(OnPictureConvertStatusErrorStream))]
    public PictureConvertStatus OnPictureConvertStatusError([EventMessage] PictureConvertStatus errorStatusItem) =>
        errorStatusItem;

    [GraphQLDescription("Subscription for new picture convert status items in success state")]
    [Subscribe(With = nameof(OnPictureConvertStatusSuccessStream))]
    public PictureConvertStatus OnPictureConvertStatusSuccess([EventMessage] PictureConvertStatus successStatusItem) =>
        successStatusItem;

    [GraphQLDescription("Subscription for convert status item for currently converted picture")]
    [Subscribe(With = nameof(OnPictureConvertStatusCurrentStream))]
    public PictureConvertStatus OnPictureConvertStatusCurrent([EventMessage] PictureConvertStatus currentStatusItem) =>
        currentStatusItem;
}