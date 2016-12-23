﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Insight.Base.Common.Entity;
using Insight.Utils.Entity;

namespace Insight.Base.Services
{
    [ServiceContract]
    public interface IUsers
    {

        #region User

        /// <summary>
        /// 根据对象实体数据新增一个用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "POST", UriTemplate = "users", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult AddUser(SYS_User user);

        /// <summary>
        /// 根据ID删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "DELETE", UriTemplate = "users/{id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult RemoveUser(string id);

        /// <summary>
        /// 根据用户ID更新用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="user">用户数据对象</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "users/{id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult UpdateUserInfo(string id, SYS_User user);

        /// <summary>
        /// 根据ID获取用户对象实体
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>Result</returns>
        [WebGet(UriTemplate = "users/{id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Result GetUser(string id);

        /// <summary>
        /// 获取用户授权信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>Result</returns>
        [WebGet(UriTemplate = "users/{id}/permission", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Result GetUserPermission(string id);

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <param name="rows">每页行数</param>
        /// <param name="page">当前页</param>
        /// <returns>Result</returns>
        [WebGet(UriTemplate = "users?rows={rows}&page={page}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Result GetUsers(string rows, string page);

        /// <summary>
        /// 根据对象实体数据注册一个用户
        /// </summary>
        /// <param name="account">登录账号</param>
        /// <param name="user">用户对象</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "POST", UriTemplate = "users/{account}/signup", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult SignUp(string account, SYS_User user);

        /// <summary>
        /// 更新指定用户Session的签名
        /// </summary>
        /// <param name="account">登录账号</param>
        /// <param name="password">新密码</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "users/{account}/signature", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult UpdateSignature(string account, string password);

        /// <summary>
        /// 用户重置登录密码
        /// </summary>
        /// <param name="account">登录账号</param>
        /// <param name="password">新密码</param>
        /// <param name="code">短信验证码</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "users/{account}/resetpw", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult ResetSignature(string account, string password, string code);

        /// <summary>
        /// 根据用户ID设置用户状态
        /// </summary>
        /// <param name="account">登录账号</param>
        /// <param name="validity">可用状态</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "users/{account}/validity", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult SetUserStatus(string account, bool validity);

        /// <summary>
        /// 设置指定用户的登录状态为离线
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "DELETE", UriTemplate = "users/{account}/token", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult UserSignOut(string account);

        #endregion

        #region UserGroup

        /// <summary>
        /// 根据对象实体数据新增一个用户组
        /// </summary>
        /// <param name="group">用户组对象</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "POST", UriTemplate = "groups", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult AddGroup(SYS_UserGroup group);

        /// <summary>
        /// 根据ID删除用户组
        /// </summary>
        /// <param name="id">用户组ID</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "DELETE", UriTemplate = "groups/{id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult RemoveGroup(string id);

        /// <summary>
        /// 根据对象实体数据更新用户组信息
        /// </summary>
        /// <param name="id">用户组ID</param>
        /// <param name="group">用户组对象</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "PUT", UriTemplate = "groups/{id}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult UpdateGroup(string id, SYS_UserGroup group);

        /// <summary>
        /// 根据ID获取用户组对象实体
        /// </summary>
        /// <param name="id">用户组ID</param>
        /// <returns>JsonResult</returns>
        [WebGet(UriTemplate = "groups/{id}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        JsonResult GetGroup(string id);

        /// <summary>
        /// 获取全部用户组
        /// </summary>
        /// <returns>JsonResult</returns>
        [WebGet(UriTemplate = "groups", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        JsonResult GetGroups();

        /// <summary>
        /// 根据参数组集合批量插入用户组成员关系
        /// </summary>
        /// <param name="id">用户组ID</param>
        /// <param name="uids">用户ID集合</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "POST", UriTemplate = "groups/{id}/members", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult AddGroupMember(string id, List<Guid> uids);

        /// <summary>
        /// 根据ID集合删除用户组成员关系
        /// </summary>
        /// <param name="ids">户组成员关系ID集合</param>
        /// <returns>JsonResult</returns>
        [WebInvoke(Method = "DELETE", UriTemplate = "groups/members", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        JsonResult RemoveMember(List<Guid> ids);

        /// <summary>
        /// 获取全部用户组的所有成员信息
        /// </summary>
        /// <returns>JsonResult</returns>
        [WebGet(UriTemplate = "groups/members", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        JsonResult GetGroupMembers();

        /// <summary>
        /// 根据ID获取组成员之外的全部用户
        /// </summary>
        /// <param name="id">用户组ID</param>
        /// <returns>JsonResult</returns>
        [WebGet(UriTemplate = "groups/{id}/other", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        JsonResult GetOtherUser(string id);

        #endregion

    }
}
