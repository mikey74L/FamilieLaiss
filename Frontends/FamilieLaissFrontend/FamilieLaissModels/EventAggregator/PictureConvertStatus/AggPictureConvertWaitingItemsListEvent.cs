﻿using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.PictureConvertStatus;

public class AggPictureConvertWaitingItemsListEvent
{
    public required IEnumerable<IPictureConvertStatusModel> Items { get; init; }
}