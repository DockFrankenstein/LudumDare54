using Stride.Engine;
using System;
using System.Collections.Generic;

using Myra;
using Myra.Graphics2D.UI;

namespace LudumDare54.UI
{
    public abstract class UICanvas :  SyncScript
    {
        public bool activeByDefault;
        public int uiPriority;

        bool _active;
        public bool Active
        {
            get => _active;
            set
            {
                if (_active == value)
                    return;

                _active = value;

                switch (_active)
                {
                    case true:
                        AddCanvas(this);
                        break;
                    case false:
                        if (ActiveCanvases.Contains(this))
                            ActiveCanvases.Remove(this);

                        break;
                }

                OnChangeState(value);
            }
        }

        [Stride.Core.DataMemberIgnore] public Desktop CanvasDesktop { get; private set; }

        public static List<UICanvas> ActiveCanvases = new List<UICanvas>();

        private static void AddCanvas(UICanvas canvas)
        {
            if (ActiveCanvases.Contains(canvas))
                return;

            int i = ActiveCanvases.Count - 1;
            for (; i >= 0; i--)
                if (ActiveCanvases[i].uiPriority > canvas.uiPriority)
                    break;

            ActiveCanvases.Insert(i + 1, canvas);
        }

        public override void Start()
        {
            CanvasDesktop = new Desktop();

            InitializeUI();
            Active = activeByDefault;
        }

        public override void Cancel()
        {
            Active = false;
        }

        public override void Update()
        {

        }

        public abstract void InitializeUI();

        public virtual void OnDrawUI()
        {

        }

        public virtual void OnChangeState(bool isActive)
        {

        }
    }
}
