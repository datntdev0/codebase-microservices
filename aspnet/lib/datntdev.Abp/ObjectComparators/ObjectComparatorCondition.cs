﻿using System;
using datntdev.Abp.Extensions;
using datntdev.Abp.Json;

namespace datntdev.Abp.ObjectComparators
{
    public class ObjectComparatorCondition<TValueType>
    {
        public string CompareType { get; set; }

        public string JsonValue { get; set; }

        public TValueType GetValue()
        {
            return JsonValue.IsNullOrWhiteSpace()
                    ? default
                    : JsonValue.FromJsonString<TValueType>();
        }

        public void SetValue(TValueType value)
        {
            JsonValue = value.ToJsonString();
        }
    }

    public class ObjectComparatorCondition<TValueType, TEnumCompareType>
        where TEnumCompareType : Enum
    {
        public TEnumCompareType CompareType { get; set; }

        public string JsonValue { get; set; }

        public TValueType GetValue()
        {
            return JsonValue.IsNullOrWhiteSpace()
                ? default
                : JsonValue.FromJsonString<TValueType>();
        }

        public void SetValue(TValueType value)
        {
            JsonValue = value.ToJsonString();
        }
    }
}
