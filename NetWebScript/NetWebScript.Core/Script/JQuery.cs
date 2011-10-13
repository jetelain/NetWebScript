using System;
using System.Collections.Generic;

namespace NetWebScript.Script
{
    [Imported]
    public class JQueryEventArgs
    {
        public int PageX;
        public int PageY;
        public int Which;
        public void PreventDefault()
        {
        }
    }

    [Imported]
    public class JQueryOffset
    {
        public int Top;
        public int Left;
    }

    [Imported]
    public sealed class JQuery
    {
        private JQuery()
        {
        }


        [ScriptAlias("$.inArray")]
        public static int inArray(object element, Array array)
        {
            return Array.IndexOf(array,element);
        }

        [ScriptAlias("$")]
        public static JQuery Select(String selector)
        {
            throw new NotImplementedException();
        }

        [ScriptAlias("$")]
        public static JQuery Select(DOMElement element)
        {
            throw new NotImplementedException();
        }

        public int Length;
        public int Index(DOMElement element)
        {
            return 0;
        }

        public DOMElement Get(int index)
        {
            return null;
        }

        public JQuery AddClass(string className) { return new JQuery(); }
        public String Attr(string attribute) { return ""; }
        //public JQuery Attr(Dictionary properties) { return new JQuery(); }
        public JQuery Attr(string key, string value) { return new JQuery(); }
        //public JQuery Attr(string key, Callback handler) { return new JQuery(); }
        //public JQuery Attr(string key, Function function) { return new JQuery(); }
        public String Html() { return ""; }
        public JQuery Html(string val) { return new JQuery(); }
        public JQuery RemoveAttr(string name) { return new JQuery(); }
        public JQuery RemoveClass(string name) { return new JQuery(); }
        public String Text() { return ""; }
        public JQuery Text(string val) { return new JQuery(); }
        public JQuery ToggleClass(string className) { return new JQuery(); }
        public String Val() { return ""; }
        public JQuery Val(string val) { return new JQuery(); }



        //
        // Manipulation
        //
        public JQuery After(string content) { return new JQuery(); }
        public JQuery After(JQuery content) { return new JQuery(); }
        public JQuery Append(string content) { return new JQuery(); }
        public JQuery Append(DOMElement content) { return new JQuery(); }
        public JQuery Append(JQuery content) { return new JQuery(); }
        public JQuery AppendTo(string content) { return new JQuery(); }
        public JQuery AppendTo(JQuery content) { return new JQuery(); }
        public JQuery Before(string content) { return new JQuery(); }
        public JQuery Before(JQuery content) { return new JQuery(); }
        public JQuery Clone() { return new JQuery(); }
        public JQuery Clone(bool deep) { return new JQuery(); }
        public JQuery Empty() { return new JQuery(); }
        public JQuery InsertAfter(string content) { return new JQuery(); }
        public JQuery InsertAfter(JQuery content) { return new JQuery(); }
        public JQuery InsertBefore(string content) { return new JQuery(); }
        public JQuery InsertBefore(JQuery content) { return new JQuery(); }
        public JQuery Prepend(string content) { return new JQuery(); }
        public JQuery Prepend(JQuery content) { return new JQuery(); }
        public JQuery PrependTo(string content) { return new JQuery(); }
        public JQuery PrependTo(JQuery content) { return new JQuery(); }
        public JQuery Remove() { return new JQuery(); }
        public JQuery Remove(string expr) { return new JQuery(); }
        public JQuery ReplaceAll(string selector) { return new JQuery(); }
        public JQuery ReplaceWith(string content) { return new JQuery(); }
        public JQuery Wrap(string html) { return new JQuery(); }
        public JQuery Wrap(DOMElement element) { return new JQuery(); }
        public JQuery WrapInner(DOMElement element) { return new JQuery(); }
        public JQuery WrapInner(string html) { return new JQuery(); }
        public JQuery WrapAll(string html) { return new JQuery(); }

        //
        // Traversing
        //
        public JQuery Add(string expr) { return new JQuery(); } // expr can be html too
        public JQuery Add(DOMElement element) { return new JQuery(); }
        public JQuery Add(DOMElement[] elements) { return new JQuery(); }
        public JQuery AndSelf() { return new JQuery(); }
        public JQuery Children(string expr) { return new JQuery(); }
        public JQuery Closest(string expr) { return new JQuery(); }
        public JQuery Contains(string str) { return new JQuery(); }
        public JQuery Contents() { return new JQuery(); }
        public JQuery End() { return new JQuery(); }
        public JQuery Filter(string expression) { return new JQuery(); }
        //public JQuery Filter(Callback handler) { return new JQuery(); }
        //public JQuery Filter(Function function) { return new JQuery(); }
        public JQuery Find(string expr) { return new JQuery(); }
        public JQuery HasClass(string className) { return new JQuery(); }
        public bool   Is(string expr) { return false; }
        public JQuery Next(string expr) { return new JQuery(); }
        public JQuery NextAll(string expr) { return new JQuery(); }
        public JQuery Not(DOMElement element) { return new JQuery(); }
        public JQuery Not(string expr) { return new JQuery(); }
        public JQuery Not(JQuery jquery) { return new JQuery(); }
        public JQuery OffsetParent() { return new JQuery(); }
        public JQuery Parent(string expr) { return new JQuery(); }
        public JQuery Parents(string expr) { return new JQuery(); }
        public JQuery Prev(string expr) { return new JQuery(); }
        public JQuery PrevAll(string expr) { return new JQuery(); }
        public JQuery Siblings(string expr) { return new JQuery(); }

        //
        // CSS
        //
        public String Css(string name) { return ""; }
        //public JQuery Css(Dictionary map) { return new JQuery(); }
        public JQuery Css(string key, string value) { return new JQuery(); }
        public JQuery Css(string key, int value) { return new JQuery(); }
        public int Height() { return 0; }
        public JQuery Height(string val) { return new JQuery(); }
        public JQuery Height(int val) { return new JQuery(); }
        public int Width() { return 0; }
        public JQuery Width(string val) { return new JQuery(); }
        public JQuery Width(int val) { return new JQuery(); }

        public JQueryOffset Offset() { return new JQueryOffset(); }
        public int OuterHeight() { return 0; }
        public int OuterWidth() { return 0; }

        //[ScriptAlias("$.each")]
        //public static void Each(Object o, Function function) { }

        [ScriptAlias("$.each")]
        public static void Each(Array array, Action<int, object> handler)
        {
            int idx = 0;
            foreach (object o in array)
            {
                handler(idx, o);
                idx++;
            }
        }

        [ScriptAlias("$.each")]
        public static void Each<T>(List<T> list, Action<int,T> handler) {
            int idx = 0;
            foreach (T o in list)
            {
                handler(idx, o);
                idx++;
            }
        }

        //public JQuery Each(Function function) { return new JQuery(); }
        public JQuery Each(Action<int, DOMElement> handler) { return new JQuery(); }

        //public static Object Extend(Object target, Object prop1, Object prop2) { return new Object(); }
        //public static ArrayList Grep(ArrayList array, Function function, bool inv) { return new ArrayList(); }
        //public static ArrayList Grep(ArrayList array, Callback handler, bool inv) { return new ArrayList(); }
        //public static ArrayList Map(ArrayList array, Function function) { return new ArrayList(); }
        //public static ArrayList Map(ArrayList array, Callback handler) { return new ArrayList(); }
        //public static ArrayList Merge(ArrayList first, ArrayList second) { return new ArrayList(); }
        public JQuery Slice(int start, int end) { return new JQuery(); }

        [ScriptAlias("$.trim")]
        public static string Trim(string str) { return str.Trim(); }

        //
        // Events
        //
        public JQuery Bind(string eventName, Action<JQueryEventArgs> handler) { return this; }
        //public JQuery Bind(string eventName, Callback handler) { return this; }
        public JQuery Bind(string eventName, object data, Action<JQueryEventArgs> handler) { return this; }
        //public JQuery Bind(string eventName, Function fn) { return this; }
        //public JQuery Bind(string eventName, Object data, Function fn) { return this; }
        public JQuery Blur() { return new JQuery(); }
        //public JQuery Blur(Function fn) { return new JQuery(); }
        public JQuery Blur(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Change(Function fn) { return new JQuery(); }
        public JQuery Change(Action<JQueryEventArgs> handler) { return new JQuery(); }
        public JQuery Click() { return new JQuery(); }
        //public JQuery Click(Function fn) { return new JQuery(); }
        public JQuery Click(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery DblClick(Function fn) { return new JQuery(); }
        public JQuery DblClick(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Error(Function fn) { return new JQuery(); }
        public JQuery Error(Action<JQueryEventArgs> handler) { return new JQuery(); }
        public JQuery Focus() { return new JQuery(); }
        //public JQuery Focus(Function fn) { return new JQuery(); }
        public JQuery Focus(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Hover(Function over, Function fnOut) { return new JQuery(); }
        public JQuery Hover(Action<JQueryEventArgs> handlerOver, Action<JQueryEventArgs> handlerOut) { return new JQuery(); }
        //public JQuery Keydown(Function fn) { return new JQuery(); }
        public JQuery Keydown(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Keypress(Function fn) { return new JQuery(); }
        //public JQuery Keypress(DOMEventHandler handler) { return new JQuery(); }
        public JQuery Keypress(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Keyup(Function fn) { return new JQuery(); }
        public JQuery Keyup(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Load(Function fn) { return new JQuery(); }
        public JQuery Load(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Mousedown(Function fn) { return new JQuery(); }
        public JQuery Mousedown(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Mousemove(Function fn) { return new JQuery(); }
        public JQuery Mousemove(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Mouseout(Function fn) { return new JQuery(); }
        public JQuery Mouseout(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Mouseover(Function fn) { return new JQuery(); }
        public JQuery Mouseover(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Mouseup(Function fn) { return new JQuery(); }
        public JQuery Mouseup(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery One(string fnType, object data, Function fn) { return new JQuery(); }
        public JQuery One(string fnType, object data, Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Ready(Function fn) { return new JQuery(); }
        public JQuery Ready(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Resize(Function fn) { return new JQuery(); }
        public JQuery Resize(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Scroll(Function fn) { return new JQuery(); }
        public JQuery Scroll(Action<JQueryEventArgs> handler) { return new JQuery(); }
        public JQuery Select() { return new JQuery(); }
        //public JQuery Select(Function fn) { return new JQuery(); }
        public JQuery Select(Action<JQueryEventArgs> handler) { return new JQuery(); }
        public JQuery Submit() { return new JQuery(); }
        //public JQuery Submit(Function fn) { return new JQuery(); }
        public JQuery Submit(Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Toggle(Function even, Function odd) { return new JQuery(); }
        public JQuery Toggle(Action<JQueryEventArgs> handler) { return new JQuery(); }
        public JQuery Trigger(string fnType, Array data) { return new JQuery(); }
        //public JQuery Unbind(string fnType, Function fn) { return new JQuery(); }
        public JQuery Unbind(string fnType, Action<JQueryEventArgs> handler) { return new JQuery(); }
        //public JQuery Unbind(string eventName, Callback handler) { return this; }
        //public JQuery Unload(Function fn) { return new JQuery(); }
        public JQuery Unload(Action<JQueryEventArgs> handler) { return new JQuery(); }

        //
        // Effects
        //
        //public JQuery Animate(Dictionary hash, string speed) { return new JQuery(); }
        //public JQuery Animate(Dictionary hash, string speed, Callback handler) { return new JQuery(); }
        //public JQuery Animate(Dictionary hash, string speed, string easing, Function callback) { return new JQuery(); }
        //public JQuery Animate(Dictionary hash, int speed, string easing, Function callback) { return new JQuery(); }
        //public JQuery Animate(Dictionary hash, string speed, string easing, Callback handler) { return new JQuery(); }
        //public JQuery Animate(Dictionary hash, int speed, string easing, Callback handler) { return new JQuery(); }

        //public JQuery FadeIn(string speed, Function callback) { return new JQuery(); }
        //public JQuery FadeIn(int speed, Function callback) { return new JQuery(); }
        //public JQuery FadeIn(string speed, Callback handler) { return new JQuery(); }
        //public JQuery FadeIn(int speed, Callback handler) { return new JQuery(); }

        //public JQuery FadeOut(string speed, Function callback) { return new JQuery(); }
        //public JQuery FadeOut(int speed, Function callback) { return new JQuery(); }
        //public JQuery FadeOut(string speed, Callback handler) { return new JQuery(); }
        //public JQuery FadeOut(int speed, Callback handler) { return new JQuery(); }

        //public JQuery FadeTo(string speed, float opacity, Function callback) { return new JQuery(); }
        //public JQuery FadeTo(int speed, float opacity, Function callback) { return new JQuery(); }
        //public JQuery FadeTo(string speed, float opacity, Callback handler) { return new JQuery(); }
        //public JQuery FadeTo(int speed, float opacity, Callback handler) { return new JQuery(); }

        public JQuery Hide() { return new JQuery(); }
        //public JQuery Hide(string speed, Function callback) { return new JQuery(); }
        //public JQuery Hide(int speed, Function callback) { return new JQuery(); }
        //public JQuery Hide(string speed, Callback handler) { return new JQuery(); }
        //public JQuery Hide(int speed, Callback handler) { return new JQuery(); }

        public JQuery Show() { return new JQuery(); }
        //public JQuery Show(string speed, Function callback) { return new JQuery(); }
        //public JQuery Show(int speed, Function callback) { return new JQuery(); }
        //public JQuery Show(string speed, Callback handler) { return new JQuery(); }
        //public JQuery Show(int speed, Callback handler) { return new JQuery(); }

        //public JQuery SlideDown(string speed, Function callback) { return new JQuery(); }
        //public JQuery SlideDown(int speed, Function callback) { return new JQuery(); }
        //public JQuery SlideDown(string speed, Callback handler) { return new JQuery(); }
        //public JQuery SlideDown(int speed, Callback handler) { return new JQuery(); }

        //public JQuery SlideToggle(string speed, Function callback) { return new JQuery(); }
        //public JQuery SlideToggle(int speed, Function callback) { return new JQuery(); }
        //public JQuery SlideToggle(string speed, Callback handler) { return new JQuery(); }
        //public JQuery SlideToggle(int speed, Callback handler) { return new JQuery(); }

        //public JQuery SlideUp(string speed, Function callback) { return new JQuery(); }
        //public JQuery SlideUp(int speed, Function callback) { return new JQuery(); }
        //public JQuery SlideUp(string speed, Callback handler) { return new JQuery(); }
        //public JQuery SlideUp(int speed, Callback handler) { return new JQuery(); }
        public JQuery Toggle() { return new JQuery(); }

        //
        // Ajax
        //
        [ScriptAlias("$.ajax")]
        public static XMLHttpRequest Ajax(JQueryAjax properties) { throw new NotImplementedException(); }
        //public JQuery AjaxComplete(Function callback) { return new JQuery(); }
        //public JQuery AjaxComplete(Callback handler) { return new JQuery(); }
        //public JQuery AjaxError(Function callback) { return new JQuery(); }
        //public JQuery AjaxError(Callback handler) { return new JQuery(); }
        //public JQuery AjaxSend(Function callback) { return new JQuery(); }
        //public JQuery AjaxSend(Callback handler) { return new JQuery(); }
        [ScriptAlias("$.ajaxSetup")]
        public static void AjaxSetup(JQueryAjax map) { throw new NotImplementedException(); }
        //public JQuery AjaxStart(Function callback) { return new JQuery(); }
        //public JQuery AjaxStart(Callback handler) { return new JQuery(); }
        //public JQuery AjaxStop(Function callback) { return new JQuery(); }
        //public JQuery AjaxStop(Callback handler) { return new JQuery(); }
        //public JQuery AjaxSuccess(Function callback) { return new JQuery(); }
        //public JQuery AjaxSuccess(Callback handler) { return new JQuery(); }
        [ScriptAlias("$.ajaxTimeout")]
        public static void AjaxTimeout(int time) { throw new NotImplementedException(); }

        /*public static XMLHttpRequest Get(string url, Dictionary map, Function callback) { return new XMLHttpRequest(); }
        public static XMLHttpRequest GetIfModified(string url, Dictionary map, Function callback) { return new XMLHttpRequest(); }
        public static XMLHttpRequest GetJSON(string url, Dictionary map, Function callback) { return new XMLHttpRequest(); }
        public static XMLHttpRequest GetScript(string url, Function callback) { return new XMLHttpRequest(); }

        public static JQuery Load(string url, Dictionary parameters, Function callback) { return new JQuery(); }
        public static JQuery LoadIfModified(string url, Dictionary map, Function callback) { return new JQuery(); }
        public static XMLHttpRequest Post(string url, Dictionary map, Function callback) { return new XMLHttpRequest(); }
        public static string Serialize() { return new String(); }*/

        [ScriptAlias("$.isFunction")]
        public static bool IsFunction(object value)
        {
            return false;
        }

        internal static string Param(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
