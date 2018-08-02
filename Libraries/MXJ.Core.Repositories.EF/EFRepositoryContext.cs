using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using MXJ.Core.Domain.Repositories;
using MXJ.Core.Infrastructure.Engine;
using MXJ.Core.Infrastructure.Logging;
using System.Data.Entity.Validation;
using MXJ.Core.Domain.Models;
using MXJ.Core.Infrastructure.LifeTime;

namespace MXJ.Core.Repositories.EF
{
    public abstract class EfRepositoryContext : IRepositoryContext
    {
        /// <summary>
        /// 获取当前使用的EntityFramework数据访问上下文对象
        /// </summary>
        protected abstract DbContext Context { get; }

        /// <summary>
        /// 获取当前单元操作是否已被提交
        /// </summary>
        public bool IsCommitted { get; private set; }

        /// <summary>
        /// 对外公布的上下文
        /// </summary>
        public DbContext DbContext { get { return Context; } }

        /// <summary>
        /// 提交当前单元操作的结果
        /// </summary>
        /// <param name="validateOnSaveEnabled">保存时是否自动验证跟踪实体</param>
        /// <returns></returns>
        public int Commit()
        {
            ILogger logger = EngineContext.Current.DependencyContainer.Resolve<ILoggerFactory>().CreateLogger();
            if (IsCommitted)
            {
                return 0;
            }
            try
            {
                int result = Context.SaveChanges();
                IsCommitted = true;
                return result;
            }
            catch (DbEntityValidationException exc)
            {
                var error = LogDbEntityValidationErrors(exc);
                logger.Error(exc, error);
                throw;
            }
            catch (DbUpdateException exc)
            {
                var error = string.Empty;
                GetInnerExceptionErrorMessage(exc, ref error);
                logger.Error(exc, error);
                throw;
            }
            catch (Exception exc)
            {
                var error = string.Empty;
                GetInnerExceptionErrorMessage(exc, ref error);
                logger.Error(exc, error);
                throw;
            }
        }

        private void GetInnerExceptionErrorMessage(Exception exc, ref string error)
        {
            if (null != exc)
            {
                error += exc.Message + ";";
                GetInnerExceptionErrorMessage(exc.InnerException, ref error);
            }
        }

        public string LogDbEntityValidationErrors(DbEntityValidationException dbEx)
        {
            var errorStringBilder = new StringBuilder();
            foreach (var entityValidationError in dbEx.EntityValidationErrors)
            {
                errorStringBilder.Append(string.Format("Entity \"{0}\" in state \"{1}\", errors:",
                    entityValidationError.Entry.Entity.GetType().Name,
                    entityValidationError.Entry.State));

                foreach (var error in entityValidationError.ValidationErrors)
                {
                    errorStringBilder.Append(string.Format(" (Property: \"{0}\", Error: \"{1}\")",
                        error.PropertyName, error.ErrorMessage));
                }
            }

            return errorStringBilder.ToString();
        }

        /// <summary>
        /// 把当前单元操作回滚成未提交状态
        /// </summary>
        public void Rollback()
        {
            IsCommitted = false;
        }

        /// <summary>
        /// 释放前提交更改
        /// </summary>
        public void Dispose()
        {
            if (!IsCommitted)
            {
                Commit();
            }
            Context.Dispose();
        }

        /// <summary>
        ///   为指定的类型返回 System.Data.Entity.DbSet，这将允许对上下文中的给定实体执行 CRUD 操作。
        /// </summary>
        /// <typeparam name="TEntity"> 应为其返回一个集的实体类型。 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns> 给定实体类型的 System.Data.Entity.DbSet 实例。 </returns>
        public DbSet<TEntity> Set<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            return Context.Set<TEntity>();
        }

        /// <summary>
        /// 注册一个新的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterCreate<TEntity, TKey>(TEntity entity) where TEntity : BaseEntity<TKey>
        {
            entity.CreatedTime = DateTime.Now;
            Guid currentUserId = Guid.Empty;
            Guid.TryParse(PerRequestManager.GetValue("CurrentUserID") == null ? Guid.Empty.ToString() : PerRequestManager.GetValue("CurrentUserID").ToString(), out currentUserId);
            if (null == entity.CreatedUser || Guid.Empty == entity.CreatedUser || !entity.CreatedUser.HasValue)
            {
                entity.CreatedUser = currentUserId;
                //entity.CreatedUserPath = PerRequestManager.GetValue("CurrentUserPath") == null ? "" : PerRequestManager.GetValue("CurrentUserPath").ToString();
            }
            EntityState state = Context.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                Context.Entry(entity).State = EntityState.Added;
            }
            IsCommitted = false;
        }

        /// <summary>
        ///  注册一个更改的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterUpdate<TEntity, TKey>(TEntity entity) where TEntity : BaseEntity<TKey>
        {
            entity.UpdatedTime = DateTime.Now;
            if (null == entity.UpdatedUser || Guid.Empty == entity.UpdatedUser || !entity.UpdatedUser.HasValue)
            {
                Guid currentUserId = Guid.Empty;
                Guid.TryParse(PerRequestManager.GetValue("CurrentUserID") == null ? Guid.Empty.ToString() : PerRequestManager.GetValue("CurrentUserID").ToString(), out currentUserId);
                entity.UpdatedUser = currentUserId;
            }
            Context.Entry(entity).State = EntityState.Modified;
            IsCommitted = false;
        }

        /// <summary>
        /// 注册一个逻辑删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterDelete<TEntity, TKey>(TEntity entity) where TEntity : BaseEntity<TKey>
        {
            entity.UpdatedTime = DateTime.Now;
            if (null == entity.UpdatedUser || Guid.Empty == entity.UpdatedUser || !entity.UpdatedUser.HasValue)
            {
                Guid currentUserId = Guid.Empty;
                Guid.TryParse(PerRequestManager.GetValue("CurrentUserID") == null ? Guid.Empty.ToString() : PerRequestManager.GetValue("CurrentUserID").ToString(), out currentUserId);
                entity.UpdatedUser = currentUserId;
            }
            entity.IsDeleted = true;
            Context.Entry(entity).State = EntityState.Modified;
            IsCommitted = false;
        }

        /// <summary>
        /// 注册一个物理删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterPhysicalDelete<TEntity, TKey>(TEntity entity) where TEntity : BaseEntity<TKey>
        {
            Context.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
        }

        /// <summary>
        /// 分离实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        public virtual void DetachedEntity<TEntity, TKey>(TEntity entity) where TEntity : BaseEntity<TKey>
        {
            Context.Entry(entity).State = EntityState.Detached;
        }
    }
}
