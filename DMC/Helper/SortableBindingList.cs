using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;

namespace DMC.Helper
{
    /// <summary>
    /// 可排序绑定列表:继承绑定方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool _isSorted;
        private ListSortDirection _sortDirection;
        private PropertyDescriptor _sortProperty;
        /// <summary>
        /// 可排序绑定列表
        /// </summary>
        /// <param name="list"></param>
        public SortableBindingList(List<T> list)
            : base(list)
        {
        }
        /// <summary>
        /// 申请排序核心
        /// </summary>
        /// <param name="prop">提供抽象类的属性</param>
        /// <param name="direction">指定排序的方向</param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            if (PropertyComparer.CanSort(prop.PropertyType))
            {
                ((List<T>)Items).Sort(new PropertyComparer(prop, direction));
                _sortDirection = direction;
                _sortProperty = prop;
                _isSorted = true;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }
        /// <summary>
        /// 移除排序核心
        /// </summary>
        protected override void RemoveSortCore()
        {
            _isSorted = false;
            _sortProperty = null;
        }
        /// <summary>
        /// 已排序核心
        /// </summary>
        protected override bool IsSortedCore
        {
            get { return _isSorted; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return _sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return _sortProperty; }
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }
        /// <summary>
        /// 定义一个泛型类 PropertyComparer，继承自 Comparer<T>  
        /// </summary>
        internal class PropertyComparer : Comparer<T>
        {
            // 声明私有字段，用于比较器、排序方向、属性描述符和使用 ToString 的标志  
            private readonly IComparer _comparer; // 用于比较属性值的比较器  
            private readonly ListSortDirection _direction; // 排序方向（升序或降序）  
            private readonly PropertyDescriptor _prop; // 要比较的属性描述符  
            private readonly bool _useToString; // 标志，表示是否使用 ToString 进行比较  
            /// <summary>
            /// PropertyComparer 的构造函数，接受一个属性描述符和排序方向  
            /// </summary>
            /// <param name="prop"></param>
            /// <param name="direction"></param>
            /// <exception cref="MissingMemberException"></exception>
            public PropertyComparer(PropertyDescriptor prop, ListSortDirection direction)
            {
                // 检查属性类型是否可以赋值给泛型类型 T 
                if (!prop.ComponentType.IsAssignableFrom(typeof(T)))
                {
                    // 如果属性类型不兼容，则抛出异常
                    throw new MissingMemberException(typeof(T).Name, prop.Name);
                }
                // 断言属性类型可以通过 IComparable 或 ToString 进行比较  
                Debug.Assert(CanSort(prop.PropertyType), "Cannot use PropertyComparer unless it can be compared by IComparable or ToString");
                // 初始化属性和排序方向 
                _prop = prop;
                _direction = direction;
                // 检查属性类型是否可以使用 IComparable 进行排序
                if (CanSortWithIComparable(prop.PropertyType))
                {
                    // 获取属性类型的默认比较器  
                    var property = typeof(Comparer<>).MakeGenericType(new[] { prop.PropertyType }).GetTypeInfo().GetDeclaredProperty("Default");
                    _comparer = (IComparer)property.GetValue(null, null);// 设置比较器 
                    _useToString = false;// 设置标志为 false，因为我们使用 IComparable
                }
                else
                {
                    // 断言属性类型可以通过 ToString 进行排序  
                    Debug.Assert(
                        CanSortWithToString(prop.PropertyType),
                        "Cannot use PropertyComparer unless it can be compared by IComparable or ToString");
                    // 使用不区分大小写的字符串比较器  
                    _comparer = StringComparer.CurrentCultureIgnoreCase;
                    _useToString = true;// 设置标志为 true，因为我们使用 ToString  
                }
            }
            /// <summary>
            /// 重写 Compare 方法，用于比较两个类型为 T 的对象 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public override int Compare(T left, T right)
            {
                // 从左侧和右侧对象中获取属性值  
                var leftValue = _prop.GetValue(left);
                var rightValue = _prop.GetValue(right);
                // 如果使用 ToString，则将值转换为字符串
                if (_useToString)
                {
                    leftValue = leftValue != null ? leftValue.ToString() : null;// 将左侧值转换为字符串或 null  
                    rightValue = rightValue != null ? rightValue.ToString() : null; // 将右侧值转换为字符串或 null  
                }
                // 检查两个值是否都是字符串  
                if (leftValue.GetType() == typeof(String) && rightValue.GetType() == typeof(String))
                {
                    // 根据排序方向比较字符串 
                    return _direction == ListSortDirection.Ascending
                               ? new StringRankComparer().Compare(leftValue.ToString(), rightValue.ToString()) // 升序  
                               : new StringRankComparer().Compare(rightValue.ToString(), leftValue.ToString()); // 降序                   
                }
                else
                {
                    // 根据排序方向比较非字符串值
                    return _direction == ListSortDirection.Ascending
                               ? _comparer.Compare(leftValue, rightValue)// 升序  
                               : _comparer.Compare(rightValue, leftValue);// 降序  
                }
            }
            /// <summary>
            /// 静态方法，检查某个类型是否可以排序 
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static bool CanSort(Type type)
            {
                // 必须通过任一方法可排序  
                return CanSortWithToString(type) || CanSortWithIComparable(type);
            }
            /// <summary>
            /// 静态方法，检查某个类型是否可以通过 IComparable 排序  
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            private static bool CanSortWithIComparable(Type type)
            {
                // 检查是否实现了 IComparable 接口 
                return type.GetInterface("IComparable") != null ||
                       (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));// 检查是否为可空类型  
            }
            /// <summary>
            /// 静态方法，检查某个类型是否可以通过 ToString 排序  
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            private static bool CanSortWithToString(Type type)
            {
                // 检查类型是否是 XNode 或其子类  
                return type.Equals(typeof(XNode)) || type.IsSubclassOf(typeof(XNode));
            }
        }
    }
    ///<summary>
    ///主要用于字符串类型排序
    ///规则：1.如果字符串不是数字开头，且数字前存在不同字符，则按照不同字符前后决定排序
    ///规则：2.如果字符串不是数字开头，且数字前字符相同，则按照数字区域整体大小排序，若数字相同，则看后续字符比较
    ///规则：3.如果两个字符存在包含则按照数据长度对比
    ///</summary>
    public class StringRankComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentException("Can't be null");
            }

            if (string.IsNullOrEmpty(x.Trim()) && !string.IsNullOrEmpty(y.Trim()))
            {
                return 1;
            }
            else if (!string.IsNullOrEmpty(x.Trim()) && string.IsNullOrEmpty(y.Trim()))
            {
                return -1;
            }
            else if (string.IsNullOrEmpty(x.Trim()) && string.IsNullOrEmpty(y.Trim()))
            {
                return 0;
            }

            char[] arr1 = x.ToCharArray();
            char[] arr2 = y.ToCharArray();
            int i = 0, j = 0;
            while (i < arr1.Length && j < arr2.Length)
            {
                if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                {
                    string s1 = "", s2 = "";
                    while (i < arr1.Length && char.IsDigit(arr1[i]))
                    {
                        s1 += arr1[i];
                        i++;
                    }
                    while (j < arr2.Length && char.IsDigit(arr2[j]))
                    {
                        s2 += arr2[j];
                        j++;
                    }
                    if (int.TryParse(s1, out int is1) && int.TryParse(s2, out int is2))
                    {
                        if (is1 > is2)
                        {
                            return 1;
                        }
                        if (is1 < is2)
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        int result = string.Compare(s1, s2, true);
                        if (result != 0)
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    if (arr1[i] > arr2[j])
                    {
                        return 1;
                    }
                    else if (arr1[i] < arr2[j])
                    {
                        return -1;
                    }
                    i++;
                    j++;
                }
            }

            return arr1.Length == arr2.Length ? 0 : arr1.Length > arr2.Length ? 1 : -1;
        }
    }


}