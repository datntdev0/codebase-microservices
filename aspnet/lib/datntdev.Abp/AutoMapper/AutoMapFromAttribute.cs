using System;
using datntdev.Abp.Collections.Extensions;
using AutoMapper;

namespace datntdev.Abp.AutoMapper;

public class AutoMapFromAttribute : AutoMapAttributeBase
{
    public MemberList MemberList { get; set; } = MemberList.Destination;

    public AutoMapFromAttribute(params Type[] targetTypes)
        : base(targetTypes)
    {

    }

    public AutoMapFromAttribute(MemberList memberList, params Type[] targetTypes)
        : this(targetTypes)
    {
        MemberList = memberList;
    }

    public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
    {
        if (TargetTypes.IsNullOrEmpty())
        {
            return;
        }

        foreach (var targetType in TargetTypes)
        {
            configuration.CreateAutoAttributeMaps(targetType, new[] { type }, MemberList);
        }
    }
}