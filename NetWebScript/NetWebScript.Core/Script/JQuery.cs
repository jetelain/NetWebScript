using System;
using System.Collections.Generic;
using NetWebScript.Script.HTML;
using NetWebScript.Script.Xml;

namespace NetWebScript.Script
{
    [Imported]
    public sealed class JQueryEvent
    {
        private JQueryEvent() { }
        /// <summary>
        /// The current DOM element within the event bubbling phase.
        /// </summary>
        [IntrinsicProperty]
        public IHTMLElement CurrentTarget { get; set; }

        /// <summary>
        /// The optional data passed to jQuery.fn.bind when the current executing handler was bound.
        /// </summary>
        [IntrinsicProperty]
        public object Data { get; set; }

        /// <summary>
        /// The element where the currently-called jQuery event handler was attached.
        /// </summary>
        [IntrinsicProperty]
        public IHTMLElement DelegateTarget { get; set; }

        /// <summary>
        /// The namespace specified when the event was triggered.
        /// </summary>
        [IntrinsicProperty]
        public string Namespace { get; set; }

        /// <summary>
        /// The mouse position relative to the left edge of the document.
        /// </summary>
        [IntrinsicProperty]
        public double PageX { get; set; }

        /// <summary>
        /// The mouse position relative to the top edge of the document.
        /// </summary>
        [IntrinsicProperty]
        public double PageY { get; set; }

        /// <summary>
        /// The other DOM element involved in the event, if any.
        /// </summary>
        [IntrinsicProperty]
        public IHTMLElement RelatedTarget { get; set; }

        /// <summary>
        /// The last value returned by an event handler that was triggered by this event, unless the value was undefined.
        /// </summary>
        [IntrinsicProperty]
        public object Result { get; set; }

        /// <summary>
        /// The DOM element that initiated the event.
        /// </summary>
        [IntrinsicProperty]
        public IHTMLElement Target { get; set; }

        /// <summary>
        /// The difference in milliseconds between the time the browser created the event and January 1, 1970.
        /// </summary>
        [IntrinsicProperty]
        public double TimeStamp { get; set; }

        /// <summary>
        /// Describes the nature of the event.
        /// </summary>
        [IntrinsicProperty]
        public string Type { get; set; }

        /// <summary>
        /// For key or button events, this attribute indicates the specific button or key that was pressed.
        /// </summary>
        [IntrinsicProperty]
        public double Which { get; set; }

        /// <summary>
        /// Returns whether event.preventDefault() was ever called on this event object.
        /// </summary>
        public bool IsDefaultPrevented()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Returns whether event.stopImmediatePropagation() was ever called on this event object.
        /// </summary>
        public bool IsImmediatePropagationStopped()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Returns whether event.stopPropagation() was ever called on this event object.
        /// </summary>
        public bool IsPropagationStopped()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// If this method is called, the default action of the event will not be triggered.
        /// </summary>
        public void PreventDefault()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Keeps the rest of the handlers from being executed and prevents the event from bubbling up the DOM tree.
        /// </summary>
        public object StopImmediatePropagation()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Prevents the event from bubbling up the DOM tree, preventing any parent handlers from being notified of the event.
        /// </summary>
        public object StopPropagation()
        {
            throw new PlatformNotSupportedException();
        }

    }
    [Imported]
    public sealed class Callbacks
    {
        private Callbacks() { }
        /// <summary>
        /// Add a callback or a collection of callbacks to a callback list.
        /// </summary>
        /// <param name="callbacks">A function, or array of functions, that are to be added to the callback list.</param>
        public void Add(Delegate callbacks)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Disable a callback list from doing anything more.
        /// </summary>
        public void Disable()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove all of the callbacks from a list.
        /// </summary>
        public void Empty()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Call all of the callbacks with the given arguments
        /// </summary>
        /// <param name="arguments">The argument or list of arguments to pass back to the callback list.</param>
        public void Fire(object arguments)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine if the callbacks have already been called at least once.
        /// </summary>
        public bool Fired()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Call all callbacks in a list with the given context and arguments.
        /// </summary>
        public void FireWith()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Call all callbacks in a list with the given context and arguments.
        /// </summary>
        /// <param name="args">An argument, or array of arguments, to pass to the callbacks in the list.</param>
        public void FireWith(object args)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Call all callbacks in a list with the given context and arguments.
        /// </summary>
        /// <param name="context">A reference to the context in which the callbacks in the list should be fired.</param>
        /// <param name="args">An argument, or array of arguments, to pass to the callbacks in the list.</param>
        public void FireWith(object context, object args)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine whether a supplied callback is in a list
        /// </summary>
        /// <param name="callback">The callback to search for.</param>
        public bool Has(Delegate callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Lock a callback list in its current state.
        /// </summary>
        public void Lock()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine if the callbacks list has been locked.
        /// </summary>
        public bool Locked()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a callback or a collection of callbacks from a callback list.
        /// </summary>
        /// <param name="callbacks">A function, or array of functions, that are to be removed from the callback list.</param>
        public void Remove(Delegate callbacks)
        {
            throw new PlatformNotSupportedException();
        }

    }
    [Imported(Name = "$", IgnoreNamespace=true)]
    public sealed partial class JQuery
    {
        private JQuery() { }
        /// <summary>
        /// Deprecated in jQuery 1.3 (see jQuery.support). States if the current page, in the user's browser, is being rendered using the W3C CSS Box Model.
        /// </summary>
        [IntrinsicProperty]
        public static bool BoxModel { get; set; }

        /// <summary>
        /// Contains flags for the useragent, read from navigator.userAgent. We recommend against using this property; please try to use feature detection instead (see jQuery.support). jQuery.browser may be moved to a plugin in a future release of jQuery.
        /// </summary>
        [IntrinsicProperty]
        public static object Browser { get; set; }

        /// <summary>
        /// The DOM node context originally passed to jQuery(); if none was passed then context will likely be the document.
        /// </summary>
        [IntrinsicProperty]
        public IHTMLElement Context { get; set; }

        /// <summary>
        /// Hook directly into jQuery to override how particular CSS properties are retrieved or set, normalize CSS property naming, or create custom properties.
        /// </summary>
        [IntrinsicProperty]
        public static object CssHooks { get; set; }

        /// <summary>
        /// A string containing the jQuery version number.
        /// </summary>
        [IntrinsicProperty]
        public string Jquery { get; set; }

        /// <summary>
        /// The number of elements in the jQuery object.
        /// </summary>
        [IntrinsicProperty]
        public double Length { get; set; }

        /// <summary>
        /// A collection of properties that represent the presence of different browser features or bugs.
        /// </summary>
        [IntrinsicProperty]
        public static object Support { get; set; }

        /// <summary>
        /// Add elements to the set of matched elements.
        /// </summary>
        /// <param name="selector">A string representing a selector expression to find additional elements to add to the set of matched elements.</param>
        /// <param name="context">The point in the document at which the selector should begin matching; similar to the context argument of the $(selector, context) method.</param>
        public JQuery Add(string selector, IHTMLElement context)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add elements to the set of matched elements.
        /// </summary>
        /// <param name="jQueryobject">An existing jQuery object to add to the set of matched elements.</param>
        public JQuery Add(JQuery jQueryobject)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add elements to the set of matched elements.
        /// </summary>
        /// <param name="elements">One or more elements to add to the set of matched elements.</param>
        public JQuery Add(IHTMLElement elements)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add elements to the set of matched elements.
        /// </summary>
        /// <param name="selector">A string representing a selector expression to find additional elements to add to the set of matched elements.</param>
        public JQuery Add(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Adds the specified class(es) to each of the set of matched elements.
        /// </summary>
        /// <param name="function">A function returning one or more space-separated class names to be added to the existing class name(s). Receives the index position of the element in the set and the existing class name(s) as arguments. Within the function, this refers to the current element in the set.</param>
        public JQuery AddClass(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Adds the specified class(es) to each of the set of matched elements.
        /// </summary>
        /// <param name="className">One or more class names to be added to the class attribute of each matched element.</param>
        public JQuery AddClass(string className)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, after each element in the set of matched elements.
        /// </summary>
        /// <param name="content">HTML string, DOM element, or jQuery object to insert after each element in the set of matched elements.</param>
        public JQuery After(string content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, after each element in the set of matched elements.
        /// </summary>
        /// <param name="content">HTML string, DOM element, or jQuery object to insert after each element in the set of matched elements.</param>
        public JQuery After(IHTMLElement content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, after each element in the set of matched elements.
        /// </summary>
        /// <param name="content">HTML string, DOM element, or jQuery object to insert after each element in the set of matched elements.</param>
        public JQuery After(JQuery content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, after each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A function that returns an HTML string, DOM element(s), or jQuery object to insert after each element in the set of matched elements. Receives the index position of the element in the set as an argument. Within the function, this refers to the current element in the set.</param>
        public JQuery After(Func<int, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Perform an asynchronous HTTP (Ajax) request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="settings">A set of key/value pairs that configure the Ajax request. All settings are optional. A default can be set for any option with $.ajaxSetup(). See jQuery.ajax( settings ) below for a complete list of all settings.</param>
        public static XMLHttpRequest Ajax(string url, object settings)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Perform an asynchronous HTTP (Ajax) request.
        /// </summary>
        /// <param name="settings">A set of key/value pairs that configure the Ajax request. All settings are optional. A default can be set for any option with $.ajaxSetup().</param>
        public static XMLHttpRequest Ajax(JQueryAjaxSettings settings)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Perform an asynchronous HTTP (Ajax) request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        public static XMLHttpRequest Ajax(string url)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Register a handler to be called when Ajax requests complete. This is an Ajax Event.
        /// </summary>
        /// <param name="handler">The function to be invoked.</param>
        public JQuery AjaxComplete(Func<object, XMLHttpRequest, object, object> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Register a handler to be called when Ajax requests complete with an error. This is an Ajax Event.
        /// </summary>
        /// <param name="handler">The function to be invoked.</param>
        public JQuery AjaxError(Func<object, XMLHttpRequest, object, object, object> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Handle custom Ajax options or modify existing options before each request is sent and before they are processed by $.ajax().
        /// </summary>
        /// <param name="handler">A handler to set default values for future Ajax requests.</param>
        public static void AjaxPrefilter(Func<object, object, XMLHttpRequest, object> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Handle custom Ajax options or modify existing options before each request is sent and before they are processed by $.ajax().
        /// </summary>
        /// <param name="dataTypes">An optional string containing one or more space-separated dataTypes</param>
        /// <param name="handler">A handler to set default values for future Ajax requests.</param>
        public static void AjaxPrefilter(string dataTypes, Func<object, object, XMLHttpRequest, object> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a function to be executed before an Ajax request is sent. This is an Ajax Event.
        /// </summary>
        /// <param name="handler">The function to be invoked.</param>
        public JQuery AjaxSend(Func<object, XMLHttpRequest, object, object> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set default values for future Ajax requests.
        /// </summary>
        /// <param name="options">A set of key/value pairs that configure the default Ajax request. All options are optional.</param>
        public static object AjaxSetup(JQueryAjaxSettings options)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Register a handler to be called when the first Ajax request begins. This is an Ajax Event.
        /// </summary>
        /// <param name="handler">The function to be invoked.</param>
        public JQuery AjaxStart(Func<object> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Register a handler to be called when all Ajax requests have completed. This is an Ajax Event.
        /// </summary>
        /// <param name="handler">The function to be invoked.</param>
        public JQuery AjaxStop(Func<object> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a function to be executed whenever an Ajax request completes successfully. This is an Ajax Event.
        /// </summary>
        /// <param name="handler">The function to be invoked.</param>
        public JQuery AjaxSuccess(Func<object, XMLHttpRequest, object, object> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add the previous set of elements on the stack to the current set.
        /// </summary>
        public JQuery AndSelf()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Perform a custom animation of a set of CSS properties.
        /// </summary>
        /// <param name="properties">A map of CSS properties that the animation will move toward.</param>
        /// <param name="complete">A function to call once the animation is complete.</param>
        public JQuery Animate(object properties, Delegate complete)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Perform a custom animation of a set of CSS properties.
        /// </summary>
        /// <param name="properties">A map of CSS properties that the animation will move toward.</param>
        public JQuery Animate(object properties)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Perform a custom animation of a set of CSS properties.
        /// </summary>
        /// <param name="properties">A map of CSS properties that the animation will move toward.</param>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="complete">A function to call once the animation is complete.</param>
        public JQuery Animate(object properties, string duration, string easing, Delegate complete)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Perform a custom animation of a set of CSS properties.
        /// </summary>
        /// <param name="properties">A map of CSS properties that the animation will move toward.</param>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="complete">A function to call once the animation is complete.</param>
        public JQuery Animate(object properties, double duration, string easing, Delegate complete)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Perform a custom animation of a set of CSS properties.
        /// </summary>
        /// <param name="properties">A map of CSS properties that the animation will move toward.</param>
        /// <param name="options">A map of additional options to pass to the method. Supported keys:				duration: A string or number determining how long the animation will run.easing: A string indicating which easing function to use for the transition.complete: A function to call once the animation is complete.step: A function to be called after each step of the animation.queue: A Boolean indicating whether to place the animation in the effects queue. If false, the animation will begin immediately. As of jQuery 1.7, the queue option can also accept a string, in which case the animation is added to the queue represented by that string.specialEasing: A map of one or more of the CSS properties defined by the properties argument and their corresponding easing functions (added 1.4).</param>
        public JQuery Animate(object properties, object options)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Perform a custom animation of a set of CSS properties.
        /// </summary>
        /// <param name="properties">A map of CSS properties that the animation will move toward.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="complete">A function to call once the animation is complete.</param>
        public JQuery Animate(object properties, string easing, Delegate complete)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, to the end of each element in the set of matched elements.
        /// </summary>
        /// <param name="content">DOM element, HTML string, or jQuery object to insert at the end of each element in the set of matched elements.</param>
        public JQuery Append(string content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, to the end of each element in the set of matched elements.
        /// </summary>
        /// <param name="content">DOM element, HTML string, or jQuery object to insert at the end of each element in the set of matched elements.</param>
        public JQuery Append(JQuery content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, to the end of each element in the set of matched elements.
        /// </summary>
        /// <param name="content">DOM element, HTML string, or jQuery object to insert at the end of each element in the set of matched elements.</param>
        public JQuery Append(IHTMLElement content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, to the end of each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A function that returns an HTML string, DOM element(s), or jQuery object to insert at the end of each element in the set of matched elements. Receives the index position of the element in the set and the old HTML value of the element as arguments. Within the function, this refers to the current element in the set.</param>
        public JQuery Append(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements to the end of the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the end of the element(s) specified by this parameter.</param>
        public JQuery AppendTo(JQuery target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements to the end of the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the end of the element(s) specified by this parameter.</param>
        public JQuery AppendTo(string target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements to the end of the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the end of the element(s) specified by this parameter.</param>
        public JQuery AppendTo(IHTMLElement target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the value of an attribute for the first element in the set of matched elements.
        /// </summary>
        /// <param name="attributeName">The name of the attribute to get.</param>
        public string Attr(string attributeName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more attributes for the set of matched elements.
        /// </summary>
        /// <param name="attributeName">The name of the attribute to set.</param>
        /// <param name="value">A value to set for the attribute.</param>
        public JQuery Attr(string attributeName, double value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more attributes for the set of matched elements.
        /// </summary>
        /// <param name="attributeName">The name of the attribute to set.</param>
        /// <param name="function">A function returning the value to set. this is the current element. Receives the index position of the element in the set and the old attribute value as arguments.</param>
        public JQuery Attr(string attributeName, Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more attributes for the set of matched elements.
        /// </summary>
        /// <param name="attributeName">The name of the attribute to set.</param>
        /// <param name="value">A value to set for the attribute.</param>
        public JQuery Attr(string attributeName, string value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more attributes for the set of matched elements.
        /// </summary>
        /// <param name="map">A map of attribute-value pairs to set.</param>
        public JQuery Attr(object map)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, before each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A function that returns an HTML string, DOM element(s), or jQuery object to insert before each element in the set of matched elements. Receives the index position of the element in the set as an argument. Within the function, this refers to the current element in the set.</param>
        public JQuery Before(Delegate function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, before each element in the set of matched elements.
        /// </summary>
        /// <param name="content">HTML string, DOM element, or jQuery object to insert before each element in the set of matched elements.</param>
        public JQuery Before(string content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, before each element in the set of matched elements.
        /// </summary>
        /// <param name="content">HTML string, DOM element, or jQuery object to insert before each element in the set of matched elements.</param>
        public JQuery Before(JQuery content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, before each element in the set of matched elements.
        /// </summary>
        /// <param name="content">HTML string, DOM element, or jQuery object to insert before each element in the set of matched elements.</param>
        public JQuery Before(IHTMLElement content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements.
        /// </summary>
        /// <param name="eventType">A string containing one or more DOM event types, such as "click" or "submit," or custom event names.</param>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="preventBubble">Setting the third argument to false will attach a function that prevents the default action from occurring and stops the event from bubbling. The default is true.</param>
        public JQuery Bind(string eventType, object eventData, bool preventBubble)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements.
        /// </summary>
        /// <param name="events">A map of one or more DOM event types and functions to execute for them.</param>
        public JQuery Bind(object events)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements.
        /// </summary>
        /// <param name="eventType">A string containing one or more DOM event types, such as "click" or "submit," or custom event names.</param>
        /// <param name="preventBubble">Setting the third argument to false will attach a function that prevents the default action from occurring and stops the event from bubbling. The default is true.</param>
        public JQuery Bind(string eventType, bool preventBubble)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements.
        /// </summary>
        /// <param name="eventType">A string containing one or more DOM event types, such as "click" or "submit," or custom event names.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Bind(string eventType, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements.
        /// </summary>
        /// <param name="eventType">A string containing one or more DOM event types, such as "click" or "submit," or custom event names.</param>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Bind(string eventType, object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "blur" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Blur(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "blur" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Blur()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "blur" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Blur(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// A multi-purpose callbacks list object that provides a powerful way to manage callback lists.
        /// </summary>
        /// <param name="flags">An optional list of space-separated flags that change how the callback list behaves.</param>
        public static object Callbacks(string flags)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "change" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Change(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "change" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Change()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "change" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Change(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the children of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Children(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the children of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        public JQuery Children()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove from the queue all items that have not yet been run.
        /// </summary>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        public JQuery ClearQueue(string queueName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove from the queue all items that have not yet been run.
        /// </summary>
        public JQuery ClearQueue()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "click" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Click(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "click" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Click(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "click" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Click()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Create a deep copy of the set of matched elements.
        /// </summary>
        public JQuery Clone()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Create a deep copy of the set of matched elements.
        /// </summary>
        /// <param name="withDataAndEvents">A Boolean indicating whether event handlers should be copied along with the elements. As of jQuery 1.4, element data will be copied as well.</param>
        public JQuery Clone(bool withDataAndEvents)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Create a deep copy of the set of matched elements.
        /// </summary>
        /// <param name="withDataAndEvents">A Boolean indicating whether event handlers and data should be copied along with the elements. The default value is false. *In jQuery 1.5.0 the default value was incorrectly true; it was changed back to false in 1.5.1 and up.</param>
        /// <param name="deepWithDataAndEvents">A Boolean indicating whether event handlers and data for all children of the cloned element should be copied. By default its value matches the first argument's value (which defaults to false).</param>
        public JQuery Clone(bool withDataAndEvents, bool deepWithDataAndEvents)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the first element that matches the selector, beginning at the current element and progressing up through the DOM tree.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        /// <param name="context">A DOM element within which a matching element may be found. If no context is passed in then the context of the jQuery set will be used instead.</param>
        public JQuery Closest(string selector, IHTMLElement context)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the first element that matches the selector, beginning at the current element and progressing up through the DOM tree.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Closest(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Gets an array of all the elements and selectors matched against the current element up through the DOM tree.
        /// </summary>
        /// <param name="selectors">An array or string containing a selector expression to match elements against (can also be a jQuery object).</param>
        /// <param name="context">A DOM element within which a matching element may be found. If no context is passed in then the context of the jQuery set will be used instead.</param>
        public Array Closest(Array selectors, IHTMLElement context)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Gets an array of all the elements and selectors matched against the current element up through the DOM tree.
        /// </summary>
        /// <param name="selectors">An array or string containing a selector expression to match elements against (can also be a jQuery object).</param>
        public Array Closest(Array selectors)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the first element that matches the selector, beginning at the current element and progressing up through the DOM tree.
        /// </summary>
        /// <param name="element">An element to match elements against.</param>
        public JQuery Closest(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the first element that matches the selector, beginning at the current element and progressing up through the DOM tree.
        /// </summary>
        /// <param name="jQueryobject">A jQuery object to match elements against.</param>
        public JQuery Closest(JQuery jQueryobject)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Check to see if a DOM element is within another DOM element.
        /// </summary>
        /// <param name="container">The DOM element that may contain the other element.</param>
        /// <param name="contained">The DOM element that may be contained by the other element.</param>
        public static bool Contains(IHTMLElement container, IHTMLElement contained)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the children of each element in the set of matched elements, including text and comment nodes.
        /// </summary>
        public JQuery Contents()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more CSS properties for the  set of matched elements.
        /// </summary>
        /// <param name="map">A map of property-value pairs to set.</param>
        public JQuery Css(object map)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more CSS properties for the  set of matched elements.
        /// </summary>
        /// <param name="propertyName">A CSS property name.</param>
        /// <param name="function">A function returning the value to set. this is the current element. Receives the index position of the element in the set and the old value as arguments.</param>
        public JQuery Css(string propertyName, Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more CSS properties for the  set of matched elements.
        /// </summary>
        /// <param name="propertyName">A CSS property name.</param>
        /// <param name="value">A value to set for the property.</param>
        public JQuery Css(string propertyName, double value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the value of a style property for the first element in the set of matched elements.
        /// </summary>
        /// <param name="propertyName">A CSS property.</param>
        public string Css(string propertyName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more CSS properties for the  set of matched elements.
        /// </summary>
        /// <param name="propertyName">A CSS property name.</param>
        /// <param name="value">A value to set for the property.</param>
        public JQuery Css(string propertyName, string value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Returns value at named data store for the element, as set by jQuery.data(element, name, value), or the full data store for the element.
        /// </summary>
        /// <param name="element">The DOM element to query for the data.</param>
        public static object Data(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Returns value at named data store for the element, as set by jQuery.data(element, name, value), or the full data store for the element.
        /// </summary>
        /// <param name="element">The DOM element to query for the data.</param>
        /// <param name="key">Name of the data stored.</param>
        public static object Data(IHTMLElement element, string key)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Store arbitrary data associated with the matched elements.
        /// </summary>
        /// <param name="obj">An object of key-value pairs of data to update.</param>
        public JQuery Data(object obj)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Returns value at named data store for the first element in the jQuery collection, as set by data(name, value).
        /// </summary>
        /// <param name="key">Name of the data stored.</param>
        public object Data(string key)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Store arbitrary data associated with the matched elements.
        /// </summary>
        /// <param name="key">A string naming the piece of data to set.</param>
        /// <param name="value">The new data value; it can be any Javascript type including Array or Object.</param>
        public JQuery Data(string key, object value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Store arbitrary data associated with the specified element. Returns the value that was set.
        /// </summary>
        /// <param name="element">The DOM element to associate with the data.</param>
        /// <param name="key">A string naming the piece of data to set.</param>
        /// <param name="value">The new data value.</param>
        public static object Data(IHTMLElement element, string key, object value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Returns value at named data store for the first element in the jQuery collection, as set by data(name, value).
        /// </summary>
        public object Data()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "dblclick" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Dblclick(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "dblclick" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Dblclick(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "dblclick" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Dblclick()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set a timer to delay execution of subsequent items in the queue.
        /// </summary>
        /// <param name="duration">An integer indicating the number of milliseconds to delay execution of the next item in the queue.</param>
        public JQuery Delay(int duration)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set a timer to delay execution of subsequent items in the queue.
        /// </summary>
        /// <param name="duration">An integer indicating the number of milliseconds to delay execution of the next item in the queue.</param>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        public JQuery Delay(int duration, string queueName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to one or more events for all elements that match the selector, now or in the future, based on a specific set of root elements.
        /// </summary>
        /// <param name="selector">A selector to filter the elements that trigger the event.</param>
        /// <param name="eventType">A string containing one or more space-separated JavaScript event types, such as "click" or "keydown," or custom event names.</param>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute at the time the event is triggered.</param>
        public JQuery Delegate(string selector, string eventType, object eventData, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to one or more events for all elements that match the selector, now or in the future, based on a specific set of root elements.
        /// </summary>
        /// <param name="selector">A selector to filter the elements that trigger the event.</param>
        /// <param name="events">A map of one or more event types and functions to execute for them.</param>
        public JQuery Delegate(string selector, object events)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to one or more events for all elements that match the selector, now or in the future, based on a specific set of root elements.
        /// </summary>
        /// <param name="selector">A selector to filter the elements that trigger the event.</param>
        /// <param name="eventType">A string containing one or more space-separated JavaScript event types, such as "click" or "keydown," or custom event names.</param>
        /// <param name="handler">A function to execute at the time the event is triggered.</param>
        public JQuery Delegate(string selector, string eventType, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Execute the next function on the queue for the matched elements.
        /// </summary>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        public JQuery Dequeue(string queueName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Execute the next function on the queue for the matched elements.
        /// </summary>
        public JQuery Dequeue()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Execute the next function on the queue for the matched element.
        /// </summary>
        /// <param name="element">A DOM element from which to remove and execute a queued function.</param>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        public static JQuery Dequeue(IHTMLElement element, string queueName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Execute the next function on the queue for the matched element.
        /// </summary>
        /// <param name="element">A DOM element from which to remove and execute a queued function.</param>
        public static JQuery Dequeue(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove the set of matched elements from the DOM.
        /// </summary>
        public JQuery Detach()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove the set of matched elements from the DOM.
        /// </summary>
        /// <param name="selector">A selector expression that filters the set of matched elements to be removed.</param>
        public JQuery Detach(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove an event handler previously attached using .live() from the elements.
        /// </summary>
        /// <param name="eventTypes">A map of one or more event types, such as click or keydown and their corresponding functions that are no longer to be executed.</param>
        public JQuery Die(object eventTypes)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove all event handlers previously attached using .live() from the elements.
        /// </summary>
        public JQuery Die()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove an event handler previously attached using .live() from the elements.
        /// </summary>
        /// <param name="eventType">A string containing a JavaScript event type, such as click or keydown.</param>
        /// <param name="handler">The function that is no longer to be executed.</param>
        public JQuery Die(string eventType, string handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove an event handler previously attached using .live() from the elements.
        /// </summary>
        /// <param name="eventType">A string containing a JavaScript event type, such as click or keydown.</param>
        public JQuery Die(string eventType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Iterate over a jQuery object, executing a function for each matched element.
        /// </summary>
        /// <param name="function">A function to execute for each matched element.</param>
        public JQuery Each(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// A generic iterator function, which can be used to seamlessly iterate over both objects and arrays. Arrays and array-like objects with a length property (such as a function's arguments object) are iterated by numeric index, from 0 to length-1. Other objects are iterated via their named properties.
        /// </summary>
        /// <param name="collection">The object or array to iterate over.</param>
        /// <param name="callback">The function that will be executed on every object.</param>
        public static object Each(object collection, Func<object, object, object> callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove all child nodes of the set of matched elements from the DOM.
        /// </summary>
        public JQuery Empty()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// End the most recent filtering operation in the current chain and return the set of matched elements to its previous state.
        /// </summary>
        public JQuery End()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to the one at the specified index.
        /// </summary>
        /// <param name="index">An integer indicating the 0-based position of the element.</param>
        public JQuery Eq(int index)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "error" JavaScript event.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Error(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Takes a string and throws an exception containing it.
        /// </summary>
        /// <param name="message">The message to send out.</param>
        public static object Error(string message)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "error" JavaScript event.
        /// </summary>
        /// <param name="handler">A function to execute when the event is triggered.</param>
        public JQuery Error(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Merge the contents of two or more objects together into the first object.
        /// </summary>
        /// <param name="target">An object that will receive the new properties if additional objects are passed in or that will extend the jQuery namespace if it is the sole argument.</param>
        public static object Extend(object target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Merge the contents of two or more objects together into the first object.
        /// </summary>
        /// <param name="target">An object that will receive the new properties if additional objects are passed in or that will extend the jQuery namespace if it is the sole argument.</param>
        /// <param name="objectN">Additional objects containing properties to merge in.</param>
        public static object Extend(object target, object objectN)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Merge the contents of two or more objects together into the first object.
        /// </summary>
        /// <param name="deep">If true, the merge becomes recursive (aka. deep copy).</param>
        /// <param name="target">The object to extend. It will receive the new properties.</param>
        /// <param name="object1">An object containing additional properties to merge in.</param>
        /// <param name="objectN">Additional objects containing properties to merge in.</param>
        public static object Extend(bool deep, object target, object object1, object objectN)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Merge the contents of two or more objects together into the first object.
        /// </summary>
        /// <param name="target">An object that will receive the new properties if additional objects are passed in or that will extend the jQuery namespace if it is the sole argument.</param>
        /// <param name="object1">An object containing additional properties to merge in.</param>
        /// <param name="objectN">Additional objects containing properties to merge in.</param>
        public static object Extend(object target, object object1, object objectN)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements by fading them to opaque.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeIn(string duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements by fading them to opaque.
        /// </summary>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeIn(Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements by fading them to opaque.
        /// </summary>
        public JQuery FadeIn()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements by fading them to opaque.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeIn(double duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements by fading them to opaque.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeIn(string duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements by fading them to opaque.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeIn(double duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements by fading them to transparent.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeOut(string duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements by fading them to transparent.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeOut(double duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements by fading them to transparent.
        /// </summary>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeOut(Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements by fading them to transparent.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeOut(string duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements by fading them to transparent.
        /// </summary>
        public JQuery FadeOut()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements by fading them to transparent.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeOut(double duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Adjust the opacity of the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="opacity">A number between 0 and 1 denoting the target opacity.</param>
        public JQuery FadeTo(double duration, double opacity)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Adjust the opacity of the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="opacity">A number between 0 and 1 denoting the target opacity.</param>
        public JQuery FadeTo(string duration, double opacity)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Adjust the opacity of the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="opacity">A number between 0 and 1 denoting the target opacity.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeTo(string duration, double opacity, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Adjust the opacity of the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="opacity">A number between 0 and 1 denoting the target opacity.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeTo(string duration, double opacity, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Adjust the opacity of the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="opacity">A number between 0 and 1 denoting the target opacity.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeTo(double duration, double opacity, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Adjust the opacity of the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="opacity">A number between 0 and 1 denoting the target opacity.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeTo(double duration, double opacity, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements by animating their opacity.
        /// </summary>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeToggle(string easing, Delegate callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements by animating their opacity.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeToggle(string duration, string easing, Delegate callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements by animating their opacity.
        /// </summary>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeToggle(Delegate callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements by animating their opacity.
        /// </summary>
        public JQuery FadeToggle()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements by animating their opacity.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery FadeToggle(double duration, string easing, Delegate callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to those that match the selector or pass the function's test.
        /// </summary>
        /// <param name="function">A function used as a test for each element in the set. this is the current DOM element.</param>
        public JQuery Filter(Func<int, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to those that match the selector or pass the function's test.
        /// </summary>
        /// <param name="element">An element to match the current set of elements against.</param>
        public JQuery Filter(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to those that match the selector or pass the function's test.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match the current set of elements against.</param>
        public JQuery Filter(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to those that match the selector or pass the function's test.
        /// </summary>
        /// <param name="jQueryobject">An existing jQuery object to match the current set of elements against.</param>
        public JQuery Filter(object jQueryobject)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the descendants of each element in the current set of matched elements, filtered by a selector, jQuery object, or element.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Find(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the descendants of each element in the current set of matched elements, filtered by a selector, jQuery object, or element.
        /// </summary>
        /// <param name="jQueryobject">A jQuery object to match elements against.</param>
        public JQuery Find(object jQueryobject)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the descendants of each element in the current set of matched elements, filtered by a selector, jQuery object, or element.
        /// </summary>
        /// <param name="element">An element to match elements against.</param>
        public JQuery Find(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to the first in the set.
        /// </summary>
        public JQuery First()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "focus" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Focus(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "focus" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Focus(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "focus" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Focus()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "focusin" event.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Focusin(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "focusin" event.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Focusin(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "focusout" JavaScript event.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Focusout(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "focusout" JavaScript event.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Focusout(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP GET request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        public static XMLHttpRequest Get(string url)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP GET request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="success">A callback function that is executed if the request succeeds.</param>
        /// <param name="dataType">The type of data expected from the server. Default: Intelligent Guess (xml, json, script, or html).</param>
        public static XMLHttpRequest Get(string url, Func<object, string, XMLHttpRequest, object> success, string dataType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Retrieve the DOM elements matched by the jQuery object.
        /// </summary>
        public object Get()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP GET request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="dataType">The type of data expected from the server. Default: Intelligent Guess (xml, json, script, or html).</param>
        public static XMLHttpRequest Get(string url, string dataType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Retrieve the DOM elements matched by the jQuery object.
        /// </summary>
        /// <param name="index">A zero-based integer indicating which element to retrieve.</param>
        public object Get(double index)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP GET request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="data">A map or string that is sent to the server with the request.</param>
        /// <param name="success">A callback function that is executed if the request succeeds.</param>
        /// <param name="dataType">The type of data expected from the server. Default: Intelligent Guess (xml, json, script, or html).</param>
        public static XMLHttpRequest Get(string url, string data, Func<object, string, XMLHttpRequest, object> success, string dataType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP GET request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="data">A map or string that is sent to the server with the request.</param>
        /// <param name="success">A callback function that is executed if the request succeeds.</param>
        /// <param name="dataType">The type of data expected from the server. Default: Intelligent Guess (xml, json, script, or html).</param>
        public static XMLHttpRequest Get(string url, object data, Func<object, string, XMLHttpRequest, object> success, string dataType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load JSON-encoded data from the server using a GET HTTP request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="data">A map or string that is sent to the server with the request.</param>
        /// <param name="success">A callback function that is executed if the request succeeds.</param>
        public static XMLHttpRequest GetJSON(string url, object data, Func<object, string, XMLHttpRequest, object> success)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load JSON-encoded data from the server using a GET HTTP request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="success">A callback function that is executed if the request succeeds.</param>
        public static XMLHttpRequest GetJSON(string url, Func<object, string, XMLHttpRequest, object> success)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load JSON-encoded data from the server using a GET HTTP request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        public static XMLHttpRequest GetJSON(string url)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load a JavaScript file from the server using a GET HTTP request, then execute it.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="success">A callback function that is executed if the request succeeds.</param>
        public static XMLHttpRequest GetScript(string url, Func<object, string, object> success)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load a JavaScript file from the server using a GET HTTP request, then execute it.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        public static XMLHttpRequest GetScript(string url)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Execute some JavaScript code globally.
        /// </summary>
        /// <param name="code">The JavaScript code to execute.</param>
        public static object GlobalEval(string code)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Finds the elements of an array which satisfy a filter function. The original array is not affected.
        /// </summary>
        /// <param name="array">The array to search through.</param>
        /// <param name="function">The function to process each item against.  The first argument to the function is the item, and the second argument is the index.  The function should return a Boolean value.  this will be the global window object.</param>
        public static Array Grep(Array array, Func<object, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Finds the elements of an array which satisfy a filter function. The original array is not affected.
        /// </summary>
        /// <param name="array">The array to search through.</param>
        /// <param name="function">The function to process each item against.  The first argument to the function is the item, and the second argument is the index.  The function should return a Boolean value.  this will be the global window object.</param>
        /// <param name="invert">If "invert" is false, or not provided, then the function returns an array consisting of all elements for which "callback" returns true.  If "invert" is true, then the function returns an array consisting of all elements for which "callback" returns false.</param>
        public static Array Grep(Array array, Func<object, object, object> function, bool invert)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to those that have a descendant that matches the selector or DOM element.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Has(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to those that have a descendant that matches the selector or DOM element.
        /// </summary>
        /// <param name="contained">A DOM element to match elements against.</param>
        public JQuery Has(IHTMLElement contained)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine whether any of the matched elements are assigned the given class.
        /// </summary>
        /// <param name="className">The class name to search for.</param>
        public bool HasClass(string className)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine whether an element has any jQuery data associated with it.
        /// </summary>
        /// <param name="element">A DOM element to be checked for data.</param>
        public static bool HasData(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current computed height for the first element in the set of matched elements.
        /// </summary>
        public int Height()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the CSS height of every matched element.
        /// </summary>
        /// <param name="value">An integer representing the number of pixels, or an integer with an optional unit of measure appended (as a string).</param>
        public JQuery Height(string value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the CSS height of every matched element.
        /// </summary>
        /// <param name="value">An integer representing the number of pixels, or an integer with an optional unit of measure appended (as a string).</param>
        public JQuery Height(double value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the CSS height of every matched element.
        /// </summary>
        /// <param name="function">A function returning the height to set. Receives the index position of the element in the set and the old height as arguments. Within the function, this refers to the current element in the set.</param>
        public JQuery Height(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Hide(double duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Hide(string duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Hide(double duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements.
        /// </summary>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Hide(Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements.
        /// </summary>
        public JQuery Hide()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        public JQuery Hide(string duration)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        public JQuery Hide(double duration)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Hide(string duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Holds or releases the execution of jQuery's ready event.
        /// </summary>
        /// <param name="hold">Indicates whether the ready hold is being requested or released</param>
        public static void HoldReady(bool hold)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind two handlers to the matched elements, to be executed when the mouse pointer enters and leaves the elements.
        /// </summary>
        /// <param name="handlerIn">A function to execute when the mouse pointer enters the element.</param>
        /// <param name="handlerOut">A function to execute when the mouse pointer leaves the element.</param>
        public JQuery Hover(Func<JQueryEvent, object> handlerIn, Func<JQueryEvent, object> handlerOut)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind a single handler to the matched elements, to be executed when the mouse pointer enters or leaves the elements.
        /// </summary>
        /// <param name="handlerInOut">A function to execute when the mouse pointer enters or leaves the element.</param>
        public JQuery Hover(Func<JQueryEvent, object> handlerInOut)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the HTML contents of each element in the set of matched elements.
        /// </summary>
        /// <param name="htmlString">A string of HTML to set as the content of each matched element.</param>
        public JQuery Html(string htmlString)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the HTML contents of the first element in the set of matched elements.
        /// </summary>
        public string Html()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the HTML contents of each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A function returning the HTML content to set. Receives the index position of the element in the set and the old HTML value as arguments. jQuery empties the element before calling the function; use the oldhtml argument to reference the previous content. Within the function, this refers to the current element in the set.</param>
        public JQuery Html(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Search for a specified value within an array and return its index (or -1 if not found).
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <param name="array">An array through which to search.</param>
        public static double InArray(object value, Array array)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Search for a specified value within an array and return its index (or -1 if not found).
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <param name="array">An array through which to search.</param>
        /// <param name="fromIndex">The index of the array at which to begin the search. The default is 0, which will search the whole array.</param>
        public static double InArray(object value, Array array, double fromIndex)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Search for a given element from among the matched elements.
        /// </summary>
        /// <param name="element">The DOM element or first element within the jQuery object to look for.</param>
        public double Index(JQuery element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Search for a given element from among the matched elements.
        /// </summary>
        /// <param name="element">The DOM element or first element within the jQuery object to look for.</param>
        public double Index(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Search for a given element from among the matched elements.
        /// </summary>
        public double Index()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Search for a given element from among the matched elements.
        /// </summary>
        /// <param name="selector">A selector representing a jQuery collection in which to look for an element.</param>
        public double Index(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current computed height for the first element in the set of matched elements, including padding but not border.
        /// </summary>
        public int InnerHeight()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current computed width for the first element in the set of matched elements, including padding but not border.
        /// </summary>
        public int InnerWidth()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements after the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted after the element(s) specified by this parameter.</param>
        public JQuery InsertAfter(JQuery target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements after the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted after the element(s) specified by this parameter.</param>
        public JQuery InsertAfter(string target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements after the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted after the element(s) specified by this parameter.</param>
        public JQuery InsertAfter(IHTMLElement target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements before the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted before the element(s) specified by this parameter.</param>
        public JQuery InsertBefore(IHTMLElement target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements before the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted before the element(s) specified by this parameter.</param>
        public JQuery InsertBefore(string target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements before the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted before the element(s) specified by this parameter.</param>
        public JQuery InsertBefore(JQuery target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Check the current matched set of elements against a selector, element, or jQuery object and return true if at least one of these elements matches the given arguments.
        /// </summary>
        /// <param name="element">An element to match the current set of elements against.</param>
        public bool Is(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Check the current matched set of elements against a selector, element, or jQuery object and return true if at least one of these elements matches the given arguments.
        /// </summary>
        /// <param name="jQueryobject">An existing jQuery object to match the current set of elements against.</param>
        public bool Is(object jQueryobject)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Check the current matched set of elements against a selector, element, or jQuery object and return true if at least one of these elements matches the given arguments.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public bool Is(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Check the current matched set of elements against a selector, element, or jQuery object and return true if at least one of these elements matches the given arguments.
        /// </summary>
        /// <param name="function">A function used as a test for the set of elements. It accepts one argument, index, which is the element's index in the jQuery collection.Within the function, this refers to the current DOM element.</param>
        public bool Is(Func<int, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine whether the argument is an array.
        /// </summary>
        /// <param name="obj">Object to test whether or not it is an array.</param>
        public static bool IsArray(object obj)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Check to see if an object is empty (contains no properties).
        /// </summary>
        /// <param name="object">The object that will be checked to see if it's empty.</param>
        public static bool IsEmptyObject(object @object)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine if the argument passed is a Javascript function object.
        /// </summary>
        /// <param name="obj">Object to test whether or not it is a function.</param>
        public static bool IsFunction(object obj)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determines whether its argument is a number.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        public static bool IsNumeric(object value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Check to see if an object is a plain object (created using "{}" or "new Object").
        /// </summary>
        /// <param name="object">The object that will be checked to see if it's a plain object.</param>
        public static bool IsPlainObject(object @object)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine whether the argument is a window.
        /// </summary>
        /// <param name="obj">Object to test whether or not it is a window.</param>
        public static bool IsWindow(object obj)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Check to see if a DOM node is within an XML document (or is an XML document).
        /// </summary>
        /// <param name="node">The DOM node that will be checked to see if it's in an XML document.</param>
        public static bool IsXMLDoc(IHTMLElement node)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Accepts a string containing a CSS selector which is then used to match a set of elements.
        /// </summary>
        /// <param name="selector">A string containing a selector expression</param>
        /// <param name="context">A DOM Element, Document, or jQuery to use as context</param>
        [ScriptAlias("$")]
        public static JQuery Query(string selector, IHTMLElement context)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Accepts a string containing a CSS selector which is then used to match a set of elements.
        /// </summary>
        /// <param name="elementArray">An array containing a set of DOM elements to wrap in a jQuery object.</param>
        [ScriptAlias("$")]
        public static JQuery Query(Array elementArray)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Accepts a string containing a CSS selector which is then used to match a set of elements.
        /// </summary>
        /// <param name="selector">A string containing a selector expression</param>
        /// <param name="context">A DOM Element, Document, or jQuery to use as context</param>
        [ScriptAlias("$")]
        public static JQuery Query(string selector, JQuery context)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Accepts a string containing a CSS selector which is then used to match a set of elements.
        /// </summary>
        /// <param name="element">A DOM element to wrap in a jQuery object.</param>
        [ScriptAlias("$")]
        public static JQuery Query(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Creates DOM elements on the fly from the provided string of raw HTML.
        /// </summary>
        /// <param name="html">A string defining a single, standalone, HTML element (e.g. <div/> or <div></div>).</param>
        /// <param name="props">An map of attributes, events, and methods to call on the newly-created element.</param>
        [ScriptAlias("$")]
        public static JQuery Query(string html, object props)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Binds a function to be executed when the DOM has finished loading.
        /// </summary>
        /// <param name="callback">The function to execute when the DOM is ready.</param>
        [ScriptAlias("$")]
        public static JQuery Query(Delegate callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Accepts a string containing a CSS selector which is then used to match a set of elements.
        /// </summary>
        [ScriptAlias("$")]
        public static JQuery Query()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Creates DOM elements on the fly from the provided string of raw HTML.
        /// </summary>
        /// <param name="html">A string of HTML to create on the fly. Note that this parses HTML, not XML.</param>
        /// <param name="ownerDocument">A document in which the new elements will be created</param>
        [ScriptAlias("$")]
        public static JQuery Query(string html, IHTMLDocument ownerDocument)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Accepts a string containing a CSS selector which is then used to match a set of elements.
        /// </summary>
        /// <param name="object">A plain object to wrap in a jQuery object.</param>
        [ScriptAlias("$")]
        public static JQuery Query(object @object)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Accepts a string containing a CSS selector which is then used to match a set of elements.
        /// </summary>
        /// <param name="selector">A string containing a selector expression</param>
        [ScriptAlias("$")]
        public static JQuery Query(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "keydown" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Keydown(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "keydown" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Keydown(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "keydown" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Keydown()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "keypress" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Keypress()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "keypress" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Keypress(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "keypress" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Keypress(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "keyup" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Keyup(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "keyup" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Keyup(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "keyup" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Keyup()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to the final one in the set.
        /// </summary>
        public JQuery Last()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach an event handler for all elements which match the current selector, now and in the future.
        /// </summary>
        /// <param name="events">A string containing a JavaScript event type, such as "click" or "keydown." As of jQuery 1.4 the string can contain multiple, space-separated event types or custom event names.</param>
        /// <param name="handler">A function to execute at the time the event is triggered.</param>
        public JQuery Live(string events, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach an event handler for all elements which match the current selector, now and in the future.
        /// </summary>
        /// <param name="events">A string containing a JavaScript event type, such as "click" or "keydown." As of jQuery 1.4 the string can contain multiple, space-separated event types or custom event names.</param>
        /// <param name="data">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute at the time the event is triggered.</param>
        public JQuery Live(string events, object data, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach an event handler for all elements which match the current selector, now and in the future.
        /// </summary>
        /// <param name="eventsmap">A map of one or more JavaScript event types and functions to execute for them.</param>
        public JQuery Live(object eventsmap)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server and place the returned HTML into the matched element.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="data">A map or string that is sent to the server with the request.</param>
        public JQuery Load(string url, object data)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server and place the returned HTML into the matched element.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="data">A map or string that is sent to the server with the request.</param>
        public JQuery Load(string url, string data)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server and place the returned HTML into the matched element.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="data">A map or string that is sent to the server with the request.</param>
        /// <param name="complete">A callback function that is executed when the request completes.</param>
        public JQuery Load(string url, object data, Func<string, string, XMLHttpRequest, object> complete)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "load" JavaScript event.
        /// </summary>
        /// <param name="handler">A function to execute when the event is triggered.</param>
        public JQuery Load(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server and place the returned HTML into the matched element.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="data">A map or string that is sent to the server with the request.</param>
        /// <param name="complete">A callback function that is executed when the request completes.</param>
        public JQuery Load(string url, string data, Func<string, string, XMLHttpRequest, object> complete)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "load" JavaScript event.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Load(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Convert an array-like object into a true JavaScript array.
        /// </summary>
        /// <param name="obj">Any object to turn into a native Array.</param>
        public static Array MakeArray(object obj)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Pass each element in the current matched set through a function, producing a new jQuery object containing the return values.
        /// </summary>
        /// <param name="callback">A function object that will be invoked for each element in the current set.</param>
        public JQuery Map(Func<int, object, object> callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Translate all items in an array or object to new array of items.
        /// </summary>
        /// <param name="array">The Array to translate.</param>
        /// <param name="callback">The function to process each item against.  The first argument to the function is the array item, the second argument is the index in array The function can return any value. Within the function, this refers to the global (window) object.</param>
        public static Array Map(Array array, Func<object, object, object> callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Translate all items in an array or object to new array of items.
        /// </summary>
        /// <param name="arrayOrObject">The Array or Object to translate.</param>
        /// <param name="callback">The function to process each item against.  The first argument to the function is the value; the second argument is the index or key of the array or object property. The function can return any value to add to the array. A returned array will be flattened into the resulting array. Within the function, this refers to the global (window) object.</param>
        public static Array Map(object arrayOrObject, Func<object, object, object> callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Merge the contents of two arrays together into the first array.
        /// </summary>
        /// <param name="first">The first array to merge, the elements of second added.</param>
        /// <param name="second">The second array to merge into the first, unaltered.</param>
        public static Array Merge(Array first, Array second)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mousedown" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mousedown(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mousedown" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Mousedown()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mousedown" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mousedown(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to be fired when the mouse enters an element, or trigger that handler on an element.
        /// </summary>
        public JQuery Mouseenter()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to be fired when the mouse enters an element, or trigger that handler on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseenter(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to be fired when the mouse enters an element, or trigger that handler on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseenter(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to be fired when the mouse leaves an element, or trigger that handler on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseleave(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to be fired when the mouse leaves an element, or trigger that handler on an element.
        /// </summary>
        public JQuery Mouseleave()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to be fired when the mouse leaves an element, or trigger that handler on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseleave(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mousemove" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mousemove(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mousemove" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Mousemove()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mousemove" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mousemove(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mouseout" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseout(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mouseout" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Mouseout()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mouseout" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseout(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mouseover" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseover(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mouseover" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseover(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mouseover" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Mouseover()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mouseup" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseup(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mouseup" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Mouseup(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "mouseup" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Mouseup()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the immediately following sibling of each element in the set of matched elements. If a selector is provided, it retrieves the next sibling only if it matches that selector.
        /// </summary>
        public JQuery Next()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the immediately following sibling of each element in the set of matched elements. If a selector is provided, it retrieves the next sibling only if it matches that selector.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Next(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all following siblings of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery NextAll(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all following siblings of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        public JQuery NextAll()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all following siblings of each element up to but not including the element matched by the selector, DOM node, or jQuery object passed.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to indicate where to stop matching following sibling elements.</param>
        /// <param name="filter">A string containing a selector expression to match elements against.</param>
        public JQuery NextUntil(string selector, string filter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all following siblings of each element up to but not including the element matched by the selector, DOM node, or jQuery object passed.
        /// </summary>
        /// <param name="element">A DOM node or jQuery object indicating where to stop matching following sibling elements.</param>
        /// <param name="filter">A string containing a selector expression to match elements against.</param>
        public JQuery NextUntil(IHTMLElement element, string filter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all following siblings of each element up to but not including the element matched by the selector, DOM node, or jQuery object passed.
        /// </summary>
        public JQuery NextUntil()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all following siblings of each element up to but not including the element matched by the selector, DOM node, or jQuery object passed.
        /// </summary>
        /// <param name="filter">A string containing a selector expression to match elements against.</param>
        public JQuery NextUntil(string filter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Relinquish jQuery's control of the $ variable.
        /// </summary>
        public static object NoConflict()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Relinquish jQuery's control of the $ variable.
        /// </summary>
        /// <param name="removeAll">A Boolean indicating whether to remove all jQuery variables from the global scope (including jQuery itself).</param>
        public static object NoConflict(bool removeAll)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// An empty function.
        /// </summary>
        public static Delegate Noop()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove elements from the set of matched elements.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Not(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove elements from the set of matched elements.
        /// </summary>
        /// <param name="elements">One or more DOM elements to remove from the matched set.</param>
        public JQuery Not(IHTMLElement elements)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove elements from the set of matched elements.
        /// </summary>
        /// <param name="function">A function used as a test for each element in the set. this is the current DOM element.</param>
        public JQuery Not(Func<int, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Return a number representing the current time.
        /// </summary>
        public static double Now()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove an event handler.
        /// </summary>
        /// <param name="events">One or more space-separated event types and optional namespaces, or just namespaces, such as "click", "keydown.myPlugin", or ".myPlugin".</param>
        /// <param name="selector">A selector which should match the one originally passed to .on() when attaching event handlers.</param>
        /// <param name="handler">A handler function previously attached for the event(s), or the special value false.</param>
        public JQuery Off(string events, string selector, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove an event handler.
        /// </summary>
        /// <param name="events">One or more space-separated event types and optional namespaces, or just namespaces, such as "click", "keydown.myPlugin", or ".myPlugin".</param>
        public JQuery Off(string events)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove an event handler.
        /// </summary>
        /// <param name="eventsmap">A map where the string keys represent one or more space-separated event types and optional namespaces, and the values represent handler functions previously attached for the event(s).</param>
        /// <param name="selector">A selector which should match the one originally passed to .on() when attaching event handlers.</param>
        public JQuery Off(object eventsmap, string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove an event handler.
        /// </summary>
        /// <param name="events">One or more space-separated event types and optional namespaces, or just namespaces, such as "click", "keydown.myPlugin", or ".myPlugin".</param>
        /// <param name="handler">A handler function previously attached for the event(s), or the special value false.</param>
        public JQuery Off(string events, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove an event handler.
        /// </summary>
        /// <param name="eventsmap">A map where the string keys represent one or more space-separated event types and optional namespaces, and the values represent handler functions previously attached for the event(s).</param>
        public JQuery Off(object eventsmap)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current coordinates of the first element in the set of matched elements, relative to the document.
        /// </summary>
        public object Offset()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the current coordinates of every element in the set of matched elements, relative to the document.
        /// </summary>
        /// <param name="function">A function to return the coordinates to set. Receives the index of the element in the collection as the first argument and the current coordinates as the second argument. The function should return an object with the new top and left properties.</param>
        public JQuery Offset(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the current coordinates of every element in the set of matched elements, relative to the document.
        /// </summary>
        /// <param name="coordinates">An object containing the properties top and left, which are integers indicating the new top and left coordinates for the elements.</param>
        public JQuery Offset(object coordinates)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the closest ancestor element that is positioned.
        /// </summary>
        public JQuery OffsetParent()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach an event handler function for one or more events to the selected elements.
        /// </summary>
        /// <param name="events">One or more space-separated event types and optional namespaces, such as "click" or "keydown.myPlugin".</param>
        /// <param name="handler">A function to execute when the event is triggered. The value false is also allowed as a shorthand for a function that simply does return false.</param>
        public JQuery On(string events, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach an event handler function for one or more events to the selected elements.
        /// </summary>
        /// <param name="events">One or more space-separated event types and optional namespaces, such as "click" or "keydown.myPlugin".</param>
        /// <param name="selector">A selector string to filter the descendants of the selected elements that trigger the event. If the selector is null or omitted, the event is always triggered when it reaches the selected element.</param>
        /// <param name="data">Data to be passed to the handler in event.data when an event is triggered.</param>
        /// <param name="handler">A function to execute when the event is triggered. The value false is also allowed as a shorthand for a function that simply does return false.</param>
        public JQuery On(string events, string selector, object data, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach an event handler function for one or more events to the selected elements.
        /// </summary>
        /// <param name="eventsmap">A map in which the string keys represent one or more space-separated event types and optional namespaces, and the values represent a handler function to be called for the event(s).</param>
        /// <param name="selector">A selector string to filter the descendants of the selected elements that will call the handler. If the selector is null or omitted, the handler is always called when it reaches the selected element.</param>
        /// <param name="data">Data to be passed to the handler in event.data when an event occurs.</param>
        public JQuery On(object eventsmap, string selector, object data)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach an event handler function for one or more events to the selected elements.
        /// </summary>
        /// <param name="events">One or more space-separated event types and optional namespaces, such as "click" or "keydown.myPlugin".</param>
        /// <param name="data">Data to be passed to the handler in event.data when an event is triggered.</param>
        /// <param name="handler">A function to execute when the event is triggered. The value false is also allowed as a shorthand for a function that simply does return false.</param>
        public JQuery On(string events, object data, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach an event handler function for one or more events to the selected elements.
        /// </summary>
        /// <param name="eventsmap">A map in which the string keys represent one or more space-separated event types and optional namespaces, and the values represent a handler function to be called for the event(s).</param>
        public JQuery On(object eventsmap)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach an event handler function for one or more events to the selected elements.
        /// </summary>
        /// <param name="eventsmap">A map in which the string keys represent one or more space-separated event types and optional namespaces, and the values represent a handler function to be called for the event(s).</param>
        /// <param name="data">Data to be passed to the handler in event.data when an event occurs.</param>
        public JQuery On(object eventsmap, object data)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements. The handler is executed at most once per element.
        /// </summary>
        /// <param name="eventsmap">A map in which the string keys represent one or more space-separated event types and optional namespaces, and the values represent a handler function to be called for the event(s).</param>
        public JQuery One(object eventsmap)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements. The handler is executed at most once per element.
        /// </summary>
        /// <param name="eventsmap">A map in which the string keys represent one or more space-separated event types and optional namespaces, and the values represent a handler function to be called for the event(s).</param>
        /// <param name="data">Data to be passed to the handler in event.data when an event occurs.</param>
        public JQuery One(object eventsmap, object data)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements. The handler is executed at most once per element.
        /// </summary>
        /// <param name="eventsmap">A map in which the string keys represent one or more space-separated event types and optional namespaces, and the values represent a handler function to be called for the event(s).</param>
        /// <param name="selector">A selector string to filter the descendants of the selected elements that will call the handler. If the selector is null or omitted, the handler is always called when it reaches the selected element.</param>
        /// <param name="data">Data to be passed to the handler in event.data when an event occurs.</param>
        public JQuery One(object eventsmap, string selector, object data)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements. The handler is executed at most once per element.
        /// </summary>
        /// <param name="events">A string containing one or more JavaScript event types, such as "click" or "submit," or custom event names.</param>
        /// <param name="data">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute at the time the event is triggered.</param>
        public JQuery One(string events, object data, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements. The handler is executed at most once per element.
        /// </summary>
        /// <param name="events">One or more space-separated event types and optional namespaces, such as "click" or "keydown.myPlugin".</param>
        /// <param name="selector">A selector string to filter the descendants of the selected elements that trigger the event. If the selector is null or omitted, the event is always triggered when it reaches the selected element.</param>
        /// <param name="data">Data to be passed to the handler in event.data when an event is triggered.</param>
        /// <param name="handler">A function to execute when the event is triggered. The value false is also allowed as a shorthand for a function that simply does return false.</param>
        public JQuery One(string events, string selector, object data, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Attach a handler to an event for the elements. The handler is executed at most once per element.
        /// </summary>
        /// <param name="events">A string containing one or more JavaScript event types, such as "click" or "submit," or custom event names.</param>
        /// <param name="handler">A function to execute at the time the event is triggered.</param>
        public JQuery One(string events, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current computed height for the first element in the set of matched elements, including padding, border, and optionally margin. Returns an integer (without "px") representation of the value or null if called on an empty set of elements.
        /// </summary>
        /// <param name="includeMargin">A Boolean indicating whether to include the element's margin in the calculation.</param>
        public int OuterHeight(bool includeMargin)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current computed height for the first element in the set of matched elements, including padding, border, and optionally margin. Returns an integer (without "px") representation of the value or null if called on an empty set of elements.
        /// </summary>
        public int OuterHeight()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current computed width for the first element in the set of matched elements, including padding and border.
        /// </summary>
        /// <param name="includeMargin">A Boolean indicating whether to include the element's margin in the calculation.</param>
        public int OuterWidth(bool includeMargin)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current computed width for the first element in the set of matched elements, including padding and border.
        /// </summary>
        public int OuterWidth()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Create a serialized representation of an array or object, suitable for use in a URL query string or Ajax request.
        /// </summary>
        /// <param name="obj">An array or object to serialize.</param>
        /// <param name="traditional">A Boolean indicating whether to perform a traditional "shallow" serialization.</param>
        public static string Param(object obj, bool traditional)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Create a serialized representation of an array or object, suitable for use in a URL query string or Ajax request.
        /// </summary>
        /// <param name="obj">An array or object to serialize.</param>
        public static string Param(object obj)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Create a serialized representation of an array or object, suitable for use in a URL query string or Ajax request.
        /// </summary>
        /// <param name="obj">An array or object to serialize.</param>
        /// <param name="traditional">A Boolean indicating whether to perform a traditional "shallow" serialization.</param>
        public static string Param(Array obj, bool traditional)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Create a serialized representation of an array or object, suitable for use in a URL query string or Ajax request.
        /// </summary>
        /// <param name="obj">An array or object to serialize.</param>
        public static string Param(Array obj)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the parent of each element in the current set of matched elements, optionally filtered by a selector.
        /// </summary>
        public JQuery Parent()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the parent of each element in the current set of matched elements, optionally filtered by a selector.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Parent(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the ancestors of each element in the current set of matched elements, optionally filtered by a selector.
        /// </summary>
        public JQuery Parents()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the ancestors of each element in the current set of matched elements, optionally filtered by a selector.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Parents(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the ancestors of each element in the current set of matched elements, up to but not including the element matched by the selector, DOM node, or jQuery object.
        /// </summary>
        public JQuery ParentsUntil()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the ancestors of each element in the current set of matched elements, up to but not including the element matched by the selector, DOM node, or jQuery object.
        /// </summary>
        /// <param name="element">A DOM node or jQuery object indicating where to stop matching ancestor elements.</param>
        /// <param name="filter">A string containing a selector expression to match elements against.</param>
        public JQuery ParentsUntil(IHTMLElement element, string filter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the ancestors of each element in the current set of matched elements, up to but not including the element matched by the selector, DOM node, or jQuery object.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to indicate where to stop matching ancestor elements.</param>
        /// <param name="filter">A string containing a selector expression to match elements against.</param>
        public JQuery ParentsUntil(string selector, string filter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the ancestors of each element in the current set of matched elements, up to but not including the element matched by the selector, DOM node, or jQuery object.
        /// </summary>
        /// <param name="filter">A string containing a selector expression to match elements against.</param>
        public JQuery ParentsUntil(string filter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Takes a well-formed JSON string and returns the resulting JavaScript object.
        /// </summary>
        /// <param name="json">The JSON string to parse.</param>
        public static object ParseJSON(string json)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Parses a string into an XML document.
        /// </summary>
        /// <param name="data">a well-formed XML string to be parsed</param>
        public static IXmlDocument ParseXML(string data)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current coordinates of the first element in the set of matched elements, relative to the offset parent.
        /// </summary>
        public object Position()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP POST request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="data">A map or string that is sent to the server with the request.</param>
        /// <param name="success">A callback function that is executed if the request succeeds.</param>
        /// <param name="dataType">The type of data expected from the server. Default: Intelligent Guess (xml, json, script, or html).</param>
        public static XMLHttpRequest Post(string url, object data, Func<object, string, XMLHttpRequest, object> success, string dataType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP POST request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="data">A map or string that is sent to the server with the request.</param>
        /// <param name="success">A callback function that is executed if the request succeeds.</param>
        /// <param name="dataType">The type of data expected from the server. Default: Intelligent Guess (xml, json, script, or html).</param>
        public static XMLHttpRequest Post(string url, string data, Func<object, string, XMLHttpRequest, object> success, string dataType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP POST request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="success">A callback function that is executed if the request succeeds.</param>
        /// <param name="dataType">The type of data expected from the server. Default: Intelligent Guess (xml, json, script, or html).</param>
        public static XMLHttpRequest Post(string url, Func<object, string, XMLHttpRequest, object> success, string dataType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP POST request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        public static XMLHttpRequest Post(string url)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Load data from the server using a HTTP POST request.
        /// </summary>
        /// <param name="url">A string containing the URL to which the request is sent.</param>
        /// <param name="dataType">The type of data expected from the server. Default: Intelligent Guess (xml, json, script, or html).</param>
        public static XMLHttpRequest Post(string url, string dataType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, to the beginning of each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A function that returns an HTML string, DOM element(s), or jQuery object to insert at the beginning of each element in the set of matched elements. Receives the index position of the element in the set and the old HTML value of the element as arguments. Within the function, this refers to the current element in the set.</param>
        public JQuery Prepend(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, to the beginning of each element in the set of matched elements.
        /// </summary>
        /// <param name="content">DOM element, array of elements, HTML string, or jQuery object to insert at the beginning of each element in the set of matched elements.</param>
        public JQuery Prepend(IHTMLElement content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, to the beginning of each element in the set of matched elements.
        /// </summary>
        /// <param name="content">DOM element, array of elements, HTML string, or jQuery object to insert at the beginning of each element in the set of matched elements.</param>
        public JQuery Prepend(JQuery content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert content, specified by the parameter, to the beginning of each element in the set of matched elements.
        /// </summary>
        /// <param name="content">DOM element, array of elements, HTML string, or jQuery object to insert at the beginning of each element in the set of matched elements.</param>
        public JQuery Prepend(string content)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements to the beginning of the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the beginning of the element(s) specified by this parameter.</param>
        public JQuery PrependTo(string target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements to the beginning of the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the beginning of the element(s) specified by this parameter.</param>
        public JQuery PrependTo(JQuery target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Insert every element in the set of matched elements to the beginning of the target.
        /// </summary>
        /// <param name="target">A selector, element, HTML string, or jQuery object; the matched set of elements will be inserted at the beginning of the element(s) specified by this parameter.</param>
        public JQuery PrependTo(IHTMLElement target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the immediately preceding sibling of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        public JQuery Prev()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the immediately preceding sibling of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Prev(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all preceding siblings of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        public JQuery PrevAll()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all preceding siblings of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery PrevAll(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all preceding siblings of each element up to but not including the element matched by the selector, DOM node, or jQuery object.
        /// </summary>
        public JQuery PrevUntil()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all preceding siblings of each element up to but not including the element matched by the selector, DOM node, or jQuery object.
        /// </summary>
        /// <param name="element">A DOM node or jQuery object indicating where to stop matching preceding sibling elements.</param>
        /// <param name="filter">A string containing a selector expression to match elements against.</param>
        public JQuery PrevUntil(IHTMLElement element, string filter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all preceding siblings of each element up to but not including the element matched by the selector, DOM node, or jQuery object.
        /// </summary>
        /// <param name="filter">A string containing a selector expression to match elements against.</param>
        public JQuery PrevUntil(string filter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get all preceding siblings of each element up to but not including the element matched by the selector, DOM node, or jQuery object.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to indicate where to stop matching preceding sibling elements.</param>
        /// <param name="filter">A string containing a selector expression to match elements against.</param>
        public JQuery PrevUntil(string selector, string filter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Return a Promise object to observe when all actions of a certain type bound to the collection, queued or not, have finished.
        /// </summary>
        /// <param name="target">Object onto which the promise methods have to be attached</param>
        public Promise Promise(object target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Return a Promise object to observe when all actions of a certain type bound to the collection, queued or not, have finished.
        /// </summary>
        public Promise Promise()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Return a Promise object to observe when all actions of a certain type bound to the collection, queued or not, have finished.
        /// </summary>
        /// <param name="type">The type of queue that needs to be observed.</param>
        /// <param name="target">Object onto which the promise methods have to be attached</param>
        public Promise Promise(string type, object target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the value of a property for the first element in the set of matched elements.
        /// </summary>
        /// <param name="propertyName">The name of the property to get.</param>
        public string Prop(string propertyName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more properties for the set of matched elements.
        /// </summary>
        /// <param name="map">A map of property-value pairs to set.</param>
        public JQuery Prop(object map)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more properties for the set of matched elements.
        /// </summary>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">A value to set for the property.</param>
        public JQuery Prop(string propertyName, string value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more properties for the set of matched elements.
        /// </summary>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">A value to set for the property.</param>
        public JQuery Prop(string propertyName, double value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more properties for the set of matched elements.
        /// </summary>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">A value to set for the property.</param>
        public JQuery Prop(string propertyName, bool value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set one or more properties for the set of matched elements.
        /// </summary>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="function">A function returning the value to set. Receives the index position of the element in the set and the old property value as arguments. Within the function, the keyword this refers to the current element.</param>
        public JQuery Prop(string propertyName, Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Takes a function and returns a new one that will always have a particular context.
        /// </summary>
        /// <param name="function">The function whose context will be changed.</param>
        /// <param name="context">The object to which the context (this) of the function should be set.</param>
        public static Delegate Proxy(Delegate function, object context)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Takes a function and returns a new one that will always have a particular context.
        /// </summary>
        /// <param name="context">The object to which the context of the function should be set.</param>
        /// <param name="name">The name of the function whose context will be changed (should be a property of the context object).</param>
        public static Delegate Proxy(object context, string name)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add a collection of DOM elements onto the jQuery stack.
        /// </summary>
        /// <param name="elements">An array of elements to push onto the stack and make into a new jQuery object.</param>
        public JQuery PushStack(Array elements)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add a collection of DOM elements onto the jQuery stack.
        /// </summary>
        /// <param name="elements">An array of elements to push onto the stack and make into a new jQuery object.</param>
        /// <param name="name">The name of a jQuery method that generated the array of elements.</param>
        /// <param name="arguments">The arguments that were passed in to the jQuery method (for serialization).</param>
        public JQuery PushStack(Array elements, string name, Array arguments)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Show the queue of functions to be executed on the matched elements.
        /// </summary>
        public Array Queue()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Manipulate the queue of functions to be executed on the matched elements.
        /// </summary>
        /// <param name="newQueue">An array of functions to replace the current queue contents.</param>
        public JQuery Queue(Array newQueue)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Show the queue of functions to be executed on the matched elements.
        /// </summary>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        public Array Queue(string queueName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Manipulate the queue of functions to be executed on the matched elements.
        /// </summary>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        /// <param name="newQueue">An array of functions to replace the current queue contents.</param>
        public JQuery Queue(string queueName, Array newQueue)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Manipulate the queue of functions to be executed on the matched elements.
        /// </summary>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        /// <param name="callback">The new function to add to the queue, with a function to call that will dequeue the next item.</param>
        public JQuery Queue(string queueName, Func<object, object> callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Manipulate the queue of functions to be executed on the matched elements.
        /// </summary>
        /// <param name="callback">The new function to add to the queue, with a function to call that will dequeue the next item.</param>
        public JQuery Queue(Func<object, object> callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Manipulate the queue of functions to be executed on the matched element.
        /// </summary>
        /// <param name="element">A DOM element on which to add a queued function.</param>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        /// <param name="callback">The new function to add to the queue.</param>
        public static JQuery Queue(IHTMLElement element, string queueName, Func<object> callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Manipulate the queue of functions to be executed on the matched element.
        /// </summary>
        /// <param name="element">A DOM element where the array of queued functions is attached.</param>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        /// <param name="newQueue">An array of functions to replace the current queue contents.</param>
        public static JQuery Queue(IHTMLElement element, string queueName, Array newQueue)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Show the queue of functions to be executed on the matched element.
        /// </summary>
        /// <param name="element">A DOM element to inspect for an attached queue.</param>
        public static Array Queue(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Show the queue of functions to be executed on the matched element.
        /// </summary>
        /// <param name="element">A DOM element to inspect for an attached queue.</param>
        /// <param name="queueName">A string containing the name of the queue. Defaults to fx, the standard effects queue.</param>
        public static Array Queue(IHTMLElement element, string queueName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Specify a function to execute when the DOM is fully loaded.
        /// </summary>
        /// <param name="handler">A function to execute after the DOM is ready.</param>
        public JQuery Ready(Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove the set of matched elements from the DOM.
        /// </summary>
        /// <param name="selector">A selector expression that filters the set of matched elements to be removed.</param>
        public JQuery Remove(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove the set of matched elements from the DOM.
        /// </summary>
        public JQuery Remove()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove an attribute from each element in the set of matched elements.
        /// </summary>
        /// <param name="attributeName">An attribute to remove; as of version 1.7, it can be a space-separated list of attributes.</param>
        public JQuery RemoveAttr(string attributeName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a single class, multiple classes, or all classes from each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A function returning one or more space-separated class names to be removed. Receives the index position of the element in the set and the old class value as arguments.</param>
        public JQuery RemoveClass(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a single class, multiple classes, or all classes from each element in the set of matched elements.
        /// </summary>
        /// <param name="className">One or more space-separated classes to be removed from the class attribute of each matched element.</param>
        public JQuery RemoveClass(string className)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a single class, multiple classes, or all classes from each element in the set of matched elements.
        /// </summary>
        public JQuery RemoveClass()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-stored piece of data.
        /// </summary>
        /// <param name="element">A DOM element from which to remove data.</param>
        public static JQuery RemoveData(IHTMLElement element)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-stored piece of data.
        /// </summary>
        /// <param name="element">A DOM element from which to remove data.</param>
        /// <param name="name">A string naming the piece of data to remove.</param>
        public static JQuery RemoveData(IHTMLElement element, string name)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-stored piece of data.
        /// </summary>
        public JQuery RemoveData()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-stored piece of data.
        /// </summary>
        /// <param name="list">An array or space-separated string naming the pieces of data to delete.</param>
        public JQuery RemoveData(Array list)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-stored piece of data.
        /// </summary>
        /// <param name="name">A string naming the piece of data to delete.</param>
        public JQuery RemoveData(string name)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a property for the set of matched elements.
        /// </summary>
        /// <param name="propertyName">The name of the property to set.</param>
        public JQuery RemoveProp(string propertyName)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Replace each target element with the set of matched elements.
        /// </summary>
        /// <param name="target">A selector expression indicating which element(s) to replace.</param>
        public JQuery ReplaceAll(string target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Replace each element in the set of matched elements with the provided new content.
        /// </summary>
        /// <param name="newContent">The content to insert. May be an HTML string, DOM element, or jQuery object.</param>
        public JQuery ReplaceWith(JQuery newContent)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Replace each element in the set of matched elements with the provided new content.
        /// </summary>
        /// <param name="newContent">The content to insert. May be an HTML string, DOM element, or jQuery object.</param>
        public JQuery ReplaceWith(IHTMLElement newContent)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Replace each element in the set of matched elements with the provided new content.
        /// </summary>
        /// <param name="newContent">The content to insert. May be an HTML string, DOM element, or jQuery object.</param>
        public JQuery ReplaceWith(string newContent)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Replace each element in the set of matched elements with the provided new content.
        /// </summary>
        /// <param name="function">A function that returns content with which to replace the set of matched elements.</param>
        public JQuery ReplaceWith(Delegate function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "resize" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Resize(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "resize" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Resize(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "resize" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Resize()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "scroll" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Scroll()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "scroll" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Scroll(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "scroll" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Scroll(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current horizontal position of the scroll bar for the first element in the set of matched elements.
        /// </summary>
        public int ScrollLeft()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the current horizontal position of the scroll bar for each of the set of matched elements.
        /// </summary>
        /// <param name="value">An integer indicating the new position to set the scroll bar to.</param>
        public JQuery ScrollLeft(double value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the current vertical position of the scroll bar for each of the set of matched elements.
        /// </summary>
        /// <param name="value">An integer indicating the new position to set the scroll bar to.</param>
        public JQuery ScrollTop(double value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current vertical position of the scroll bar for the first element in the set of matched elements.
        /// </summary>
        public int ScrollTop()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "select" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Select(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "select" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Select(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "select" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Select()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Encode a set of form elements as a string for submission.
        /// </summary>
        public string Serialize()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Encode a set of form elements as an array of names and values.
        /// </summary>
        public Array SerializeArray()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Show(double duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        public JQuery Show(string duration)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements.
        /// </summary>
        public JQuery Show()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        public JQuery Show(double duration)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Show(string duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Show(double duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Show(string duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements.
        /// </summary>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Show(Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the siblings of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        public JQuery Siblings()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the siblings of each element in the set of matched elements, optionally filtered by a selector.
        /// </summary>
        /// <param name="selector">A string containing a selector expression to match elements against.</param>
        public JQuery Siblings(string selector)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Return the number of elements in the jQuery object.
        /// </summary>
        public double Size()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to a subset specified by a range of indices.
        /// </summary>
        /// <param name="start">An integer indicating the 0-based position at which the elements begin to be selected. If negative, it indicates an offset from the end of the set.</param>
        /// <param name="end">An integer indicating the 0-based position at which the elements stop being selected. If negative, it indicates an offset from the end of the set. If omitted, the range continues until the end of the set.</param>
        public JQuery Slice(int start, int end)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reduce the set of matched elements to a subset specified by a range of indices.
        /// </summary>
        /// <param name="start">An integer indicating the 0-based position at which the elements begin to be selected. If negative, it indicates an offset from the end of the set.</param>
        public JQuery Slice(int start)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideDown(double duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements with a sliding motion.
        /// </summary>
        public JQuery SlideDown()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideDown(string duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements with a sliding motion.
        /// </summary>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideDown(Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideDown(string duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideDown(double duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideToggle(string duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideToggle(double duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements with a sliding motion.
        /// </summary>
        public JQuery SlideToggle()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideToggle(Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideToggle(string duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideToggle(double duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideUp(double duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideUp(Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements with a sliding motion.
        /// </summary>
        public JQuery SlideUp()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideUp(string duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideUp(double duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Hide the matched elements with a sliding motion.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery SlideUp(string duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Stop the currently-running animation on the matched elements.
        /// </summary>
        /// <param name="queue">The name of the queue in which to stop animations.</param>
        /// <param name="clearQueue">A Boolean indicating whether to remove queued animation as well. Defaults to false.</param>
        /// <param name="jumpToEnd">A Boolean indicating whether to complete the current animation immediately. Defaults to false.</param>
        public JQuery Stop(string queue, bool clearQueue, bool jumpToEnd)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Stop the currently-running animation on the matched elements.
        /// </summary>
        /// <param name="clearQueue">A Boolean indicating whether to remove queued animation as well. Defaults to false.</param>
        /// <param name="jumpToEnd">A Boolean indicating whether to complete the current animation immediately. Defaults to false.</param>
        public JQuery Stop(bool clearQueue, bool jumpToEnd)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Stop the currently-running animation on the matched elements.
        /// </summary>
        /// <param name="jumpToEnd">A Boolean indicating whether to complete the current animation immediately. Defaults to false.</param>
        public JQuery Stop(bool jumpToEnd)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Stop the currently-running animation on the matched elements.
        /// </summary>
        public JQuery Stop()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Creates a new copy of jQuery whose properties and methods can be modified without affecting the original jQuery object.
        /// </summary>
        public static JQuery Sub()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "submit" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Submit(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "submit" JavaScript event, or trigger that event on an element.
        /// </summary>
        public JQuery Submit()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "submit" JavaScript event, or trigger that event on an element.
        /// </summary>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Submit(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the combined text contents of each element in the set of matched elements, including their descendants.
        /// </summary>
        public string Text()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the content of each element in the set of matched elements to the specified text.
        /// </summary>
        /// <param name="function">A function returning the text content to set. Receives the index position of the element in the set and the old text value as arguments.</param>
        public JQuery Text(Func<int, string, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the content of each element in the set of matched elements to the specified text.
        /// </summary>
        /// <param name="textString">A string of text to set as the content of each matched element.</param>
        public JQuery Text(string textString)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Retrieve all the DOM elements contained in the jQuery set, as an array.
        /// </summary>
        public Array ToArray()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Toggle(string duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements.
        /// </summary>
        /// <param name="showOrHide">A Boolean indicating whether to show or hide the elements.</param>
        public JQuery Toggle(bool showOrHide)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind two or more handlers to the matched elements, to be executed on alternate clicks.
        /// </summary>
        /// <param name="handler">A function to execute every even time the element is clicked.</param>
        public JQuery Toggle(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Toggle(string duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Toggle(double duration, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements.
        /// </summary>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Toggle(Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements.
        /// </summary>
        public JQuery Toggle()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Display or hide the matched elements.
        /// </summary>
        /// <param name="duration">A string or number determining how long the animation will run.</param>
        /// <param name="easing">A string indicating which easing function to use for the transition.</param>
        /// <param name="callback">A function to call once the animation is complete.</param>
        public JQuery Toggle(double duration, string easing, Action callback)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add or remove one or more classes from each element in the set of matched elements, depending on either the class's presence or the value of the switch argument.
        /// </summary>
        public JQuery ToggleClass()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add or remove one or more classes from each element in the set of matched elements, depending on either the class's presence or the value of the switch argument.
        /// </summary>
        /// <param name="className">One or more class names (separated by spaces) to be toggled for each element in the matched set.</param>
        /// <param name="switch">A Boolean (not just truthy/falsy) value to determine whether the class should be added or removed.</param>
        public JQuery ToggleClass(string className, bool @switch)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add or remove one or more classes from each element in the set of matched elements, depending on either the class's presence or the value of the switch argument.
        /// </summary>
        /// <param name="className">One or more class names (separated by spaces) to be toggled for each element in the matched set.</param>
        public JQuery ToggleClass(string className)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add or remove one or more classes from each element in the set of matched elements, depending on either the class's presence or the value of the switch argument.
        /// </summary>
        /// <param name="function">A function that returns class names to be toggled in the class attribute of each element in the matched set. Receives the index position of the element in the set, the old class value, and the switch as arguments.</param>
        public JQuery ToggleClass(Func<int, object, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add or remove one or more classes from each element in the set of matched elements, depending on either the class's presence or the value of the switch argument.
        /// </summary>
        /// <param name="switch">A boolean value to determine whether the class should be added or removed.</param>
        public JQuery ToggleClass(bool @switch)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add or remove one or more classes from each element in the set of matched elements, depending on either the class's presence or the value of the switch argument.
        /// </summary>
        /// <param name="function">A function that returns class names to be toggled in the class attribute of each element in the matched set. Receives the index position of the element in the set, the old class value, and the switch as arguments.</param>
        /// <param name="switch">A boolean value to determine whether the class should be added or removed.</param>
        public JQuery ToggleClass(Func<int, object, object, object> function, bool @switch)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Execute all handlers and behaviors attached to the matched elements for the given event type.
        /// </summary>
        /// <param name="event">A jQuery.Event object.</param>
        public JQuery Trigger(JQueryEvent @event)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Execute all handlers and behaviors attached to the matched elements for the given event type.
        /// </summary>
        /// <param name="eventType">A string containing a JavaScript event type, such as click or submit.</param>
        /// <param name="extraParameters">Additional parameters to pass along to the event handler.</param>
        public JQuery Trigger(string eventType, object extraParameters)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Execute all handlers attached to an element for an event.
        /// </summary>
        /// <param name="eventType">A string containing a JavaScript event type, such as click or submit.</param>
        /// <param name="extraParameters">An array of additional parameters to pass along to the event handler.</param>
        public object TriggerHandler(string eventType, Array extraParameters)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove the whitespace from the beginning and end of a string.
        /// </summary>
        /// <param name="str">The string to trim.</param>
        public static string Trim(string str)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine the internal JavaScript [[Class]] of an object.
        /// </summary>
        /// <param name="obj">Object to get the internal JavaScript [[Class]] of.</param>
        public static string Type(object obj)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-attached event handler from the elements.
        /// </summary>
        public JQuery Unbind()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-attached event handler from the elements.
        /// </summary>
        /// <param name="handler">The function that is to be no longer executed.</param>
        public JQuery Unbind(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-attached event handler from the elements.
        /// </summary>
        /// <param name="eventType">A string containing a JavaScript event type, such as click or submit.</param>
        /// <param name="handler">The function that is to be no longer executed.</param>
        public JQuery Unbind(string eventType, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-attached event handler from the elements.
        /// </summary>
        /// <param name="event">A JavaScript event object as passed to an event handler.</param>
        public JQuery Unbind(object @event)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a previously-attached event handler from the elements.
        /// </summary>
        /// <param name="eventType">A string containing a JavaScript event type, such as click or submit.</param>
        /// <param name="false">Unbinds the corresponding 'return false' function that was bound using .bind( eventType, false ).</param>
        public JQuery Unbind(string eventType, bool @false)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a handler from the event for all elements which match the current selector, based upon a specific set of root elements.
        /// </summary>
        /// <param name="selector">A selector which will be used to filter the event results.</param>
        /// <param name="eventType">A string containing a JavaScript event type, such as "click" or "keydown"</param>
        /// <param name="handler">A function to execute at the time the event is triggered.</param>
        public JQuery Undelegate(string selector, string eventType, Delegate handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a handler from the event for all elements which match the current selector, based upon a specific set of root elements.
        /// </summary>
        /// <param name="selector">A selector which will be used to filter the event results.</param>
        /// <param name="events">A map of one or more event types and previously bound functions to unbind from them.</param>
        public JQuery Undelegate(string selector, object events)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a handler from the event for all elements which match the current selector, based upon a specific set of root elements.
        /// </summary>
        public JQuery Undelegate()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a handler from the event for all elements which match the current selector, based upon a specific set of root elements.
        /// </summary>
        /// <param name="selector">A selector which will be used to filter the event results.</param>
        /// <param name="eventType">A string containing a JavaScript event type, such as "click" or "keydown"</param>
        public JQuery Undelegate(string selector, string eventType)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove a handler from the event for all elements which match the current selector, based upon a specific set of root elements.
        /// </summary>
        /// <param name="namespace">A string containing a namespace to unbind all events from.</param>
        public JQuery Undelegate(string @namespace)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Sorts an array of DOM elements, in place, with the duplicates removed. Note that this only works on arrays of DOM elements, not strings or numbers.
        /// </summary>
        /// <param name="array">The Array of DOM elements.</param>
        public static Array Unique(Array array)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "unload" JavaScript event.
        /// </summary>
        /// <param name="handler">A function to execute when the event is triggered.</param>
        public JQuery Unload(Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Bind an event handler to the "unload" JavaScript event.
        /// </summary>
        /// <param name="eventData">A map of data that will be passed to the event handler.</param>
        /// <param name="handler">A function to execute each time the event is triggered.</param>
        public JQuery Unload(object eventData, Func<JQueryEvent, bool> handler)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Remove the parents of the set of matched elements from the DOM, leaving the matched elements in their place.
        /// </summary>
        public JQuery Unwrap()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current value of the first element in the set of matched elements.
        /// </summary>
        public object Val()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the value of each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A function returning the value to set. this is the current element. Receives the index position of the element in the set and the old value as arguments.</param>
        public JQuery Val(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the value of each element in the set of matched elements.
        /// </summary>
        /// <param name="value">A string of text or an array of strings corresponding to the value of each matched element to set as selected/checked.</param>
        public JQuery Val(string value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Provides a way to execute callback functions based on one or more objects, usually Deferred objects that represent asynchronous events.
        /// </summary>
        /// <param name="deferreds">One or more Deferred objects, or plain JavaScript objects.</param>
        public static Promise When(Deferred deferreds)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the CSS width of each element in the set of matched elements.
        /// </summary>
        /// <param name="value">An integer representing the number of pixels, or an integer along with an optional unit of measure appended (as a string).</param>
        public JQuery Width(double value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the CSS width of each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A function returning the width to set. Receives the index position of the element in the set and the old width as arguments. Within the function, this refers to the current element in the set.</param>
        public JQuery Width(Func<int, object, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Get the current computed width for the first element in the set of matched elements.
        /// </summary>
        public int Width()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Set the CSS width of each element in the set of matched elements.
        /// </summary>
        /// <param name="value">An integer representing the number of pixels, or an integer along with an optional unit of measure appended (as a string).</param>
        public JQuery Width(string value)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Wrap an HTML structure around each element in the set of matched elements.
        /// </summary>
        /// <param name="wrappingElement">An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.</param>
        public JQuery Wrap(string wrappingElement)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Wrap an HTML structure around each element in the set of matched elements.
        /// </summary>
        /// <param name="wrappingElement">An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.</param>
        public JQuery Wrap(IHTMLElement wrappingElement)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Wrap an HTML structure around each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A callback function returning the HTML content or jQuery object to wrap around the matched elements. Receives the index position of the element in the set as an argument. Within the function, this refers to the current element in the set.</param>
        public JQuery Wrap(Func<int, object> function)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Wrap an HTML structure around each element in the set of matched elements.
        /// </summary>
        /// <param name="wrappingElement">An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.</param>
        public JQuery Wrap(JQuery wrappingElement)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Wrap an HTML structure around all elements in the set of matched elements.
        /// </summary>
        /// <param name="wrappingElement">An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.</param>
        public JQuery WrapAll(string wrappingElement)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Wrap an HTML structure around all elements in the set of matched elements.
        /// </summary>
        /// <param name="wrappingElement">An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.</param>
        public JQuery WrapAll(IHTMLElement wrappingElement)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Wrap an HTML structure around all elements in the set of matched elements.
        /// </summary>
        /// <param name="wrappingElement">An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the matched elements.</param>
        public JQuery WrapAll(JQuery wrappingElement)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Wrap an HTML structure around the content of each element in the set of matched elements.
        /// </summary>
        /// <param name="wrappingElement">An HTML snippet, selector expression, jQuery object, or DOM element specifying the structure to wrap around the content of the matched elements.</param>
        public JQuery WrapInner(string wrappingElement)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Wrap an HTML structure around the content of each element in the set of matched elements.
        /// </summary>
        /// <param name="function">A callback function which generates a structure to wrap around the content of the matched elements. Receives the index position of the element in the set as an argument. Within the function, this refers to the current element in the set.</param>
        public JQuery WrapInner(Func<int, object> function)
        {
            throw new PlatformNotSupportedException();
        }

    }
    
    [Imported]
    public sealed class Promise
    {
        private Promise() { }
    }

    [Imported]
    public sealed class Deferred
    {
        private Deferred() { }
        /// <summary>
        /// Add handlers to be called when the Deferred object is either resolved or rejected.
        /// </summary>
        /// <param name="alwaysCallbacks">A function, or array of functions, that is called when the Deferred is resolved or rejected.</param>
        public Deferred Always(Delegate alwaysCallbacks)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add handlers to be called when the Deferred object is resolved.
        /// </summary>
        /// <param name="doneCallbacks">A function, or array of functions, that are called when the Deferred is resolved.</param>
        public Deferred Done(Delegate doneCallbacks)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add handlers to be called when the Deferred object is rejected.
        /// </summary>
        /// <param name="failCallbacks">A function, or array of functions, that are called when the Deferred is rejected.</param>
        public Deferred Fail(Delegate failCallbacks)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine whether a Deferred object has been rejected.
        /// </summary>
        public bool IsRejected()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine whether a Deferred object has been resolved.
        /// </summary>
        public bool IsResolved()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Call the progressCallbacks on a Deferred object with the given args.
        /// </summary>
        /// <param name="args">Optional arguments that are passed to the progressCallbacks.</param>
        public Deferred Notify(object args)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Call the progressCallbacks on a Deferred object with the given context and args.
        /// </summary>
        /// <param name="context">Context passed to the progressCallbacks as the this object.</param>
        /// <param name="args">Optional arguments that are passed to the progressCallbacks.</param>
        public Deferred NotifyWith(object context, object args)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Call the progressCallbacks on a Deferred object with the given context and args.
        /// </summary>
        /// <param name="context">Context passed to the progressCallbacks as the this object.</param>
        public Deferred NotifyWith(object context)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Utility method to filter and/or chain Deferreds.
        /// </summary>
        /// <param name="doneFilter">An optional function that is called when the Deferred is resolved.</param>
        /// <param name="failFilter">An optional function that is called when the Deferred is rejected.</param>
        /// <param name="progressFilter">An optional function that is called when the Deferred is rejected.</param>
        public Promise Pipe(Delegate doneFilter, Delegate failFilter, Delegate progressFilter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Utility method to filter and/or chain Deferreds.
        /// </summary>
        public Promise Pipe()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Utility method to filter and/or chain Deferreds.
        /// </summary>
        /// <param name="failFilter">An optional function that is called when the Deferred is rejected.</param>
        public Promise Pipe(Delegate failFilter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Utility method to filter and/or chain Deferreds.
        /// </summary>
        /// <param name="doneFilter">An optional function that is called when the Deferred is resolved.</param>
        /// <param name="failFilter">An optional function that is called when the Deferred is rejected.</param>
        public Promise Pipe(Delegate doneFilter, Delegate failFilter)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add handlers to be called when the Deferred object generates progress notifications.
        /// </summary>
        /// <param name="progressCallbacks">A function, or array of functions, that is called when the Deferred generates progress notifications.</param>
        public Deferred Progress(Delegate progressCallbacks)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Return a Deferred's Promise object.
        /// </summary>
        public Promise Promise()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Return a Deferred's Promise object.
        /// </summary>
        /// <param name="target">Object onto which the promise methods have to be attached</param>
        public Promise Promise(object target)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reject a Deferred object and call any failCallbacks with the given args.
        /// </summary>
        /// <param name="args">Optional arguments that are passed to the failCallbacks.</param>
        public Deferred Reject(object args)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reject a Deferred object and call any failCallbacks with the given context and args.
        /// </summary>
        /// <param name="context">Context passed to the failCallbacks as the this object.</param>
        public Deferred RejectWith(object context)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Reject a Deferred object and call any failCallbacks with the given context and args.
        /// </summary>
        /// <param name="context">Context passed to the failCallbacks as the this object.</param>
        /// <param name="args">An optional array of arguments that are passed to the failCallbacks.</param>
        public Deferred RejectWith(object context, Array args)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Resolve a Deferred object and call any doneCallbacks with the given args.
        /// </summary>
        /// <param name="args">Optional arguments that are passed to the doneCallbacks.</param>
        public Deferred Resolve(object args)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Resolve a Deferred object and call any doneCallbacks with the given context and args.
        /// </summary>
        /// <param name="context">Context passed to the doneCallbacks as the this object.</param>
        /// <param name="args">An optional array of arguments that are passed to the doneCallbacks.</param>
        public Deferred ResolveWith(object context, Array args)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Resolve a Deferred object and call any doneCallbacks with the given context and args.
        /// </summary>
        /// <param name="context">Context passed to the doneCallbacks as the this object.</param>
        public Deferred ResolveWith(object context)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Determine the current state of a Deferred object.
        /// </summary>
        public string State()
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add handlers to be called when the Deferred object is resolved or rejected.
        /// </summary>
        /// <param name="doneCallbacks">A function, or array of functions, called when the Deferred is resolved.</param>
        /// <param name="failCallbacks">A function, or array of functions, called when the Deferred is rejected.</param>
        public Deferred Then(Delegate doneCallbacks, Delegate failCallbacks)
        {
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Add handlers to be called when the Deferred object is resolved or rejected.
        /// </summary>
        /// <param name="doneCallbacks">A function, or array of functions, called when the Deferred is resolved.</param>
        /// <param name="failCallbacks">A function, or array of functions, called when the Deferred is rejected.</param>
        /// <param name="progressCallbacks">A function, or array of functions, called when the Deferred notifies progress.</param>
        public Deferred Then(Delegate doneCallbacks, Delegate failCallbacks, Delegate progressCallbacks)
        {
            throw new PlatformNotSupportedException();
        }

    }
    [AnonymousObject]
    public sealed class JQueryAjaxSettings
    {
        /// <summary>
        /// The content type sent in the request header that tells the server what kind of response it will accept in return. If the accepts setting needs modification, it is recommended to do so once in the $.ajaxSetup() method
        /// </summary>
        public object Accepts;

        /// <summary>
        /// By default, all requests are sent asynchronously (i.e. this is set to true by default). If you need synchronous requests, set this option to false. Cross-domain requests and dataType: "jsonp" requests do not support synchronous operation. Note that synchronous requests may temporarily lock the browser, disabling any actions while the request is active.
        /// </summary>
        public bool Async;

        /// <summary>
        /// A pre-request callback function that can be used to modify the jqXHR (in jQuery 1.4.x, XMLHTTPRequest) object before it is sent. Use this to set custom headers, etc. The jqXHR and settings maps are passed as arguments. This is an Ajax Event. Returning false in the beforeSend function will cancel the request. As of jQuery 1.5, the beforeSend option will be called regardless of the type of request.
        /// </summary>
        public Func<XMLHttpRequest, object, object> BeforeSend;

        /// <summary>
        /// If set to false, it will force requested pages not to be cached by the browser. Setting cache to false also appends a query string parameter, "_=[TIMESTAMP]", to the URL.
        /// </summary>
        public bool Cache;

        /// <summary>
        /// A function to be called when the request finishes (after success and error callbacks are executed). The function gets passed two arguments: The jqXHR (in jQuery 1.4.x, XMLHTTPRequest) object and a string categorizing the status of the request ("success", "notmodified", "error", "timeout", "abort", or "parsererror"). As of jQuery 1.5, the complete setting can accept an array of functions. Each function will be called in turn. This is an Ajax Event.
        /// </summary>
        public object Complete;

        /// <summary>
        /// A map of string/regular-expression pairs that determine how jQuery will parse the response, given its content type.
        /// </summary>
        public object Contents;

        /// <summary>
        /// When sending data to the server, use this content-type. Default is "application/x-www-form-urlencoded", which is fine for most cases. If you explicitly pass in a content-type to $.ajax() then it'll always be sent to the server (even if no data is sent). Data will always be transmitted to the server using UTF-8 charset; you must decode this appropriately on the server side.
        /// </summary>
        public string ContentType;

        /// <summary>
        /// This object will be made the context of all Ajax-related callbacks. By default, the context is an object that represents the ajax settings used in the call ($.ajaxSettings merged with the settings passed to $.ajax). For example specifying a DOM element as the context will make that the context for the complete callback of a request, like so: $.ajax({	url: "test.html",	context: document.body,	success: function(){		$(this).addClass("done");	}});
        /// </summary>
        public object Context;

        /// <summary>
        /// A map of dataType-to-dataType converters. Each converter's value is a function that returns the transformed value of the response
        /// </summary>
        public object Converters;

        /// <summary>
        /// If you wish to force a crossDomain request (such as JSONP) on the same domain, set the value of crossDomain to true. This allows, for example, server-side redirection to another domain
        /// </summary>
        public object CrossDomain;

        /// <summary>
        /// Data to be sent to the server. It is converted to a query string, if not already a string. It's appended to the url for GET-requests. See processData option to prevent this automatic processing. Object must be Key/Value pairs. If value is an Array, jQuery serializes multiple values with same key based on the value of the traditional setting (described below).
        /// </summary>
        public object Data;

        /// <summary>
        /// A function to be used to handle the raw response data of XMLHttpRequest.This is a pre-filtering function to sanitize the response. You should return the sanitized data. The function accepts two arguments: The raw data returned from the server and the 'dataType' parameter.
        /// </summary>
        public Func<object, object, object> DataFilter;

        /// <summary>
        /// The type of data that you're expecting back from the server. If none is specified, jQuery will try to infer it based on the MIME type of the response (an XML MIME type will yield XML, in 1.4 JSON will yield a JavaScript object, in 1.4 script will execute the script, and anything else will be returned as a string). The available types (and the result passed as the first argument to your success callback) are:					"xml": Returns a XML document that can be processed via jQuery."html": Returns HTML as plain text; included script tags are evaluated when inserted in the DOM."script": Evaluates the response as JavaScript and returns it as plain text. Disables caching by appending a query string parameter, "_=[TIMESTAMP]", to the URL unless the cache option is set to true. Note: This will turn POSTs into GETs for remote-domain requests. "json": Evaluates the response as JSON and returns a JavaScript object. In jQuery 1.4 the JSON data is parsed in a strict manner; any malformed JSON is rejected and a parse error is thrown. (See json.org for more information on proper JSON formatting.)"jsonp": Loads in a JSON block using JSONP. Adds an extra "?callback=?" to the end of your URL to specify the callback. Disables caching by appending a query string parameter, "_=[TIMESTAMP]", to the URL unless the cache option is set to true."text": A plain text string.multiple, space-separated values: As of jQuery 1.5, jQuery can convert a dataType from what it received in the Content-Type header to what you require. For example, if you want a text response to be treated as XML, use "text xml" for the dataType. You can also make a JSONP request, have it received as text, and interpreted by jQuery as XML: "jsonp text xml." Similarly, a shorthand string such as "jsonp xml" will first attempt to convert from jsonp to xml, and, failing that, convert from jsonp to text, and then from text to xml.
        /// </summary>
        public string DataType;

        /// <summary>
        /// A function to be called if the request fails. The function receives three arguments: The jqXHR (in jQuery 1.4.x, XMLHttpRequest) object, a string describing the type of error that occurred and an optional exception object, if one occurred. Possible values for the second argument (besides null) are "timeout", "error", "abort", and "parsererror". When an HTTP error occurs, errorThrown receives the textual portion of the HTTP status, such as "Not Found" or "Internal Server Error."  As of jQuery 1.5, the error setting can accept an array of functions. Each function will be called in turn.  Note:This handler is not called for cross-domain script and JSONP requests. This is an Ajax Event.
        /// </summary>
        public Action<XMLHttpRequest, string, object> Error;

        /// <summary>
        /// Whether to trigger global Ajax event handlers for this request. The default is true. Set to false to prevent the global handlers like ajaxStart or ajaxStop from being triggered. This can be used to control various Ajax Events.
        /// </summary>
        public bool Global;

        /// <summary>
        /// A map of additional header key/value pairs to send along with the request. This setting is set before the beforeSend function is called; therefore, any values in the headers setting can be overwritten from within the beforeSend function.
        /// </summary>
        public object Headers;

        /// <summary>
        /// Allow the request to be successful only if the response has changed since the last request. This is done by checking the Last-Modified header. Default value is false, ignoring the header. In jQuery 1.4 this technique also checks the 'etag' specified by the server to catch unmodified data.
        /// </summary>
        public bool IfModified;

        /// <summary>
        /// Allow the current environment to be recognized as "local," (e.g. the filesystem), even if jQuery does not recognize it as such by default. The following protocols are currently recognized as local: file, *-extension, and widget. If the isLocal setting needs modification, it is recommended to do so once in the $.ajaxSetup() method.
        /// </summary>
        public bool IsLocal;

        /// <summary>
        /// Override the callback function name in a jsonp request.  This value will be used instead of 'callback' in the 'callback=?' part of the query string in the url.  So {jsonp:'onJSONPLoad'} would result in 'onJSONPLoad=?' passed to the server. As of jQuery 1.5, setting the jsonp option to false prevents jQuery from adding the "?callback" string to the URL or attempting to use "=?" for transformation. In this case, you should also explicitly set the jsonpCallback setting. For example, { jsonp: false, jsonpCallback: "callbackName" }
        /// </summary>
        public string Jsonp;

        /// <summary>
        /// Specify the callback function name for a JSONP request.  This value will be used instead of the random name automatically generated by jQuery. It is preferable to let jQuery generate a unique name as it'll make it easier to manage the requests and provide callbacks and error handling. You may want to specify the callback when you want to enable better browser caching of GET requests. As of jQuery 1.5, you can also use a function for this setting, in which case the value of jsonpCallback is set to the return value of that function.
        /// </summary>
        public object JsonpCallback;

        /// <summary>
        /// A mime type to override the XHR mime type.
        /// </summary>
        public string MimeType;

        /// <summary>
        /// A password to be used in response to an HTTP access authentication request.
        /// </summary>
        public string Password;

        /// <summary>
        /// By default, data passed in to the data option as an object (technically, anything other than a string) will be processed and transformed into a query string, fitting to the default content-type "application/x-www-form-urlencoded". If you want to send a DOMDocument, or other non-processed data, set this option to false.
        /// </summary>
        public bool ProcessData;

        /// <summary>
        /// Only for requests with "jsonp" or "script" dataType and "GET" type. Forces the request to be interpreted as a certain charset. Only needed for charset differences between the remote and local content.
        /// </summary>
        public string ScriptCharset;

        /// <summary>
        /// A map of numeric HTTP codes and functions to be called when the response has the corresponding code. For example, the following will alert when the response status is a 404:$.ajax({	statusCode: {		404: function() {			alert('page not found');		}	}});If the request is successful, the status code functions take the same parameters as the success callback; if it results in an error, they take the same parameters as the error callback.
        /// </summary>
        public object StatusCode;

        /// <summary>
        /// A function to be called if the request succeeds. The function gets passed three arguments: The data returned from the server, formatted according to the dataType parameter; a string describing the status; and the jqXHR (in jQuery 1.4.x, XMLHttpRequest) object. As of jQuery 1.5, the success setting can accept an array of functions. Each function will be called in turn. This is an Ajax Event.
        /// </summary>
        public object Success;

        /// <summary>
        /// Set a timeout (in milliseconds) for the request. This will override any global timeout set with $.ajaxSetup(). The timeout period starts at the point the $.ajax call is made; if several other requests are in progress and the browser has no connections available, it is possible for a request to time out before it can be sent. In jQuery 1.4.x and below, the XMLHttpRequest object will be in an invalid state if the request times out; accessing any object members may throw an exception. In Firefox 3.0+ only, script and JSONP requests cannot be cancelled by a timeout; the script will run even if it arrives after the timeout period.
        /// </summary>
        public double Timeout;

        /// <summary>
        /// Set this to true if you wish to use the traditional style of param serialization.
        /// </summary>
        public bool Traditional;

        /// <summary>
        /// The type of request to make ("POST" or "GET"), default is "GET". Note: Other HTTP request methods, such as PUT and DELETE, can also be used here, but they are not supported by all browsers.
        /// </summary>
        public string Type;

        /// <summary>
        /// A string containing the URL to which the request is sent.
        /// </summary>
        public string Url;

        /// <summary>
        /// A username to be used in response to an HTTP access authentication request.
        /// </summary>
        public string Username;

        /// <summary>
        /// Callback for creating the XMLHttpRequest object. Defaults to the ActiveXObject when available (IE), the XMLHttpRequest otherwise. Override to provide your own implementation for XMLHttpRequest or enhancements to the factory.
        /// </summary>
        public Delegate Xhr;

        /// <summary>
        /// A map of fieldName-fieldValue pairs to set on the native XHR object. For example, you can use it to set withCredentials to true for cross-domain requests if needed.$.ajax({	 url: a_cross_domain_url,	 xhrFields: {			withCredentials: true	 }});In jQuery 1.5, the withCredentials property was not propagated to the native XHR and thus CORS requests requiring it would ignore this flag. For this reason, we recommend using jQuery 1.5.1+ should you require the use of it.
        /// </summary>
        public object XhrFields;

    }
}
