using System;
using System.Collections.Generic;
using System.Text;

namespace AssetLibrary
{
    // 摘要:
    //     封装一个方法，该方法具有两个参数并且不返回值。
    //
    // 参数:
    //   arg1:
    //     此委托封装的方法的第一个参数。
    //
    //   arg2:
    //     此委托封装的方法的第二个参数。
    //
    // 类型参数:
    //   T1:
    //     此委托封装的方法的第一个参数类型。
    //
    //   T2:
    //     此委托封装的方法的第二个参数类型。
    public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);

    // 摘要:
    //     封装一个方法，该方法具有三个参数并且不返回值。
    //
    // 参数:
    //   arg1:
    //     此委托封装的方法的第一个参数。
    //
    //   arg2:
    //     此委托封装的方法的第二个参数。
    //
    //   arg3:
    //     此委托封装的方法的第三个参数。
    //
    // 类型参数:
    //   T1:
    //     此委托封装的方法的第一个参数类型。
    //
    //   T2:
    //     此委托封装的方法的第二个参数类型。
    //
    //   T3:
    //     此委托封装的方法的第三个参数类型。
    public delegate void Action<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
}
