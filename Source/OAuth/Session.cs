﻿using System;
using System.Linq;
using System.Threading;
using Insight.Base.Common;
using Insight.Base.Common.Entity;
using Insight.Utils.Common;
using Insight.Utils.Entity;

namespace Insight.Base.OAuth
{
    /// <summary>
    /// 用户会话信息
    /// </summary>
    public class Session:AccessToken
    {
        // 进程同步基元
        private static readonly Mutex _Mutex = new Mutex();

        // 用户签名
        private string _Signature;

        // 支付密码
        private string _PayPassword;

        // 刷新密码
        private string _RefreshKey;

        // 连续失败次数
        private int _FailureCount;

        // 上次连接时间
        private DateTime _LastConnectTime;

        // 最后一次验证通过的TokenId
        private Guid _LastConnectId;

        /// <summary>
        /// Secret过期时间
        /// </summary>
        public DateTime ExpiryTime { get; private set; }

        /// <summary>
        /// Secret失效时间
        /// </summary>
        public DateTime FailureTime { get; private set; }

        /// <summary>
        /// 用户在线状态
        /// </summary>
        public bool OnlineStatus { get; private set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 绑定的手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public bool Validity { get; set; }

        /// <summary>
        /// 构造方法，根据UserID构建对象
        /// </summary>
        /// <param name="uid">UserID</param>
        public Session(Guid uid)
        {
            using (var context = new BaseEntities())
            {
                var user = context.SYS_User.SingleOrDefault(s => s.ID == uid);
                if (user == null) return;

                _Signature = Util.Decrypt(Params.RSAKey, user.Password);
                _PayPassword = user.PayPassword;

                id = Guid.NewGuid();
                UserType = user.Type;
                account = user.LoginName;
                Mobile = user.Mobile;
                userId = user.ID;
                userName = user.Name;
                Validity = user.Validity;
            }
        }

        /// <summary>
        /// 产生Code(有效时间：3秒)
        /// </summary>
        /// <returns>string Code</returns>
        public string GenerateCode()
        {
            var tid = id;
            id = Guid.NewGuid();

            var code = tid.ToString("N");
            var sign = Util.Hash(_Signature + code);

            var db = Params.Redis.GetDatabase();
            db.StringSet(sign, tid.ToString(), TimeSpan.FromSeconds(3));
            return code;
        }

        /// <summary>
        /// 检验是否已经连续错误5次
        /// </summary>
        /// <param name="tokenId">TokenId</param>
        /// <returns>bool 是否已经连续错误5次</returns>
        public bool Ckeck(Guid tokenId)
        {
            if (tokenId == _LastConnectId) return true;

            var now = DateTime.Now;
            var span = now - _LastConnectTime;
            if (span.TotalMinutes > 15) _FailureCount = 0;

            _LastConnectTime = now;
            if (_FailureCount >= 5) return false;

            _LastConnectId = tokenId;
            return true;
        }

        /// <summary>
        /// 验证Token合法性
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="type">类型：1、验证Secret；2、验证RefreshKey</param>
        /// <returns>bool 是否通过验证</returns>
        public bool Verify(string key, int type)
        {
            if (type == 1 && secret == key || type == 2 && _RefreshKey == key) return true;

            _FailureCount++;
            return false;
        }

        /// <summary>
        /// 验证支付密码
        /// </summary>
        /// <param name="password">支付密码</param>
        /// <returns>bool 是否通过验证</returns>
        public bool? Verify(string password)
        {
            if (_PayPassword == null) return null;

            return Util.Hash(userId + password) == _PayPassword;
        }

        /// <summary>
        /// 设置Secret及过期时间
        /// </summary>
        /// <param name="force">是否强制</param>
        public void InitSecret(bool force = false)
        {
            var now = DateTime.Now;
            if (!force && now < FailureTime) return;

            _Mutex.WaitOne();
            if (now < FailureTime)
            {
                _Mutex.ReleaseMutex();
                return;
            }

            secret = Util.Hash(Guid.NewGuid() + _Signature + now);
            _RefreshKey = Util.Hash(Guid.NewGuid() + secret);
            ExpiryTime = now.AddHours(2);
            FailureTime = now.AddHours(Core.Expired);
            _Mutex.ReleaseMutex();
        }

        /// <summary>
        /// 刷新Secret过期时间
        /// </summary>
        public void Refresh()
        {
            var now = DateTime.Now;
            if (now < ExpiryTime) return;

            ExpiryTime = now.AddHours(2);
        }

        /// <summary>
        /// 使Session在线
        /// </summary>
        /// <param name="did">用户登录部门ID</param>
        public void Online(Guid? did)
        {
            deptId = did;
            OnlineStatus = true;
            _FailureCount = 0;
        }

        /// <summary>
        /// 注销Session
        /// </summary>
        public void SignOut()
        {
            if (!Params.SignOut) return;

            ExpiryTime = DateTime.Now;
            FailureTime = DateTime.Now;
            secret = Guid.NewGuid().ToString();
            OnlineStatus = false;
        }

        /// <summary>
        /// 生成用户签名
        /// </summary>
        /// <param name="password">用户密码</param>
        public void Sign(string password)
        {
            _Signature = Util.Hash(account.ToUpper() + password);
        }

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="password">支付密码</param>
        /// <returns></returns>
        public bool SetPayPW(string password)
        {
            var pw = Util.Hash(userId + password);
            if (pw == _PayPassword) return true;

            _PayPassword = pw;
            using (var context = new BaseEntities())
            {
                var user = context.SYS_User.SingleOrDefault(s => s.ID == userId);
                if (user == null) return false;

                user.PayPassword = _PayPassword;
                return context.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="tid">TokenID</param>
        /// <returns>string 序列化为Json的Token数据</returns>
        public object CreatorKey(Guid? tid = null)
        {
            if (!tid.HasValue) tid = id;

            var token = new
            {
                accessToken = Util.Base64(new {id = tid, userId, deptId, account, userName, secret}),
                refreshToken = Util.Base64(new {id = tid, account, Secret = _RefreshKey}),
                expiryTime = ExpiryTime,
                failureTime = FailureTime
            };

            return token;
        }

        /// <summary>
        /// 根据Account判断用户是否相同
        /// </summary>
        /// <param name="loginName">用户账号</param>
        /// <returns>bool 用户是否相同</returns>
        public bool UserIsSame(string loginName)
        {
            return Util.StringCompare(account, loginName);
        }
    }
}