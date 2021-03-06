using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace LiveScore.Core.Tests.Mocks
{
    public static class MockXamForms
    {
        public static void Init()
        {
            Device.Info = new MockDeviceInfo();
            Device.PlatformServices = new MockPlatformServices();

            DependencyService.Register<MockResourcesProvider>();
            DependencyService.Register<MockDeserializer>();
        }

        internal class MockPlatformServices : IPlatformServices
        {
            private readonly Action<Action> _invokeOnMainThread;
            private readonly Action<Uri> _openUriAction;
            private readonly Func<Uri, CancellationToken, Task<Stream>> _getStreamAsync;

            public MockPlatformServices(Action<Action> invokeOnMainThread = null, Action<Uri> openUriAction = null, Func<Uri, CancellationToken, Task<Stream>> getStreamAsync = null)
            {
                _invokeOnMainThread = invokeOnMainThread;
                _openUriAction = openUriAction;
                _getStreamAsync = getStreamAsync;
            }

            public string GetMD5Hash(string input)
            {
                throw new NotImplementedException();
            }

            public double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes)
            {
                switch (size)
                {
                    case NamedSize.Default:
                        return 10;

                    case NamedSize.Micro:
                        return 4;

                    case NamedSize.Small:
                        return 8;

                    case NamedSize.Medium:
                        return 12;

                    case NamedSize.Large:
                        return 16;

                    default:
                        throw new ArgumentOutOfRangeException(paramName: nameof(size));
                }
            }

            public void OpenUriAction(Uri uri)
            {
                if (_openUriAction != null)
                    _openUriAction(uri);
                else
                    throw new NotImplementedException();
            }

            public bool IsInvokeRequired => false;

            public string RuntimePlatform { get; set; }

            public void BeginInvokeOnMainThread(Action action)
            {
                try
                {
                    if (_invokeOnMainThread == null)
                        action();
                    else
                        _invokeOnMainThread(action);
                }
#pragma warning disable S108 // Nested blocks of code should not be left empty
                catch
                {
                }
#pragma warning restore S108 // Nested blocks of code should not be left empty
            }

            public Ticker CreateTicker()
            {
                return new MockTicker();
            }

            public void StartTimer(TimeSpan interval, Func<bool> callback)
            {
                Timer timer = null;
                void onTimeout(object o) => BeginInvokeOnMainThread(() =>
                 {
                     if (callback())
                         return;

                     timer.Dispose();
                 });
                timer = new Timer(onTimeout, null, interval, interval);
            }

            public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
            {
                if (_getStreamAsync == null)
                    throw new NotImplementedException();
                return _getStreamAsync(uri, cancellationToken);
            }

            public Assembly[] GetAssemblies()
            {
                return new Assembly[0];
            }

            public IIsolatedStorageFile GetUserStoreForApplication()
            {
                throw new NotImplementedException();
            }

            public void QuitApplication()
            {
                throw new NotImplementedException();
            }

            public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
            {
                throw new NotImplementedException();
            }
        }

        internal class MockDeserializer : IDeserializer
        {
            public Task<IDictionary<string, object>> DeserializePropertiesAsync()
            {
                return Task.FromResult<IDictionary<string, object>>(new Dictionary<string, object>());
            }

            public Task SerializePropertiesAsync(IDictionary<string, object> properties)
            {
                return Task.FromResult(false);
            }
        }

        internal class MockResourcesProvider : ISystemResourcesProvider
        {
            public IResourceDictionary GetSystemResources()
            {
                var dictionary = new ResourceDictionary();
                Style style = new Style(typeof(Label));
                dictionary[Device.Styles.BodyStyleKey] = style;

                style = new Style(typeof(Label));
                style.Setters.Add(Label.FontSizeProperty, 50);
                dictionary[Device.Styles.TitleStyleKey] = style;

                style = new Style(typeof(Label));
                style.Setters.Add(Label.FontSizeProperty, 40);
                dictionary[Device.Styles.SubtitleStyleKey] = style;

                style = new Style(typeof(Label));
                style.Setters.Add(Label.FontSizeProperty, 30);
                dictionary[Device.Styles.CaptionStyleKey] = style;

                style = new Style(typeof(Label));
                style.Setters.Add(Label.FontSizeProperty, 20);
                dictionary[Device.Styles.ListItemTextStyleKey] = style;

                style = new Style(typeof(Label));
                style.Setters.Add(Label.FontSizeProperty, 10);
                dictionary[Device.Styles.ListItemDetailTextStyleKey] = style;

                return dictionary;
            }
        }

        internal class MockTicker : Ticker
        {
            private bool _enabled;

            protected override void EnableTimer()
            {
                _enabled = true;

                while (_enabled)
                {
                    SendSignals(16);
                }
            }

            protected override void DisableTimer()
            {
                _enabled = false;
            }
        }

        internal class MockDeviceInfo : DeviceInfo
        {
            public override Size PixelScreenSize => throw new NotImplementedException();

            public override Size ScaledScreenSize => throw new NotImplementedException();

            public override double ScalingFactor => throw new NotImplementedException();
        }
    }
}