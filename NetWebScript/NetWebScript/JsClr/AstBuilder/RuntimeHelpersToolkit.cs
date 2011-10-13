using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.Ast;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace NetWebScript.JsClr.AstBuilder
{
    internal static class RuntimeHelpersToolkit
    {

        internal static void InitializeArray(int ilOffset, ArrayCreationExpression array, LiteralExpression literalExpr)
        {
            FieldInfo field = literalExpr.Value as FieldInfo;
            if (field == null)
            {
                throw new InvalidOperationException();
            }

            Type itemType = array.ItemType;
            if (itemType != typeof(int))
            {
                throw new NotImplementedException(itemType.FullName);
            }

            object data = (object)field.GetValue(null);
            int size = Marshal.SizeOf(data);
            byte[] rawdata = new byte[size];
            GCHandle handle = GCHandle.Alloc(rawdata, GCHandleType.Pinned);
            Marshal.StructureToPtr(data, handle.AddrOfPinnedObject(), false);
            handle.Free();

            array.Initialize = new List<Expression>();
            for (int i = 0; i < size / 4; ++i)
            {
                array.Initialize.Add(new LiteralExpression(ilOffset, BitConverter.ToInt32(rawdata, i * 4)));
            }
        }

    }
}
