using System;
using System.Collections.Generic;

namespace RecyclingApi.Application.DTOs
{
    /// <summary>
    /// 分页请求的基类
    /// </summary>
    public class PagedRequestDto
    {
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private const int MaxPageSize = 50;

        /// <summary>
        /// 当前页码（从1开始）
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value < 1 ? 1 : value;
        }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? 10 : value);
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortBy { get; set; } = "Id";

        /// <summary>
        /// 是否降序排序
        /// </summary>
        public bool IsDesc { get; set; } = false;
    }
} 