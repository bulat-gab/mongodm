﻿using Digicando.DomainHelper;
using Digicando.MongODM.ProxyModels;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.GeoJsonObjectModel.Serializers;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Digicando.MongODM.Serialization.Serializers
{
    public class GeoPointSerializer<TInModel> : SerializerBase<TInModel>
        where TInModel : class
    {
        private readonly MemberInfo latitudeMemberInfo;
        private readonly MemberInfo longitudeMemberInfo;
        private readonly GeoJsonPointSerializer<GeoJson2DGeographicCoordinates> pointSerializer;
        private readonly IProxyGenerator proxyGenerator;

        public GeoPointSerializer(
            Expression<Func<TInModel, double>> longitudeMember,
            Expression<Func<TInModel, double>> latitudeMember,
            IProxyGenerator proxyGenerator)
        {
            longitudeMemberInfo = ReflectionHelper.GetMemberInfoFromLambda(longitudeMember);
            latitudeMemberInfo = ReflectionHelper.GetMemberInfoFromLambda(latitudeMember);
            pointSerializer = new GeoJsonPointSerializer<GeoJson2DGeographicCoordinates>();
            this.proxyGenerator = proxyGenerator;
        }

        public override TInModel Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            // Deserialize point.
            var point = pointSerializer.Deserialize(context, args);
            if (point == null)
            {
                return null;
            }

            // Create model instance.
            var model = proxyGenerator.CreateInstance<TInModel>();

            // Copy data.
            ReflectionHelper.SetValue(model, longitudeMemberInfo, point.Coordinates.Values[0]);
            ReflectionHelper.SetValue(model, latitudeMemberInfo, point.Coordinates.Values[1]);

            return model;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TInModel value)
        {
            // Check null value.
            if (value == null)
            {
                pointSerializer.Serialize(context, args, null);
                return;
            }

            // Create point.
            var coordinate = new GeoJson2DGeographicCoordinates(
                (double)ReflectionHelper.GetValue(value, longitudeMemberInfo),
                (double)ReflectionHelper.GetValue(value, latitudeMemberInfo));
            var point = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(coordinate);

            // Serialize point.
            pointSerializer.Serialize(context, args, point);
        }
    }
}