using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationB.Management.Common
{
    public static class MapperExtension
    {
        public static IList<TDestination> MapTo<TSource, TDestination>(this IList<TSource> source, Action<IMapperConfigurationExpression> action)
        {
            MapperConfiguration config = new MapperConfiguration(action);

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IList<TSource>, IList<TDestination>>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, Action<IMapperConfigurationExpression> action)
        {
            MapperConfiguration config = new MapperConfiguration(action);

            IMapper mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(source);
        }
    }
}
