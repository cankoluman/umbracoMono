﻿using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.EntityBase;
using Umbraco.Core.Models.Rdbms;
using Umbraco.Core.Persistence.Caching;
using Umbraco.Core.Persistence.Factories;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Core.Persistence.UnitOfWork;

namespace Umbraco.Core.Persistence.Repositories
{
    /// <summary>
    /// Represents a repository for doing CRUD operations for <see cref="IMediaType"/>
    /// </summary>
    internal class MediaTypeRepository : ContentTypeBaseRepository<int, IMediaType>, IMediaTypeRepository
    {
		public MediaTypeRepository(IDatabaseUnitOfWork work)
            : base(work)
        {
        }

		public MediaTypeRepository(IDatabaseUnitOfWork work, IRepositoryCacheProvider cache)
            : base(work, cache)
        {
        }

        #region Overrides of RepositoryBase<int,IMedia>

        protected override IMediaType PerformGet(int id)
        {
            var contentTypeSql = GetBaseQuery(false);
            contentTypeSql.Where(GetBaseWhereClause(), new { Id = id});

            var dto = Database.Query<ContentTypeDto, NodeDto>(contentTypeSql).FirstOrDefault();

            if (dto == null)
                return null;

            var factory = new MediaTypeFactory(NodeObjectTypeId);
            var contentType = factory.BuildEntity(dto);
            
            contentType.AllowedContentTypes = GetAllowedContentTypeIds(id);
            contentType.PropertyGroups = GetPropertyGroupCollection(id);

            var list = Database.Fetch<ContentType2ContentTypeDto>("WHERE childContentTypeId = @Id", new{ Id = id});
            foreach (var contentTypeDto in list)
            {
                bool result = contentType.AddContentType(Get(contentTypeDto.ParentId));
                //Do something if adding fails? (Should hopefully not be possible unless someone create a circular reference)
            }

            ((ICanBeDirty)contentType).ResetDirtyProperties();
            return contentType;
        }

        protected override IEnumerable<IMediaType> PerformGetAll(params int[] ids)
        {
            if (ids.Any())
            {
                foreach (var id in ids)
                {
                    yield return Get(id);
                }
            }
            else
            {
                var nodeDtos = Database.Fetch<NodeDto>("WHERE nodeObjectType = @NodeObjectType", new { NodeObjectType = NodeObjectTypeId });
                foreach (var nodeDto in nodeDtos)
                {
                    yield return Get(nodeDto.NodeId);
                }
            }
        }

        protected override IEnumerable<IMediaType> PerformGetByQuery(IQuery<IMediaType> query)
        {
            var sqlClause = GetBaseQuery(false);
            var translator = new SqlTranslator<IMediaType>(sqlClause, query);
            var sql = translator.Translate();

            var documentTypeDtos = Database.Fetch<ContentTypeDto, NodeDto>(sql);

            foreach (var dto in documentTypeDtos)
            {
                yield return Get(dto.NodeId);
            }
        }

        #endregion

        public IEnumerable<IMediaType> GetByQuery(IQuery<PropertyType> query)
        {
            var ints = PerformGetByQuery(query);
            foreach (var i in ints)
            {
                yield return Get(i);
            }
        }

        #region Overrides of PetaPocoRepositoryBase<int,IMedia>

        protected override Sql GetBaseQuery(bool isCount)
        {
            var sql = new Sql();
            sql.Select(isCount ? "COUNT(*)" : "*");
            sql.From("cmsContentType");
            sql.InnerJoin("umbracoNode ON (cmsContentType.nodeId = umbracoNode.id)");
            sql.Where("umbracoNode.nodeObjectType = @NodeObjectType", new { NodeObjectType = NodeObjectTypeId });
            return sql;
        }

        protected override string GetBaseWhereClause()
        {
            return "umbracoNode.id = @Id";
        }

        protected override IEnumerable<string> GetDeleteClauses()
        {
            var list = new List<string>
                           {
                               string.Format("DELETE FROM umbracoUser2NodeNotify WHERE nodeId = @Id"),
                               string.Format("DELETE FROM umbracoUser2NodePermission WHERE nodeId = @Id"),
                               string.Format("DELETE FROM cmsTagRelationship WHERE nodeId = @Id"),
                               string.Format("DELETE FROM cmsContentTypeAllowedContentType WHERE Id = @Id"),
                               string.Format("DELETE FROM cmsContentTypeAllowedContentType WHERE AllowedId = @Id"),
                               string.Format("DELETE FROM cmsContentType2ContentType WHERE parentContentTypeId = @Id"),
                               string.Format("DELETE FROM cmsContentType2ContentType WHERE childContentTypeId = @Id"),
                               string.Format("DELETE FROM cmsPropertyType WHERE contentTypeId = @Id"),
                               string.Format("DELETE FROM cmsPropertyTypeGroup WHERE contenttypeNodeId = @Id"),
                               string.Format("DELETE FROM cmsContentType WHERE NodeId = @Id"),
                               string.Format("DELETE FROM umbracoNode WHERE id = @Id")
                           };
            return list;
        }

        protected override Guid NodeObjectTypeId
        {
            get { return new Guid("4EA4382B-2F5A-4C2B-9587-AE9B3CF3602E"); }
        }

        #endregion

        #region Unit of Work Implementation

        protected override void PersistNewItem(IMediaType entity)
        {
            ((MediaType)entity).AddingEntity();

            var factory = new MediaTypeFactory(NodeObjectTypeId);
            var dto = factory.BuildDto(entity);

            PersistNewBaseContentType(dto, entity);

            ((ICanBeDirty)entity).ResetDirtyProperties();
        }

        protected override void PersistUpdatedItem(IMediaType entity)
        {
            //Updates Modified date
            ((MediaType)entity).UpdatingEntity();

            //Look up parent to get and set the correct Path if ParentId has changed
            if (((ICanBeDirty)entity).IsPropertyDirty("ParentId"))
            {
                var parent = Database.First<NodeDto>("WHERE id = @ParentId", new { ParentId = entity.ParentId });
                entity.Path = string.Concat(parent.Path, ",", entity.Id);
            }

            var factory = new MediaTypeFactory(NodeObjectTypeId);
            var dto = factory.BuildDto(entity);
            
            PersistUpdatedBaseContentType(dto, entity);

            ((ICanBeDirty)entity).ResetDirtyProperties();
        }

        #endregion
    }
}