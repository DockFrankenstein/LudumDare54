using System;
using Myra;
using Myra.Graphics2D.UI;
using Stride.Core;
using Myra.Graphics2D.Brushes;
using Stride.Core.Mathematics;

namespace LudumDare54.UI
{
    public class FaderUI : UICanvas
    {
        [DataMemberIgnore] public static FaderUI Instance { get; private set; }

        Panel _panel;

        bool _fading;
        double _time;
        double _toSeconds;
        double _fromSeconds;
        Action _onCoverScreen;
        Action _onFadeOut;
        bool _coverTriggered;

        public override void InitializeUI()
        {
            _panel = new Panel();
            CanvasDesktop.Root = _panel;
            Instance = this;
        }

        public override void Update()
        {
            base.Update();

            if (!_fading) return;

            _time += Game.UpdateTime.Elapsed.TotalSeconds;

            var value = Math.Clamp(_time / _toSeconds, 0.0, 1.0) -
                Math.Clamp((_time - _toSeconds) / _fromSeconds, 0.0, 1.0);

            _panel.Opacity = (float)value;

            if (!_coverTriggered && _time >= _toSeconds)
            {
                _coverTriggered = true;
                _onCoverScreen?.Invoke();
            }

            if (_time >= _toSeconds + _fromSeconds)
            {
                _panel.Opacity = 0f;
                _fading = false;
                Active = false;
                _onFadeOut?.Invoke();
            }
        }

        public void Fade(Color color, double toSeconds, double fromSeconds, Action onCoverScreen = null, Action onFadeOut = null)
        {
            _panel.Background = new SolidBrush(color);
            _fading = true;
            _time = 0.0;
            _toSeconds = toSeconds;
            _fromSeconds = fromSeconds;
            _onCoverScreen = onCoverScreen;
            _onFadeOut = onFadeOut;
            _coverTriggered = false;
            Active = true;
        }
    }
}