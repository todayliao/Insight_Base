﻿using System;
using System.Collections.Generic;
using Insight.WS.Base.Common;
using Insight.WS.Base.Common.Entity;
using static Insight.WS.Base.Common.Util;

namespace Insight.WS.Base.Service
{

    public partial class BaseService : Iorganizations
    {
        /// <summary>
        /// 根据对象实体数据新增一个组织机构节点
        /// </summary>
        /// <param name="org">组织节点对象</param>
        /// <param name="index">原序号</param>
        /// <returns>JsonResult</returns>
        public JsonResult AddOrg(SYS_Organization org, int index)
        {
            Session session;
            var result = General.Authorize(out session, "88AC97EF-52A3-4F7F-8121-4C311206535F");
            if (!result.Successful) return result;

            return InsertData(session.UserId, org, index) ? result.Created() : result.DataBaseError();
        }

        /// <summary>
        /// 根据ID删除组织机构节点
        /// </summary>
        /// <param name="id">节点ID</param>
        /// <returns>JsonResult</returns>
        public JsonResult RemoveOrg(string id)
        {
            var result = General.Authorize("71803766-97FE-4E6E-82DB-D5C90D2B7004");
            if (!result.Successful) return result;

            Guid gid;
            if (!Guid.TryParse(id, out gid)) return result.InvalidGuid();

            return DeleteOrg(gid) ? result : result.DataBaseError();
        }

        /// <summary>
        /// 根据对象实体数据更新组织机构信息
        /// </summary>
        /// <param name="obj">组织节点对象</param>
        /// <param name="index">原序号</param>
        /// <returns>JsonResult</returns>
        public JsonResult UpdateOrg(SYS_Organization obj, int index)
        {
            var result = General.Authorize("542D5E28-8102-40C6-9C01-190D13DBF6C6");
            if (!result.Successful) return result;

            return Update(obj, index) ? result : result.DataBaseError();
        }

        /// <summary>
        /// 根据ID获取机构对象实体
        /// </summary>
        /// <param name="id">节点ID</param>
        /// <returns>JsonResult</returns>
        public JsonResult GetOrg(string id)
        {
            var result = General.Authorize("928C7527-A2F7-49A3-A548-12B3834D8822");
            if (!result.Successful) return result;

            Guid gid;
            if (!Guid.TryParse(id, out gid)) return result.InvalidGuid();

            var org = GetOrg(gid);
            return org == null ? result.NotFound() : result.Success(Serialize(org));
        }

        /// <summary>
        /// 获取组织机构树
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetOrgTree()
        {
            var result = General.Authorize("928C7527-A2F7-49A3-A548-12B3834D8822");
            if (!result.Successful) return result;

            var data = GetOrgList();
            return data.Rows.Count > 0 ? result.Success(Serialize(data)) : result.NoContent();
        }

        /// <summary>
        /// 根据对象实体数据新增一条组织机构节点合并记录
        /// </summary>
        /// <param name="org">组织节点合并对象</param>
        /// <returns>JsonResult</returns>
        public JsonResult AddOrgMerger(SYS_OrgMerger org)
        {
            Session session;
            var result = General.Authorize(out session, "DAE7F2C5-E379-4F74-8043-EB616D4A5F8B");
            if (!result.Successful) return result;

            return InsertData(session.UserId, org) ? result : result.DataBaseError();
        }

        /// <summary>
        /// 根据对象实体数据更新组织机构表ParentId字段
        /// </summary>
        /// <param name="org">组织节点对象</param>
        /// <returns>JsonResult</returns>
        public JsonResult SetOrgParent(SYS_Organization org)
        {
            var result = General.Authorize("DB1A4EA2-1B3E-41AD-91FA-A3945AB7D901");
            if (!result.Successful) return result;

            return Update(org) ? result : result.DataBaseError();
        }

        /// <summary>
        /// 根据参数组集合批量插入职位成员关系
        /// </summary>
        /// <param name="id">节点ID</param>
        /// <param name="uids">用户ID集合</param>
        /// <returns>JsonResult</returns>
        public JsonResult AddOrgMember(string id, List<Guid> uids)
        {
            Session session;
            var result = General.Authorize(out session, "1F29DDEA-A4D7-4EF9-8136-0D4AFE88CB08");
            if (!result.Successful) return result;

            Guid gid;
            if (!Guid.TryParse(id, out gid)) return result.InvalidGuid();

            return InsertData(session.UserId, gid, uids) ? result : result.DataBaseError();
        }

        /// <summary>
        /// 根据ID集合删除职位成员关系
        /// </summary>
        /// <param name="ids">职位成员关系ID集合</param>
        /// <returns>JsonResult</returns>
        public JsonResult RemoveOrgMember(List<Guid> ids)
        {
            var result = General.Authorize("70AC8EEB-F920-468D-8C8F-2DBA049ADAE9");
            if (!result.Successful) return result;

            return DeleteOrgMember(ids) ? result : result.DataBaseError();
        }

        /// <summary>
        /// 获取所有职位成员用户
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetOrgMembers()
        {
            var result = General.Authorize("928C7527-A2F7-49A3-A548-12B3834D8822");
            if (!result.Successful) return result;

            var data = GetOrgMemberList();
            return data.Rows.Count > 0 ? result.Success(Serialize(data)) : result.NoContent();
        }

        /// <summary>
        /// 获取职位成员之外的所有用户
        /// </summary>
        /// <param name="id">节点ID</param>
        public JsonResult GetOtherOrgMember(string id)
        {
            var result = General.Authorize("928C7527-A2F7-49A3-A548-12B3834D8822");
            if (!result.Successful) return result;

            Guid gid;
            if (!Guid.TryParse(id, out gid)) return result.InvalidGuid();

            var data = GetOrgMemberBeSides(gid);
            return data.Rows.Count > 0 ? result : result.NoContent();
        }

    }
}