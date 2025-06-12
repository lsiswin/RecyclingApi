using System;
using System.Collections.Generic;

namespace RecyclingApi.Application.DTOs
{
    /// <summary>
    /// 分页响应的基类
    /// </summary>
    public class PagedResponseDto<T> where T : class
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPrevious => PageIndex > 1;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNext => PageIndex < TotalPages;

        /// <summary>
        /// 分页数据
        /// </summary>
        public IEnumerable<T> Items { get; set; }
    }
} 