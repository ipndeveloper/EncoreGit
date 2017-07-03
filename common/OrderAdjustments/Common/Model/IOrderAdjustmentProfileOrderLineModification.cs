﻿using NetSteps.Encore.Core.Dto;

namespace NetSteps.OrderAdjustments.Common.Model
{
    [DTO]
    public interface IOrderAdjustmentProfileOrderLineModification
    {
        string Description { get; set; }
        string Property { get; set; }
        int ModificationOperationID { get; set; }
        decimal? ModificationValue { get; set; }
    }
}