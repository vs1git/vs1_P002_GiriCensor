using System;
using System.Collections.Generic;

namespace Vs1Plugin
{
    public class SubComponentManager
    {
        List<SubComponentI> _subComponents = new List<SubComponentI>();

        public List<SubComponentI> subComponents {
            get { return _subComponents; }
        }

        public void Add(SubComponentI sub)
        {
            _subComponents.Add(sub);
        }

        public void Remove(SubComponentI sub)
        {
            _subComponents.Remove(sub);
        }

        public SubComponentI GetSubComponentAnyByType(string componentType)
        {
            foreach (var com in _subComponents)
            {
                if (com.componentType == componentType)
                {
                    return com;
                }
            }

            return null;
        }

        // -----------------

        public void Init() { 
            foreach (var sub in _subComponents)
            {
                try
                {
                    sub.Init();
                }
                catch (Exception e)
                {
                    Log.LogError(e.ToString());
                }
            }
        }

        public void Start()
        {
            foreach (var sub in _subComponents)
            {
                try
                {
                    sub.Start();
                }
                catch (Exception e)
                {
                    Log.LogError(e.ToString());
                }
            }
        }

        public void Update()
        {
            foreach (var sub in _subComponents)
            {
                try
                {
                    sub.Update();
                }
                catch (Exception e)
                {
                    Log.LogError(e.ToString());
                }
            }
        }

        public void FixedUpdate()
        {
            foreach (var sub in _subComponents)
            {
                try
                {
                    sub.FixedUpdate();
                }
                catch (Exception e)
                {
                    Log.LogError(e.ToString());
                }
            }
        }

        public void OnDestroy()
        {
            foreach (var sub in _subComponents)
            {
                try
                {
                    sub.OnDestroy();
                }
                catch (Exception e)
                {
                    Log.LogError(e.ToString());
                }
            }
        }
    }
}