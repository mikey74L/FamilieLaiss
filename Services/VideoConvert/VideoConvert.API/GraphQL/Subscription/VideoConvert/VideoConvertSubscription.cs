﻿using FamilieLaissSharedObjects.Enums;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using VideoConvert.API.Enums;
using VideoConvert.Domain.Entities;
using VideoConvert.Infrastructure.DBContext;

namespace VideoConvert.API.GraphQL.Subscription.VideoConvert;

[ExtendObjectType(typeof(Subscription))]
public class VideoConvertSubscription
{
    public async IAsyncEnumerable<VideoConvertStatus> OnVideoConvertStatusWaitingStream(
        [Service] IDbContextFactory<VideoConvertServiceDbContext> dbContextFactory,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        //Create source stream first so that no events are missed
        var sourceStream =
            await eventReceiver.SubscribeAsync<VideoConvertStatus>(
                nameof(SubscriptionType.VideoConvertStatusWaiting), cancellationToken).ConfigureAwait(false);

        //Create database context
        var databaseContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        //First send existing items from database
        var waitingItems =
            databaseContext.ConvertStatusEntries.Where(x =>
                    x.Status == EnumVideoConvertStatus.WaitingForConversion)
                .Include(nameof(VideoConvertStatus.UploadVideo));

        foreach (var waitingItem in waitingItems)
        {
            yield return waitingItem;
        }

        //Dispose database context
        await databaseContext.DisposeAsync().ConfigureAwait(false);

        //Then send new items from event stream
        await foreach (VideoConvertStatus statusItem in sourceStream.ReadEventsAsync()
                           .WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return statusItem;
        }
    }

    public async IAsyncEnumerable<VideoConvertStatus> OnVideoConvertStatusErrorStream(
        [Service] IDbContextFactory<VideoConvertServiceDbContext> dbContextFactory,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        //Create source stream first so that no events are missed
        var sourceStream =
            await eventReceiver.SubscribeAsync<VideoConvertStatus>(nameof(SubscriptionType.VideoConvertStatusError),
                cancellationToken).ConfigureAwait(false);

        //Create database context
        var databaseContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        //First send existing items from database
        var errorItems =
            databaseContext.ConvertStatusEntries.Where(x =>
                    x.Status == EnumVideoConvertStatus.ConvertedWithErrors)
                .Include(nameof(VideoConvertStatus.UploadVideo));

        foreach (var errorItem in errorItems)
        {
            yield return errorItem;
        }

        //Dispose database context
        await databaseContext.DisposeAsync().ConfigureAwait(false);

        //Then send new items from event stream
        await foreach (VideoConvertStatus statusItem in sourceStream.ReadEventsAsync()
                           .WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return statusItem;
        }
    }

    public async IAsyncEnumerable<VideoConvertStatus> OnVideoConvertStatusSuccessStream(
        [Service] IDbContextFactory<VideoConvertServiceDbContext> dbContextFactory,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        //Create source stream first so that no events are missed
        var sourceStream =
            await eventReceiver.SubscribeAsync<VideoConvertStatus>(
                nameof(SubscriptionType.VideoConvertStatusSuccess), cancellationToken).ConfigureAwait(false);

        //Create database context
        var databaseContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        //First send existing items from database
        var successItems =
            databaseContext.ConvertStatusEntries.Where(x =>
                    x.Status == EnumVideoConvertStatus.SucessfullyConverted)
                .Include(nameof(VideoConvertStatus.UploadVideo));

        foreach (var successItem in successItems)
        {
            yield return successItem;
        }

        //Dispose database context
        await databaseContext.DisposeAsync().ConfigureAwait(false);

        //Then send new items from event stream
        await foreach (VideoConvertStatus statusItem in sourceStream.ReadEventsAsync()
                           .WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return statusItem;
        }
    }

    public async IAsyncEnumerable<VideoConvertStatus> OnVideoConvertStatusCurrentStream(
        [Service] IDbContextFactory<VideoConvertServiceDbContext> dbContextFactory,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        //Create source stream first so that no events are missed
        var sourceStream =
            await eventReceiver.SubscribeAsync<VideoConvertStatus>(
                nameof(SubscriptionType.VideoConvertStatusCurrent), cancellationToken).ConfigureAwait(false);

        //Create database context
        var databaseContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

        //First send existing items from database
        var currentItem =
            await databaseContext.ConvertStatusEntries.Where(x =>
                    x.Status != EnumVideoConvertStatus.WaitingForConversion &&
                    x.Status != EnumVideoConvertStatus.SucessfullyConverted &&
                    x.Status != EnumVideoConvertStatus.ConvertedWithErrors &&
                    x.Status != EnumVideoConvertStatus.TransientError)
                .Include(nameof(VideoConvertStatus.UploadVideo))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (currentItem != null)
        {
            yield return currentItem;
        }

        //Dispose database context
        await databaseContext.DisposeAsync().ConfigureAwait(false);

        //Then send new items from event stream
        await foreach (VideoConvertStatus statusItem in sourceStream.ReadEventsAsync()
                           .WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            yield return statusItem;
        }
    }

    [GraphQLDescription("Subscription for new video convert status items in waiting state")]
    [Subscribe(With = nameof(OnVideoConvertStatusWaitingStream))]
    public VideoConvertStatus OnVideoConvertStatusWaiting([EventMessage] VideoConvertStatus waitingStatusItem) =>
        waitingStatusItem;

    [GraphQLDescription("Subscription for new video convert status items in error state")]
    [Subscribe(With = nameof(OnVideoConvertStatusErrorStream))]
    public VideoConvertStatus OnVideoConvertStatusError([EventMessage] VideoConvertStatus errorStatusItem) =>
        errorStatusItem;

    [GraphQLDescription("Subscription for new video convert status items in success state")]
    [Subscribe(With = nameof(OnVideoConvertStatusSuccessStream))]
    public VideoConvertStatus OnVideoConvertStatusSuccess([EventMessage] VideoConvertStatus successStatusItem) =>
        successStatusItem;

    [GraphQLDescription("Subscription for convert status item for currently converted picture")]
    [Subscribe(With = nameof(OnVideoConvertStatusCurrentStream))]
    public VideoConvertStatus OnVideoConvertStatusCurrent([EventMessage] VideoConvertStatus currentStatusItem) =>
        currentStatusItem;
}